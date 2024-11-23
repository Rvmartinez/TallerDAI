namespace GraficosTaller.UI;

public class ConfigChart
{
    public enum ModoVision
    {
        Anual,
        Mensual
    }
    public ModoVision? Modo { get; set; }
    public int? Anno { get; set; }
    public string? Cliente { get; set; }
    public bool? FechaFin { get; set; }
}