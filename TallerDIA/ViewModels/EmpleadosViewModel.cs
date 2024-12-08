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
        Empleados=new ObservableCollection<Empleado>(SharedDB.Instance.RegistroEmpleados.Empleados.ToList());
        
        /*
        Empleados = new ObservableCollection<Empleado>();//ControlesEmpleado.ObtenerListaEmpleados();
        EmpleadoActual = new Empleado();
        EmpleadoSeleccionado = new Empleado();
        Console.Out.WriteLine("EmpleadosViewModel en marcha...");
        Aviso = "Bienvenido a la ventana de gestón de Empleados del Taller.";*/
    }
    
    

    /*public EmpleadosViewModel(Empleado empleado)
    {
        Empleados = new ObservableCollection<Empleado>();//ControlesEmpleado.ObtenerListaEmpleados();
        EmpleadoActual = new Empleado();
        if (SharedDB.Instance.FiltrarEntradasEmpleado(empleado) && ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), empleado) != null)
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
    }/*


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

    


    // FUNCIONES PARA ACTUALIZAR LA INTERFAZ Y FILTRAR LAS ENTRADAS.
    
    // Funcion para que el datagrid muestre los datos de la lista propiedad "Empleados".
    public void ActualizarDgEmpleados()
    {
        //List<Empleado> empleados = Empleados.ToList();
        Empleados =  new ObservableCollection<Empleado>(RegistroEmpleados.Empleados);
        ObservableCollection<Empleado> EmpleadosNueva = new ObservableCollection<Empleado>(Empleados);
       // Empleados.Clear();
        Empleados = new ObservableCollection<Empleado>(EmpleadosNueva);
    }
    
    // EVENTOS DE CONTROLADOR.
    // Cuando se haga click en el botón correspondiente, se ejecutan respectivamente.
    /*
    [RelayCommand]
    public void btAnadirEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando introducir...");
        if (ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoActual) &&
            ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), EmpleadoActual) == null)
            //&& ControlesEmpleado.FiltrarEmpleadoRegex(EmpleadoActual))
        {
            Empleados.Add(EmpleadoActual);
            //EmpleadoActual.Tickets = new List<DateTime>();
            //ActualizarDgEmpleados();
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
    */
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
                SharedDB.BuscarEmpleado(Empleados.ToList(), nuevoEmpleado) == null)
                //&& ControlesEmpleado.FiltrarEmpleadoRegex(EmpleadoActual))
            {
                List<Empleado> temp = Empleados.ToList();
                //Empleados.Clear();
                //temp.Add(nuevoEmpleado);
                Empleados.Add(nuevoEmpleado);
                RegistroEmpleados.Add(nuevoEmpleado);
                //Empleados = new ObservableCollection<Empleado>(temp);
                //OnPropertyChanged(nameof(Empleados));
            }
        }
        
    }
    /*
    [RelayCommand]
    public void btModificarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando modificar...");
        if (ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoActual) && 
            ControlesEmpleado.FiltrarEntradasEmpleado(EmpleadoSeleccionado) &&
            ControlesEmpleado.BuscarEmpleado(Empleados.ToList(), EmpleadoSeleccionado) != null)
        {
            //List<Empleado> nEmpleados = new List<Empleado>(Empleados);
            
            Empleados[Empleados.IndexOf(EmpleadoSeleccionado)] = new Empleado(EmpleadoActual.Dni, EmpleadoSeleccionado.Nombre, EmpleadoActual.Email);
            EmpleadoSeleccionado = new Empleado(EmpleadoActual.Dni, EmpleadoSeleccionado.Nombre, EmpleadoActual.Email);
            //EmpleadoActual.Tickets = new List<DateTime>();
            //ActualizarDgEmpleados();
            Console.Out.WriteLine("Modificado exitoso.");
            Aviso = "Empleado modificado exitosamente.";
        }
        else
        {
            Console.Out.WriteLine("Modificado fallido.");
            Aviso = "Fallo al modificar, no existe ese empleado o hay campos en blanco.";
        }
        EmpleadoActual=new Empleado();
    }*/

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
                SharedDB.BuscarEmpleado(Empleados.ToList(), EmpleadoSeleccionado) != null)
                //&& ControlesEmpleado.FiltrarEmpleadoRegex(EmpleadoActual))
            {
                //List<Empleado> temp = Empleados.ToList();
                //Empleados.Clear();
                //temp.Add(nuevoEmpleado);
                RegistroEmpleados.ActualizarEmpleado(EmpleadoSeleccionado,nuevoEmpleado.Dni,nuevoEmpleado.Nombre,nuevoEmpleado.Email);
                Empleados[Empleados.IndexOf(EmpleadoSeleccionado)]=nuevoEmpleado;
                EmpleadoSeleccionado = nuevoEmpleado;
                //Empleados = new ObservableCollection<Empleado>(temp);
                //OnPropertyChanged(nameof(Empleados));
            }
        }
    }

    [RelayCommand]
    public void btEliminarEmpleado_OnClick()
    {
        Console.Out.WriteLine("Intentando eliminar...");
        if (SharedDB.FiltrarEntradasEmpleado(EmpleadoSeleccionado) && 
            SharedDB.BuscarEmpleado(Empleados.ToList(), EmpleadoSeleccionado) != null)
        {
            Empleados.Remove(EmpleadoSeleccionado);
            //ActualizarDgEmpleados();
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
        //ActualizarDgEmpleados();
        Aviso = "Entradas y empleado seleccionado reseteados.";
    }
    

    [RelayCommand]
    public void btTicketsSelecc_OnClick()
    {
        //var window = new (EmpleadoSeleccionado);
        //window.Show();
    }
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
                            return new ObservableCollection<Empleado>(Empleados.Where(e => e.Dni.Contains(FilterText)));
                        case "Nombre":
                            return new ObservableCollection<Empleado>(Empleados.Where(e => e.Nombre.Contains(FilterText)));
                        case "Email":
                            return new ObservableCollection<Empleado>(Empleados.Where(e => e.Email.Contains(FilterText)));
                       default:
                            return Empleados;
                    }
                }
                catch (FormatException e)
                {
                    //TODO: find a good way to communicate this to the user
                    Console.WriteLine("Fecha de busqueda no válida");
                    return Empleados;
                }
            }
            else
            {
                return Empleados;
            }
        }
    }
};
