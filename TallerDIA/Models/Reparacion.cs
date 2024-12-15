
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerDIA.Models
{
    public class Reparacion
    {

       
        private Cliente _cliente;
        private Empleado _empleado;
        private string _asunto;
        private DateTime _fechaInicio;
        private DateTime? _fechaFin;
        private string _nota;
        private static string toret = "11/11/1111 11:11:11";
        private static readonly DateTime _BASE_FINFECHA = DateTime.Parse(toret);

        public Reparacion(string asunto, string nota)
        {
            this.FechaInicio = DateTime.Now;
            this.Asunto = asunto;
            this.Nota = nota;
        }
        public Reparacion(string asunto, string nota, Cliente cliente, Empleado empleado)
        {
            this.FechaInicio = DateTime.Now;
            this.Asunto = asunto;
            this.Nota = nota;
            this.Cliente = cliente;
            this.Empleado = empleado;
            this._fechaFin = _BASE_FINFECHA;
        }

        public Reparacion()
        {
            throw new NotImplementedException();
        }


        public Cliente Cliente
        {
            get => _cliente;

            set => _cliente = value;
        }

        public Empleado Empleado
        {
            get => _empleado;

            init => _empleado = value;
        }


        public string Asunto
        {
            get => _asunto;

            set => _asunto = value;
        }

        public DateTime FechaInicio
        {
            get;
            set;

        }



        public DateTime FechaFin
        {
            get => (DateTime)_fechaFin;
            set => _fechaFin = value;
        }

        public string Nota
        {
            get => _nota;

            set => _nota = value;
        }
        public string ClienteNombre
        {
            get => _cliente.Nombre;

        }
        public string EmpleadoNombre
        {
            get => _empleado.Nombre;

        }




        public void asignarFechaFin()
        {
            FechaFin = DateTime.Now;
        }

       
            
        public int? GetAnno(Boolean fin)
        {
            if(fin)
            {
                if(FechaFin.Equals(new DateTime()))
                {
                    return null;
                }
                else
                {
                    return FechaFin.Year;
                }
            }
            else
            {
                return FechaInicio.Year;
            }
        }
        
        public int? GetMes(Boolean fin)
        {
            if(fin)
            {
                if(FechaFin.Equals(new DateTime()))
                {
                    return null;
                }
                else
                {
                    return FechaFin.Month;
                }
            }
            else
            {
                return FechaInicio.Month;
            }
        }

        public override string ToString()
        {
            return "" + Asunto + "," + Nota + "," + Cliente.ToString() + "," + Empleado.ToString();
        }
    }
    
}
