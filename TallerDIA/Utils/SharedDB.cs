using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
            Garaje = new GarajeCoches(LoadGarajeCochesXml("coches.xml"));
            Reparaciones = new Reparaciones(LoadReparacionesFromXml());
            Console.WriteLine("Comprobar lista reparaciones");
            for (int i = 0; i < Reparaciones.Count; i++)
            {
                Console.WriteLine(Reparaciones.Get(i).ToString());
            }
            
        }

        private ObservableCollection<Reparacion> LoadReparacionesFromXml(string filePath = "")
        {
            Cliente c1 = new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
            Cliente c2 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 3 };
            
            Empleado emp = new Empleado { Dni = "12345678A", Nombre = "Gonzalo Gonzalez", Email = "goonzaloz@gmail.com"};
            Empleado emp2 = new Empleado { Dni = "87654321B", Nombre = "Bort Sing-Song", Email = "boruto@hotmail.com"};
           return new ObservableCollection<Reparacion>
            {
                new Reparacion("asunto1", "nota1", c1, emp),
                new Reparacion("asunto2", "nota2", c2, emp),
                new Reparacion("asunto3", "nota3", c1, emp2)
                
            };
           
        }

        public ObservableCollection<Cliente> LoadClientesFromXml(string filePath = "")
        {
            return new ObservableCollection<Cliente>
            {
                new Cliente { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 },
                new Cliente { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com", IdCliente = 2 },
                new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 3 }
            };
        }
        
        public ObservableCollection<Coche> LoadGarajeCochesXml(string filePath)
        {
            var c1 = new Cliente
                { DNI = "12345678", Nombre = "Juan Perez", Email = "juan.perez@example.com", IdCliente = 1 };
            var c2 = new Cliente
                { DNI = "87654321", Nombre = "Ana Lopez", Email = "ana.lopez@example.com", IdCliente = 2 };
            var c3 = new Cliente
                { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 3 };
            return new ObservableCollection<Coche>
            {
                new Coche("4089fks", Coche.Marcas.Citroen, "c3",c1),
                new Coche("1234trt", Coche.Marcas.Ferrari, "rojo",c2),
                new Coche("9876akd", Coche.Marcas.Lamborghini, "huracan",c3),
            };
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
            return CarteraClientes.Clientes.OrderByDescending(c => c.IdCliente).FirstOrDefault().IdCliente;
        }
        
        public bool EditReparacion(Reparacion reparacion,Reparacion updated)
        {
            Reparacion toupdate = Reparaciones.Get(reparacion);

            if (toupdate == null)
            {
                return false;
            }
            
            
               //toupdate.Cliente = updated.Cliente;
                //toupdate.FechaFin = updated.FechaFin;
                //toupdate.FechaInicio = updated.FechaInicio;
                toupdate.Asunto = updated.Asunto;
                toupdate.Nota = updated.Nota;
                toupdate.Empleado = updated.Empleado;
                return true;
            

            
        }


        public bool EditClient(Cliente cliente,Cliente updated)
        {
            Cliente toupdate = CarteraClientes.Clientes.Where(c => c.IdCliente == cliente.IdCliente).FirstOrDefault();

            if (toupdate == null) return false;

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
        

            
        
        
        
        ///////////////// SECCION DE EMPLEADOS DE ALEX ////////////////////////
        
        public ObservableCollection<Empleado> LoadEmpleadosFromXml(string filePath = "")
        {
            return new ObservableCollection<Empleado>
            {
                new Empleado { Dni = "12345678A", Nombre = "Gonzalo Gonzalez", Email = "goonzaloz@gmail.com"},
                new Empleado { Dni = "87654321B", Nombre = "Bort Sing-Song", Email = "boruto@hotmail.com"},
                new Empleado { Dni = "99999999Z", Nombre = "Missing No", Email = "error@example.org"}
            };
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
        
        public bool AddReparacion(Reparacion r)
        {
           
            Reparaciones.Add(r);

            return true;
        }

    }
}
