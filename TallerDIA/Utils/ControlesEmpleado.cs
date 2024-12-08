using System;
using System.Collections.Generic;
using TallerDIA.Models;

namespace TallerDIA.Utils;

public static class ControlesEmpleado
{
    public static bool FiltrarEntradasEmpleado(Empleado empleado)
    {
        if (empleado != null)
        {
            if (empleado.Email == null || empleado.Nombre == null || empleado.Email == null)
            {
                Console.Out.WriteLine("Empleado con algun campo nulo.");
                return false;
            }
            else if (empleado.Email.Trim()=="" || empleado.Nombre.Trim() == "" || empleado.Email.Trim()=="")
            {
                Console.Out.WriteLine("Empleado con algun campo no introducido.");
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Console.Out.WriteLine("Empleado nulo.");
            return false;
        }
    }
    public static Empleado BuscarEmpleado(List<Empleado> listaEmpleados,Empleado empleado)
    {
        if (listaEmpleados!=null && listaEmpleados.IndexOf(listaEmpleados.Find(x => x.Dni == empleado.Dni)) != -1)
        {
            return empleado;
        }
        else
        {
            return null;
        }
    }
}