using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using TallerDIA.Models;
using TallerDIA.ViewModels;
using TallerDIA.Views.Dialogs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace TallerDIA.ViewModels;

public partial class ClientesViewModel : ViewModelBase
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

    public  ObservableCollection<Cliente> Clientes
    {
        get => _Clientes;
        set
        {
            SetProperty(ref _Clientes, value);

        }
    }

    public ClientesViewModel(ObservableCollection<Cliente> clientes)
    {
        Clientes = clientes;
        ToDelete = false;
    }

    public ClientesViewModel()
    {
        ToDelete = false;
        Clientes = new ObservableCollection<Cliente>
        {
            new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com",IdCliente=1  },
            new Cliente { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com" ,IdCliente=2 },
            new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com",IdCliente=3  }
        };
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
                Clientes.Add(c);
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

}
