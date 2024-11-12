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
    public int? GetMes()
    {
        return FechaInicio.Month;
    }

    public int GetAnno()
    {
        return FechaInicio.Year;
    }
}