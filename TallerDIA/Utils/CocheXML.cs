using System;
namespace TallerDIA.Utils;
using Models;
using System.Xml;


public class CocheXML
{
    public static void GuardarGaraje(GarajeCoches garaje)
    {
        string rutaArchivo = "../../../XmlFiles/garaje.xml";
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

            root.AppendChild(cocheElement);
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
    public static Coche CargarDeXml(XmlElement cocheElement)
    {
        try
        {
            string matricula = cocheElement["Matricula"].InnerText;
            string marca = cocheElement["Marca"].InnerText;
            string modelo = cocheElement["Modelo"].InnerText;
            if (Enum.TryParse(marca, true, out Coche.Marcas marcas))
            {
                return new Coche(matricula, marcas, modelo);
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