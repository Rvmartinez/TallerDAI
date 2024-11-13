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