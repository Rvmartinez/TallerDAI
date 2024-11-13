using System;

namespace GraficosTaller.Corefake;

public class Reparacion
{
    public required DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public required Cliente Cliente { get; set; }

    public Reparacion()
    {
        FechaFin = null;
    }
    public int? GetMes(Boolean fin = false)
    {
        if(fin == true)
        {
            return FechaFin?.Month ?? null;
        }
        else
        {
            return FechaInicio.Month;
        }
    }

    public int? GetAnno(Boolean fin = false)
    {
        if(fin == true)
        {
            return FechaFin?.Year ?? null;
        }
        else
        {
            return FechaInicio.Year;
        }
    }
}