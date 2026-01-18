using Autodesk.Revit.DB;

namespace TaskAPI8_1_WallGeometryStatistics.Abstractions
{
    public interface IWallSelectionService
    {
        Wall PickWall();
    }
}
