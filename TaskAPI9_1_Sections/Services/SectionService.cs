using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using TaskAPI9_1_Sections.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI9_1_Sections.Services
{
    internal class SectionService : ISectionService
    {
        private readonly ExternalCommandData _commandData;

        public SectionService(ExternalCommandData commandData)
        {
            this._commandData = commandData;
        }
        public FamilyInstance PickObject()
        {
            try
            {
                Reference reference = _commandData.Application.ActiveUIDocument.Selection.PickObject(ObjectType.Element, new InstanceSelectionFilter());
                FamilyInstance instance = _commandData.Application.ActiveUIDocument.Document.GetElement(reference) as FamilyInstance;
                return instance;
            }
            catch
            {
                return null;
            }
        }
    }
}
