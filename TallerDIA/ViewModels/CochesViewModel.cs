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
        var c1 = new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
        var c2 = new Cliente { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com", IdCliente = 2 };
        var c3 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 3 };
        _garaje.Add(new Coche("4089fks", Coche.Marcas.Citroen, "c3",c1));
        _garaje.Add(new Coche("1234trt", Coche.Marcas.Ferrari, "rojo",c2));
        _garaje.Add(new Coche("9876akd", Coche.Marcas.Lamborghini, "huracan",c3));
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

            _garaje.RemoveMatricula(SelectedCar.Matricula);
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

        if (!cocheDlg.IsCanceled)
        {
            Coche.Marcas marcaConcreta = Enum.Parse<Coche.Marcas>(cocheDlg.MarcasCb.SelectedItem.ToString());
            Coche car = new Coche(cocheDlg.MatriculaTb.Text, marcaConcreta, cocheDlg.ModeloTb.Text);
            _garaje.Add(car);
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

        if (!cocheDlg.IsCanceled)
        {
            _garaje.RemoveMatricula(SelectedCar.Matricula);
            _garaje.Add(new Coche(cocheDlg.MatriculaTb.Text, SelectedCar.Marca, SelectedCar.Modelo));
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
       /*var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        var cliDlg = new ClienteDlg(SelectedCar.Owner);
        await cliDlg.ShowDialog(mainWindow);
       */

        
        
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