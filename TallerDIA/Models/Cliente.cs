using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerDIA.Models
{
    public class Cliente
    {
        private string dni;
        private string nombre;
        private string email;
        private int idCliente;
        private Coche mainCar;

        public required string DNI
        {
            get => dni;
            set => dni = value;
        }

        public required string Nombre
        {
            get => nombre;
            set => nombre = value;
        }

        public required string Email
        {
            get => email;
            set => email = value;
        }

        public required int IdCliente
        {
            get => idCliente;
            set => idCliente = value;
        }

        public Cliente() { }
        public Cliente(string dni, string nombre, string email, int idCliente)
        {
            DNI = dni;
            Nombre = nombre;
            Email = email;
            IdCliente = idCliente;
        }

    }
}
