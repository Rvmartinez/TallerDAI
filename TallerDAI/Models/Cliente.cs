using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerDAI.Models
{
    public class Cliente
    {
        public required string DNI { get; set; }
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public required int IdCliente { get; set; }

        public Cliente() { }

    }
}
