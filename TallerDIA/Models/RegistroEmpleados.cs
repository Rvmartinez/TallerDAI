using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TallerDIA.Models
{
    public class RegistroEmpleados
    {
        private ObservableCollection<Empleado> _Empleados;

        public ObservableCollection<Empleado> Empleados
        {
            get => _Empleados;
            private set => _Empleados = value;
        }
        public int Count => Empleados.Count;

        /// <summary>
        /// Crea el registro de Empleados a partir de una coleccion de Empleados pasada por argumento.
        /// </summary>
        /// <param name="Empleados"></param>
        public RegistroEmpleados(IEnumerable<Empleado> e)
        {
            Empleados = new ObservableCollection<Empleado>(e);
        }

        /// <summary>
        /// Aumenta el registro con la lista de Empleados pasados como argumento
        /// </summary>
        /// <param name="Empleado"></param>
        public void AddRange(IEnumerable<Empleado> e)
        {
            foreach (Empleado empleado in e)
            {
                Empleados.Add(empleado);
            }
        }

        /// <summary>
        /// Añade la instancia de Empleado pasada como argumento al registro de Empleados.
        /// </summary>
        /// <param name="Empleado"></param>
        public void Add(Empleado empleado)
        {
            Empleados.Add(empleado);
        }


        /// <summary>
        /// Elimina la primera instancia del Empleado con la matricula asociada.
        /// En caso de encontrar los datos elimina el Empleado con ella y devuelve true.
        /// En caso de no encontrar los datos devuelve false.
        /// </summary>
        /// <param name="datosempleado"></param>
        public bool RemoveEmpleado(string dni)
        {

            Empleado selectedEmp = Empleados.Where(c => c.Dni.ToUpper() == dni.ToUpper()).First();
            if (selectedEmp != null)
            {
                Empleados.Remove(selectedEmp);
                return true;
            
            }
            return false;
        }

        public bool RemoveEmpleado(Empleado em)
        {

            Empleado selectedEmp = Empleados.Where(e => e.Dni == em.Dni).First();
            if (selectedEmp != null)
            {
                Empleados.Remove(selectedEmp);
                return true;

            }
            return false;
        }

        /// <summary>
        /// Devuelve el Empleado que se encuentre en la i posición.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Empleado Get(int i)
        {
            return Empleados[i];
        }

        /// <summary>
        /// Devuelve el Empleado con los datos que se pasam por parametro, en caso de no encontrarlo devuelve null.
        /// </summary>
        /// <param name="datosempleado"></param>
        /// <returns></returns>
        public Empleado GetEmpByName(string name) => Empleados.Where(e => e.Nombre.ToUpper() == name.ToUpper()).First();
        public Empleado GetEmpByDni(string dni) => Empleados.Where(e => e.Dni.ToUpper() == dni.ToUpper()).First();
        public Empleado GetEmpByEmail(string email) => Empleados.Where(e => e.Email.ToUpper() == email.ToUpper()).First();

        public bool ActualizarEmpleado(Empleado forUpdate,string dni, string nombre,string email)
        {
            Empleado c = Empleados.Where(c => c.Dni == forUpdate.Dni).First();

            if (c != null)
            {
                c.Email = email;
                c.Nombre = nombre;
                c.Dni = dni;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder toret = new StringBuilder();
            toret.Append("Numero de Empleados en el registro: ");
            toret.Append(Count);
            toret.Append("\n");
            foreach (var empleado in this.Empleados)
            {
                toret.Append(empleado.ToString());
                toret.Append("\n");
            }
            return toret.ToString();
        }


        public void Clear()
        {
            Empleados.Clear();
        }
    }
}


