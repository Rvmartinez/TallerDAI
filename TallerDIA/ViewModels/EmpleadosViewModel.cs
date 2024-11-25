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
        Empleados = ControlesEmpleado.ObtenerListaEmpleados();/**/
        EmpleadoActual = new Empleado();
        EmpleadoSeleccionado = new Empleado();
        Console.Out.WriteLine("EmpleadosViewModel en marcha...");
        Aviso = "Bienvenido a la ventana de gestón de Empleados del Taller.";
    }

    public EmpleadosViewModel(Empleado empleado)
    {
        Empleados = ControlesEmpleado.ObtenerListaEmpleados();
        EmpleadoActual = new Empleado();
        if (ControlesEmpleado.FiltrarEntradasEmpleado(empleado) && ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), empleado) != null)
        {
            EmpleadoSeleccionado=empleado;
            Aviso = "Empleado mostrado con éxito.";
            Console.Out.WriteLine("EmpleadosViewModel en marcha y mostrando Empleado seleccionado...");
        }
        else
        {
            Aviso = "Error al introducir empleado.";
            Console.Out.WriteLine("EmpleadosViewModel en marcha pero sin mostrar el Empleado seleccionado...");
        }
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
            ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), EmpleadoActual) == null &&
            ControlesEmpleado.FiltrarEmpleadoRegex(EmpleadoActual))
        {
            Empleados.Add(EmpleadoActual);
            //EmpleadoActual.Tickets = new List<DateTime>();
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
    [RelayCommand]
    public void btTicketsSelecc_OnClick()
    {
        //var window = new (EmpleadoSeleccionado);
        //window.Show();
    }

}
