using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ConsoleApp1;
using Test4TallerAfc.ViewModels;

namespace Test4TallerAfc.Views;

public partial class MainWindow : Window
{
    // Lista de Empleados con la que se trabaja en el programa.
    // Esta será la que guardaremos y cargaremos en XML.
    public List<Empleado> Empleados { get; set; }
    
    // Lista que almacena los Tickets del Empleado seleccionado.
    //public List<Empleado> EmpleadoActualTickets { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        // !!! - PROVISIONAL, AQUÍ SE CARGARÍA EL XML - !!!
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
        Empleados = new List<Empleado>(empleados);
        ActualizarDgEmpleados(Empleados);
    }
    
    // Cuando el usuario haga click en una fila del datagrid, se mostrarán los datos de ese Empleado
    // en los TextBoxes correspondientes.
    // Atada al datagrid en XAML mediante 'SelectionChanged="NuevoEmpleadoSeleccionado"'.
    private void NuevoEmpleadoSeleccionado(object? sender, SelectionChangedEventArgs e)
    {
        Empleado empleado = DgEmpleados.SelectedItem as Empleado;
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
        //MostrarTickets(empleado);*/
    }

    
    // FUNCIONES PARA ACTUALIZAR LA INTERFAZ Y FILTRAR LAS ENTRADAS.
    
    // Funcion para que el datagrid muestre los datos de la lista propiedad "Empleados".
    public void ActualizarDgEmpleados(List<Empleado> empleadosNueva )
    {
        //LbTickets.ItemsSource = empleadosNueva[0].Tickets;
        DgEmpleados.ItemsSource = new ObservableCollection<Empleado>(empleadosNueva); ; 
    }
    
    // Funcion para que el listbox de al lado del datagrid muestre los tickets de el empleado seleccionado.
    private void MostrarTickets(Empleado empleado)
    {
        List<DateTime> tickets = new List<DateTime>(empleado.Tickets);
        LbTickets.ItemsSource = new ObservableCollection<DateTime>(tickets);
    }
    
    
    // MISCELÄNEA.
    
    // Esta función busca al empleado con un DNI igual al introducido por parámetro.
    // Si no encuentra ninguno, devuelve NULL.
    public Empleado BuscarEmpleado(string dni)
    {
        // Si no se encuentra un empleado con ese DNI en la lista, devuelve NULL.
        Empleado toret=null;
        bool encontrado=false;
        foreach (Empleado empleado in Empleados){
            if (empleado.Dni == dni)
            {
                encontrado=true;
                toret = empleado;
            }
        }
        return toret;
    }
    
    // Esto recoje el texto introducido en los TextBoxes y crea un objeto de la clase Empleado con ellos,
    // Los tickets de dicho empleado son los que están en el ListBox.
    private Empleado RecojerDatosEmpleado()
    {
        List<DateTime> listaTickets = new List<DateTime>();
        if (LbTickets.ItemsSource!=null || LbTickets.ItemsSource != new ObservableCollection<DateTime>())
        {
            Console.Out.WriteLine("Lista de listbox tickets vacía.");
        }
        else
        {
            listaTickets = new List<DateTime>(LbTickets.ItemsSource as List<DateTime>);
            if (listaTickets == null || listaTickets.Count == 0)
            {
                listaTickets = new List<DateTime>();
            }
            else
            {
                Console.Out.WriteLine("Lista de tickets vacía.");
            }
        }
        
        Empleado empleado =new Empleado(tbDni.Text.ToString().Trim(),tbNombre.Text.ToString().Trim(),tbEmail.Text.ToString().Trim(),listaTickets);
        return empleado;
    }
    
    // Esta sirve para avisar de que cualquier campo está introducido incorrectamente.
    public bool FiltrarEntradasEmpleados()
    {
        string dni=tbDni.Text;
        string nombre=tbNombre.Text;
        string email=tbEmail.Text;
        //List<string> listaTickets = LbTickets.ItemsSource as List<string>;
        if (dni == null || nombre == null  || email == null || dni == " " || nombre == " "  || email == " " ) // || listaTickets == null  )
        {
            
            tblAvisos.Text = "Algún campo en blanco o no válido. ";
            return false;
        }
        else
        {
            dni=tbDni.Text.Trim();
            nombre=tbNombre.Text.Trim();
            email=tbEmail.Text.Trim();
            if (dni == "" || nombre == ""  || email == "" ) // || listaTickets == null  )
            {
                tblAvisos.Text = "Algún campo en blanco o no válido. ";
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
    
    
    // FUNCIONES DE ALTA, BAJA Y MODIFICACIÓN.
    
    // Busca primero si no existe un DNI igual, y, si es así,
    // introduce el Empleado pasado por parámetro dentro de nuestra lista Empleados.
    public void IntroducirEmpleado(Empleado empleado)
    {
        if (BuscarEmpleado(empleado.Dni) == null)
        {
            Empleados.Add(empleado);
            ActualizarDgEmpleados(Empleados);
            tblAvisos.Text="Empleado introducido exitosamente.";
        }
        else
        {
            tblAvisos.Text="Fallo al introducir empleado. Ya existe un empleado con ese DNI.";
        }
    }
    
    // Busca primero si existe el DNI pasado por parámetro, y, si es así,
    // elimina al Empleado con ese mismo DNI de la lista Empleados.
    public void EliminarEmpleado(string dni)
    {
        if (BuscarEmpleado(dni) != null)
        {
            Empleados.Remove(BuscarEmpleado(dni));
            ActualizarDgEmpleados(Empleados);
            tblAvisos.Text="Empleado eliminado exitosamente.";
        }
        else
        {
            tblAvisos.Text="Fallo al eliminar. No existe un empleado con ese DNI.";
        }
    }
    
    // Busca primero si el Empleado con un DNI igual está en la y si está,
    // en su posición lo sustituye por el Empleado pasado por parámetro en la lista Empleados.
    public void ModificarEmpleado(Empleado empleado)
    {
        if (BuscarEmpleado(empleado.Dni) != null )
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
            ActualizarDgEmpleados(Empleados);
            tblAvisos.Text="Empleado modificado exitosamente.";
        }
        else
        {
            tblAvisos.Text="Fallo al modificar. No existe un empleado con ese DNI.";
        }
    }
    
    
    // EVENTOS DE CONTROLADOR.
    // Cuando se haga click en el botón correspondiente, se ejecutan respectivamente.
    private void BtAnadirEmpleado_OnClick(object? sender, RoutedEventArgs e)
    {
        //Empleado empleado =new Empleado(tbDni.Text.ToString(),tbNombre.Text.ToString(),tbEmail.Text.ToString(),LbTickets.ItemsSource as ObservableCollection<string>);
        if (FiltrarEntradasEmpleados())
        {
            IntroducirEmpleado(RecojerDatosEmpleado());
        }
    }
    private void btModificarEmpleado_OnClick(object? sender, RoutedEventArgs e)
    {
        if (FiltrarEntradasEmpleados())
        {
            ModificarEmpleado(RecojerDatosEmpleado());
        }
    }

    private void btEliminarEmpleado_OnClick(object? sender, RoutedEventArgs e)
    {
        if (tbDni.Text != null && tbDni.Text != "" && tbDni.Text.Trim() != " ")
        {
            EliminarEmpleado(tbDni.Text.ToString().Trim());
        }
        else
        {
            tblAvisos.Text = "Dni mal introducido.";
        }

    }

    private void btAbrirReparaciones_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}