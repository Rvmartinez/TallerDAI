using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Models;
using TallerDIA.Models;
using TallerDIA.Utils;
using TallerDIA.ViewModels;
namespace TallerDIA.ViewModels;
public partial class EmpleadosViewModel : ViewModelBase
{
    public EmpleadosViewModel()
    {
        DateTime reparacion1 = new DateTime(2019,06,11,10,15,10);
        DateTime reparacion2 = new DateTime(2020,05,07,9,10,1);
        DateTime reparacion3 = new DateTime(2018,10,08,18,1,59);
        DateTime reparacion4 = new DateTime(2021,01,10,7,45,22);
        List<DateTime> reparaciones1 = new List<DateTime>{reparacion1,reparacion2};
        List<DateTime> reparaciones2 = new List<DateTime>{reparacion3,reparacion4};
        Empleado empleado1 = new Empleado("12345678A", "Abelardo", "averelardo@hotcorreo.coom", reparaciones1);
        Empleado empleado2 = new Empleado("22345678B", "Luffy", "onepieceismid@ymail.com", reparaciones2);
        List<Empleado> empleados = new List<Empleado> 
        {
            empleado1,empleado2
        };
        Empleados = new ObservableCollection<Empleado>(empleados);/**/
        EmpleadoActual = new Empleado();
        EmpleadoSeleccionado = new Empleado();
        Console.Out.WriteLine("EmpleadosViewModel en marcha...");
        Aviso = "Bienvenido a la ventana de gestón de Empleados del Taller.";
    }
    
    
    // Cuando el usuario haga click en una fila del datagrid, se mostrarán los datos de ese Empleado
    // en los TextBoxes correspondientes.
    // Atada al datagrid en XAML mediante 'SelectionChanged="NuevoEmpleadoSeleccionado"'.
    /*private void NuevoEmpleadoSeleccionado()
    {
        Empleado empleado = EmpleadoActual;
        if (empleado != null)
        {
            tbDni.Text = empleado.Dni.ToString();
            tbNombre.Text = empleado.Nombre.ToString();
            tbEmail.Text = empleado.Email.ToString();
            LbTickets.ItemsSource = new ObservableCollection<DateTime>(BuscarEmpleado(empleado.Dni.ToString()).Tickets);
            tblAvisos.Text="Nuevo empleado seleccionado.";
        }
        else
        {
            Console.Out.WriteLine("Seleccion nula.");
        }
        //MostrarTickets(empleado);
    }*/

    private ObservableCollection<Empleado> _Empleados;
    public  ObservableCollection<Empleado>  Empleados
    {
        get => _Empleados;
        set
        {
            //Empleados = value;
            //_Empleados = value;
            SetProperty(ref _Empleados, value);
        }
    }
    
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
    
    private Empleado _EmpleadoSeleccionado;
    public Empleado EmpleadoSeleccionado
    {
        get => _EmpleadoSeleccionado;
        set
        {
            SetProperty(ref _EmpleadoSeleccionado, value);
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

    
    // FUNCIONES PARA ACTUALIZAR LA INTERFAZ Y FILTRAR LAS ENTRADAS.
    
    // Funcion para que el datagrid muestre los datos de la lista propiedad "Empleados".
    public void ActualizarDgEmpleados()
    {
        List<Empleado> empleados = Empleados.ToList();
        Empleados.Clear();
        Empleados = new ObservableCollection<Empleado>(empleados);
    }
    
    // EVENTOS DE CONTROLADOR.
    // Cuando se haga click en el botón correspondiente, se ejecutan respectivamente.
    [RelayCommand]
    public void btAnadirEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando introducir...");
        if (ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoActual) &&
            ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), EmpleadoActual) == null)
        {
            Empleados.Add(EmpleadoActual);
            EmpleadoActual.Tickets = new List<DateTime>();
            ActualizarDgEmpleados();
            Console.Out.WriteLine("Insertado exitoso.");
            Aviso = "Empleado insertado exitosamente.";
        }
        else
        {
            Console.Out.WriteLine("Insertado fallido.");
            Aviso = "Fallo al insertar, ya existe ese empleado o hay campos en blanco.";
        }
        EmpleadoActual=new Empleado();
    }
    [RelayCommand]
    public void btModificarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando modificar...");
        if (ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoActual) && 
            ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoSeleccionado) &&
            ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), EmpleadoSeleccionado) != null)
        {
            Empleados.Remove(EmpleadoSeleccionado);
            //EmpleadoActual.Tickets = new List<DateTime>();
            Empleados.Add(EmpleadoActual);
            ActualizarDgEmpleados();
            Console.Out.WriteLine("Modificado exitoso.");
            Aviso = "Empleado modificado exitosamente.";
        }
        else
        {
            Console.Out.WriteLine("Modificado fallido.");
            Aviso = "Fallo al modificar, no existe ese empleado o hay campos en blanco.";
        }
        EmpleadoActual=new Empleado();
    }
    [RelayCommand]
    public void btEliminarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando eliminar...");
        if (ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoSeleccionado) && 
            ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), EmpleadoSeleccionado) != null)
        {
            Empleados.Remove(EmpleadoSeleccionado);
            ActualizarDgEmpleados();
            Console.Out.WriteLine("Eliminado exitoso.");
            Aviso = "Empleado eliminado exitosamente.";
        }
        else
        {
            Console.Out.WriteLine("Eliminado fallido.");
            Aviso = "Fallo al modificar, no existe ese empleado.";

        }
        EmpleadoActual=new Empleado();
    }
    [RelayCommand]
    public void btNuevoEmpleado_OnClick()
    {
        EmpleadoSeleccionado=new Empleado();
        EmpleadoActual=new Empleado();
        ActualizarDgEmpleados();
        Aviso = "Entradas y empleado seleccionado reseteados.";
    }
}
