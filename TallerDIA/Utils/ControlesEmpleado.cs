using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Models;

namespace TallerDIA.Utils;

public static class ControlesEmpleado
{
    public static ObservableCollection<Empleado> ObtenerListaEmpleados()
    {
        DateTime reparacion1 = new DateTime(2019,06,11,10,15,10);
        DateTime reparacion2 = new DateTime(2020,05,07,9,10,1);
        DateTime reparacion3 = new DateTime(2018,10,08,18,1,59);
        DateTime reparacion4 = new DateTime(2021,01,10,7,45,22);
        List<DateTime> reparaciones1 = new List<DateTime>{reparacion1,reparacion2};
        List<DateTime> reparaciones2 = new List<DateTime>{reparacion3,reparacion4};
        Empleado empleado1 = new Empleado("12345678A", "Abelardo", "averelardo@hotcorreo.coom", reparaciones1);
        Empleado empleado2 = new Empleado("22345678B", "Luffy", "onepieceismid@ymail.com", reparaciones2);
        List<Empleado> empleados = new List<Empleado> 
        {
            empleado1,empleado2
        };
        ObservableCollection<Empleado> emps = new ObservableCollection<Empleado>(empleados);/**/
        return emps;
    }

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
    public static bool FiltrarEmpleadoRegex(Empleado empleado)
    {
        String regexEmail = @"^[\w.-]+@[\w.-]+\.\w+$";
        String regexDni = @"^[0-9]{8}[a-zA-Z]$";
        if (Regex.IsMatch(regexDni, empleado.Dni) && Regex.IsMatch(regexEmail, empleado.Email) )
        {
            return true;
        }
        else
        {
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