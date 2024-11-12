using System;

namespace GraficosTaller.Corefake;

public class Cliente
{
    public int DNI { get; private set; }
    public char LetraDNI { get; private set; }
    public string Nombre { get; private set; }
    public string Correo { get; private set; }

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
        Nombre = ('A' + rnd.Next(0, 26)).ToString();
        Correo = Nombre + "@fakeprovider.com";
    }
    
}