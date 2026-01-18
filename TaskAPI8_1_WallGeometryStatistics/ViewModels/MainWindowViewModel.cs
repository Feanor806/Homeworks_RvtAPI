using Autodesk.Revit.DB;
using System.Windows.Input;
using TaskAPI8_1_WallGeometryStatistics.Abstractions;
using TaskAPI8_1_WallGeometryStatistics.Models;
using TaskAPI8_1_WallGeometryStatistics.Services;

namespace TaskAPI8_1_WallGeometryStatistics.ViewModels
{
    /// <summary>
    /// Модель представления основного окна
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        /// <summary>
        /// Конструктор модели представления основного окна <see cref="MainWindowViewModel"/>
        /// </summary>
        /// <param name="selectionService">Сервис для работы с выбором стен в приложении. Предоставляет функциональность для выбора, фильтрации и управления стенами.</param>
        /// <param name="geometryService">Сервис для работы с геометрией стен. Предоставляет методы для расчета геометрических параметров, преобразований и анализа стен.</param>
        public MainWindowViewModel(IWallSelectionService selectionService, IWallGeometryService geometryService)
        {
            AskWall = new RelayCommand(OnAskWallExecute);
            this._selectionService = selectionService;
            this._geometryService = geometryService;
        }

        private AWall _aWall;
        public AWall AWall
        {
            get => _aWall;
            set
            {
                _aWall = value;
                OnPropertyChanged();
            }
        }
        private string _statusmessage;
        public string Statusmessage
        {
            get { return _statusmessage; }
            set
            {
                _statusmessage = value;
                OnPropertyChanged();
            }
        }
        private double _limit = 200;
        private readonly IWallSelectionService _selectionService;
        private readonly IWallGeometryService _geometryService;

        public double Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                OnPropertyChanged();
            }
        }

        public ICommand AskWall { get; }

        private void OnAskWallExecute(object parameter)
        {
            Wall wall = _selectionService.PickWall();
            if (wall != null)
            {
                AWall = _geometryService.GetWallInfo(wall, Limit);

                // Обновляем статус
                Statusmessage = AWall.Status
                    ? "Норма"
                    : "Превышение";
            }
            else
            {
                // Если пользователь отменил выбор
                AWall = null;
                Statusmessage = "Стена не выбрана";
            }
        }
    }
}
