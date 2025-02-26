namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

public class CrearEstimacionResource
{
    public Guid UsuarioId { get; set; }
    public int ProyectoId { get; set; }
    public string CodPam { get; set; }
    public List<SubEstimacionResource> SubEstimaciones { get; set; } // ✅ Nueva estructura
}

