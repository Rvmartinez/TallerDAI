using System.Collections.Generic;

namespace Gestión_de_reparaciones.CORE;

public class Empleado
{
    public bool Disponible { get; set; }
    public string Dni { get; set; }
    public string Nombre { get; set; }
    //private string Nombre { get; set; }
    public string Email { get; set; }
    
//altas, bajas, modificaciones, consultas(reparaciones asignadas)
    public Empleado()
    {
        this.Disponible=false;
        this.Dni = "";
        this.Nombre = "";
        this.Email = "";
        
    }
    public Empleado(string dni, string nombre, string email, bool disponible)
    {
        this.Disponible=disponible;
        this.Dni = dni;
        this.Nombre = nombre;
        this.Email = email;
        
    }
    
    
    public bool EstaDisponible()
    {
        return Disponible;
    }
    
    public void DarDeAlta()
    {
        this.Disponible=false;
    }
    
    public void DarDeBaja()
    {
        this.Disponible=true;
    }
}
