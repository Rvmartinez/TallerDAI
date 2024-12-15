using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using TallerDIA.Models;
using TallerDIA.Views.Dialogs;

namespace TallerDIA.Utils
{
    public class SharedDB
    {
        private static SharedDB _instance;
        public static SharedDB Instance => _instance ??= new SharedDB();
        public CarteraClientes CarteraClientes { get; }

        public RegistroEmpleados RegistroEmpleados { get; }

        public GarajeCoches Garaje { get; }
        public Reparaciones Reparaciones { get; }


        private SharedDB()
        {
            CarteraClientes = new CarteraClientes(LoadClientesFromXml());
            RegistroEmpleados = new RegistroEmpleados(LoadEmpleadosFromXml());
            Garaje = new GarajeCoches(LoadGarajeCochesXml());
            Reparaciones = new Reparaciones(LoadReparacionesFromXml());
            Console.WriteLine("Comprobar lista reparaciones");
            for (int i = 0; i < Reparaciones.Count; i++)
            {
                Console.WriteLine(Reparaciones.Get(i).ToString());
            }
            
        }

        


        private bool CanAddCliente(Cliente cliente)
        {
            
            Cliente aux = CarteraClientes.Clientes.Where(c => c.DNI.ToLower() == cliente.DNI.ToLower() || c.Email.ToLower() == cliente.Email.ToLower()).FirstOrDefault();

            return  aux == null;
        }

        public void BajaCliente(Cliente cliente)
        {
            CarteraClientes.RemoveCliente(cliente);
        }
        public void EliminarReparacion(Reparacion reparacion)
        {
            Reparaciones.Remove(reparacion);
        }

        public void BajaClienteByDNI(string dni)
        {
            CarteraClientes.RemoveCliente(ConsultaClienteByDni(dni));
        }

        public Cliente ConsultaCliente(int clienteId) => CarteraClientes.Clientes.FirstOrDefault(c => c.IdCliente == clienteId);
        public Cliente ConsultaClienteByDni(string dni) => CarteraClientes.Clientes.FirstOrDefault(c => c.DNI.Trim().ToUpper() == dni.Trim().ToUpper());
        private int GetLastClientId()
        {
            Cliente c = CarteraClientes.Clientes.OrderByDescending(c => c.IdCliente).FirstOrDefault();
            if (c != null)
                return c.IdCliente;
            else 
                return 0;
        }
        
        public bool EditReparacion(Reparacion reparacion,Reparacion updated)
        {
            Reparacion toupdate = Reparaciones.Get(reparacion);

            if (toupdate == null)
            {
                return false;
            }
            Reparaciones.Remove(toupdate);
            Reparaciones.Add(updated);
            
               
                return true;
            

            
        }


        public bool EditClient(Cliente cliente,Cliente updated)
        {
            Cliente toupdate = CarteraClientes.Clientes.Where(c => c.IdCliente == cliente.IdCliente).FirstOrDefault();

            if (toupdate == null) return false;
            EditarClienteDeCoches(toupdate, updated);
            toupdate.DNI = updated.DNI;
            toupdate.Nombre = updated.Nombre;
            toupdate.Email = updated.Email;
            
            

            return true;
        }

        public bool AddClient(Cliente c)
        {
            if(!CanAddCliente(c)) 
                return false;
           
            c.IdCliente = GetLastClientId()+1;
            CarteraClientes.Add(c);

            return true;
        }
        
        
        public static bool FiltrarEntradasEmpleado(Empleado empleado)
        {
            if (empleado != null)
            {
                if (empleado.Email == null || empleado.Nombre == null || empleado.Email == null)
                {
                    Console.Out.WriteLine("Empleado con algun campo nulo.");
                    return false;
                }
                else if (empleado.Email.Trim()=="" || empleado.Nombre.Trim() == "" || empleado.Email.Trim()=="")
                {
                    Console.Out.WriteLine("Empleado con algun campo no introducido.");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                Console.Out.WriteLine("Empleado nulo.");
                return false;
            }
        }
        public static Empleado BuscarEmpleado(List<Empleado> listaEmpleados,Empleado empleado)
        {
            if (listaEmpleados!=null && listaEmpleados.IndexOf(listaEmpleados.Find(x => x.Dni == empleado.Dni)) != -1)
            {
                return empleado;
            }
            else
            {
                return null;
            }
        }
        

        public bool RemoveCar(string matricula)
        {
            return Garaje.RemoveMatricula(matricula);
        }

        public bool AddCar(Coche c)
        {
            if (c is null) return false;
            return Garaje.Add(c);
        }

        public bool EditCarMatricula(Coche antiguo, string matriculaNueva)
        {
            if (Garaje.RemoveMatricula(antiguo.Matricula))
            {
                return Garaje.Add(new Coche(matriculaNueva, antiguo.Marca, antiguo.Modelo, antiguo.Owner));
            }

            return false;
        }

        public Coche getCocheMatricula(Coche c)
        {
            return Garaje.GetMatricula(c.Matricula);
        }

        public void EditarClienteDeCoches(Cliente antiguo, Cliente nuevo)
        {
            if (antiguo is not null && nuevo is not null)
            {
                foreach (var car in Garaje.Coches)
                {
                    if (car.Owner.DNI == antiguo.DNI)
                    {
                        car.Owner = nuevo;
                    }
                }
            }
        }
        
        public bool AddReparacion(Reparacion r)
        {
           
            Reparaciones.Add(r);

            return true;
        }

        #region XML

        public ObservableCollection<Empleado> LoadEmpleadosFromXml()
        {
            string filePath = Settings.Instance.GetFilepath("plantilla");
            List<Empleado> toret = new();
            if (File.Exists(filePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    Console.WriteLine("Archivo XML cargado exitosamente.");

                    // Mostrar contenido del archivo XML (opcional)
                    Console.WriteLine(xmlDoc.OuterXml);
                    XmlNodeList empleados = xmlDoc.GetElementsByTagName("Empleado");

                    foreach (XmlElement empleado in empleados)
                    {
                        toret.Add(EmpleadoXML.CargarDeXml(empleado));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar el archivo XML: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe. Verifique la ruta.");
            }

            return new ObservableCollection<Empleado>(toret);
        }

        private ObservableCollection<Reparacion> LoadReparacionesFromXml()
        {
            String filePath = Settings.Instance.GetFilepath("trabajo");

            Console.WriteLine("Importando Reparaciones...");
            Console.WriteLine(filePath);
            ObservableCollection<Reparacion> trabajos = new ObservableCollection<Reparacion>();
            if (File.Exists(filePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    Console.WriteLine("Archivo XML cargado exitosamente.");

                    Console.WriteLine(xmlDoc.OuterXml);
                    XmlNodeList reparaciones = xmlDoc.GetElementsByTagName("Reparacion");

                    foreach (XmlElement reparacion in reparaciones)
                    {
                        trabajos.Add(ReparacionXML.CargarDeXml(reparacion));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar el archivo XML: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe. Verifique la ruta.");
            }

            return trabajos;
        }

        public ObservableCollection<Cliente> LoadClientesFromXml()
        {
            string filePath = Settings.Instance.GetFilepath("clientes");

            List<Cliente> toret = new List<Cliente>();
            if (File.Exists(filePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    Console.WriteLine("Archivo XML cargado exitosamente.");

                    // Mostrar contenido del archivo XML (opcional)
                    Console.WriteLine(xmlDoc.OuterXml);
                    XmlNodeList clientes = xmlDoc.GetElementsByTagName("Cliente");

                    foreach (XmlElement cliente in clientes)
                    {
                        toret.Add(ClienteXML.CargarDeXml(cliente));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar el archivo XML: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe. Verifique la ruta.");
            }

            return new ObservableCollection<Cliente>(toret);
        }

        public ObservableCollection<Coche> LoadGarajeCochesXml()
        {
            string filePath = Settings.Instance.GetFilepath("garaje");
            List<Coche> toret = new List<Coche>();
            if (File.Exists(filePath))
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    Console.WriteLine("Archivo XML cargado exitosamente.");
                    Console.WriteLine(xmlDoc.OuterXml);
                    XmlNodeList coches = xmlDoc.GetElementsByTagName("Coche");

                    foreach (XmlElement coche in coches)
                    {
                        toret.Add(CocheXML.CargarDeXml(coche));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar el archivo XML: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo no existe. Verifique la ruta.");
            }

            return new ObservableCollection<Coche>(toret);
        }
        #endregion


        public void SaveAllXml()
        {
            ReparacionXML.GuardarEnXML(Reparaciones.Reps);
            CocheXML.GuardarGaraje(this.Garaje);
            ClienteXML.GuardarCartera(this.CarteraClientes);
            EmpleadoXML.GuardarEmpleados(this.RegistroEmpleados.Empleados);
        }
        

    }
}