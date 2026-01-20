using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskAPI9_1_Sections.Abstractions;

namespace TaskAPI9_1_Sections.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly ISelectionService _selectionService;
        private readonly ISectionService _sectionService;
        private readonly RevitTask _revitTask;
        private string _sectionName = "Разрез";
        private double _widthOffsetMm = 100.0;
        private double _heightOffsetMm = 100.0;
        private double _depthOffsetMm = 100.0;

        public MainWindowViewModel(ISelectionService selectionService, ISectionService sectionService, RevitTask revitTask)
        {
            CreateSectionCommand = new AsyncRelayCommand(OnCreateSectionCommandExecute);
            _selectionService = selectionService;
            _sectionService = sectionService;
            _revitTask = revitTask;
        }

        public string SectionName 
        { 
            get => _sectionName;
            set => SetProperty(ref _sectionName, value); 
        }

        public double WidthOffsetMm 
        { 
            get => _widthOffsetMm; 
            set => SetProperty(ref _widthOffsetMm, value);
        }

        public double HeightOffsetMm 
        { 
            get => _heightOffsetMm; 
            set => SetProperty(ref _heightOffsetMm, value); 
        }

        public double DepthOffsetMm 
        { 
            get => _depthOffsetMm; 
            set => SetProperty(ref _depthOffsetMm, value); 
        }

        public AsyncRelayCommand CreateSectionCommand { get; }

        private async Task OnCreateSectionCommandExecute()
        {
            FamilyInstance familyInstance = _selectionService.PickObject();
            if (familyInstance == null)
            {
                return;
            }

            bool isCreated = await _revitTask.Run<bool>(app=>_sectionService.CreateSection(familyInstance, WidthOffsetMm, HeightOffsetMm, DepthOffsetMm, SectionName));

            if (!isCreated) 
            {
                TaskDialog.Show("Ошибка", "Что-то пошло не так");
            }
            else
            {
                TaskDialog.Show("Успех", "Разрез построен");
            }
        }
    }
}
