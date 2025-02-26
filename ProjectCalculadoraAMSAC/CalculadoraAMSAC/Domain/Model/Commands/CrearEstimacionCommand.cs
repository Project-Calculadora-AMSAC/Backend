using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public class CrearEstimacionCommand
{
    public Guid UsuarioId { get; }
    public int ProyectoId { get; }
    public string CodPam { get; }
    public DateTime FechaEstimacion { get; }
    public List<CrearSubEstimacionCommand> SubEstimaciones { get; } // ✅ Asegurar que este sea el tipo correcto

    public CrearEstimacionCommand(
        Guid usuarioId,
        int proyectoId,
        string codPam,
        DateTime fechaEstimacion,
        List<CrearSubEstimacionCommand> subEstimaciones) // ✅ Cambiar de List<SubEstimacion> a List<CrearSubEstimacionCommand>
    {
        UsuarioId = usuarioId;
        ProyectoId = proyectoId;
        CodPam = codPam ?? throw new ArgumentNullException(nameof(codPam));
        FechaEstimacion = fechaEstimacion;
        SubEstimaciones = subEstimaciones ?? new List<CrearSubEstimacionCommand>();
    }
}