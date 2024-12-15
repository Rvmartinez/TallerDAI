using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using TallerDIA.Models;
using TallerDIA.Utils;
using TallerDIA.Views.Dialogs;
using ChartWindow = TallerDIA.Views.ChartWindow;
using ConfigChart = TallerDIA.Views.ConfigChart;
using ReparacionesView = TallerDIA.Views.ReparacionesView;

namespace TallerDIA.ViewModels
{

    public partial class ReparacionesViewModel : FilterViewModel<Reparacion>
    {
        
       

        private static string toret = "11/11/1111 11:11:11";
        private static readonly DateTime _BASE_FINFECHA = DateTime.Parse(toret);
        private Reparaciones _reparaciones = SharedDB.Instance.Reparaciones;
        public Reparaciones ReparacionesColection;

        private DateTimeOffset _minDate  = new DateTimeOffset(new DateTime(2020, 1, 1));
        private DateTimeOffset _maxDate = new DateTimeOffset(new DateTime(2030, 1, 1));
        public DateTimeOffset MinDate
        {
            get => _minDate;
            set
            {
                SetProperty(ref _minDate,value);
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public DateTimeOffset MaxDate
        {
            get => _maxDate;
            set
            {
                SetProperty(ref _maxDate, value);
                OnPropertyChanged(nameof(FilteredItems));
            }
        }

        public ObservableCollection<Reparacion> Reparaciones
        {
            get => _reparaciones.Reps;
        }

        private Reparacion _selectedRepair;
        public Reparacion SelectedRepair
        {
            get => _selectedRepair;
            set
            {
                SetProperty(ref _selectedRepair, value);
            }
        }

        public ReparacionesViewModel()
        {
            ReparacionesColection = SharedDB.Instance.Reparaciones;
        }


        public ReparacionesViewModel(Empleado empleado)
        {
            ReparacionesColection.Reps =  new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Empleado == empleado));
        }
        
        public ReparacionesViewModel(Cliente cliente)
        {
            ReparacionesColection.Reps =  new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Cliente == cliente));
        }

        #region Popup
       


     

       

        #endregion

        [RelayCommand]
        public async Task OnButtonEliminarReparacion()
        {
           
            if (SelectedRepair == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Alerta de eliminacion",
                    "Debe de seleccionar registro a eliminar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);

                await message.ShowAsync();
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Eliminacion de reparacion",
                    "Estas seguro de eliminar la reparacion seleccionada?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question, WindowStartupLocation.CenterOwner);

                var respuesta = await message.ShowAsync();

                switch (respuesta)
                {
                    case ButtonResult.Yes:
                        
                        Reparaciones.Remove(SelectedRepair);
                       
                        ForceUpdateUI();
                        
                        SelectedRepair = null!;
                        break;
                    case ButtonResult.No:
                        break;
                    default:
                        Console.WriteLine("No se puede eliminar el reparacion");
                        break;
                }
            }


        }

        [RelayCommand]
        private async void OnButtonFinalizarReparacion()
        {
            if (SelectedRepair == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Alerta de modificacion",
                    "Debe de seleccionar registro a finalizar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);

                await message.ShowAsync();
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Modificacion de reparacion",
                    "Estas seguro de marcar como finalizada la reparacion seleccionada?", ButtonEnum.YesNo, MsBox.Avalonia.Enums.Icon.Question, WindowStartupLocation.CenterOwner);

                var respuesta = await message.ShowAsync();

                switch (respuesta)
                {
                    case ButtonResult.Yes:

                        Reparacion r;
                        foreach (Reparacion rep in ReparacionesColection.Reps)
                        {
                            if (rep.Equals(SelectedRepair))
                            {
                                Console.WriteLine("Reparacion encontrada");
                                Console.WriteLine(rep.ToString());
                                r = rep;
                                r.asignarFechaFin();
                                SharedDB.Instance.EditReparacion(SelectedRepair,r);
                                ForceUpdateUI();
                                Console.WriteLine(r.ToString());
                                break;
                            }
                           
                        }
                       

                        break;
                    case ButtonResult.No:
                        break;
                    default:
                        Console.WriteLine("No se puede finalizar el reparacion");
                        break;
                }

                SelectedRepair = null!;
            }
        }

        
        public async  Task AddRepaisCommand()
        {
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            var ReparacionDlg = new ReparacionDlg();
            await ReparacionDlg.ShowDialog(mainWindow);

            if (!ReparacionDlg.IsCanceled)
            {
                if ( ReparacionDlg.AsuntoTb.Text==null|| ReparacionDlg.NotaTb.Text==null  )
                {
                    
                    var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no completados" ,
                        "Debe completar todos los campos", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
                    await message.ShowAsync();
                    
                }
                else if(ReparacionDlg.EmpleadoTb.SelectedItem == null || ReparacionDlg.ClienteTb.SelectedItem == null)
                {
                    var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no completados" ,
                        "Debe escribir el dni o nombre del empleado o cliente y seleccionar los disponibles", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
                    await message.ShowAsync();
                }else
                {
                    Console.WriteLine("Creando y guradando reparacion");
                    string[] clienteSelect = ReparacionDlg.ClienteTb.SelectedItem.ToString().Split("_");
                    string[] empleadoSelect = ReparacionDlg.EmpleadoTb.SelectedItem.ToString().Split("_");

                    IEnumerable<Cliente> clienteI = SharedDB.Instance.CarteraClientes.Clientes.Where(c => c.DNI.Contains(clienteSelect[0]) && c.Nombre.Contains(clienteSelect[1]));
                    Cliente cliente = clienteI.First();
                    if (cliente == null)
                    {
                        var message = MessageBoxManager.GetMessageBoxStandard("Alerta datos no encontrados" ,
                            "Cliente no encontrado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
                        await message.ShowAsync();
                    }
                    else
                    {
                        IEnumerable<Empleado> empleadoI = SharedDB.Instance.RegistroEmpleados.Empleados.Where(c => c.Dni.Contains(empleadoSelect[0]) && c.Nombre.Contains(empleadoSelect[1]));
                        Empleado empleado = empleadoI.First();
                        Console.WriteLine("Reparacion creada: " + empleado.ToString());
                        if (empleado == null)
                        {
                            var message = MessageBoxManager.GetMessageBoxStandard("Alerta datos no encontrados" ,
                                "Empleado no encontrado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner );
            
                            await message.ShowAsync();
                        }
                        else
                        {
                            Reparacion rep = new Reparacion(ReparacionDlg.AsuntoTb.Text, ReparacionDlg.NotaTb.Text, cliente, empleado);
                            Console.WriteLine("Reparacion creada: " + rep.ToString()); 
                            
                            
                            ReparacionesColection.Reps.Add(rep);
                            ForceUpdateUI();
                        } 
                    }
                    
                    

                   
                   
                   
                    ReparacionDlg.EmpleadoTb.BorderBrush = Brushes.Black;
                    ReparacionDlg.ClienteTb.BorderBrush = Brushes.Black;
                    ReparacionDlg.AsuntoTb.BorderBrush = Brushes.Black;
                    ReparacionDlg.NotaTb.BorderBrush = Brushes.Black;
                }
                
            }
        
        }
        
       [RelayCommand]
        public async Task ModifyRepaisCommand()
        {



            if (SelectedRepair == null || SelectedRepair.FechaFin != _BASE_FINFECHA)
            {
                return; 
            }
                
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

            var ReparacionDlg = new ReparacionDlg(SelectedRepair);
            await ReparacionDlg.ShowDialog(mainWindow);

            if (!ReparacionDlg.IsCanceled)
            {

                if (ReparacionDlg.AsuntoTb.Text == "" || ReparacionDlg.NotaTb.Text == "")
                {

                    var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no completados",
                        "Debe completar todos los campos", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning,
                        WindowStartupLocation.CenterOwner);

                    await message.ShowAsync();
                    return;

                }

                
                
                Console.WriteLine("Creando y guradando reparacion");

                string[] empleadoSelect;
                IEnumerable<Empleado> empleadoI;
                Empleado empleado;
                if (ReparacionDlg.EmpleadoTbNuevo.SelectedItem != null)
                {
                     empleadoSelect = ReparacionDlg.EmpleadoTbNuevo.SelectedItem.ToString().Split("_");
                   
                   empleadoI = SharedDB.Instance.RegistroEmpleados.Empleados.Where(c => c.Dni.Contains(empleadoSelect[0]) && c.Nombre.Contains(empleadoSelect[1]));
                   
                     empleado = empleadoI.First();
                    
                    Console.WriteLine("Reparacion creada: " + empleado.ToString());


                       
                       


                    
                }
                else
                {
                    
                   
                    empleado = SelectedRepair.Empleado;
                    
                    Console.WriteLine("Reparacion creada: " + empleado.ToString());
                }
                
                if (empleado == null)
                {
                    var message = MessageBoxManager.GetMessageBoxStandard("Alerta datos no encontrados",
                        "Empleado no encontrado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning,
                        WindowStartupLocation.CenterOwner);

                    await message.ShowAsync();
                }

                if (empleado == SelectedRepair.Empleado)
                {
                    var message = MessageBoxManager.GetMessageBoxStandard("Modificacion de datos",
                        "Se modifico el asunto/nota", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info,
                        WindowStartupLocation.CenterOwner);

                    await message.ShowAsync();
                }
                else
                {
                    var message = MessageBoxManager.GetMessageBoxStandard("Modificacion de datos",
                        "Se modifico el asunto/nota y empleado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info,
                        WindowStartupLocation.CenterOwner);

                    await message.ShowAsync();
                }
                Reparacion rep = new Reparacion(ReparacionDlg.AsuntoTb.Text, ReparacionDlg.NotaTb.Text, SelectedRepair.Cliente, empleado);
                        
                Console.WriteLine(rep.ToString());
                        
                SharedDB.Instance.EditReparacion(SelectedRepair, rep);
                       

                ReparacionDlg = null;
                SelectedRepair = null;
                ForceUpdateUI();
               





                
            }


        }



        [RelayCommand]
        public async Task ButtonNevegarCommand()
        {
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            var reparacionNavegarDlg = new ReparacionNavegarDlg();
            await reparacionNavegarDlg.ShowDialog(mainWindow);

            if (!reparacionNavegarDlg.IsCancelled)
            {
                return;

            }

            if (reparacionNavegarDlg.VerEmpleado)
            {
                
            }
            if (reparacionNavegarDlg.VerCliente)
            {
                
            }
        }
        
        public async Task ButtonAbrirGrafica()
        {
            if (SharedDB.Instance.Reparaciones.Count > 0)
            {
                var mainWindow =
                    Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                        ? desktop.MainWindow
                        : null;
                var reps = SharedDB.Instance.Reparaciones;
                var reparacionNavegarDlg = new ChartWindow(reps, new ConfigChart(){FechaFin = false});
                await reparacionNavegarDlg.ShowDialog(mainWindow);
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("No hay reparaciones",
                    "No hay reparaciones que mostrar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);

                var respuesta = await message.ShowAsync();
            }


        }
        
        


        public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Asunto","Nota", "Nombre cliente", "DNI cliente", "Nombre empleado","DNI empleado"]);

        public override ObservableCollection<Reparacion> FilteredItems
        {
            get
            {
                Console.WriteLine("Tamaño de Reparaciones antes de busqueda por fecha= " + Reparaciones.Count.ToString());
                var aux = Reparaciones.Where(r => r.FechaInicio >= MinDate && r.FechaFin <= MaxDate);
                Console.WriteLine("Tamaño de Reparaciones despues de busqueda por fecha= " + aux.Count().ToString());
                if (FilterText != "")
                {
                    var Text = FilterText.ToLower();
                    switch (FilterModes[SelectedFilterMode])
                    {
                        case "Asunto":
                            return new ObservableCollection<Reparacion>(aux.Where(r => r.Asunto.ToLower().Contains(Text)));
                        case "Nota":
                            return new ObservableCollection<Reparacion>(aux.Where(r => r.Nota.ToLower().Contains(Text)));
                        case "Nombre cliente":
                            return new ObservableCollection<Reparacion>(aux.Where(r => r.Cliente.Nombre.ToLower().Contains(Text)));
                        case "DNI cliente":
                            return new ObservableCollection<Reparacion>(aux.Where(r => r.Cliente.DNI.ToLower().Contains(Text)));
                        case "Nombre empleado":
                            return new ObservableCollection<Reparacion>(aux.Where(r => r.Empleado.Nombre.ToLower().Contains(Text)));
                        case "DNI empleado":
                            return new ObservableCollection<Reparacion>(aux.Where(r => r.Empleado.Dni.ToLower().Contains(Text)));
                        default:
                            return new ObservableCollection<Reparacion>(aux);

                    }
                }
                else
                {
                    return new ObservableCollection<Reparacion>(aux);
                }
            }
        }
        
        
        [RelayCommand]
        public void ForceUpdateUI()
        {

            List<Reparacion> list = SharedDB.Instance.Reparaciones.Reps.ToList();
            Reparaciones.Clear();

            foreach (Reparacion rep in list)
            {
                Reparaciones.Add(rep);
            }
            OnPropertyChanged(nameof(ReparacionesColection));
            OnPropertyChanged(nameof(FilteredItems));


        }
    }
}
