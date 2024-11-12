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

    public int GetReparacionesAnno(int anno)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetAnno() == anno);
    }

    public int GetReparacionesMes(int mes, int anno)
    {
        return ReparacionesLista.Count(reparacion => reparacion.GetMes() == mes && reparacion.GetAnno() == anno);
    }

    public int[] getAnnosReparaciones()
    {
        var a  = ReparacionesLista.GroupBy(reparacion => reparacion.GetAnno());
        return a.Select(reparacion => reparacion.Key).OrderBy(i => i).ToArray();
        
    }
}