using System.Windows;
using TaskAPI8_1_WallGeometryStatistics.ViewModels;

namespace TaskAPI8_1_WallGeometryStatistics.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            this.DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}
