using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI9_1_Sections.Abstractions
{
    public interface ISelectionService
    {
        FamilyInstance PickObject();
    }
}
