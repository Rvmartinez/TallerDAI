﻿using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using TallerDIA.Models;
using TallerDIA.Views.Dialogs;

namespace TallerDIA.ViewModels
{

    public partial class ReparacionesViewModel : ViewModelBase
    {
        static Cliente cliente1 = new Cliente
        {
            DNI = "12345678A",
            Nombre = "Juan Pérez",
            Email = "juan.perez@example.com",
            IdCliente = 1
        };
        static Cliente cliente2 = new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
        static Cliente cliente3 = new Cliente { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com", IdCliente = 2 };
        static Cliente cliente4 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 3 };

        private List<Reparacion> reparacionesBackup;

        private ObservableCollection<Reparacion> _Reparaciones;

        public ObservableCollection<Reparacion> Reparaciones
        {
            get => _Reparaciones;
            set
            {
                SetProperty(ref _Reparaciones, value);

            }
        }

        private Reparacion _SelectedRepair;
        public Reparacion SelectedRepair
        {
            get => _SelectedRepair;
            set
            {
                SetProperty(ref _SelectedRepair, value);
            }
        }



        private bool _MostrarTerminados;
        public bool MostrarTerminados
        {
            get => _MostrarTerminados;
            set
            {
                SetProperty(ref _MostrarTerminados, value);
                List<Reparacion> aux = reparacionesBackup.Where(r => !r.FechaFin.Equals(new DateTime())).ToList();
                if (aux.Count == 0)
                    return;
                else if (_MostrarTerminados)
                {
                    Reparaciones = new ObservableCollection<Reparacion>(aux);
                }
                else
                    Reparaciones = new ObservableCollection<Reparacion>(reparacionesBackup);
            }
        }
        
        
        private bool _MostrarNoTerminados;
        public bool MostrarNoTerminados
        {
            get => _MostrarNoTerminados;
            set
            {
                SetProperty(ref _MostrarNoTerminados, value);
                List<Reparacion> aux = reparacionesBackup.Where(r => r.FechaFin.Equals(new DateTime())).ToList();
                if (aux.Count == 0)
                    return;
                else if (_MostrarNoTerminados)
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
        static List<Cliente> clientes = new List<Cliente> { cliente1, cliente2, cliente3, cliente4 };

        //static List<Empleado> empleados = new List<Empleado> { empleado1, empleado2 };

        List<String> clientesSorce = new List<String>();
        List<String> empleadosSorce = new List<String>();

        
        public ReparacionesViewModel()
        {
            Reparaciones = new ObservableCollection<Reparacion>();
            Cliente c1 = new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
            Cliente c2 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 2 };
            Empleado emp = new Empleado { Dni = "111", Email = "111",Nombre="rrr"};
            Reparaciones.Add(new Reparacion("asunto1", "nota1", c1, emp));
            Reparaciones.Add(new Reparacion("asunto2", "nota1", c1, emp));
            Reparaciones.Add(new Reparacion("asunto3", "nota1", c1, emp));
            Reparaciones[0].FechaFin = DateTime.Now;
            reparacionesBackup = Reparaciones.ToList();
            foreach (Cliente cliente in clientes)
            {
                clientesSorce.Add(cliente.DNI + "_" + cliente.Nombre);
            }

           /* foreach (Empleado empleado in empleados)
            {
                empleadosSorce.Add(empleado.Dni + "_" + empleado.Nombre);
            }*/
        }

        #region Popup
       


     

        private async void OnButtonCrear()
        {
            /*var textAsunto = this.FindControl<TextBox>("TextAsunto");
            var textNota = this.FindControl<TextBox>("TextNota");
            var listClientes = this.FindControl<AutoCompleteBox>("ListClientes");
            var listEmpleados = this.FindControl<AutoCompleteBox>("ListEmpleados");
            if (listClientes.SelectedItem == null || listEmpleados.SelectedItem == null)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no seleccionados",
                    "Debe de seleccionar un cliente y un empleado", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);

                await message.ShowAsync();
                listClientes.BorderBrush = Brushes.Red;
                listEmpleados.BorderBrush = Brushes.Red;

            }
            else if (textAsunto.Text == "" || textNota.Text == "")
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Alerta de campos no completados",
                    "Debe escribir un Asunto y unha Nota", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning, WindowStartupLocation.CenterOwner);

                await message.ShowAsync();
                textAsunto.BorderBrush = Brushes.Red;
                textNota.BorderBrush = Brushes.Red;
            }
            else

            {
                string[] clienteDniNombre = listClientes.SelectedItem.ToString().Split('_');

                string[] empleadoDniNombre = listEmpleados.SelectedItem.ToString().Split('_');
                Cliente cliente = new Cliente();
                Empleado empleado = new Empleado();

                foreach (Cliente cli in clientes)
                {
                    if (cli.DNI.Equals(clienteDniNombre[0]) && cli.Nombre.Equals(clienteDniNombre[1]))
                    {


                        cliente = new Cliente(cli.DNI, cli.Nombre, cli.Email, cli.IdCliente);

                    }
                }

                foreach (Empleado empl in empleados)
                {
                    if (empl.Dni == empleadoDniNombre[0] && empl.Nombre == empleadoDniNombre[1])
                    {
                        empleado = empl;
                    }
                }




                this.RegistroReparacion.Add(new Reparacion(textAsunto.Text, textNota.Text, cliente, empleado));

                textAsunto.Clear();
                textNota.Clear();
                listClientes.Text = "";
                listEmpleados.Text = "";
                textAsunto.BorderBrush = Brushes.Black;
                textNota.BorderBrush = Brushes.Black;
                listClientes.BorderBrush = Brushes.Black;
                listEmpleados.BorderBrush = Brushes.Black;
            }


            */

        }

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
                        
                        SelectedRepair = null;
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

                SelectedRepair = null;
            }
        }

        [RelayCommand]
        public async Task AddRepaisCommand()
        {
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            var ClienteDlg = new ClienteDlg();
            await ClienteDlg.ShowDialog(mainWindow);

            if (!ClienteDlg.IsCancelled)
            {
               // Cliente c  = new Cliente() { DNI = ClienteDlg.DniTB.Text, Email = ClienteDlg.EmailTB.Text, Nombre = ClienteDlg.NombreTB.Text, IdCliente = this.GetLastClientId()+1 };
               
                    //Reparaciones.Add(c);
                
            }
        
        }

       
    }
}