using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI6_1_WallDistance
{
    [Transaction(TransactionMode.Manual)]
    public class WallDistance : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            List<Wall> walls = new List<Wall>();

            //Получаем элементы стен, валидируем ввод
            try
            {
                IList<Reference> references = uiDoc.Selection.PickObjects(
                    ObjectType.Element,
                    new WallFilter(),
                    "Выберите две стены"
                );
                if (references.Count != 2)
                {
                    TaskDialog.Show("Ошибка", $"Необходимо выбрать ровно 2 стены. Выбрано: {references.Count}");
                    return Result.Failed;
                }
                foreach (Reference reference in references)
                {
                    Element element = doc.GetElement(reference);
                    if (element is Wall wall)
                    {
                        walls.Add(wall);
                    }
                }
                // Дополнительная проверка
                if (walls.Count != 2)
                {
                    TaskDialog.Show("Ошибка", "Выбраны не стены!");
                    return Result.Failed;
                }
            }
            //Обработка отмены выбора
            catch
            {
                TaskDialog.Show("Ошибка", "Пользователь отменил выбор");
                return Result.Failed;
            }

            //Получаем нормализованное направление обоих стен вспомогательным методом
            XYZ direction1 = GetWallDirection(walls.First());
            XYZ direction2 = GetWallDirection(walls.Last());

            //Определяем скалярное произведение 
            double dotProduct = direction1.DotProduct(direction2);
            double tolerance = 0.001;

            //Если векторы не являются сонаправленными или противоположжно направленными - работа завершается
            if (!(Math.Abs(Math.Abs(dotProduct) - 1) < tolerance))
            {
                TaskDialog.Show("Ошибка", "Стены не параллельны");
                return Result.Failed;
            }

            //Определяем центра линий вспомогательным методом
            XYZ center1 = GetWallCenter(walls.First());
            XYZ center2 = GetWallCenter(walls.Last());

            //Получаем вектор, соединяющий центра стен
            XYZ centerVector = center1 - center2;

            //Получаем проекцию вектора межжду центрами на вектор, перпендикулярный направлению стен
            XYZ cross = direction1.CrossProduct(centerVector);

            //Получаем расстояние между стенами, учитывая толщину обоих стен
            double distance = Math.Round((cross.GetLength() - walls.First().Width/2 - walls.Last().Width/2) * 304.8,0);

            TaskDialog.Show("Результат", $"Расстояние межжду стенами {distance} мм");

            return Result.Succeeded;
        }

        /// <summary>
        /// Метод для определения нормализованного направления стены
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private XYZ GetWallDirection(Element wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            if (location == null) return null;
            Curve curve = location.Curve;
            XYZ start = curve.GetEndPoint(0);
            XYZ end = curve.GetEndPoint(1);

            return (end - start).Normalize();
        }

        /// <summary>
        /// Метод для определения координаты центра стены
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private XYZ GetWallCenter(Element wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            if (location == null) return null;
            Curve curve = location.Curve;
            XYZ start = curve.GetEndPoint(0);
            XYZ direction = GetWallDirection(wall);
            double wallLength = curve.Length;

            return start+direction*(wallLength/2);
        }
    }
    /// <summary>
    /// Фильтр выбора элементов стен
    /// </summary>
    internal class WallFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            // Разрешаем только стены
            return elem is Wall;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
