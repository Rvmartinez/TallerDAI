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
            SharedDB.Instance.EditClient(SelectedClient, new Cliente { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = 0 });
            SelectedClient = null;
            ForceUpdateUI();
        }
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

            ForceUpdateUI();
        }

    }

    public async Task ButtonAbrirGrafica()
    { 
        if (SelectedClient != null)
        {
            /*var mainWindow =
                Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                    ? desktop.MainWindow
                    : null;
            var colRep = _Reparaciones.OfType<Reparacion>().ToList();
            var reps = new Reparaciones();
            reps.AnadirReparaciones(colRep);
            var reparacionNavegarDlg = new ChartWindow(reps, new ConfigChart() { FechaFin = false });
            await reparacionNavegarDlg.ShowDialog(mainWindow);*/
        }
        else
        {
            var message = MessageBoxManager.GetMessageBoxStandard("No hay un cliente seleccionado",
                "No hay un cliente seleccionado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning,
                WindowStartupLocation.CenterOwner);

            var respuesta = await message.ShowAsync();
        }
    }





    [RelayCommand]
    public void ForceUpdateUI()
    {

        List<Cliente> list = SharedDB.Instance.CarteraClientes.Clientes.ToList();
        CarteraClientes.Clear();

        foreach (Cliente cliente in list)
        {
            CarteraClientes.Add(cliente);
        }
        OnPropertyChanged(nameof(CarteraClientes));


    }

    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Nombre", "DNI", "Email", "ID Cliente"]);

    public override ObservableCollection<Cliente> FilteredItems
    {
        get
        {
            var text = FilterText.ToLower();
            if (FilterText != "")
            {
                switch (FilterModes[SelectedFilterMode])
                {
                    case "Nombre":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.Nombre.ToLower().Contains(text)));
                    case "DNI":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.DNI.ToLower().Contains(text)));
                    case "Email":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.Email.ToLower().Contains(text)));
                    case "ID Cliente":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.IdCliente.ToString().ToLower().Contains(text)));
                    default:
                        // maybe this should be an exception or unreachable
                        return CarteraClientes.Clientes;
                }
            }
            else
            {
                return CarteraClientes.Clientes;
            }
        }
    }
}
