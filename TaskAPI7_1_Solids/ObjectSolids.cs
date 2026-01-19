using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI7_1_Solids
{
    [Transaction(TransactionMode.Manual)]
    public class ObjectSolids : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Element element = null;
            List<Solid> solids = new List<Solid>();
            List<Face> faces = new List<Face>();
            List<Edge> edges = new List<Edge>();
            double sumVolume = 0.0;
            double sumArea = 0.0;
            double edgesLength = 0.0;

            try
            {
                Reference reference = uiDoc.Selection.PickObject(
                    ObjectType.Element,
                    new NotFamilyInstanceFilter(),
                    "Выберите экземпляр системного семейства"
                );
                if (reference is null)
                {
                    TaskDialog.Show("Ошибка", $"Не удалось получить элемент");
                    return Result.Failed;
                }
                element = doc.GetElement(reference);
            }
            //Обработка отмены выбора
            catch
            {
                TaskDialog.Show("Ошибка", "Пользователь отменил выбор");
                return Result.Failed;
            }

            Options geomOptions = new Options();
            geomOptions.DetailLevel = ViewDetailLevel.Fine;
            geomOptions.ComputeReferences = true;
            geomOptions.IncludeNonVisibleObjects = true;

            
            GeometryElement geometry = element.get_Geometry(geomOptions);

            if (geometry != null)
            {
                foreach (GeometryObject geomObj in geometry)
                {
                    if (geomObj is Solid solid)
                    {
                        if (solid != null && solid.Volume > 0)
                        {
                            solids.Add(solid);
                            sumVolume += solid.Volume;
                            foreach(Face face in solid.Faces)
                            {
                                double area = 0.0;
                                try
                                {
                                    area = face.Area;
                                }
                                catch
                                {
                                    continue;
                                }
                                if(!(area > 0.0))
                                {
                                    continue;
                                }
                                sumArea += area;
                                faces.Add(face);
                            }
                            foreach (Edge edge in solid.Edges)
                            {
                                try
                                {
                                    Curve curve = edge.AsCurve();
                                    if (curve != null && curve.Length > 0.0)
                                    {
                                        edgesLength += curve.Length;
                                        edges.Add(edge);
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            else
            {
                TaskDialog.Show("Ошибка", "Не удалось получить геометрию");
                return Result.Failed;
            }
            sumVolume = UnitUtils.ConvertFromInternalUnits(sumVolume, DisplayUnitType.DUT_CUBIC_METERS);
            sumArea = UnitUtils.ConvertFromInternalUnits(sumArea, DisplayUnitType.DUT_SQUARE_METERS);
            edgesLength = UnitUtils.ConvertFromInternalUnits(edgesLength, DisplayUnitType.DUT_METERS);

            TaskDialog.Show("Готово", $"Получен объект категории {element.Category.Name}\n" +
                $"Элемент состоит из {solids.Count} солидов с суммарным объемом {Math.Round(sumVolume,2)} м3\n" +
                $"Элемент имеет {faces.Count} граней с суммарной площадью {Math.Round(sumArea, 2)} м2\n" +
                $"Элемент имеет {edges.Count} ребер с суммарной длиной {Math.Round(edgesLength, 2)} м");
            return Result.Succeeded;
        }
    }

    internal class NotFamilyInstanceFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return !(elem is FamilyInstance);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}