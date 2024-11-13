using System.Collections.Generic;
using System.Xml.Linq;

namespace ConsoleApp1;

public class EmpleadoXML
{
    private Empleado Empleado { get; set; }
    public XElement ToXml()
    {
        XElement toret = new XElement("Empleado",
            new XAttribute("Nombre", this.Empleado.Nombre),
            new XAttribute("Dni", this.Empleado.Dni),
            new XAttribute("Email", this.Empleado.Email));
         List<string> ticks=new List<string>(Empleado.Tickets);
         foreach (var tick in ticks)
         {
             XElement tickXML = new XElement("Ticket",tick);
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
        return emp;
    }
}