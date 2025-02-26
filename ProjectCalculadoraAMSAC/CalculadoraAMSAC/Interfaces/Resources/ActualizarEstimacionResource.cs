namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

public class ActualizarEstimacionResource
{
    public int EstimacionId { get; set; }

    public List<ActualizarSubEstimacionResource> SubEstimaciones { get; set; } // ✅ Nueva estructura

}

public class ActualizarSubEstimacionResource
{
    public int SubEstimacionId { get; set; } // ✅ ID para actualizar la SubEstimacion específica
    
    public int TipoPamId {get; set;}
    public int Cantidad { get; set; }
    public Dictionary<int, string> Valores { get; set; }
}