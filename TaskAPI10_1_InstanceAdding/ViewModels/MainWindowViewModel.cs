using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskAPI10_1_InstanceAdding.Abstractions;
using TaskAPI10_1_InstanceAdding.Models;
using Autodesk.Revit.UI;

namespace TaskAPI10_1_InstanceAdding.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private FurnitureType _seletedFurnitureType;
        private int _count;
        private string _statusMessage;
        private readonly IPlacementService _placementService;

        public MainWindowViewModel(IPlacementService placementService)
        {
            PlaceCommand = new RelayCommand(PlaceFurniture);
            FurnitureTypes = new ObservableCollection<FurnitureType> 
            {
                FurnitureType.Table,
                FurnitureType.Chair,
                FurnitureType.Cabinet
            };
            _placementService = placementService;
        }

        private void PlaceFurniture()
        {
           CSharpFunctionalExtensions.Result result = _placementService.Place(SelectedFurnitureType, Count);
            if (result.IsSuccess)
            {
                StatusMessage = $"Размещено {Count} единиц мебели.";
                TaskDialog.Show("Размещение мебели", StatusMessage);
            }
            else
            {
                StatusMessage = $"Ошибка {result.Error}";
                TaskDialog.Show("Размещение мебели", StatusMessage);
            }
        }

        public ObservableCollection<FurnitureType> FurnitureTypes { get; }

        public FurnitureType SelectedFurnitureType 
        { 
            get => _seletedFurnitureType; 
            set => SetProperty(ref _seletedFurnitureType, value); 
        }

        public int Count 
        { 
            get => _count; 
            set => SetProperty(ref _count, value); 
        }

        public string StatusMessage 
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand PlaceCommand { get; }
    }
}
