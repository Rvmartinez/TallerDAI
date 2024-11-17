using System;

namespace GraficosTaller.Corefake;

public class Cliente
{
    public int DNI { get; }
    public char LetraDNI { get;}
    public string Nombre { get;}
    public string Correo { get; }

    public Cliente(int dni, char letraDNI, string nombre, string correo)
    {
        DNI = dni;
        LetraDNI = letraDNI;
        Nombre = nombre;
        Correo = correo;
    }

    public Cliente()
    {
        Random rnd = new Random();
        DNI = rnd.Next(10000000, 99999999);
        LetraDNI = (char)('A' + rnd.Next(0, 26));
        Nombre = ((char)('A' + rnd.Next(0, 26))).ToString();
        Correo = Nombre + "@fakeprovider.com";
    }
    
}