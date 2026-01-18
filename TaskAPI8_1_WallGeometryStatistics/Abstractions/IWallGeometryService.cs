using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI8_1_WallGeometryStatistics.Models;

namespace TaskAPI8_1_WallGeometryStatistics.Abstractions
{
    public interface IWallGeometryService
    {
        AWall GetWallInfo(Wall wall, double limit);
    }
}
