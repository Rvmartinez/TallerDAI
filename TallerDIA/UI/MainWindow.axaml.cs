using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Gestión_de_reparaciones.CORE;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;



namespace Gestión_de_reparaciones;

public partial class MainWindow : Window
{
    static Cliente cliente1 = new Cliente
    {
        DNI = "12345678A",
        Nombre = "Juan Pérez",
        Email = "juan.perez@example.com",
        IdCliente = 1
    };

    static Cliente cliente2 = new Cliente("87654321B", "Ana Gómez", "ana.gomez@example.com", 2);
    static Cliente cliente3 = new Cliente("87845321V", "Jose Hierro", "jose.h@example.com", 3);
    static Cliente cliente4 = new Cliente("12394321H", "Marta Laurence", "marta.lau@example.com", 4);
        
        
    

    static Empleado empleado1 = new Empleado("12345678B", "Mario Pérez", "mario.perez@example.com",  true);

    static Empleado empleado2 = new Empleado("20345678C", "Marisa Almanera", "mars.alma@example.com", false);
        
    static List<Cliente> clientes = new List<Cliente> { cliente1, cliente2 , cliente3, cliente4 };
   
    static List<Empleado> empleados = new List<Empleado> { empleado1, empleado2 };
    
    List<String> clientesSorce = new List<String>();
    List<String> empleadosSorce = new List<String>();
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        var dataGrid = this.FindControl<DataGrid>("DataGridReparaciones");
        this.RegistroReparacion = new RegistroReparacion();
        dataGrid.ItemsSource = this.RegistroReparacion;
       
       
        
        
        
        foreach (Cliente cliente in clientes)
        {
            clientesSorce.Add(cliente.DNI + "_" + cliente.Nombre);
        }
        
        foreach (Empleado empleado in empleados)
        {
            empleadosSorce.Add(empleado.Dni + "_" + empleado.Nombre);
        }

        clientesO.ItemsSource = clientesSorce;
        empleadosO.ItemsSource = empleadosSorce;




    }

    public RegistroReparacion RegistroReparacion { get; set; }


    private void OnButtonBtCP(object? sender, RoutedEventArgs e)
    {
        var mainPanel = this.GetControl<DockPanel>( "MainPanel" );
        var panel = this.GetControl<DockPanel>("GridPanel");
        var dataGrid = this.GetControl<DataGrid>( "DataGridReparaciones" );
        var buttonEliminar = this.GetControl<Button>( "BtEliminarReparacion" );
        var buttonFinalizar = this.GetControl<Button>( "BtFinalizarReparacion" );
        var label = this.GetControl<Label>( "Label_Filtro" );
        var checkTerminados = this.GetControl<CheckBox>( "CheckTerminados" );
        var checkNoTerminados = this.GetControl<CheckBox>( "CheckNoTerminados" );
        var paneles = mainPanel.Children;
        
        foreach (var p in paneles)
        {
            p.IsVisible = false;
        }
        
        panel.IsVisible = true;
        dataGrid.IsVisible = true;
        panel.Width = mainPanel.Bounds.Width;
        panel.Height = mainPanel.Bounds.Height;
        buttonEliminar.IsVisible = true;
        buttonFinalizar.IsVisible = true;
        label.IsVisible = true;
        checkTerminados.IsVisible = true;
        checkNoTerminados.IsVisible = true;
        Console.WriteLine("Entro en el metodo de mostrar el datagrid");
        
    }

   
        
        
       
    

    private void OnButtonCrearReparacion(object? sender, RoutedEventArgs e)
    {
        var mainPanel = this.GetControl<DockPanel>( "MainPanel" );
        var panel = this.GetControl<DockPanel>("CreateReparacion");
        var paneles = mainPanel.Children;
        var buttonEliminar = this.GetControl<Button>( "BtEliminarReparacion" );
       
        var buttonFinalizar = this.GetControl<Button>( "BtFinalizarReparacion" );
        var label = this.GetControl<Label>( "Label_Filtro" );
        var checkTerminados = this.GetControl<CheckBox>( "CheckTerminados" );
        var checkNoTerminados = this.GetControl<CheckBox>( "CheckNoTerminados" );
        
        foreach (var p in paneles)
        {
            p.IsVisible = false;
        }
        
        panel.IsVisible = true;
        panel.Width = mainPanel.Bounds.Width;
        panel.Height = mainPanel.Bounds.Height;
        buttonEliminar.IsVisible = false;
       
        buttonFinalizar.IsVisible = false;
        label.IsVisible = false;
        checkTerminados.IsVisible = false;
        checkNoTerminados.IsVisible = false;
    }

    private async void OnButtonCrear(object? sender, RoutedEventArgs e)
    {
        var textAsunto = this.FindControl<TextBox>( "TextAsunto" );
        var textNota = this.FindControl<TextBox>( "TextNota" );
        var listClientes = this.FindControl<AutoCompleteBox>( "ListClientes" );
        var listEmpleados = this.FindControl<AutoCompleteBox>( "ListEmpleados" );
        if (listClientes.SelectedItem == null || listEmpleados.SelectedItem == null )  {
            var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no seleccionados" ,
                "Debe de seleccionar un cliente y un empleado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
            await message.ShowAsync();
            listClientes.BorderBrush=Brushes.Red;
            listEmpleados.BorderBrush = Brushes.Red;
                                
        }
        else if (textAsunto.Text == "" || textNota.Text == "")
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no completados" ,
                "Debe escribir un Asunto y unha Nota", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
            await message.ShowAsync();
            textAsunto.BorderBrush = Brushes.Red;
            textNota.BorderBrush = Brushes.Red;
        }else
            
        {
            string[] clienteDniNombre = listClientes.SelectedItem.ToString().Split('_');
            
            string[] empleadoDniNombre = listEmpleados.SelectedItem.ToString().Split('_');
            Cliente cliente = new Cliente();
            Empleado empleado =new Empleado();
            
            foreach (Cliente cli in clientes)
            {
                if (cli.DNI.Equals(clienteDniNombre[0]) && cli.Nombre.Equals(clienteDniNombre[1]))
                {
                    
                    
                    cliente =new Cliente(cli.DNI, cli.Nombre,cli.Email,cli.IdCliente);
                    
                }
            }
            
            foreach (Empleado empl in empleados)
            {
                if (empl.Dni== empleadoDniNombre[0] && empl.Nombre == empleadoDniNombre[1])
                {
                    empleado = empl;
                }
            }

            
            
            
            this.RegistroReparacion.Add(new Reparacion(textAsunto.Text, textNota.Text,cliente,empleado));
           
            textAsunto.Clear();
            textNota.Clear();
            listClientes.Text = "";
            listEmpleados.Text = "";
            textAsunto.BorderBrush = Brushes.Black;
            textNota.BorderBrush = Brushes.Black;
            listClientes.BorderBrush = Brushes.Black;
            listEmpleados.BorderBrush = Brushes.Black;
        }
        
        
        
    }

    

    private async void OnButtonEliminarReparacion(object? sender, RoutedEventArgs e)
    {
        var grid = this.FindControl<DataGrid>("DataGridReparaciones");
        Reparacion reparacion = grid.SelectedItem as Reparacion;
        if (reparacion == null)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alerta de eliminacion" ,
                "Debe de seleccionar registro a eliminar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
            await message.ShowAsync();
        }
        else
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Eliminacion de reparacion" ,
                "Estas seguro de eliminar la reparacion seleccionada?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question, WindowStartupLocation.CenterOwner );
            
            var respuesta = await message.ShowAsync();

            switch (respuesta)
            {
                case ButtonResult.Yes:
                    ControladorReparacion.Eliminar(reparacion,RegistroReparacion);
                    //RegistroReparacion.Remove(reparacion);
                    break;
                case ButtonResult.No:
                    break;
                default:
                    Console.WriteLine("No se puede eliminar el reparacion");
                    break;
            }
        }
       
        
    }
/*
    private async void OnButtonModificarReparacion(object? sender, RoutedEventArgs e)
    {
        var grid = this.FindControl<DataGrid>("DataGridReparaciones");
        if (grid.SelectedItem != null)
        {
            grid.IsReadOnly = false;
        }
        else
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alerta de modificacion" ,
                "Debe de seleccionar un campo a editar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
            await message.ShowAsync();
        }
        
    }
    */

    private async void OnButtonFinalizarReparacion(object? sender, RoutedEventArgs e)
    {
        var grid = this.FindControl<DataGrid>("DataGridReparaciones");
        
        if (grid.SelectedItem == null)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alerta de modificacion" ,
                "Debe de seleccionar registro a finalizar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
            await message.ShowAsync();
        }
        else
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Midificacion de reparacion" ,
                "Estas seguro de marcar como finalizada la reparacion seleccionada?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question, WindowStartupLocation.CenterOwner );
            
            var respuesta = await message.ShowAsync();

            switch (respuesta)
            {
                case ButtonResult.Yes:
                    Reparacion reparacion = (Reparacion) grid.SelectedItem;
                    
                   /*
                    reparaciones.Remove(reparacion);
                    reparacion.asignarFechaFin();
                    reparaciones.Add(reparacion);
                    
                    actualizarGrid(RegistroReparacion);*/

                    foreach (Reparacion rep in RegistroReparacion)
                    {
                        if (rep.Equals(reparacion))
                        {
                            Console.WriteLine("Reparacion encontrada");
                            Console.WriteLine(rep.ToString());
                            rep.asignarFechaFin();
                        }
                        Console.WriteLine(rep.ToString());
                    }

                    grid.ItemsSource = RegistroReparacion;
                    
                    //actualizarGrid(RegistroReparacion);
                    
                    
                    break;
                case ButtonResult.No:
                    break;
                default:
                    Console.WriteLine("No se puede finalizar el reparacion");
                    break;
            }
        }
    }

    private void Filtro_Terminado(object? sender, RoutedEventArgs e)
    {
        var checkTerminados = this.FindControl<CheckBox>("CheckTerminados");
        var checkNoTerminados = this.FindControl<CheckBox>("CheckNoTerminados");
        var grid = this.FindControl<DataGrid>("DataGridReparaciones");
        if (checkTerminados.IsChecked == true)
        {
            checkNoTerminados.IsChecked = false;
            grid.ItemsSource = null;
            List<Reparacion> reparacionesTerminadas =  ControladorReparacion.BuscarTerminados(RegistroReparacion);
            grid.ItemsSource = reparacionesTerminadas;
        }
        else
        {
            grid.ItemsSource = null;
            grid.ItemsSource = RegistroReparacion;
        }

        

    }

    private void Filtro_NoTerminado(object? sender, RoutedEventArgs e)
    {
        
        var checkTerminados = this.FindControl<CheckBox>("CheckTerminados");
        var checkNoTerminados = this.FindControl<CheckBox>("CheckNoTerminados");
        var grid = this.FindControl<DataGrid>("DataGridReparaciones");
        if (checkNoTerminados.IsChecked == true)
        {
            checkTerminados.IsChecked = false;
            grid.ItemsSource = null;
            List<Reparacion> reparacionesTerminadas =  ControladorReparacion.BuscarNoTerminados(RegistroReparacion);
            grid.ItemsSource = reparacionesTerminadas;
        }
        else
        {
            grid.ItemsSource = null;
            grid.ItemsSource = RegistroReparacion;
        }
    }
}