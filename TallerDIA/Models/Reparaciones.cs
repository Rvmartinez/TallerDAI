using System;
using System.Collections.Generic;
using System.Linq;

namespace TallerDIA.Models;

public class Reparaciones
{
    private List<Reparacion> ReparacionesLista { get; set; }

    public Reparaciones()
    {
        ReparacionesLista = new List<Reparacion>();
    }

    public void AnadirReparacion(Reparacion reparacion)
    {
        ReparacionesLista.Add(reparacion);
    }

    public void AnadirReparaciones(List<Reparacion> reparacionesLista)
    {
        ReparacionesLista.AddRange(reparacionesLista);
    }
    
    

    public int GetReparacionesAnno(int anno, Boolean fin)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetAnno(fin) == anno);
    }
    
    public int GetReparacionesMes(int mes, int anno, Boolean fin)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetMes(fin) == mes && reparacion.GetAnno(fin) == anno);
    }
    
    
    public Reparaciones GetReparacionesCliente(String cliente)
    {
        Reparaciones reparaciones = new Reparaciones();
        reparaciones.ReparacionesLista = ReparacionesLista.Where(reparacion => reparacion.Cliente.Nombre == cliente).ToList();
        return reparaciones;
    }
    
    public string[] GetClientesReparaciones()
    {
        List<string> clientes = new List<string>();
        foreach(var reparacion in ReparacionesLista)
        {
            if (!clientes.Contains(reparacion.Cliente.Nombre))
            {
                clientes.Add(reparacion.Cliente.Nombre);
            }
        }

        return clientes.ToArray();
    }

    public int[] GetAnnosReparaciones(Boolean fin)
    {
        List<int> annos = new List<int>();
        foreach(var reparacion in ReparacionesLista)
        {
            var anno = reparacion.GetAnno(fin);
            if (anno.HasValue && !annos.Contains(anno.Value))
            {
                annos.Add(anno.Value);
            }
        }

        return annos.OrderDescending().ToArray();


    }
}