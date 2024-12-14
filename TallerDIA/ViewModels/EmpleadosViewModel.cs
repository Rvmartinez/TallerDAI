using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;
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
            //_EmpleadoActual = value;
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
        RegistroEmpleados = new RegistroEmpleados(SharedDB.Instance.RegistroEmpleados.Empleados);
        //Empleados=new ObservableCollection<Empleado>(SharedDB.Instance.RegistroEmpleados.Empleados.ToList());
    } 
    
    public EmpleadosViewModel(Empleado nuevoEmpleadoSeleccionado)
    {
        RegistroEmpleados = new RegistroEmpleados(SharedDB.Instance.RegistroEmpleados.Empleados);
        EmpleadoSeleccionado = nuevoEmpleadoSeleccionado;
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
        FilteredItems.Clear();

        foreach (Empleado empleado in list)
        {
            FilteredItems.Add(empleado);
            //RegistroEmpleados.Add(empleado);
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
            Empleado nuevoEmpleado  = new Empleado() { Dni = EmpleadoDlg.DniTB.Text, Email = EmpleadoDlg.EmailTB.Text, Nombre = EmpleadoDlg.NombreTB.Text};
            if (SharedDB.FiltrarEntradasEmpleado(nuevoEmpleado) &&
                SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), nuevoEmpleado) == null)
            {
                RegistroEmpleados.Add(nuevoEmpleado);
            }
        }
    }

    [RelayCommand]
    public async Task btModificarEmpleado_OnClick()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        var EmpleadoDlg = new EmpleadoDlg();
        
        await EmpleadoDlg.ShowDialog(mainWindow);

        if (!EmpleadoDlg.IsCancelled)
        {
            Empleado nuevoEmpleado = new Empleado()
                { Dni = EmpleadoDlg.DniTB.Text, Email = EmpleadoDlg.EmailTB.Text, Nombre = EmpleadoDlg.NombreTB.Text };
            if (SharedDB.FiltrarEntradasEmpleado(EmpleadoSeleccionado) &&
                SharedDB.FiltrarEntradasEmpleado(nuevoEmpleado) &&
                SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), EmpleadoSeleccionado) != null)
            {
                RegistroEmpleados.ActualizarEmpleado(EmpleadoSeleccionado,nuevoEmpleado.Dni,nuevoEmpleado.Nombre,nuevoEmpleado.Email);
                EmpleadoSeleccionado = nuevoEmpleado;
                //FilteredItems = new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.ToList());
                ForceUpdateUI();
            }
        }
    }

    [RelayCommand]
    public void btEliminarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando eliminar...");
        if (SharedDB.FiltrarEntradasEmpleado(EmpleadoSeleccionado) && 
            SharedDB.BuscarEmpleado(RegistroEmpleados.Empleados.ToList(), EmpleadoSeleccionado) != null)
        {
            RegistroEmpleados.Empleados.Remove(EmpleadoSeleccionado);
            Console.Out.WriteLine("Eliminado exitoso.");
            Aviso = "Empleado eliminado exitosamente.";
        }
        else
        {
            Console.Out.WriteLine("Eliminado fallido.");
            Aviso = "Fallo al modificar, no existe ese empleado.";

        }
        //EmpleadoActual=new Empleado();
    }
    [RelayCommand]
    public void btNuevoEmpleado_OnClick()
    {
        EmpleadoSeleccionado=new Empleado();
        EmpleadoActual=new Empleado();
        Aviso = "Entradas y empleado seleccionado reseteados.";
    }
    

    [RelayCommand]
    public void btTicketsSelecc_OnClick()
    {
        //var window = new (EmpleadoSeleccionado);
        //window.Show();
    }
    
    //private ObservableCollection<Empleado> _FilteredItems;
    public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["DNI","Nombre","Email"]);
    public override ObservableCollection<Empleado> FilteredItems
    {
        get
        {
            if (FilterText != "")
            {
                DateTime date;
                try
                {
                    switch (FilterModes[SelectedFilterMode])
                    {
                        case "DNI":
                            return new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.Where(e => e.Dni.Contains(FilterText)));
                        case "Nombre":
                            return new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.Where(e => e.Nombre.Contains(FilterText)));
                        case "Email":
                            return new ObservableCollection<Empleado>(RegistroEmpleados.Empleados.Where(e => e.Email.Contains(FilterText)));
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
