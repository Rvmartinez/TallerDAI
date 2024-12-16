using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using TallerDIA.Models;
using TallerDIA.Views.Dialogs;
using Avalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TallerDIA.Utils;
using System.Threading.Tasks;
namespace TallerDIA.ViewModels;

public partial class CochesViewModel : FilterViewModel<Coche>
{
    private GarajeCoches _garaje = SharedDB.Instance.Garaje;
    public ObservableCollection<Coche> Coches => _garaje.Coches;
    

    private Coche _SelectedCar;
    public Coche SelectedCar
    {
        get => _SelectedCar;
        set
        {
            SetProperty(ref _SelectedCar, value);
        }
    }

    public void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is string clienteId)
        {
            FilteredItems.Where(c => c.Owner.IdCliente == 1);
        }
    }

    public CochesViewModel()
    {
        
    }

    public CochesViewModel(IEnumerable<Coche> coches)
    {
        _garaje.AddRange(coches);
    }


    [RelayCommand]
    public async Task BorrarCocheCommand()
    {
        if(SelectedCar == null) { return; }
        
        var box = MessageBoxManager
            .GetMessageBoxStandard("Atención", "Los datos se borrarán irreversiblemente.¿Desea continuar?", ButtonEnum.OkCancel);

        var result = await box.ShowAsync();
        if (result == ButtonResult.Ok)
        {

            this.RemoveCoche(SelectedCar.Matricula);
            SelectedCar = null;

        }
        else
        {

            SelectedCar = null;
            return;
        }
    }

    [RelayCommand]
    public async Task SubirCocheCommand()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        var cocheDlg = new CocheDlg();
        if (mainWindow == null) return;
        await cocheDlg.ShowDialog(mainWindow);

        if (!cocheDlg.IsCanceled)
        {
            Cliente c = SharedDB.Instance.ConsultaClienteByDni(cocheDlg.ClientesCb.SelectedItem.ToString());
            Coche.Marcas marcaConcreta = Enum.Parse<Coche.Marcas>(cocheDlg.MarcasCb.SelectedItem.ToString());
            Coche car = new Coche(cocheDlg.MatriculaTb.Text, marcaConcreta, cocheDlg.ModeloTb.Text, c);
            this.AddCoche(car);
        }
    }

    [RelayCommand]
    public async Task EditarCocheCommand()
    {
        if(SelectedCar == null) { return; }
        
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
                if (mainWindow == null) return;
        var cocheDlg = new CocheDlg(SelectedCar);
        cocheDlg.MarcasCb.IsEnabled = false;
        cocheDlg.ModeloTb.IsEnabled = false;
        await cocheDlg.ShowDialog(mainWindow);

        Coche antiguo = new Coche(SelectedCar.Matricula, SelectedCar.Marca, SelectedCar.Modelo, SelectedCar.Owner);

        if (!cocheDlg.IsCanceled)
        {
            Coche nuevo = new Coche(cocheDlg.MatriculaTb.Text, antiguo.Marca, antiguo.Modelo, antiguo.Owner);
            this.EditCoche(antiguo,nuevo);
        }
    }

    [RelayCommand]
    public void MostrarClienteCommand()
    {
        if (SelectedCar == null) { return; }

        NavigationService.Instance.NavigateTo<ClientesViewModel>(SelectedCar.Owner.DNI);

        //CochesClientes(SelectedCar.Owner);
    }

    public async void CochesClientes(Cliente cli)
    {
       var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
                if (mainWindow == null) return;
       var ClienteDlg = new ClienteDlg(SelectedCar.Owner);
        await ClienteDlg.ShowDialog(mainWindow);
       
       if (!ClienteDlg.IsCancelled)
       {
            Cliente antiguo = new Cliente
            {
                DNI = cli.DNI,
                Email = cli.DNI,
                Nombre = cli.Nombre,
                IdCliente = 0
            };
            Cliente nuevo = new Cliente
            {
                DNI = ClienteDlg.DniTB.Text,
                Email = ClienteDlg.EmailTB.Text,
                Nombre = ClienteDlg.NombreTB.Text,
                IdCliente = 0
            };
           SharedDB.Instance.EditClient(SelectedCar.Owner, nuevo);
           foreach (var car in _garaje.Coches)
           {
               Console.WriteLine("***");
               Console.WriteLine(car.Owner.DNI +" == "+ antiguo.DNI);
               if (car.Owner.DNI == antiguo.DNI)
               {
                   car.Owner = nuevo;
               }
           }
           SelectedCar.Owner = nuevo;
       }

    }

    public void AddCoche(Coche coche)
    {
        _garaje.Add(coche);
    }

    public void RemoveCoche(string matricula)
    {
        _garaje.RemoveMatricula(matricula);
    }

    public void EditCoche(Coche antiguo, Coche nuevo)
    {
        _garaje.RemoveMatricula(antiguo.Matricula);
        _garaje.Add(nuevo);
        
    }

    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Matricula","Marca","Modelo"]);

    public override ObservableCollection<Coche> FilteredItems
    {
        get
        {
            var text = FilterText.ToLower();
            if (FilterText != "")
            {
                switch (FilterModes[SelectedFilterMode])
                {
                    case "Matricula":
                        return new ObservableCollection<Coche>(Coches.Where(c => c.Matricula.ToLower().Contains(text)));
                    case "Marca":
                        return new ObservableCollection<Coche>(Coches.Where(c => c.Marca.ToString().ToLower().Contains(text)));
                    case "Modelo":
                        return new ObservableCollection<Coche>(Coches.Where(c => c.Modelo.ToLower().Contains(text)));
                    default:
                        return Coches;
                }
            }
            else
            {
                return Coches;
            }
        }
    }
}