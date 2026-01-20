using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI9_1_Sections.Services
{
    internal class InstanceSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (!(elem is FamilyInstance)) return false;
            return true;
        }
        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
