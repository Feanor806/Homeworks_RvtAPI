using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI9_1_Sections.Abstractions;

namespace TaskAPI9_1_Sections.Services
{
    public class SectionService : ISectionService
    {
        private readonly ExternalCommandData _commandData;

        public SectionService(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }
        public bool CreateSection(FamilyInstance instance, double widthOffsetMm, double heightOffsetMm, double depthOffsetMm, string sectionName)
        {

            Document doc = _commandData.Application.ActiveUIDocument.Document;

            BoundingBoxXYZ bbox = instance.get_BoundingBox(null);
            if (bbox == null) return false;

            //Характерные точки BoundingBox
            XYZ min = bbox.Min;
            XYZ max = bbox.Max;
            XYZ center = (bbox.Min + bbox.Max) / 2;
            XYZ size = bbox.Max - bbox.Min;

            //Флег положения коробочки в основной модели
            bool sideIsFront = false;
            if ((bbox.Max.Y - bbox.Min.Y) > (bbox.Max.X - bbox.Min.X)) { sideIsFront = true; }

            //Координатная сетка при взгляде на плоскость XZ модели
            Transform transform1 = Transform.CreateTranslation(XYZ.Zero);
            transform1.Origin = center;
            transform1.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            transform1.BasisY = XYZ.BasisZ;
            transform1.BasisZ = XYZ.BasisY;

            //Координатная сетка при взгляде на плоскость YZ модели
            Transform transform2 = Transform.CreateTranslation(XYZ.Zero);
            transform2.Origin = center;
            transform2.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisX)).Normalize();
            transform2.BasisY = XYZ.BasisZ;
            transform2.BasisZ = XYZ.BasisX;

            //Координатная сетка при взгляде на плоскость XY модели
            Transform transformTop = Transform.CreateTranslation(XYZ.Zero);
            transformTop.Origin = center;

            //Координатные сетки для видов спереди и сбоку. Логика определения
            Transform transformFront = Transform.CreateTranslation(XYZ.Zero);
            Transform transformSide = Transform.CreateTranslation(XYZ.Zero);

            //Выбор координатных сеток разрезов при различной конфигурации коробки в модели
            if (sideIsFront)
            {
                transformFront = transform2;
                transformSide = transform1;
                transformTop.BasisX = XYZ.BasisY;
                transformTop.BasisY = (-XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
                transformTop.BasisZ = -XYZ.BasisZ;
            }
            else
            {
                transformFront = transform1;
                transformSide = transform2;
                transformTop.BasisX = XYZ.BasisX;
                transformTop.BasisY = (-XYZ.BasisZ.CrossProduct(XYZ.BasisX)).Normalize();
                transformTop.BasisZ = -XYZ.BasisZ;
            }

            double widthOffset = UnitUtils.ConvertToInternalUnits(widthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            double heightOffset = UnitUtils.ConvertToInternalUnits(heightOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            double depthOffset = UnitUtils.ConvertToInternalUnits(depthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);

            //Размеры элемента на разрезах. X - всегда ширина, Y - всегда высота, Z - всегда глубина
            XYZ sizesFront = new XYZ();
            XYZ sizesSide = new XYZ();
            XYZ sizesTop = new XYZ();

            //Размеры в зависимисти от пропорций коробки в модели
            if (sideIsFront)
            {
                sizesFront = new XYZ(size.Y,size.Z,size.X);
                sizesSide = new XYZ(size.X, size.Z, size.Y);
                sizesTop = new XYZ(size.Y, size.X, size.Z);
            }
            else
            {
                sizesFront = new XYZ(size.X, size.Z, size.Y);
                sizesSide = new XYZ(size.Y, size.Z, size.X);
                sizesTop = new XYZ(size.X, size.Y, size.Z);
            }

            //Коробочки разрезов
            BoundingBoxXYZ sectionBoxFront = CreateSectionBox(sizesFront, widthOffset, heightOffset, depthOffset, transformFront);
            BoundingBoxXYZ sectionBoxSide = CreateSectionBox(sizesSide, widthOffset, heightOffset, depthOffset, transformSide);
            BoundingBoxXYZ sectionBoxTop = CreateSectionBox(sizesTop, widthOffset, heightOffset, depthOffset, transformTop);

            //Тип разреза
            ViewFamilyType viewTypeSection = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .OfType<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Section);

            ViewFamilyType viewTypePlan = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .OfType<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.FloorPlan);

            if (viewTypeSection == null || viewTypePlan == null) return false;

            try
            {
                using (var transaction = new Transaction(doc, "Построение разрезов"))
                {
                    transaction.Start();
                    ViewSection viewSectionFront = ViewSection.CreateSection(doc, viewTypeSection.Id, sectionBoxFront);
                    ViewSection viewSectionSide = ViewSection.CreateSection(doc, viewTypeSection.Id, sectionBoxSide);
                    ViewSection viewSectionTop = ViewSection.CreateSection(doc, viewTypeSection.Id, sectionBoxTop);

                    //Условно отвязался от формы. Имена фиксированные
                    viewSectionFront.Name = "Вид спереди";
                    viewSectionSide.Name = "Вид сбоку";
                    viewSectionTop.Name = "Вид сверху";
                    transaction.Commit();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private BoundingBoxXYZ CreateSectionBox(XYZ sizes, double widthOffset, double heightOffset, double depthOffset, Transform transform)
        {
            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;
            sectionBox.Min = new XYZ(-sizes.X / 2 - widthOffset, -sizes.Y / 2 - heightOffset, -sizes.Z / 2 - depthOffset);
            sectionBox.Max = new XYZ(sizes.X / 2 + widthOffset, sizes.Y / 2 + heightOffset, sizes.Z / 2 + depthOffset);
            return sectionBox;
        }
    }
}
