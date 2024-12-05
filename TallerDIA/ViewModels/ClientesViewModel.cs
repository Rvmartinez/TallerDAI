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
        CarteraClientes = new CarteraClientes(SharedDB.Instance.Clientes);
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
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        var ClienteDlg = new ClienteDlg();
        await ClienteDlg.ShowDialog(mainWindow);

        if (!ClienteDlg.IsCancelled)
        {

            string dni = ClienteDlg.DniTB.Text;
            string email = ClienteDlg.EmailTB.Text;
            string nombre = ClienteDlg.NombreTB.Text;
            SharedDB.Instance.AddClient(
                new Cliente() { DNI = dni, Email = email, Nombre = nombre, IdCliente = 0 });

            ForceUpdateUI();
        }

    }



    [RelayCommand]
    public void ForceUpdateUI()
    {

        List<Cliente> list = SharedDB.Instance.Clientes.ToList();
        foreach (Cliente cliente in list)
        {
            CarteraClientes.Add(cliente);
        }
        OnPropertyChanged(nameof(CarteraClientes));
        OnPropertyChanged(nameof(FilteredItems));


    }

    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Nombre", "DNI", "Email", "ID Cliente"]);

    public override ObservableCollection<Cliente> FilteredItems
    {
        get
        {
            if (FilterText != "")
            {

                switch (FilterModes[SelectedFilterMode])
                {
                    case "Nombre":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.Nombre.Contains(FilterText)));
                    case "DNI":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.DNI.Contains(FilterText)));
                    case "Email":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.Email.Contains(FilterText)));
                    case "ID Cliente":
                        return new ObservableCollection<Cliente>(CarteraClientes.Clientes.Where(c => c.IdCliente.ToString().Contains(FilterText)));
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
