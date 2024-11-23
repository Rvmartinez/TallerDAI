using System;

namespace GraficosTaller.Corefake;

public interface IReparaciones
{
    void AnadirReparacion(Reparacion reparacion);
    int GetReparacionesAnno(int anno, Boolean fin);
    int GetReparacionesMes(int mes, int anno, Boolean fin);
    Reparaciones GetReparacionesCliente(String cliente);
    string[] GetClientesReparaciones();
    int[] GetAnnosReparaciones(Boolean fin);
}