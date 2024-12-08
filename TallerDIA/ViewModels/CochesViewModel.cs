using System;
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
using ColorTextBlock.Avalonia;
using TallerDIA.Utils;
using TallerDIA.Views;
namespace TallerDIA.ViewModels;

public partial class CochesViewModel : FilterViewModel<Coche>
{
    private GarajeCoches _garaje = new GarajeCoches();
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

    public CochesViewModel()
    {
        
        _garaje.AddRange(SharedDB.Instance.Garaje.Coches);
    }

    public CochesViewModel(IEnumerable<Coche> coches)
    {
        _garaje.AddRange(coches);
    }
    

    [RelayCommand]
    public async void BorrarCocheCommand()
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
    public async void SubirCocheCommand()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        var cocheDlg = new CocheDlg();
        await cocheDlg.ShowDialog(mainWindow);
        
        //-------------------------Faltaria ver como seleccionar cliente(lo mas facil seria un dialog)----------------------
        Cliente c = new Cliente
            { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 34 };
        
        if (!cocheDlg.IsCanceled)
        {
            Coche.Marcas marcaConcreta = Enum.Parse<Coche.Marcas>(cocheDlg.MarcasCb.SelectedItem.ToString());
            Coche car = new Coche(cocheDlg.MatriculaTb.Text, marcaConcreta, cocheDlg.ModeloTb.Text, c);
            this.AddCoche(car);
        }
    }

    [RelayCommand]
    public async void EditarCocheCommand()
    {
        if(SelectedCar == null) { return; }
        
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
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
        
        CochesClientes(SelectedCar.Owner);
    }

    public async void CochesClientes(Cliente cli)
    {
       var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        var cliDlg = new ClienteDlg(SelectedCar.Owner);
        await cliDlg.ShowDialog(mainWindow);

    }

    public void AddCoche(Coche coche)
    {
        _garaje.Add(coche);
        SharedDB.Instance.AddCar(coche);
        ToString();
    }

    public void RemoveCoche(string matricula)
    {
        _garaje.RemoveMatricula(matricula);
        SharedDB.Instance.RemoveCar(matricula);
        ToString();
    }

    public void EditCoche(Coche antiguo, Coche nuevo)
    {
        _garaje.RemoveMatricula(antiguo.Matricula);
        _garaje.Add(nuevo);
        
        SharedDB.Instance.RemoveCar(antiguo.Matricula);
        SharedDB.Instance.AddCar(nuevo);
        ToString();
    }

    public void ToString()
    {
        Console.WriteLine(SharedDB.Instance.Garaje);
    }

    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Matricula","Marca","Modelo"]);

    public override ObservableCollection<Coche> FilteredItems
    {
        get
        {
            if (FilterText != "")
            {

                switch (FilterModes[SelectedFilterMode])
                {
                    case "Matricula":
                        return new ObservableCollection<Coche>(Coches.Where(c => c.Matricula.Contains(FilterText)));
                    case "Marca":
                        return new ObservableCollection<Coche>(Coches.Where(c => c.Marca.ToString().Contains(FilterText)));
                    case "Modelo":
                        return new ObservableCollection<Coche>(Coches.Where(c => c.Modelo.Contains(FilterText)));
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