using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskAPI8_1_WallGeometryStatistics.Abstractions;
using TaskAPI8_1_WallGeometryStatistics.Models;
using TaskAPI8_1_WallGeometryStatistics.Services;

namespace TaskAPI8_1_WallGeometryStatistics.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly ExternalCommandData _commandData;
        public MainWindowViewModel(/*ExternalCommandData commandData*/)
        {
            //_commandData = commandData;
            AskWall = new RelayCommand(OnAskWallExecute);
            Limit = 200;
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
        private double _limit;
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
            //SelectionService selectionService = new SelectionService(_commandData);
            //Wall wall = selectionService.PickWall();
            //if (wall != null)
            //{
            //    GeometryService geometryService = new GeometryService();
            //    AWall = geometryService.GetWallInfo(wall, Limit);

            //    // Обновляем статус
            //    Statusmessage = AWall.Status
            //        ? "Норма"
            //        : "Превышение";
            //}
            //else
            //{
            //    // Если пользователь отменил выбор
            //    AWall = null;
            //    Statusmessage = "Стена не выбрана";
            //}
        }
    }
}
