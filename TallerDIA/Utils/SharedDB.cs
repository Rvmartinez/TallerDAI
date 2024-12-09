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
        public Reparaciones Reparaciones { get; }

        private SharedDB()
        {
            CarteraClientes = new CarteraClientes(LoadClientesFromXml());
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
            Cliente c2 = new Cliente { DNI = "11223344", Nombre = "Carlos Garcia", Email = "carlos.garcia@example.com", IdCliente = 2 };
            Empleado emp = new Empleado { Dni = "111", Email = "111",Nombre="rrr"};
            Empleado emp2 = new Empleado { Dni = "222", Email = "222",Nombre="ccc"};
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


        private bool CanAddCliente(Cliente cliente)
        {

            Cliente aux = CarteraClientes.Clientes.Where(c => c.DNI.ToLower() == cliente.DNI.ToLower() || c.Email.ToLower() == cliente.Email.ToLower()).First();

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
            else
            {
                toupdate.Cliente = updated.Cliente;
                toupdate.FechaFin = updated.FechaFin;
                toupdate.FechaInicio = updated.FechaInicio;
                toupdate.Asunto = updated.Asunto;
                toupdate.Nota = updated.Nota;
            }

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
        
        public bool AddReparacion(Reparacion r)
        {
           
            Reparaciones.Add(r);

            return true;
        }
    }
}
