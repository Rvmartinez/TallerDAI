using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TallerDIA.Models;

namespace TallerDIA.Utils
{
    public class SharedDB
    {
        private static SharedDB _instance;
        public static SharedDB Instance => _instance ??= new SharedDB();
        public ObservableCollection<Cliente> Clientes { get; }

        private SharedDB()
        {
            Clientes = LoadClientesFromXml();
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
    }
}
