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

public partial class CochesViewModel : ViewModelBase
{
    private GarajeCoches _garaje = new GarajeCoches();
    public ObservableCollection<Coche> Coches => _garaje.Coches;

    private bool _ToDelete;
    public bool ToDelete
    {
        get => _ToDelete;
        set
        {
            SetProperty(ref _ToDelete, value);
        }
    }

    private Coche _SelectedCar;
    public Coche SelectedCar
    {
        get => _SelectedCar;
        set
        {
            SetProperty(ref _SelectedCar, value);
            OnSelectedChanged(value);
        }
    }

    public CochesViewModel()
    {
        _garaje.Add(new Coche("4089fks", Coche.Marcas.Citroen, "c3"));
        _garaje.Add(new Coche("1234trt", Coche.Marcas.Ferrari, "rojo"));
        _garaje.Add(new Coche("9876akd", Coche.Marcas.Lamborghini, "huracan"));
        ToDelete = false;
    }

    public CochesViewModel(IEnumerable<Coche> coches)
    {
        _garaje.AddRange(coches);
        ToDelete = false;
    }

    private async void OnSelectedChanged(Coche value)
    {
        if (value == null) return;

        if (ToDelete)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Atención", "Los datos se borrarán irreversiblemente.¿Desea continuar?",
                    ButtonEnum.OkCancel);

            var result = await box.ShowAsync();
            if (result == ButtonResult.Ok)
            {

                SelectedCar = null;
                BajaCoche(value);
            }
            else
            {

                SelectedCar = null;
                return;
            }
        }
        else
        {
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;

            var cocheDlg = new CocheDlg(value);
            cocheDlg.MarcasCb.IsEnabled = false;
            cocheDlg.ModeloTb.IsEnabled = false;
            await cocheDlg.ShowDialog(mainWindow);

            if (!cocheDlg.IsCanceled)
            {
                SelectedCar = null;
                _garaje.RemoveMatricula(value.Matricula);
                _garaje.Add(new Coche(cocheDlg.MatriculaTb.Text, value.Marca, value.Modelo));

            }

        }
    }

    [RelayCommand]
    public async void BorrarCocheCommand()
    {
        ToDelete = !ToDelete;
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


    private void BajaCoche(Coche value)
    {
        _garaje.RemoveMatricula(value.Matricula);
    }


}