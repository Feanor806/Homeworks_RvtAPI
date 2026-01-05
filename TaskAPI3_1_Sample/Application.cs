using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TaskAPI3_1_Sample
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = @"C:\Revit SDK 2019\Software Development Kit\Samples\ScheduleCreation\CS\bin\Debug\ScheduleCreation.dll";
            try
            {
                application.CreateRibbonTab("ПИК-Привет");
            }
            catch { }

            RibbonPanel panel = application.GetRibbonPanels("ПИК-Привет").FirstOrDefault(rp => rp.Name == "Общее") ?? null;

            if (panel == null)
            {
                panel = application.CreateRibbonPanel("ПИК-Привет", "Общее");
            }
            PushButton testRooms = panel.AddItem(new PushButtonData("sampleShedule", "Спецификация стен", assemblyPath, "Revit.SDK.Samples.ScheduleCreation.CS.Command")) as PushButton;

            return Result.Succeeded;
        }
    }
}
