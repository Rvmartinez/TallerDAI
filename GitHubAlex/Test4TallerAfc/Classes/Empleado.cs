using System.Collections.Generic;

namespace ConsoleApp1;


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
    public List<string> Tickets { get; set; }
//altas, bajas, modificaciones, consultas(reparaciones asignadas)
    public Empleado()
    {
        this.Dni = "";
        this.Nombre = "";
        this.Email = "";
        this.Tickets = new List<string>();
    }
    public Empleado(string dni, string nombre, string email, List<string> tickets)
    {
        this.Dni = dni;
        this.Nombre = nombre;
        this.Email = email;
        this.Tickets = new List<string>(tickets);
    }
    public bool TieneTicket(string ticket)
    {
        bool toret = Tickets.Contains(ticket);
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
    
    
    
    
