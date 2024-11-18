﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Cliente
{
    public required string DNI { get; set; }
    public required string Nombre { get; set; }
    public required string Email { get; set; }
    public required int IdCliente { get; set; }

    public Cliente() { }
    public Cliente(string dni, string nombre,string email,int id)
    {
        DNI = dni;
        Nombre = nombre;
        Email = email;
        IdCliente = id;
    }

}

