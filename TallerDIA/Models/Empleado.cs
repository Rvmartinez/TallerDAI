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
//altas, bajas, modificaciones, consultas(reparaciones asignadas)
    public Empleado()
    {
        this.Dni = "";
        this.Nombre = "";
        this.Email = "";
    }
    public Empleado(string dni, string nombre, string email)
    {
        this.Dni = dni;
        this.Nombre = nombre;
        this.Email = email;
    }
    
    public override string ToString()
    {
        string toret = "El empleado "+this.Nombre+" (DNI=" + this.Dni+") con Email: "+this.Email+" ;";

        return toret;
    }
}
    
    
    
    
