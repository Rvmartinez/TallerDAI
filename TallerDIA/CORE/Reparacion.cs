using System;
using System.Collections.Generic;


namespace Gestión_de_reparaciones.CORE;

public class Reparacion
{
      
        
        private Cliente _cliente;
        private Empleado _empleado;
        private string _asunto;
        private DateTime _fechaInicio;
        private DateTime? _fechaFin;
        private string _nota;

        public Reparacion(string asunto, string nota)
        {
                                this.FechaInicio = DateTime.Now;
                                this.Asunto = asunto;
                                this.Nota = nota;
        }
        public Reparacion(string asunto, string nota,Cliente cliente, Empleado empleado)
        {
                this.FechaInicio = DateTime.Now;
                this.Asunto = asunto;
                this.Nota = nota;
                this.Cliente = cliente;
                this.Empleado = empleado;
                this._fechaFin = new DateTime();
        }
        
        
        public Cliente Cliente
        {
                get => _cliente;

               init  => _cliente = value;
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
                get ;
                init;

        }

        

        public DateTime FechaFin
        {
                get => (DateTime)_fechaFin;
                set=>_fechaFin = value;
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

        public override string ToString()
        {
                return "Fecha Inicio: " + FechaInicio.ToString("dd/MM/yyyy") + ", Asunto: " + Asunto + ", Nota: " + Nota + "Fecha Fin: " + FechaInicio.ToString("dd/MM/yyyy") + ", Cliente: " + ClienteNombre + ", Empleado: " + EmpleadoNombre;
        }
}