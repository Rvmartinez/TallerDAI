using System.Collections.Generic;
using System.Xml.Linq;
using TallerDAI.Models;

namespace TallerDAI.Utils;

public class EmpleadoXML
{
    private Empleado Empleado { get; set; }
    public XElement ToXml()
    {
        XElement toret = new XElement("Empleado",
           new XAttribute("Nombre", Empleado.Nombre),
           new XAttribute("Dni", Empleado.Dni),
           new XAttribute("Email", Empleado.Email),
           new XAttribute("Disponible", Empleado.Disponible));
        List<string> ticks = new List<string>(Empleado.Tickets);
        foreach (var tick in ticks)
        {
            XElement tickXML = new XElement("Ticket", tick);
            toret.Add(tickXML);
        }
        return toret;
    }

    public Empleado FromXml(XElement xet)
    {
        Empleado emp = new Empleado();
        XElement xet2 = xet.Element("Empleado");
        emp.Nombre = xet2.Attribute("Nombre").ToString();
        emp.Dni = xet2.Attribute("Dni").ToString();
        emp.Email = xet2.Attribute("Email").ToString();
        emp.Disponible = false;
        if (xet2.Attribute("Disponible").Value.ToString() == "true")
        {
            emp.Disponible = true;
        }
        return emp;
    }
}