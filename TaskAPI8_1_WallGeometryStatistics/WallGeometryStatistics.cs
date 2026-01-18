using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using TaskAPI8_1_WallGeometryStatistics.Abstractions;
using TaskAPI8_1_WallGeometryStatistics.Services;
using TaskAPI8_1_WallGeometryStatistics.ViewModels;
using TaskAPI8_1_WallGeometryStatistics.Views;

namespace TaskAPI8_1_WallGeometryStatistics
{
    [Transaction(TransactionMode.Manual)]
    public class WallGeometryStatistics : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ExternalCommandData>(commandData);
            services.AddSingleton<IWallSelectionService, WallSelectionService>();
            services.AddSingleton<IWallGeometryService, WallGeometryService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            IServiceProvider provider = services.BuildServiceProvider();

            MainWindow mainWindow = provider.GetRequiredService<MainWindow>();

            mainWindow.Show();
            return Result.Succeeded;
        }
    }
}
