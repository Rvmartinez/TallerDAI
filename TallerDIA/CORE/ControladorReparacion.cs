using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Avalonia.Controls;

namespace Gestión_de_reparaciones.CORE;

public class ControladorReparacion:ObservableCollection<Reparacion>
{
    private static ObservableCollection<Reparacion> reparaciones = new ObservableCollection<Reparacion>();
    

    public static void guardarReparacion(Reparacion reparacion)
    {
        reparaciones.Add(reparacion);
    }

    public static  ObservableCollection<Reparacion> crearReparacion(string textAsunto, string textNota)
    {
        Reparacion reparacion = new Reparacion(textAsunto, textNota);
        guardarReparacion(reparacion);
       
        return reparaciones;
    }

    public static List<Reparacion> BuscarTerminados(RegistroReparacion registroReparacion)
    {
        List<Reparacion> reparaciones = new List<Reparacion>();
        DateTime date = new DateTime();
        foreach (Reparacion rep in registroReparacion)
        {
            if (!rep.FechaFin.Equals(date))
            {
                reparaciones.Add(rep);
            }
        }
        
        return reparaciones;
    }

    public static List<Reparacion> BuscarNoTerminados(RegistroReparacion registroReparacion)
    {
        List<Reparacion> reparaciones = new List<Reparacion>();
        DateTime date = new DateTime();
        foreach (Reparacion rep in registroReparacion)
        {
            if (rep.FechaFin.Equals(date))
            {
                reparaciones.Add(rep);
            }
        }
        
        return reparaciones;
    }

    public static void Eliminar(Reparacion reparacion, RegistroReparacion registroReparacion)
    {
        registroReparacion.Remove(reparacion);
    }
}