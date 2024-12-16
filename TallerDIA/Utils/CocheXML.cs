using System;
namespace TallerDIA.Utils;
using Models;
using System.IO;
using System.Xml;


public class CocheXML
{
    public static void GuardarGaraje(GarajeCoches garaje)
    {

        String rutaArchivo = Settings.Instance.GetFilepath("garaje");
        string directorio = Path.GetDirectoryName(rutaArchivo) ?? "";
        if (directorio == "")
            return;
        else if (!Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }


        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("GarajeCoches");
        doc.AppendChild(root);

        foreach (var coche in garaje.Coches)
        {
            XmlElement cocheElement = doc.CreateElement("Coche");

            XmlElement matriculaElement = doc.CreateElement("Matricula");
            matriculaElement.InnerText = coche.Matricula;
            cocheElement.AppendChild(matriculaElement);

            XmlElement marcaElement = doc.CreateElement("Marca");
            marcaElement.InnerText = coche.Marca.ToString();
            cocheElement.AppendChild(marcaElement);

            XmlElement modeloElement = doc.CreateElement("Modelo");
            modeloElement.InnerText = coche.Modelo;
            cocheElement.AppendChild(modeloElement);

            ClienteXML.InsertarEnXml(cocheElement, doc, coche.Owner.DNI, coche.Owner.Nombre,
                coche.Owner.Email, coche.Owner.IdCliente);

            root.AppendChild(cocheElement);
        }

        try
        {
            doc.Save(rutaArchivo);
            Console.WriteLine("Archivo XML guardado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public static Coche CargarDeXml(XmlElement cocheElement)
    {
        try
        {
            string matricula = cocheElement["Matricula"]?.InnerText ?? "";
            string marca = cocheElement["Marca"]?.InnerText ?? "";
            string modelo = cocheElement["Modelo"]?.InnerText ?? "";
            XmlElement clienteElement = cocheElement["Cliente"];
            Cliente cliente = ClienteXML.CargarDeXml(clienteElement);
            if (Enum.TryParse(marca, true, out Coche.Marcas marcas))
            {
                return new Coche(matricula, marcas, modelo, cliente);
            }
            else
            {
                throw new Exception("El texto no coincide con ningún valor del enum.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        throw new XmlException("Error al cargar XML");
    }
}