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
        
        private bool _mostrarTerminados =false;
        private bool _mostrarNoTerminados =false;

        private Reparaciones _reparaciones = SharedDB.Instance.Reparaciones;
        public Reparaciones ReparacionesColection
        {
            get => _reparaciones;
            set
            {
                SetProperty(ref _reparaciones, value);

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

        public ReparacionesViewModel()
        {
           
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
                        
                        _reparaciones.Remove(SelectedRepair);
                       
                        
                        
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
                            _reparaciones = SharedDB.Instance.Reparaciones;
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
        
        [RelayCommand]
        public async Task ModifyRepaisCommand()
        {



            if (SelectedRepair == null || SelectedRepair.FechaFin != new DateTime())
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
                    
               
                Reparacion rep = new Reparacion(ReparacionDlg.AsuntoTb.Text, ReparacionDlg.NotaTb.Text, SelectedRepair.Cliente, empleado);
                        
                Console.WriteLine(rep.ToString());
                        
                SharedDB.Instance.EditReparacion(SelectedRepair, rep);
                       

                ReparacionDlg = null;
                SelectedRepair = null;
                ForceUpdateUI();
               





                
            }


        }


        public bool MostrarTerminados
        {
            get => _mostrarTerminados;
            set
            {
                SetProperty(ref _mostrarTerminados, value);
                List<Reparacion> aux = ReparacionesColection.Reps.Where(r => !r.FechaFin.Equals(new DateTime())).ToList();
                if (_mostrarTerminados)
                {
                    ReparacionesColection.Reps=new ObservableCollection<Reparacion>();
                    ReparacionesColection.Reps = new ObservableCollection<Reparacion>(aux);
                    
                    
                  
                    
                   
                }

                else if(!_mostrarTerminados)
                {
                    ReparacionesColection.Reps= null;
                    ReparacionesColection.Reps = new ObservableCollection<Reparacion>(SharedDB.Instance.Reparaciones.Reps);
                    
                }
                    

                
            }
        }

       


        public bool MostrarNoTerminados
        {
            get => _mostrarNoTerminados;
            set
            {
                SetProperty(ref _mostrarNoTerminados, value);
                List<Reparacion> aux = ReparacionesColection.Reps.Where(r => r.FechaFin.Equals(new DateTime())).ToList();
                if (_mostrarNoTerminados && !_mostrarTerminados)
                {
                    
                    
                    ReparacionesColection.Reps.Clear();
                    
                    foreach (Reparacion rep in aux)
                    {
                        ReparacionesColection.Reps.Add(rep);
                    }
                   
                }
                
                else 
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
            if (_reparaciones.Count > 0)
            {
                var mainWindow =
                    Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                        ? desktop.MainWindow
                        : null;
                var colRep = ReparacionesColection.Reps.OfType<Reparacion>().ToList();
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
                            return new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Asunto.ToLower().Contains(Text)));
                        case "Nota":
                            return new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Nota.ToLower().Contains(Text)));
                        case "Fecha entre":
                            try
                            {
                                var date = DateTime.Parse(Text);
                                return new ObservableCollection<Reparacion>(
                                    ReparacionesColection.Reps.Where(r => r.FechaInicio <= date && r.FechaFin >= date));
                            }
                            catch (FormatException ex)
                            {
                                //TODO: SHOW THIS TO THE USER
                                Console.Out.WriteLine("Fecha no válida");
                                return ReparacionesColection.Reps;
                            }
                        case "Nombre cliente":
                            return new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Cliente.Nombre.ToLower().Contains(Text)));
                        case "DNI cliente":
                            return new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Cliente.DNI.ToLower().Contains(Text)));
                        case "Nombre empleado":
                            return new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Empleado.Nombre.ToLower().Contains(Text)));
                        case "DNI empleado":
                            return new ObservableCollection<Reparacion>(ReparacionesColection.Reps.Where(r => r.Empleado.Dni.ToLower().Contains(Text)));
                        default:
                            return ReparacionesColection.Reps;
                    }
                }
                else
                {
                    return ReparacionesColection.Reps;
                }
            }
        }
        
        
        [RelayCommand]
        public void ForceUpdateUI()
        {

            List<Reparacion> list = SharedDB.Instance.Reparaciones.Reps.ToList();
            ReparacionesColection.Reps.Clear();

            foreach (Reparacion rep in list)
            {
                ReparacionesColection.Add(rep);
            }
            OnPropertyChanged(nameof(ReparacionesColection));


        }
    }
}
