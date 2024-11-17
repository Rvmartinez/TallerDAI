using System;

namespace GraficosTaller.Corefake;

public class Reparacion
{
    public required DateTime FechaInicio { get; init; }
    public DateTime? FechaFin { get; set; }
    public required Cliente Cliente { get; init; }

    public Reparacion()
    {
        FechaFin = null;
    }
    public int? GetMes(Boolean fin)
    {
        if(fin)
        {
            return FechaFin?.Month ?? null;
        }
        else
        {
            return FechaInicio.Month;
        }
    }

    public int? GetAnno(Boolean fin)
    {
        if(fin)
        {
            return FechaFin?.Year ?? null;
        }
        else
        {
            return FechaInicio.Year;
        }
    }
}