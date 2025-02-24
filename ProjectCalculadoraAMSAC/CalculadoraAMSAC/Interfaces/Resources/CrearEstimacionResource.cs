namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

public class CrearEstimacionResource
{
    public Guid UsuarioId { get; set; }
    public int ProyectoId { get; set; }
    public int TipoPamId { get; set; }
    public string CodPam { get; set; }
    public DateTime FechaEstimacion { get; set; }
    public IDictionary<int, string> Valores { get; set; } = new Dictionary<int, string>();
}