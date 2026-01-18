using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using TaskAPI8_1_WallGeometryStatistics.Abstractions;

namespace TaskAPI8_1_WallGeometryStatistics.Services
{
    public class WallSelectionService : IWallSelectionService
    {
        private readonly ExternalCommandData _commandData;

        public WallSelectionService(ExternalCommandData commandData) 
        {
            this._commandData = commandData;
        }
        public Wall PickWall()
        {
            try
            {
                Reference reference = _commandData.Application.ActiveUIDocument.Selection.PickObject(ObjectType.Element, new OpeningSelectionFilter());
                Wall wall = _commandData.Application.ActiveUIDocument.Document.GetElement(reference) as Wall;
                return wall;
            }
            catch
            {
                return null;
            }
        }
    }
}
