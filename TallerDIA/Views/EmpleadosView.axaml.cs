using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TallerDIA.Models;
using TallerDIA.ViewModels;

namespace TallerDIA.Views;

public partial class EmpleadosView : UserControl
{
    // Lista de Empleados con la que se trabaja en el programa.
    // Esta será la que guardaremos y cargaremos en XML.
    //public List<Empleado> Empleados { get; set; }
    
    // Lista que almacena los Tickets del Empleado seleccionado.
    //public List<Empleado> EmpleadoActualTickets { get; set; }
    EmpleadosViewModel viewModel;
    public EmpleadosView()
    {
        InitializeComponent();
        DataContext = viewModel = new EmpleadosViewModel();

    }
    
    public EmpleadosView(Empleado empleado)
    {
        InitializeComponent();
        DataContext = viewModel = new EmpleadosViewModel(empleado);

    }
}