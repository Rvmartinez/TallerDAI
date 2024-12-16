using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerDIA.ViewModels;

namespace TallerDIA.Utils
{
    public class NavigationService
    {
        private MainWindowViewModel _mainWindowViewModel;

        private static NavigationService _instance;

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NavigationService();
                }
                return _instance;
            }
        }

        public void Initialize(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public void NavigateTo<TViewModel>(params object[] parameters) where TViewModel : ViewModelBase
        {
            if (_mainWindowViewModel is null)
                throw new InvalidOperationException("NavigationService not initialized.");

            var paneItem = _mainWindowViewModel.PaneItems
                .FirstOrDefault(item => item.ModelType == typeof(TViewModel));

            if (paneItem != null)
            {
                    var viewModelFactory = paneItem.ViewModelFactory;

                    if (viewModelFactory != null)
                    {
                        // Crear instancia utilizando la fábrica de ViewModel
                        var instance = viewModelFactory.Invoke();
                        InitializeViewModel(instance, parameters);
                        _mainWindowViewModel.SelectedPaneItem = paneItem;
                    }
                    else
                    {
                        // Crear instancia directamente
                        var instance = (TViewModel)Activator.CreateInstance(typeof(TViewModel), parameters);
                        InitializeViewModel(instance, parameters);
                        _mainWindowViewModel.CurrentPage = instance;
                    }
            }
        }


        private void InitializeViewModel(object viewModel, object[] parameters)
        {
            switch(viewModel)
            {
                case ClientesViewModel:
                    ClientesViewModel vm = (ClientesViewModel)viewModel;
                    vm.Initialize(parameters);
                    break;
                case ReparacionesViewModel:
                    ReparacionesViewModel rep = (ReparacionesViewModel)viewModel;
                    rep.Initialize(parameters);
                    break;
                case CochesViewModel:

                    CochesViewModel coches = (CochesViewModel)viewModel;
                    coches.Initialize(parameters);
                    break;
                case EmpleadosViewModel:

                    EmpleadosViewModel empleados = (EmpleadosViewModel)viewModel;
                    empleados.Initialize(parameters);
                    break;
                default:
                    MainWindowViewModel mwvm = (MainWindowViewModel)viewModel;
                    mwvm.Initialize(parameters);
                    break;
            }
            

        }
    }
}
