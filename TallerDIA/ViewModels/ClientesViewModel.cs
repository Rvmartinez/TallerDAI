using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks.Dataflow;
using TallerDIA.Models;
using TallerDIA.ViewModels;
using TallerDIA.Views.Dialogs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;
using TallerDIA.Utils;
using System.IO;
using Avalonia.Controls;
using Avalonia.Animation;
using DesgloseWindow = TallerDIA.Views.Dialogs.DesgloseWindow;
using ConfigChart = TallerDIA.Utils.ConfigChart;

namespace TallerDIA.ViewModels
{
    public partial class ClientesViewModel : ViewModelBase
    {
        private Cliente? _SelectedClient;

        public Cliente? SelectedClient
        {
            get => _SelectedClient;
            set
            {
                SetProperty(ref _SelectedClient, value);
            }
        }

        private String _FilteredText;
        public String FilteredText
        {
            get => _FilteredText;

            set { SetProperty(ref _FilteredText, value); Filtrar(); OnPropertyChanged("FilteredItems"); }
        }


        private CarteraClientes _carteraClientes;
        public CarteraClientes CarteraClientes
        {
            get
            {
                return _carteraClientes;
            }
            set
            {
                SetProperty(ref _carteraClientes, value);
            }
        }
        public ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["DNI", "ID Cliente", "Nombre", "Email"]);
        public ObservableCollection<String>? FilterModes
        {
            get => _FilterModes;
        }

        public ObservableCollection<Cliente>? FilteredItems
        {
            get;
        }

        private int _SelectedFilterMode;
        public int SelectedFilterMode
        {
            get => _SelectedFilterMode;
            set
            {
                SetProperty(ref _SelectedFilterMode, value); OnPropertyChanged("FilteredItems");
            }
        }


        public ClientesViewModel()
        {
            CarteraClientes = _carteraClientes = SharedDB.Instance.CarteraClientes;
            FilteredItems = new ObservableCollection<Cliente>(CarteraClientes.Clientes);
        }

        public ClientesViewModel(string dni)
        {
            CarteraClientes = SharedDB.Instance.CarteraClientes;

            FilteredItems = new ObservableCollection<Cliente>(CarteraClientes.Clientes);
            FilteredText = dni;
            Filtrar();

        }

        public void Initialize(params object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is string clienteId)
            {

                FilteredText = clienteId;
                Filtrar();
            }
        }
        private void Filtrar()
        {
            List<Cliente> aux = new List<Cliente>();
            if (FilteredText != null && FilteredText.Length > 0)
                switch (FilterModes[SelectedFilterMode])
                {
                    case "Nombre":
                        aux = CarteraClientes.Clientes.Where(c => c.Nombre.Contains(FilteredText)).ToList();
                        break;
                    case "DNI":
                        aux = CarteraClientes.Clientes.Where(c => c.DNI.Contains(FilteredText)).ToList();
                        break;
                    case "Email":
                        aux = CarteraClientes.Clientes.Where(c => c.Email.Contains(FilteredText)).ToList();
                        break;
                    case "ID Cliente":
                        aux = CarteraClientes.Clientes.Where(c => c.IdCliente.ToString().Contains(FilteredText)).ToList();
                        break;
                    default:
                        aux = CarteraClientes.Clientes.ToList();
                        break;
                }
            else
                aux = CarteraClientes.Clientes.ToList();

            FilteredItems.Clear();
            foreach (Cliente cliente in aux)
            {
                FilteredItems.Add(cliente);
            }
            OnPropertyChanged("FilteredItems");
        }



        [RelayCommand]
        public void GoToClientesView()
        {
            if (SelectedClient == null)
                return;
            NavigationService.Instance.NavigateTo<ReparacionesViewModel>(SelectedClient.DNI, nameof(ClientesViewModel)); // Pasa el ID del cliente
        }
        [RelayCommand]
        public async Task EditClientCommand()
        {
            if (SelectedClient == null) return;
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

            if (mainWindow == null) return;
            var ClienteDlg = new ClienteDlg(SelectedClient);
            await ClienteDlg.ShowDialog(mainWindow);

            if (!ClienteDlg.IsCancelled)
            {
                if (ClienteDlg.DniTB.Text == null || ClienteDlg.EmailTB.Text == null || ClienteDlg.NombreTB.Text == null) return;
                SharedDB.Instance.EditClient(SelectedClient, new Cliente { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = SelectedClient.IdCliente });
                int index = FilteredItems.IndexOf(SelectedClient);
                if (index >= 0)
                {
                    CarteraClientes = SharedDB.Instance.CarteraClientes; // Refresca CarteraClientes
                    Filtrar(); // Refresca la vista filtrada
                }
            }

            Filtrar();
        }

        [RelayCommand]
        public async Task OnDeleteCommand()
        {
            if (SelectedClient == null) return;
            var box = MessageBoxManager
               .GetMessageBoxStandard("Atención", "Los datos se borrarán irreversiblemente.¿Desea continuar?", ButtonEnum.OkCancel);

            var result = await box.ShowAsync();
            if (result == ButtonResult.Ok)
            {
                SharedDB.Instance.BajaCliente(SelectedClient);
                SelectedClient = null;
            }
            else
            {
                SelectedClient = null;
                return;
            }
            Filtrar();

        }

        [RelayCommand]
        public async Task AddClientCommand()
        {
            Window mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            ClienteDlg ClienteDlg = new ClienteDlg();
            if (mainWindow == null) return;
            await ClienteDlg.ShowDialog(mainWindow);

            if (!ClienteDlg.IsCancelled)
            {

                string dni = ClienteDlg.DniTB.Text;
                string email = ClienteDlg.EmailTB.Text;
                string nombre = ClienteDlg.NombreTB.Text;

                if (!SharedDB.Instance.AddClient(
                    new Cliente() { DNI = dni, Email = email, Nombre = nombre, IdCliente = 0 }))
                {
                    var box = MessageBoxManager
                                .GetMessageBoxStandard("Atención", "Ya existe un cliente con ese DNI o Email", ButtonEnum.Ok).ShowAsync();
                }
                Filtrar();
            }

        }

        public async Task ButtonAbrirGrafica()
        {
            if (SharedDB.Instance.Reparaciones.Count != 0 && SharedDB.Instance.CarteraClientes.Count != 0)
            {
                if (SelectedClient != null)
                {
                    var mainWindow =
                        Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                            ? desktop.MainWindow
                            : null;
                    if (mainWindow == null) return;
                    var reps = SharedDB.Instance.Reparaciones;
                    ;
                    var reparacionNavegarDlg = new DesgloseWindow(reps,
                        new ConfigChart()
                        {
                            Modo = ConfigChart.ModoVision.Mensual,
                            Cliente = SelectedClient.Nombre,
                            FechaFin = false
                        });
                    await reparacionNavegarDlg.ShowDialog(mainWindow);
                }
                else
                {
                    var mainWindow =
                        Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                            ? desktop.MainWindow
                            : null;
                    if (mainWindow == null) return;
                    var reps = SharedDB.Instance.Reparaciones;
                    ;
                    var reparacionNavegarDlg = new DesgloseWindow(reps,
                        new ConfigChart() { Modo = ConfigChart.ModoVision.Anual, FechaFin = false });
                    await reparacionNavegarDlg.ShowDialog(mainWindow);
                }
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("No hay reparaciones y/o clientes",
                    "No hay reparaciones y/o clientes que mostrar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning,
                    WindowStartupLocation.CenterOwner);

                var respuesta = await message.ShowAsync();
            }
        }
    }
}