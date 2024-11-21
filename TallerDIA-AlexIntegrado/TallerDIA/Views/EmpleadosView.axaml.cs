using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Models;
using TallerDIA.Models;
using Test4TallerAfc.ViewModels;

namespace Test4TallerAfc.Views;

public partial class EmpleadosView : Window
{
    // Lista de Empleados con la que se trabaja en el programa.
    // Esta será la que guardaremos y cargaremos en XML.
    public List<Empleado> Empleados { get; set; }
    
    // Lista que almacena los Tickets del Empleado seleccionado.
    //public List<Empleado> EmpleadoActualTickets { get; set; }
    EmpleadosViewModel viewModel;

    public EmpleadosView()
    {
        InitializeComponent();
        DataContext = viewModel = new EmpleadosViewModel();

    }
}