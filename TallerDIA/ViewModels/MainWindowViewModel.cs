using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TallerDIA.Utils;
using TallerDIA.ViewModels;

namespace TallerDIA.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private bool _IsPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _IsPaneOpen;
            set
            {
                _IsPaneOpen = value;
                OnPropertyChanged(nameof(IsPaneOpen));
            }
        }

        [RelayCommand]
        public void TogglePaneCommand()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        [ObservableProperty]
        private ViewModelBase _currentPage = new HomeViewModel();

        public ObservableCollection<PaneListItemTemplate> PaneItems { get; } =
        [
            new PaneListItemTemplate(typeof(HomeViewModel),"mdi-home"),
            new PaneListItemTemplate(typeof(ClientesViewModel),"mdi-account-multiple"),
            new PaneListItemTemplate(typeof(EmpleadosViewModel),"mdi-account-hard-hat"),
            new PaneListItemTemplate(typeof(CochesViewModel),"mdi-car-back")
        ];

        [ObservableProperty]
        private PaneListItemTemplate _selectedPaneItem;

        partial void OnSelectedPaneItemChanged(PaneListItemTemplate value)
        {
            if (value is null) return;
            var instance = Activator.CreateInstance(value.ModelType);
            if(instance is null ) return;
            CurrentPage = (ViewModelBase)instance;
        }

    }
}
