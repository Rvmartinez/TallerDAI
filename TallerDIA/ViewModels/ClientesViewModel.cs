using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Xml;
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
    
    public string ImportPath { get; set; }

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
        CarteraClientes = new CarteraClientes(SharedDB.Instance.CarteraClientes.Clientes);
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
       //OnPropertyChanged(nameof(FilteredItems));


    }

    public async Task GuardarCarteraCommand()
    {
        ClienteXML.GuardarCartera(_carteraClientes);
    }

    public async Task ImportarClientesCommand()
    {
        Console.WriteLine("Importando Clientes...");
        Console.WriteLine(ImportPath);

        if (File.Exists(ImportPath))
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ImportPath);
                Console.WriteLine("Archivo XML cargado exitosamente.");

                // Mostrar contenido del archivo XML (opcional)
                Console.WriteLine(xmlDoc.OuterXml);
                XmlNodeList clientes = xmlDoc.GetElementsByTagName("Cliente");

                foreach (XmlElement cliente in clientes)
                {
                    _carteraClientes.Add(ClienteXML.CargarDeXml(cliente));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el archivo XML: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("El archivo no existe. Verifique la ruta.");
        }
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
