using Autodesk.Revit.DB;
using System;
using TaskAPI8_1_WallGeometryStatistics.Abstractions;
using TaskAPI8_1_WallGeometryStatistics.Models;

namespace TaskAPI8_1_WallGeometryStatistics.Services
{
    public class WallGeometryService : IWallGeometryService
    {
        public AWall GetWallInfo(Wall wall, double limit)
        {
            if (wall == null) return null;
            Document doc = wall.Document;
            WallType wallType = wall.WallType;

            double thickness = Math.Round(UnitUtils.ConvertFromInternalUnits(wall.Width, DisplayUnitType.DUT_MILLIMETERS),0);

            double height = 0.0;

            BoundingBoxXYZ wallBox = wall.get_BoundingBox(null);

            if(wallBox == null) 
            {
                height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
            }
            else
            {
                height = wallBox.Max.Z - wallBox.Min.Z;
            }  

            double length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();

            double area = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

            double volume = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble();

            return new AWall()
            {
                WallName = wall.Name,
                WallType = wallType.Name,
                Length = Math.Round(UnitUtils.ConvertFromInternalUnits(length,DisplayUnitType.DUT_MILLIMETERS),0),
                Height = Math.Round(UnitUtils.ConvertFromInternalUnits(height, DisplayUnitType.DUT_MILLIMETERS),0),
                Thickness = thickness,
                Volume = Math.Round(UnitUtils.ConvertFromInternalUnits(volume, DisplayUnitType.DUT_CUBIC_METERS),2),
                Area = Math.Round(UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS),2),
                Status = !(thickness > limit)
            };
        }
    }
}
