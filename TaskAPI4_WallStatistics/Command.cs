using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI4_WallStatistics
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<Wall> docWalls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .WhereElementIsNotElementType()
                .OfType<Wall>()
                .ToList();

            if (docWalls.Count < 1)
            {
                TaskDialog.Show("В модели нет стен","В модели нет размещенных стен");
                return Result.Failed;
            }

            double maxlength = 0.0;
            double minlength = 0.0;

            ElementId shortWallId = null;
            ElementId longWallId = null;

            foreach (Wall wall in docWalls)
            {
                Curve wallCurve = (wall.Location as LocationCurve).Curve;

                //Поиск большего
                if(wallCurve.Length> maxlength)
                {
                    maxlength = wallCurve.Length;
                    longWallId = wall.Id;
                }

                //Поиск меньшего
                if (wallCurve.Length < minlength || minlength == 0.0)
                {
                    minlength = wallCurve.Length;
                    shortWallId = wall.Id;
                }
            }

            using (var transaction = new Transaction(doc, "Запись параметров в стены"))
            {
                transaction.Start();

                var longWallComment = doc.GetElement(longWallId).get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                longWallComment.Set("Самая длинная стена");

                var shortWallComment = doc.GetElement(shortWallId).get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                shortWallComment.Set("Самая короткая стена");

                transaction.Commit();
            }

            TaskDialog.Show("Стены определены", $"Всего стен в модели:{docWalls.Count}\nСтена {longWallId.IntegerValue} с наибольшей длиной {Math.Round(maxlength*304.8,0)} мм\nСтена {shortWallId.IntegerValue} с наименьшей длиной {Math.Round(minlength * 304.8, 0)} мм");
            return Result.Failed;
        }
    }
}
