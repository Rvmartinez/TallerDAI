using System;
using System.Collections.Generic;
using System.Linq;

namespace GraficosTaller.Corefake;

public class Reparaciones
{
    public List<Reparacion> ReparacionesLista { get; set; }

    public Reparaciones()
    {
        ReparacionesLista = new List<Reparacion>();
    }

    public void AnadirReparacion(Reparacion reparacion)
    {
        ReparacionesLista.Add(reparacion);
    }
    
    

    public int GetReparacionesAnno(int anno, Boolean fin = false)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetAnno(fin) == anno);
    }
    
    public int GetReparacionesMes(int mes, int anno, Boolean fin = false)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetMes(fin) == mes && reparacion.GetAnno(fin) == anno);
    }
    
    public int GetReparacionesAnnoCliente(int anno, Cliente cliente, Boolean fin = false)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetAnno(fin) == anno && reparacion.Cliente == cliente);
    }
    
    public Reparaciones GetReparacionesCliente(Cliente cliente)
    {
        Reparaciones reparaciones = new Reparaciones();
        reparaciones.ReparacionesLista = ReparacionesLista.Where(reparacion => reparacion.Cliente == cliente).ToList();
        return reparaciones;
    }
    
    public String[] GetClientesReparaciones()
    {
        List<String> clientes = new List<String>();
        foreach(var reparacion in ReparacionesLista)
        {
            if (!clientes.Contains(reparacion.Cliente.Nombre))
            {
                clientes.Add(reparacion.Cliente.Nombre);
            }
        }

        return clientes.ToArray();
    }

    public int[] getAnnosReparaciones(Boolean fin = false)
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