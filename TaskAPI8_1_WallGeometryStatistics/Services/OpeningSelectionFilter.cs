using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace TaskAPI8_1_WallGeometryStatistics.Services
{
    public class OpeningSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (!(elem is Wall)) return false;
            return true;
        }

        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
