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

    private bool _ToDelete;
    public bool ToDelete
    {
        get => _ToDelete;
        set
        {
            SetProperty(ref _ToDelete, value);
        }
    }

    [RelayCommand]
    public async Task EditClientCommand()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

        var ClienteDlg = new ClienteDlg(SelectedClient);
        await ClienteDlg.ShowDialog(mainWindow);

        if (!ClienteDlg.IsCancelled)
        {
            Cliente toupdate = Clientes.Where(c => c.IdCliente == SelectedClient.IdCliente).FirstOrDefault();
            toupdate.DNI = ClienteDlg.DniTB.Text;
            toupdate.Nombre = ClienteDlg.NombreTB.Text;
            toupdate.Email = ClienteDlg.EmailTB.Text;

            SelectedClient = null;
            ForceUpdateUI();

        }
    }

    private ObservableCollection<Cliente> _Clientes;
    public ObservableCollection<Cliente> Clientes
    {
        get
        {
            return _Clientes;
        }
        set
        {
            SetProperty(ref _Clientes, value);

        }
    }

    public ClientesViewModel(ObservableCollection<Cliente> clientes)
    {
        Clientes = clientes;
    }

    [RelayCommand]
    public async Task OnDeleteCommand()
    {
        var box = MessageBoxManager
           .GetMessageBoxStandard("Atención", "Los datos se borrarán irreversiblemente.¿Desea continuar?", ButtonEnum.OkCancel);

        var result = await box.ShowAsync();
        if (result == ButtonResult.Ok)
        {

            BajaCliente(SelectedClient);
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
            Cliente c  = new Cliente() { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = this.GetLastClientId()+1 };
            if (CanAddCliente(c))
            {
                List<Cliente> temp = Clientes.ToList();
                Clientes.Clear();

                temp.Add(c);
                Clientes = new ObservableCollection<Cliente>(temp);
                OnPropertyChanged(nameof(Clientes));

            }
        }
        
    }

    private bool CanAddCliente(Cliente c )
    {
        return !Clientes.Contains(c);
    }

    [RelayCommand]
    public void BajaCliente(Cliente cliente)
    {
        Clientes.Remove(cliente);
    }

    public void BajaClienteByDNI(string dni)
    {
        Clientes.Remove(ConsultaClienteByDni(dni));
    }

    public Cliente ConsultaCliente(int clienteId) => Clientes.FirstOrDefault(c => c.IdCliente == clienteId);
    public Cliente ConsultaClienteByDni(string dni) => Clientes.FirstOrDefault(c => c.DNI.Trim().ToUpper() == dni.Trim().ToUpper());
    
    private int GetLastClientId()
    {
        return Clientes.OrderByDescending(c => c.IdCliente).FirstOrDefault().IdCliente;
    }
    

    private void ForceUpdateUI()
    {
        List<Cliente> lista = Clientes.ToList();
        Clientes.Clear();
        Clientes = new ObservableCollection<Cliente>(lista);
        
    }

    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Nombre","DNI","Email","ID Cliente"]);

    public override ObservableCollection<Cliente> FilteredItems
    {
        get
        {
            if (FilterText != "")
            {

                switch (FilterModes[SelectedFilterMode])
                {
                    case "Nombre":
                        return new ObservableCollection<Cliente>(Clientes.Where(c => c.Nombre.Contains(FilterText)));
                    case "DNI":
                        return new ObservableCollection<Cliente>(Clientes.Where(c => c.DNI.Contains(FilterText)));
                    case "Email":
                        return new ObservableCollection<Cliente>(Clientes.Where(c => c.Email.Contains(FilterText)));
                    case "ID Cliente":
                        return new ObservableCollection<Cliente>(Clientes.Where(c => c.IdCliente.ToString().Contains(FilterText)));
                    default:
                        // maybe this should be an exception or unreachable
                        return Clientes;
                }
            }
            else
            {
                return Clientes;
            }
        }
    }
}
