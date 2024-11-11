using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TallerDIA.Models;
using TallerDIA.ViewModels;
using TallerDIA.Views.Dialogs;

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
            //OnSelectedChanged();
        }
    }

    private async void OnSelectedChanged(Cliente value)
    {
       /* var ClienteDlg = new ClienteDlg();
        await ClienteDlg.ShowDialog(this);

        if (!ClienteDlg.IsCancelled)
        {
            AddCliente(new Cliente() { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = 1 });
        }*/
    }

    private ObservableCollection<Cliente> _Clientes;

    public ObservableCollection<Cliente> Clientes
    {
        get => _Clientes;
        set
        {
            _Clientes = value;
            OnPropertyChanged("Clientes");
        }
    }

    public ClientesViewModel(ObservableCollection<Cliente> clientes)
    {
        Clientes = clientes;
    }

    public ClientesViewModel()
    {
        Clientes = new ObservableCollection<Cliente>
        {
            new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com",IdCliente=1  },
            new Cliente { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com" ,IdCliente=2 },
            new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com",IdCliente=3  }
        };
    }

    [RelayCommand]
    public void AddCliente(Cliente cliente)
    {

        Clientes.Add(cliente);
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
    


}
