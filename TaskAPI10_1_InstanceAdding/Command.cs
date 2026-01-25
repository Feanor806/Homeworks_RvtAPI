using Autodesk.Revit.Attributes;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Command.Revit;
using RxBim.Shared;
using System;
using TaskAPI10_1_InstanceAdding.Views;

namespace TaskAPI10_1_InstanceAdding
{
    [Transaction(TransactionMode.Manual)]
    public class Command : RxBimCommand
    {
        public PluginResult ExecuteCommand(IServiceProvider provider)
        {
            var mainWindow = provider.GetService<MainWindow>();
            mainWindow.ShowDialog();
            return PluginResult.Succeeded;
        }
    }
}
