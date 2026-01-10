using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI7_1_Solids
{
    [Transaction(TransactionMode.Manual)]
    public class ObjectSolids
    {

    }

    public class NotFamilyInstanceFilter : ISelectionFilter
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
