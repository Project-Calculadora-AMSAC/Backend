namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

public class SubEstimacionResource
{
    public int EstimacionId { get; set; }
    public int TipoPamId { get; set; }
    public int Cantidad { get; set; }
    public IDictionary<int, string> Valores { get; set; } = new Dictionary<int, string>();
}