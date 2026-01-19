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

            XYZ min = bbox.Min;
            XYZ max = bbox.Max;
            XYZ center = (bbox.Min + bbox.Max) / 2;
            XYZ size = bbox.Max - bbox.Min;

            Transform transform = Transform.CreateTranslation(XYZ.Zero);
            transform.Origin = center;
            transform.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            transform.BasisY = XYZ.BasisZ;
            transform.BasisZ = XYZ.BasisY;

            double widthOffset = UnitUtils.ConvertToInternalUnits(widthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            double heightOffset = UnitUtils.ConvertToInternalUnits(heightOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            double depthOffset = UnitUtils.ConvertToInternalUnits(depthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);

            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;
            sectionBox.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - heightOffset, -size.Y / 2 - depthOffset);
            sectionBox.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + heightOffset, size.Y / 2 + depthOffset);

            ViewFamilyType viewType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .OfType<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Section);

            if (viewType == null) return false;

            try
            {
                using (var transaction = new Transaction(doc, "Построение разреза"))
                {
                    transaction.Start();
                    ViewSection viewSection = ViewSection.CreateSection(doc, viewType.Id, sectionBox);
                    viewSection.Name = sectionName;
                    transaction.Commit();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
