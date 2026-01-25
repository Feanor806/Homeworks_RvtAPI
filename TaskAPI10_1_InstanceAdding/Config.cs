using Microsoft.Extensions.DependencyInjection;
using RxBim.Di;
using TaskAPI10_1_InstanceAdding.Abstractions;
using TaskAPI10_1_InstanceAdding.Services;
using TaskAPI10_1_InstanceAdding.ViewModels;
using TaskAPI10_1_InstanceAdding.Views;

namespace TaskAPI10_1_InstanceAdding
{
    internal class Config : ICommandConfiguration
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IPlacementService,PlacementService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow,MainWindow>();
        }
    }
}
