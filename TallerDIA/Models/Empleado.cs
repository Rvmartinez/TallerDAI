using System;
using System.Collections.Generic;
namespace TallerDIA.Models;
/*
 *
 Gestión de personal (altas, bajas, modificaciones, consultas(reparaciones asignadas)), salvaguarda y recuperación.
    DNI
    Nombre
    Email
    Tickets asignados
 * 
 */

public class Empleado
{
    public string Dni { get; set; }
    public string Nombre { get; set; }
    //private string Nombre { get; set; }
    public string Email { get; set; }
    public List<DateTime> Tickets { get; set; }
//altas, bajas, modificaciones, consultas(reparaciones asignadas)
    public Empleado()
    {
        this.Dni = "";
        this.Nombre = "";
        this.Email = "";
        this.Tickets = new List<DateTime>();
    }
    public Empleado(string dni, string nombre, string email, List<DateTime> tickets)
    {
        this.Dni = dni;
        this.Nombre = nombre;
        this.Email = email;
        this.Tickets = new List<DateTime>(tickets);
    }
    public bool TieneTicket(DateTime fechaInicio)
    {
        bool toret = Tickets.Contains(fechaInicio);
        return toret;
    }
    
    public override string ToString()
    {
        string toret = "El empleado "+this.Nombre+" (DNI=" + this.Dni+") con Email: "+this.Email+" ;";

        if (this.Tickets != null && this.Tickets.Count > 0)
        {
            toret += "\n Y tiene asignados los Tickets:";
            foreach (var ticket in Tickets)
            {
                toret+="\n"+ticket;
            }
        }

        return toret;
    }
}
    
    
    
    
