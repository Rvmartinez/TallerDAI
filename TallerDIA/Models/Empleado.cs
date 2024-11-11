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
    public bool Disponible { get; set; }
    public string Dni { get; set; }
    public string Nombre { get; set; }
    //private string Nombre { get; set; }
    public string Email { get; set; }
    public List<string> Tickets { get; set; }

    public Empleado()
    {
        Disponible = false;
        Dni = "";
        Nombre = "";
        Email = "";
        Tickets = new List<string>();
    }
    public Empleado(string dni, string nombre, string email, List<string> tickets, bool disponible)
    {
        Disponible = disponible;
        Dni = dni;
        Nombre = nombre;
        Email = email;
        Tickets = new List<string>(tickets);
    }
    public bool TieneTicket(string ticket)
    {
        bool toret = Tickets.Contains(ticket);
        return toret;
    }

    public bool EstaDisponible()
    {
        return Disponible;
    }

    public void DarDeAlta()
    {
        Disponible = false;
    }

    public void DarDeBaja()
    {
        Disponible = true;
    }
}




