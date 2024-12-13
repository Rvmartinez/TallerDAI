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
using GraficosTaller.UI;
using TallerDIA.Models;
using TallerDIA.Utils;
using TallerDIA.Views;
using TallerDIA.Views.Dialogs;

namespace TallerDIA.ViewModels
{

    public partial class ReparacionesViewModel : FilterViewModel<Reparacion>
    {
        static Cliente _cliente1 = new Cliente
        {
            DNI = "12345678A",
            Nombre = "Juan Pérez",
            Email = "juan.perez@example.com",
            IdCliente = 1
        };
        static Cliente _cliente2 = new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
        static Cliente _cliente3 = new Cliente { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com", IdCliente = 2 };
        static Cliente _cliente4 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 3 };

        private static List<Reparacion> reparacionesBackup;

        private ObservableCollection<Reparacion> _Reparaciones;

        public ObservableCollection<Reparacion> Reparaciones
        {
            get => _Reparaciones;
            set
            {
                SetProperty(ref _Reparaciones, value);

            }
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



        private bool _mostrarTerminados;
        private bool _mostrarNoTerminados;
        private static ReparacionesView dataContext = new ReparacionesView();

        public bool MostrarTerminados
        {
            get => _mostrarTerminados;
            set
            {
                SetProperty(ref _mostrarTerminados, value);
                List<Reparacion> aux = reparacionesBackup.Where(r => !r.FechaFin.Equals(new DateTime())).ToList();
                if (_mostrarTerminados && !_mostrarNoTerminados)
                {
                    
                    
                    Reparaciones = new ObservableCollection<Reparacion>(aux);
                   dataContext.Toogle();
                   
                }
                
                else
                    Reparaciones = new ObservableCollection<Reparacion>(reparacionesBackup);

                
            }
        }

       


        public bool MostrarNoTerminados
        {
            get => _mostrarNoTerminados;
            set
            {
                SetProperty(ref _mostrarNoTerminados, value);
                List<Reparacion> aux = reparacionesBackup.Where(r => r.FechaFin.Equals(new DateTime())).ToList();
                if (_mostrarNoTerminados && !_mostrarTerminados)
                {
                    
                    Reparaciones = new ObservableCollection<Reparacion>(aux);
                }
                else
                    Reparaciones = new ObservableCollection<Reparacion>(reparacionesBackup);
            }
        }

        public object AddReparacion { get; }
        


        /* static Empleado empleado1 = new Empleado("12345678B", "Mario Pérez", "mario.perez@example.com", true);

         static Empleado empleado2 = new Empleado("20345678C", "Marisa Almanera", "mars.alma@example.com", false);
        */
        
        //static List<Empleado> empleados = new List<Empleado> { empleado1, empleado2 };

        //List<String> clientesSorce = new List<String>();
        //List<String> empleadosSorce = new List<String>();

        
        public ReparacionesViewModel()
        {

            _Reparaciones = new ObservableCollection<Reparacion>();
            for (int i = 0; i < SharedDB.Instance.Reparaciones.Count; i++)
            {
                _Reparaciones.Add(SharedDB.Instance.Reparaciones.Get(i));
            }
            /*
             foreach (Cliente cliente in _clientes)
             {
                 clientesSorce.Add(cliente.DNI + "_" + cliente.Nombre);
             }
             */
            /* foreach (Empleado empleado in empleados)
             {
                 empleadosSorce.Add(empleado.Dni + "_" + empleado.Nombre);
             }*/
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
                        //ControladorReparacion.Eliminar(reparacion, RegistroReparacion);
                        //RegistroReparacion.Remove(reparacion);
                        Reparaciones.Remove(SelectedRepair);
                        reparacionesBackup = Reparaciones.ToList();
                        
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
                        /*
                         reparaciones.Remove(reparacion);
                         reparacion.asignarFechaFin();
                         reparaciones.Add(reparacion);

                         actualizarGrid(RegistroReparacion);*/

                        foreach (Reparacion rep in Reparaciones)
                        {
                            if (rep.Equals(SelectedRepair))
                            {
                                Console.WriteLine("Reparacion encontrada");
                                Console.WriteLine(rep.ToString());
                                rep.asignarFechaFin();
                                
                            }
                            Console.WriteLine(rep.ToString());
                        }
                        reparacionesBackup = Reparaciones.ToList();
                        Reparaciones = new ObservableCollection<Reparacion>(reparacionesBackup);

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

        
        public async Task AddRepaisCommand()
        {
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            var ReparacionDlg = new ReparacionDlg();
            await ReparacionDlg.ShowDialog(mainWindow);

            if (!ReparacionDlg.IsCanceled)
            {
                if ( ReparacionDlg.AsuntoTb.Text == "" || ReparacionDlg.NotaTb.Text == "" )
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
                            SharedDB.Instance.Reparaciones.Add(rep);
                            Reparaciones = new ObservableCollection<Reparacion>();
                            for (int i = 0; i < SharedDB.Instance.Reparaciones.Count; i++)
                            {
                                Reparaciones.Add(SharedDB.Instance.Reparaciones.Get(i));
                            }
                        } 
                    }
                    
                    

                   
                   // reparacionesBackup = Reparaciones.ToList();
                    //Reparaciones = new ObservableCollection<Reparacion>(reparacionesBackup);
                   
                    ReparacionDlg.EmpleadoTb.BorderBrush = Brushes.Black;
                    ReparacionDlg.ClienteTb.BorderBrush = Brushes.Black;
                    ReparacionDlg.AsuntoTb.BorderBrush = Brushes.Black;
                    ReparacionDlg.NotaTb.BorderBrush = Brushes.Black;
                }
                
            }
        
        }

        public async Task ModifyRepaisCommand()
        {
           
                
                
            if (SelectedRepair == null) return;
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;

            var ReparacionDlg = new ReparacionDlg(SelectedRepair);
            await ReparacionDlg.ShowDialog(mainWindow);

            if (!ReparacionDlg.IsCanceled)
            {
                //SharedDB.Instance.EditClient(ReparacionDlg, new Reparacion { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = 0 });
                ReparacionDlg = null;
                //ForceUpdateUI();
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
                // Cliente c  = new Cliente() { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = this.GetLastClientId()+1 };
               
                //Reparaciones.Add(c);
                
            }
        }
        
        public async Task ButtonAbrirGrafica()
        {
            if (_Reparaciones.Count > 0)
            {
                var mainWindow =
                    Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                        ? desktop.MainWindow
                        : null;
                var colRep = _Reparaciones.OfType<Reparacion>().ToList();
                var reps = new Reparaciones(colRep);
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
        
        


        public override ObservableCollection<string> _FilterModes { get; } = new ObservableCollection<string>(["Asunto","Nota","Fecha entre", "Nombre cliente", "DNI cliente", "Nombre empleado","DNI empleado"]);

        public override ObservableCollection<Reparacion> FilteredItems
        {
            get
            {
                if (FilterText != "")
                {
                    var Text = FilterText.ToLower();
                    switch (FilterModes[SelectedFilterMode])
                    {
                        case "Asunto":
                            return new ObservableCollection<Reparacion>(Reparaciones.Where(r => r.Asunto.ToLower().Contains(Text)));
                        case "Nota":
                            return new ObservableCollection<Reparacion>(Reparaciones.Where(r => r.Nota.ToLower().Contains(Text)));
                        case "Fecha entre":
                            try
                            {
                                var date = DateTime.Parse(Text);
                                return new ObservableCollection<Reparacion>(
                                    Reparaciones.Where(r => r.FechaInicio <= date && r.FechaFin >= date));
                            }
                            catch (FormatException ex)
                            {
                                //TODO: SHOW THIS TO THE USER
                                Console.Out.WriteLine("Fecha no válida");
                                return Reparaciones;
                            }
                        case "Nombre cliente":
                            return new ObservableCollection<Reparacion>(Reparaciones.Where(r => r.Cliente.Nombre.ToLower().Contains(Text)));
                        case "DNI cliente":
                            return new ObservableCollection<Reparacion>(Reparaciones.Where(r => r.Cliente.DNI.ToLower().Contains(Text)));
                        case "Nombre empleado":
                            return new ObservableCollection<Reparacion>(Reparaciones.Where(r => r.Empleado.Nombre.ToLower().Contains(Text)));
                        case "DNI empleado":
                            return new ObservableCollection<Reparacion>(Reparaciones.Where(r => r.Empleado.Dni.ToLower().Contains(Text)));
                        default:
                            return Reparaciones;
                    }
                }
                else
                {
                    return Reparaciones;
                }
            }
        }
    }
}
