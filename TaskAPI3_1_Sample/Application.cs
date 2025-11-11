using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            try
            {
                application.CreateRibbonTab("Общее");
            }
            catch { }

            RibbonPanel panel = application.GetRibbonPanels("Общее").FirstOrDefault(rp => rp.Name == "ПИК-Привет") ?? null;

            if (panel == null)
            {
                panel = application.CreateRibbonPanel("Общее", "ПИК-Привет");
            }
            PushButton testRooms = panel.AddItem(new PushButtonData("testRooms", "Помещения", assemblyPath, "WindowOrientationPlugIn.WindowOrientation")) as PushButton;
            testRooms.ToolTip = "Плагин для определения ориентации окон по сторонам горизонта\n\n Версия: v0.5. Автор: Михаил Солнцев";
            testRooms.LongDescription = "Плагин выполняет анализ ориентации и обмер элементов категорий \"Окна\", \"Двери\" и витражи, которые граничат с наружным воздухом.\n" +
                                        "Ориентация определяется в зависимости от положения элемента относительно условного и истенного севера модели\n" +
                                        "Также плагин формирует файл с заданием для передачи отделу ЭЭ";
            testRooms.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, "https://disk.yandex.ru/i/Ty4_njHOqaSD4Q"));
            testRooms.LargeImage = new BitmapImage(new Uri(Path.GetDirectoryName(assemblyPath) + @"\32x32_WO_pict.png"));
            return Result.Succeeded;
        }
    }
}
