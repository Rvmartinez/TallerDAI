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
using DesgloseWindow = TallerDIA.Views.DesgloseWindow;
using ConfigChart = TallerDIA.Views.ConfigChart;

namespace TallerDIA.ViewModels;

public partial class ClientesViewModel : FilterViewModel<Cliente>
{
    private Cliente _SelectedClient;

    public Cliente SelectedClient
    {
        get => _SelectedClient;
        set
        {
            SetProperty(ref _SelectedClient, value);
        }
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

    public ClientesViewModel()
    {
        CarteraClientes = SharedDB.Instance.CarteraClientes;
        FilteredItems = new ObservableCollection<Cliente>(CarteraClientes.Clientes);
    }

    public ClientesViewModel(string id)
    {
        int i = int.Parse(id);
        CarteraClientes = SharedDB.Instance.CarteraClientes;
    }

    public void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is string clienteId)
        {

            FilteredText = clienteId;
            //Filtrar();
            //SelectedClient = FilteredItems[0];
        }
    }


    [RelayCommand]
    public void GoToClientesView()
    {
        NavigationService.Instance.NavigateTo<ClientesViewModel>("12345"); // Pasa el ID del cliente
    }
    [RelayCommand]
    public async Task EditClientCommand()
    {
        if (SelectedClient == null) return;
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

        var ClienteDlg = new ClienteDlg(SelectedClient);
        await ClienteDlg.ShowDialog(mainWindow);

        if (!ClienteDlg.IsCancelled)
        {

            SharedDB.Instance.EditClient(SelectedClient, new Cliente { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = SelectedClient.IdCliente});
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
        Window  mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        ClienteDlg ClienteDlg = new ClienteDlg();
        await ClienteDlg.ShowDialog(mainWindow);

        if (!ClienteDlg.IsCancelled)
        {

            string dni = ClienteDlg.DniTB.Text;
            string email = ClienteDlg.EmailTB.Text;
            string nombre = ClienteDlg.NombreTB.Text;

            if(!SharedDB.Instance.AddClient(
                new Cliente() { DNI = dni, Email = email, Nombre = nombre, IdCliente = 0 }))
            {
                var box = MessageBoxManager
                            .GetMessageBoxStandard("Atención", "Ya existe un cliente con ese DNI o Email", ButtonEnum.Ok);
                
            }
            Filtrar();
        }

    }

    public async Task ButtonAbrirGrafica()
    { 
            var mainWindow =
                Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                    ? desktop.MainWindow
                    : null;
            var reps = SharedDB.Instance.Reparaciones;
            ;
            var reparacionNavegarDlg = new DesgloseWindow(reps, new ConfigChart() {Modo = ConfigChart.ModoVision.Anual, FechaFin = false });
            await reparacionNavegarDlg.ShowDialog(mainWindow);
        }
    }

    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Nombre", "DNI", "Email", "ID Cliente"]);

    public override ObservableCollection<Cliente> FilteredItems
    {
        get;
    }

    private void Filtrar()
    {
        List<Cliente> aux = new List<Cliente>();
        if(FilteredText != null && FilteredText.Length > 0)
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
                    // maybe this should be an exception or unreachable
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

    private String _FilteredText;
    public String FilteredText
    {
        get => _FilteredText;

        set { SetProperty(ref _FilteredText, value); Filtrar(); OnPropertyChanged("FilteredItems"); }
    }
}
