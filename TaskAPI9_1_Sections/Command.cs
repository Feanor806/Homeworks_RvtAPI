using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace TaskAPI9_1_Sections
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Выберете элемент");
            FamilyInstance instance = doc.GetElement(reference) as FamilyInstance;

            BoundingBoxXYZ bbox = instance.get_BoundingBox(null);
            XYZ min = bbox.Min;
            XYZ max = bbox.Max;
            XYZ center = (bbox.Min + bbox.Max) / 2;
            XYZ size = bbox.Max - bbox.Min;

            Transform transform = Transform.CreateTranslation(XYZ.Zero);
            transform.Origin = center;
            transform.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            transform.BasisY = XYZ.BasisZ;
            transform.BasisZ = XYZ.BasisY;

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;
            sectionBox.Min = new XYZ(-size.X / 2, -size.Z / 2, -size.Y / 2);
            sectionBox.Max = new XYZ(size.X / 2, size.Z / 2, size.Y / 2);

            ViewFamilyType viewType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .OfType<ViewFamilyType>()
                .First(x => x.ViewFamily == ViewFamily.Section);

            using (var transaction = new Transaction(doc, "VisualizeTransform"))
            {
                transaction.Start();
                ViewSection viewSection = ViewSection.CreateSection(doc, viewType.Id, sectionBox);
                viewSection.Name = "Разрез 1";
                transaction.Commit();
            }

            return Result.Succeeded;
        }

        public void VisualizeTransform(Transform transform, Document document, double scale = 3)
        {
            var colors = new List<Color>()
            {
                new Color(255, 0,0),   // X - красный
                new Color (0, 255,0),  // Y - зеленый
                new Color(0, 0, 255)   // Z - синий
            };

            var colorToLines = Enumerable.Range(0, 3)
                .Select(transform.get_Basis)
                .Select(x => Line.CreateBound(
                    transform.Origin,
                    transform.Origin + x * scale))
                .Zip(colors, (line, color) => (Line: line, Color: color))
                .ToList();

            foreach (var (line, color) in colorToLines)
            {
                var directShape = DirectShape.CreateElement(document, new ElementId(BuiltInCategory.OST_GenericModel));
                directShape.SetShape(new List<GeometryObject>() { line });

                var overrideGraphics = new OverrideGraphicSettings();
                overrideGraphics.SetProjectionLineColor(color);
                overrideGraphics.SetProjectionLineWeight(4);
                document.ActiveView.SetElementOverrides(directShape.Id, overrideGraphics);
            }
        }

        public Solid CreateSolidFromBoundingBox(BoundingBoxXYZ bbox)
        {
            var min = bbox.Min;
            var max = bbox.Max;
            var transform = bbox.Transform;

            // Получаем все 8 вершин параллелепипеда в локальной системе координат
            var vertices = new List<XYZ>
            {
                new XYZ(min.X, min.Y, min.Z), // 0: min-min-min
                new XYZ(max.X, min.Y, min.Z), // 1: max-min-min
                new XYZ(max.X, max.Y, min.Z), // 2: max-max-min
                new XYZ(min.X, max.Y, min.Z), // 3: min-max-min
                new XYZ(min.X, min.Y, max.Z), // 4: min-min-max
                new XYZ(max.X, min.Y, max.Z), // 5: max-min-max
                new XYZ(max.X, max.Y, max.Z), // 6: max-max-max
                new XYZ(min.X, max.Y, max.Z)  // 7: min-max-max
            };

            // Преобразуем вершины в глобальную систему координат
            var globalVertices = vertices.Select(v => transform.OfPoint(v)).ToList();

            // Создаем Solid через GeometryCreationUtilities
            // Используем метод создания через выдавливание (extrusion)
            var profileCurves = new List<Curve>();

            // Создаем профиль из нижней грани (Z = min.Z в локальной системе)
            var bottomFace = new List<XYZ>
            {
                globalVertices[0], // min-min-min
                globalVertices[1], // max-min-min
                globalVertices[2], // max-max-min
                globalVertices[3]  // min-max-min
            };

            // Создаем замкнутый контур нижней грани
            for (int i = 0; i < bottomFace.Count; i++)
            {
                var start = bottomFace[i];
                var end = bottomFace[(i + 1) % bottomFace.Count];
                profileCurves.Add(Line.CreateBound(start, end));
            }

            // Обертываем кривые в CurveLoop
            var profile = new CurveLoop();
            foreach (var curve in profileCurves)
            {
                profile.Append(curve);
            }

            // Направление выдавливания = высота параллелепипеда в глобальной системе
            var height = globalVertices[4] - globalVertices[0]; // от нижней к верхней вершине

            // Создаем Solid через выдавливание
            var solid = GeometryCreationUtilities.CreateExtrusionGeometry(
                new List<CurveLoop> { profile },
                height.Normalize(),
                height.GetLength());

            return solid;
        }

        public void VisualizeAsSolid(Document doc, Solid solid)
        {
            var directShape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            directShape.SetShape(new List<GeometryObject>() { solid });
        }
    }
}
