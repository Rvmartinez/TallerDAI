using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using TallerDIA.Models;
using TallerDIA.Utils;
namespace TallerDIA.Utils;

public class ReparacionXML
{

    public static void GuardarEnXML(ObservableCollection<Reparacion> trabajos)
    {
        XmlDocument doc = new XmlDocument();

        XmlElement root = doc.CreateElement("Trabajos");
        doc.AppendChild(root);

        String rutaArchivo = Settings.Instance.GetFilepath("trabajo");
        string directorio = Path.GetDirectoryName(rutaArchivo) ?? "";
        if (directorio == "")
            return;
        else if (!Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }


        foreach (var reparacion in trabajos)
        {
            XmlElement reparacionElement = doc.CreateElement("Reparacion");


            EmpleadoXML.InsertarEnXml(reparacionElement, doc, reparacion.Empleado.Dni, reparacion.Empleado.Nombre,
                reparacion.Empleado.Email);

            ClienteXML.InsertarEnXml(reparacionElement, doc, reparacion.Cliente.DNI, reparacion.Cliente.Nombre,
                reparacion.Cliente.Email, reparacion.Cliente.IdCliente);

            XmlElement asuntoElement = doc.CreateElement("Asunto");
            asuntoElement.InnerText = reparacion.Asunto;
            reparacionElement.AppendChild(asuntoElement);

            XmlElement fechaInicialElement = doc.CreateElement("FechaInicio");
            fechaInicialElement.InnerText = reparacion.FechaInicio.ToString("yyyy-MM-dd HH:mm:ss");
            reparacionElement.AppendChild(fechaInicialElement);

            XmlElement fechaFinalElement = doc.CreateElement("FechaFin");
            fechaFinalElement.InnerText = reparacion.FechaFin.ToString("yyyy-MM-dd HH:mm:ss");
            reparacionElement.AppendChild(fechaFinalElement);

            XmlElement notasElement = doc.CreateElement("Nota");
            notasElement.InnerText = reparacion.Nota;
            reparacionElement.AppendChild(notasElement);

            root.AppendChild(reparacionElement);
        }

        try
        {
            Console.WriteLine(AppContext.BaseDirectory);
            doc.Save(rutaArchivo);
            Console.WriteLine("Archivo XML guardado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static Reparacion CargarDeXml(XmlElement root)
    {

        XmlElement empleadoElement = root["Empleado"];
        Empleado trabajador = EmpleadoXML.CargarDeXml(empleadoElement);

        XmlElement clienteElement = root["Cliente"];
        Cliente cliente = ClienteXML.CargarDeXml(clienteElement);

        string asunto = root["Asunto"].InnerText;
        Console.WriteLine("Cargado asunto");

        DateTime fechaInicial = DateTime.Parse(root["FechaInicio"]?.InnerText);
        DateTime fechaFinal = DateTime.Parse(root["FechaFin"]?.InnerText);

        string notas = root["Nota"].InnerText;

        return new Reparacion(trabajador, cliente, asunto, fechaInicial, fechaFinal, notas);
    }
}
