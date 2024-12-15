using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TallerDIA.Models;
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

        public event Action<ViewModelBase> ChangeViewRequested;


        [RelayCommand]
        public void TogglePaneCommand()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        [ObservableProperty]
        private ViewModelBase _currentPage = new HomeViewModel();

        public ObservableCollection<PaneListItemTemplate> PaneItems { get; private set; }
        [ObservableProperty]
        private PaneListItemTemplate _selectedPaneItem;

        partial void OnSelectedPaneItemChanged(PaneListItemTemplate value)
        {
            if (value is null) return;
            var instance = value.ViewModelFactory?.Invoke() ?? Activator.CreateInstance(value.ModelType);
            if (instance is null) return;
            CurrentPage = (ViewModelBase)instance;
        }

        public MainWindowViewModel()
        {

            /**
             * 
             * AQUI INICIALIZAR TODAS LAS LISTAS NECESARIAS PARA PODER ACCEDER A ELLAS
             * por ejemplo. al cargar de xml de clientes:
             * 
             *  SharedDB.Instance.LoadClientesFromXml(clientesFilePath);
             */
            SharedDB.Instance.LoadClientesFromXml();
            PaneItems = new ObservableCollection<PaneListItemTemplate>
            {
                new PaneListItemTemplate(typeof(HomeViewModel), "mdi-home"),
                //new PaneListItemTemplate(typeof(ClientesViewModel), "mdi-account-multiple", () => new ClientesViewModel(new CarteraClientes())),
                new PaneListItemTemplate(typeof(ClientesViewModel), "mdi-account-multiple", () => new ClientesViewModel()),
                new PaneListItemTemplate(typeof(EmpleadosViewModel), "mdi-account-hard-hat"),
                new PaneListItemTemplate(typeof(CochesViewModel), "mdi-car-back"),
                new PaneListItemTemplate(typeof(ReparacionesViewModel), "mdi-car-cog")
            };

            NavigationService.Instance.Initialize(this);
        }


        public void Initialize(params object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is string clienteId)
            {
            }
        }
    }
}

