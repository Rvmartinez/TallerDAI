using System;

namespace Gestión_de_reparaciones.CORE;

public class Cliente
{
    private string nombre;
    private string dni;
    private string email;
    private int idCliente;

    public Cliente(string dni, string nombre, string email, int idCliente)
    {
        DNI = dni;
        Nombre = nombre;
        Email = email;
        IdCliente = idCliente;
    }

    public Cliente()
    {
       
    }
    public  string DNI {
        get => dni;
        set => dni = value; 
    }
    public  string Nombre
    {
        get => nombre;
        set => nombre = value; 
    }

    public  string Email { get; set; }
    public  int IdCliente { get; set; }
}

