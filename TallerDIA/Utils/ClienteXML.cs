using System;
namespace TallerDIA.Utils;
using Models;
using System.Xml;


public class ClienteXML
{
    public static void GuardarCartera(CarteraClientes cartera)
    {
        string rutaArchivo = "../../../XmlFiles/cartera.xml";
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("CarteraClientes");
        doc.AppendChild(root);

        foreach (var cliente in cartera.Clientes)
        {
            XmlElement clienteElement = doc.CreateElement("Cliente");

            XmlElement dniElement = doc.CreateElement("DNI");
            dniElement.InnerText = cliente.DNI;
            clienteElement.AppendChild(dniElement);

            XmlElement nombreElement = doc.CreateElement("Nombre");
            nombreElement.InnerText = cliente.Nombre;
            clienteElement.AppendChild(nombreElement);
            
            XmlElement mailElement = doc.CreateElement("Email");
            mailElement.InnerText = cliente.Email;
            clienteElement.AppendChild(mailElement);
            
            
            XmlElement idElement = doc.CreateElement("IdCliente");
            idElement.InnerText = cliente.IdCliente.ToString();
            clienteElement.AppendChild(idElement);

            root.AppendChild(clienteElement);
        }

        try
        {
            doc.Save(rutaArchivo);
            Console.WriteLine("Archivo XML guardado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("No se ha podido guardar el archivo");
        }
    }
    
    public static void InsertarEnXml(XmlElement parent, XmlDocument doc, string dni, string nombre, string email, int idCliente)
    {
        XmlElement clienteElement = doc.CreateElement("Cliente");

        XmlElement dniElement = doc.CreateElement("DNI");
        dniElement.InnerText = dni;
        clienteElement.AppendChild(dniElement);

        XmlElement nombreElement = doc.CreateElement("Nombre");
        nombreElement.InnerText = nombre;
        clienteElement.AppendChild(nombreElement);

        XmlElement emailElement = doc.CreateElement("Email");
        emailElement.InnerText = email;
        clienteElement.AppendChild(emailElement);
        
        XmlElement idElement = doc.CreateElement("IdCliente");
        idElement.InnerText = idCliente.ToString();
        clienteElement.AppendChild(idElement);

        parent.AppendChild(clienteElement);
    }
    public static Cliente CargarDeXml(XmlElement clienteElement)
    {
        try
        {
            string dni = clienteElement["DNI"].InnerText;
            string nombre = clienteElement["Nombre"].InnerText;
            string email = clienteElement["Email"].InnerText;
            int idCliente = Convert.ToInt32(clienteElement["IdCliente"].InnerText);

            return new Cliente(dni, nombre, email, idCliente)
            {
                DNI = dni,
                Nombre = nombre,
                Email = email,
                IdCliente = idCliente
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        throw new XmlException("Error al cargar XML");
    }
}