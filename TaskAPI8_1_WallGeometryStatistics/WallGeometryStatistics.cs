using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI8_1_WallGeometryStatistics.Views;

namespace TaskAPI8_1_WallGeometryStatistics
{
    [Transaction(TransactionMode.Manual)]
    public class WallGeometryStatistics : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            return Result.Succeeded;
        }
    }
}
