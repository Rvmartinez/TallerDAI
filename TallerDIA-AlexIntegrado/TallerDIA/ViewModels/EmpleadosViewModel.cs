using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Models;
using TallerDIA.Models;
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
        Console.Out.WriteLine("EmpleadosMwVm");
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
    
    
    // MISCELÄNEA.
    
    // Esta función busca al empleado con un DNI igual al introducido por parámetro.
    // Si no encuentra ninguno, devuelve NULL.
    public Empleado BuscarEmpleado(string dni)
    {
        // Si no se encuentra un empleado con ese DNI en la lista, devuelve NULL.
        Empleado toret=null;
        if (Empleados != null && dni!=null && Empleados.ToList().Count > 0)
        {
            foreach (Empleado empleado in Empleados){
                if (empleado!=null && empleado.Dni !=null && empleado.Dni  == dni)
                {
                    toret = empleado;
                }
            }
        }
        return toret;
    }
    
    // Esto recoje el texto introducido en los TextBoxes y crea un objeto de la clase Empleado con ellos,
    // Los tickets de dicho empleado son los que están en el ListBox.
    
    
    // Esta sirve para avisar de que cualquier campo está introducido incorrectamente.
    public bool FiltrarEntradasEmpleados()
    {
        string dni = "";
        string nombre = "";
        string email = "";
        if (EmpleadoActual == null)
        {
            Aviso = "Sin entradas. ";
            return false;
        }
        else
        {
            dni = EmpleadoActual.Dni;
            nombre = EmpleadoActual.Nombre;
            email = EmpleadoActual.Email;
            if (dni == null || nombre == null || email == null || dni == " " || nombre == " " ||
                email == " ") // || listaTickets == null  )
            {
                Aviso = "Algún campo en blanco o no válido. ";
                return false;
            }
            else
            {
                dni = EmpleadoActual.Dni.Trim();
                nombre = EmpleadoActual.Nombre.Trim();
                email = EmpleadoActual.Email.Trim();
                if (dni == "" || nombre == "" || email == "") // || listaTickets == null  )
                {
                    Aviso = "Algún campo en blanco o no válido. ";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }


    // FUNCIONES DE ALTA, BAJA Y MODIFICACIÓN.
    
    // Busca primero si no existe un DNI igual, y, si es así,
    // introduce el Empleado pasado por parámetro dentro de nuestra lista Empleados.
    public void IntroducirEmpleado(Empleado empleado)
    {
        if (BuscarEmpleado(empleado.Dni) == null && FiltrarEntradasEmpleados())
        {
            Empleados.Add(empleado);
            Empleados = new ObservableCollection<Empleado>(Empleados);
            ActualizarDgEmpleados();
            Empleado empAux=EmpleadoActual;
            EmpleadoActual = null;
            EmpleadoActual = new Empleado(empAux.Dni, empAux.Nombre, empAux.Email, empAux.Tickets);
            Aviso="Empleado introducido exitosamente.";
        }
        else
        {
            Aviso="Fallo al introducir empleado. Ya existe un empleado con ese DNI.";
        }
    }
    
    // Busca primero si existe el DNI pasado por parámetro, y, si es así,
    // elimina al Empleado con ese mismo DNI de la lista Empleados.
    public void EliminarEmpleado(string dni)
    {
        if (dni!=null && BuscarEmpleado(dni) != null)
        {
            Empleados.Remove(BuscarEmpleado(dni));
            ActualizarDgEmpleados();
            Aviso="Empleado eliminado exitosamente.";
        }
        else
        {
            Aviso="Fallo al eliminar. No existe un empleado con ese DNI.";
        }
    }
    
    // Busca primero si el Empleado con un DNI igual está en la y si está,
    // en su posición lo sustituye por el Empleado pasado por parámetro en la lista Empleados.
    public void ModificarEmpleado(Empleado empleado)
    {
        if (FiltrarEntradasEmpleados() && empleado!=null && empleado.Dni!=null && BuscarEmpleado(empleado.Dni) != null )
        {
            if (Empleados[Empleados.IndexOf(BuscarEmpleado(empleado.Dni))].Tickets != null && Empleados[Empleados.IndexOf(BuscarEmpleado(empleado.Dni))].Tickets.Count > 0)
            {
                empleado.Tickets=new List<DateTime>(Empleados[Empleados.IndexOf(BuscarEmpleado(empleado.Dni))].Tickets);
            }
            else
            {
                empleado.Tickets=new List<DateTime>();
            }
            Empleados[Empleados.IndexOf(BuscarEmpleado(empleado.Dni))] = empleado;
            //Empleados = new ObservableCollection<Empleado>(Empleados);
            ActualizarDgEmpleados();
            Aviso="Empleado modificado exitosamente.";
        }
        else
        {
            Aviso="Fallo al modificar. No existe un empleado con ese DNI.";
        }
    }
    
    
    // EVENTOS DE CONTROLADOR.
    // Cuando se haga click en el botón correspondiente, se ejecutan respectivamente.
    [RelayCommand]
    public void btAnadirEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando añadir.");
        //Empleado empleado =new Empleado(tbDni.Text.ToString(),tbNombre.Text.ToString(),tbEmail.Text.ToString(),LbTickets.ItemsSource as ObservableCollection<string>);
        if (FiltrarEntradasEmpleados())
        {
            IntroducirEmpleado(EmpleadoActual);
        }
    }
    [RelayCommand]
    private void btModificarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando modificar.");
        if (FiltrarEntradasEmpleados())
        {
            ModificarEmpleado(EmpleadoActual);
        }
    }
    [RelayCommand]
    private void btEliminarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando eliminar.");
        if (EmpleadoActual.Dni != null && EmpleadoActual.Dni != "" && EmpleadoActual.Dni.Trim() != "")
        {
            EliminarEmpleado(EmpleadoActual.Dni);
        }
        else
        {
            Aviso = "Dni mal introducido.";
        }

    }
    [RelayCommand]
    private void btAbrirReparaciones_OnClick()
    {
        Console.Out.WriteLine("Sneed");
    }
}
