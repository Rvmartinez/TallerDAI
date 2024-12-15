using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TallerDIA.Models
{
    public class CarteraClientes
    {
        private List<Cliente> _Clientes;

        public List<Cliente> Clientes
        {
            get => _Clientes;
            private set => _Clientes = value;
        }
        public int Count => Clientes.Count;

        /// <summary>
        /// Crea el garaje de Clientes a partir de una coleccion de Clientes pasada por argumento.
        /// </summary>
        /// <param name="Clientes"></param>
        public CarteraClientes(IEnumerable<Cliente> c)
        {
            Clientes = new List<Cliente>(c);
        }

        /// <summary>
        /// Aumenta el garaje con la lista de Clientes pasados como argumento
        /// </summary>
        /// <param name="Clientes"></param>
        public void AddRange(IEnumerable<Cliente> c)
        {
            foreach (Cliente Cliente in c)
            {
                Clientes.Add(Cliente);
            }
        }

        /// <summary>
        /// Añade la instancia de Cliente pasada como argumento al garaje de Clientes.
        /// </summary>
        /// <param name="Cliente"></param>
        public void Add(Cliente Cliente)
        {
            Clientes.Add(Cliente);
        }


        /// <summary>
        /// Elimina la primera instancia del Cliente con la matricula asociada.
        /// En caso de encontrar la matricula elimina el Cliente con ella y devuelve true.
        /// En caso de no encontrar la matricula devuelve false.
        /// </summary>
        /// <param name="matricula"></param>
        public bool RemoveCliente(string dni)
        {

            Cliente selectedClient = Clientes.Where(c => c.DNI.ToUpper() == dni.ToUpper()).First();
            if (selectedClient != null)
            {
                Clientes.Remove(selectedClient);
                return true;
            
            }
            return false;
        }

        public bool RemoveCliente(Cliente cl)
        {

            Cliente selectedClient = Clientes.Where(c => c.IdCliente == cl.IdCliente).First();
            if (selectedClient != null)
            {
                Clientes.Remove(selectedClient);
                return true;

            }
            return false;
        }

        /// <summary>
        /// Devuelve el Cliente que se encuentre en la i posición.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Cliente Get(int i)
        {
            return Clientes[i];
        }

        /// <summary>
        /// Devuelve el Cliente con la matricula que se pasa por parametro, en caso de no encontrarlo devuelve null.
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public Cliente GetUserByName(string name) => Clientes.Where(c => c.Nombre.ToUpper() == name.ToUpper()).First();
        public Cliente GetUserByDNI(string dni) => Clientes.Where(c => c.DNI.ToUpper() == dni.ToUpper()).First();
        public Cliente GetUserByEmail(string email) => Clientes.Where(c => c.Email.ToUpper() == email.ToUpper()).First();

        public bool ActualizarCliente(Cliente forUpdate,string dni, string nombre,string email)
        {
            Cliente c = Clientes.Where(c => c.IdCliente == forUpdate.IdCliente).First();

            if (c != null)
            {
                c.Email = email;
                c.Nombre = nombre;
                c.DNI = dni;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder toret = new StringBuilder();
            toret.Append("Numero de Clientes en la cartera: ");
            toret.Append(Count);
            toret.Append("\n");
            foreach (var Cliente in this.Clientes)
            {
                toret.Append(Cliente.ToString());
                toret.Append("\n");
            }
            return toret.ToString();
        }


        public void Clear()
        {
            Clientes.Clear();
        }
    }
}


