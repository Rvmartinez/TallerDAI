namespace GraficosTaller.UI;

/// <summary>
/// Permite configurar una ventana ChartWindow o DesgloseWindow. Los datos no especificados se especificarán por ComboBox.
/// </summary>
public class ConfigChart
{
   
    public enum ModoVision
    {
        Anual,
        Mensual
    }
    /// <summary>
    /// Elige si se muestran los datos mes a mes o año a año. Usar con cautela en DesgloseWindow.
    /// </summary>
    public ModoVision? Modo { get; set; }
    /// <summary>
    /// Año a mostrar
    /// </summary>
    public int? Anno { get; set; }
    /// <summary>
    /// El nombre del cliente del que mostrar datos. Solo funcional en DesgloseWindow.
    /// </summary>
    public string? Cliente { get; set; }
    /// <summary>
    /// Permite elegir si se usa la fecha de fin o la de inicio de la reparación.
    /// </summary>
    public bool? FechaFin { get; set; }
}