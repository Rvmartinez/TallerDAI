using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TallerDIA.Models;
using TallerDIA.Utils;
using TallerDIA.ViewModels;
using TallerDIA.Views.Dialogs;

namespace TallerDIA.ViewModels;
public partial class EmpleadosViewModel : FilterViewModel<Empleado>
{
    private Empleado _EmpleadoActual;
    public Empleado EmpleadoActual
    {
        get => _EmpleadoActual;
        set
        {
            SetProperty(ref _EmpleadoActual, value);
        }
    }
    
    private RegistroEmpleados _registroEmpleados;
    public RegistroEmpleados RegistroEmpleados
    {
        get
        {
            return _registroEmpleados;
        }
        set
        {
            SetProperty(ref _registroEmpleados, value);

        }
    }
    
    public EmpleadosViewModel()
    {
        RegistroEmpleados = SharedDB.Instance.RegistroEmpleados;
    } 
    public EmpleadosViewModel(string empleadoId)
    {
        //Todo hacer algo con el empleado
        RegistroEmpleados = SharedDB.Instance.RegistroEmpleados;
        btModificarEmpleado_OnClickCommand.Execute(null);
    }


    public void Initialize(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is string empleadoId)
        {
            FilteredItems.Where(c => c.Dni == empleadoId);
        }
    }


    private Empleado _EmpleadoSeleccionado;
    public Empleado EmpleadoSeleccionado
    {
        get => _EmpleadoSeleccionado;
        set
        {
            SetProperty(ref _EmpleadoSeleccionado, value);
            EmpleadoActual=_EmpleadoSeleccionado;
        }
    }

    private string  _Aviso;
    public string Aviso
    {
        get => _Aviso;
        set
        {
            //_Aviso = value;
            SetProperty(ref _Aviso, value);
        }
    }
    
    public void ForceUpdateUI()
    {
        List<Empleado> list = SharedDB.Instance.RegistroEmpleados.Empleados.ToList();
        RegistroEmpleados.Clear();
        foreach (Empleado empleado in list)
        {
            RegistroEmpleados.Add(empleado);
        }
        OnPropertyChanged(nameof(FilteredItems));
    }
    
    [RelayCommand]
    public async Task btAnadirEmpleado_OnClick()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        var EmpleadoDlg = new EmpleadoDlg();
        await EmpleadoDlg.ShowDialog(mainWindow);
        if (!EmpleadoDlg.IsCancelled)
        {
            Console.WriteLine("Intentando insertar...");
            Empleado nuevoEmpleado  = new Empleado() { Dni = EmpleadoDlg.DniTB.Text, Email = EmpleadoDlg.EmailTB.Text, Nombre = EmpleadoDlg.NombreTB.Text};
            if (SharedDB.FiltrarEntradasEmpleado(nuevoEmpleado) &&
                SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), nuevoEmpleado) == null)
            {
                RegistroEmpleados.Add(nuevoEmpleado);
                Console.WriteLine("Insertado exitoso.");
            }
            else
            {
                Console.WriteLine("Fallo al insertar. ");
                var message=MessageBoxManager.GetMessageBoxStandard("Fallo al insertar. ","No existe el empleado seleccionado o el DNI del empleado introducido ya está en uso. ",ButtonEnum.Ok,Icon.Error,WindowStartupLocation.CenterScreen);
                await message.ShowAsync();
            }
        }
    }

    [RelayCommand]
    public async Task btModificarEmpleado_OnClick()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        var EmpleadoDlg = new EmpleadoDlg(EmpleadoSeleccionado);
        await EmpleadoDlg.ShowDialog(mainWindow);
        if (!EmpleadoDlg.IsCancelled)
        {
            Console.Out.WriteLine("Intentando modificar...");
            Empleado nuevoEmpleado = new Empleado()
                { Dni = EmpleadoDlg.DniTB.Text, Email = EmpleadoDlg.EmailTB.Text, Nombre = EmpleadoDlg.NombreTB.Text };
            if (SharedDB.FiltrarEntradasEmpleado(EmpleadoSeleccionado) &&
                SharedDB.FiltrarEntradasEmpleado(nuevoEmpleado))
            {
                if (SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), EmpleadoSeleccionado) != null && (EmpleadoSeleccionado.Dni == nuevoEmpleado.Dni || SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), nuevoEmpleado) == null))
                {
                    RegistroEmpleados.RemoveEmpleado(EmpleadoSeleccionado);
                    ForceUpdateUI();
                    RegistroEmpleados.Add(nuevoEmpleado);
                    ForceUpdateUI();
                    Console.Out.WriteLine("Modificado exitoso.");
                }
                else
                {
                    Console.Out.WriteLine("Fallo al modificar.");
                    var message=MessageBoxManager.GetMessageBoxStandard("Fallo al modificar. ","No existe el empleado seleccionado o el DNI del empleado introducido ya está en uso. ",ButtonEnum.Ok,Icon.Error,WindowStartupLocation.CenterScreen);
                    await message.ShowAsync();
                }
            }
        }
    }

    [RelayCommand]
    public async Task btEliminarEmpleado_OnClick()
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard("Atención", "Los datos se borrarán irreversiblemente.¿Desea continuar?", ButtonEnum.OkCancel);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Ok && SharedDB.FiltrarEntradasEmpleado(EmpleadoSeleccionado) && 
            SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), EmpleadoSeleccionado) != null)
        {
            //SharedDB.Instance.Reparaciones.Reps.Where(r => r.Empleado == EmpleadoSeleccionado);
            foreach (var r in SharedDB.Instance.Reparaciones.Reps)
            {
                if (r.Empleado.Dni == EmpleadoSeleccionado.Dni)
                {
                    r.Empleado.Dni = "00000000O";
                    r.Empleado.Nombre = "(Empleado Eliminado)";
                    r.Empleado.Email = "eliminado@eliminado.eliminado";
                }
            }
            Console.Out.WriteLine("Intentando eliminar...");
            RegistroEmpleados.RemoveEmpleado(EmpleadoSeleccionado);
            Console.Out.WriteLine("Eliminado exitoso.");
        }
        else
        {
            Console.Out.WriteLine("Eliminado fallido.");
            var message=MessageBoxManager.GetMessageBoxStandard("Fallo al eliminar. ","No se encuentra el empleado a eliminar. ",ButtonEnum.Ok,Icon.Error,WindowStartupLocation.CenterScreen);
            await message.ShowAsync();
        }
    }
    [RelayCommand]
    public  async Task btNuevoEmpleado_OnClick()
    {
        EmpleadoSeleccionado=new Empleado();
        EmpleadoActual=new Empleado();
        Aviso = "Entradas y empleado seleccionado reseteados.";
    }
    

    [RelayCommand]
    public  async Task btTicketsSelecc_OnClick()
    {
        if (EmpleadoSeleccionado == null) return;
        NavigationService.Instance.NavigateTo<ReparacionesViewModel>(EmpleadoSeleccionado.Dni);
    }
    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["DNI","Nombre","Email"]);
    public override ObservableCollection<Empleado> FilteredItems
    {
        get
        {
            var text = FilterText.ToLower();
            if (FilterText != "")
            {
                DateTime date;
                try
                {
                    switch (FilterModes[SelectedFilterMode])
                    {
                        case "DNI":
                            return new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.Where(e => e.Dni.ToLower().Contains(text)));
                        case "Nombre":
                            return new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.Where(e => e.Nombre.ToLower().Contains(text)));
                        case "Email":
                            return new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.Where(e => e.Email.ToLower().Contains(text)));
                       default:
                            return RegistroEmpleados.Empleados;
                    }
                }
                catch (FormatException e)
                {
                    //TODO: find a good way to communicate this to the user
                    Console.WriteLine("Fecha de busqueda no válida");
                    return RegistroEmpleados.Empleados;
                }
            }
            else
            {
                return RegistroEmpleados.Empleados;
            }
        }
    }
};
