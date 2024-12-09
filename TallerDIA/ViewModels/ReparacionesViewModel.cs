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
using GraficosTaller.UI;
using TallerDIA.Models;
using TallerDIA.Views;
using TallerDIA.Views.Dialogs;

namespace TallerDIA.ViewModels
{

    public partial class ReparacionesViewModel : ViewModelBase
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
        public bool MostrarTerminados
        {
            get => _mostrarTerminados;
            set
            {
                SetProperty(ref _mostrarTerminados, value);
                List<Reparacion> aux = reparacionesBackup.Where(r => !r.FechaFin.Equals(new DateTime())).ToList();
                if (aux.Count == 0 || _mostrarTerminados)
                {
                    _mostrarNoTerminados = false;
                    
                    Reparaciones = new ObservableCollection<Reparacion>(aux);
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
                if (aux.Count == 0 || _mostrarNoTerminados)
                {
                    _mostrarTerminados = false;
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
        static List<Cliente> _clientes = new List<Cliente> { _cliente1, _cliente2, _cliente3, _cliente4 };

        //static List<Empleado> empleados = new List<Empleado> { empleado1, empleado2 };

        //List<String> clientesSorce = new List<String>();
        //List<String> empleadosSorce = new List<String>();

        
        public ReparacionesViewModel()
        {
            Reparaciones = new ObservableCollection<Reparacion>();
            Cliente c1 = new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
            Cliente c2 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 2 };
            Empleado emp = new Empleado { Dni = "111", Email = "111",Nombre="rrr"};
            Reparaciones.Add(new Reparacion("asunto1", "nota1", c1, emp));
            Reparaciones.Add(new Reparacion("asunto2", "nota1", c2, emp));
            Reparaciones.Add(new Reparacion("asunto3", "nota1", c1, emp));
            Reparaciones[0].FechaFin = DateTime.Now;
            reparacionesBackup = Reparaciones.ToList();
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

            if (!ReparacionDlg.IsCancelled)
            {
               // Cliente c  = new Cliente() { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = this.GetLastClientId()+1 };
               
                    //Reparaciones.Add(c);
                
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
                var reps = new Reparaciones();
                reps.AnadirReparaciones(colRep);
                var reparacionNavegarDlg = new ChartWindow(reps, null);
                await reparacionNavegarDlg.ShowDialog(mainWindow);
            }
            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("No hay reparaciones",
                    "No hay reparaciones que mostrar", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);

                var respuesta = await message.ShowAsync();
            }


        }
        
        

       
    }
}
