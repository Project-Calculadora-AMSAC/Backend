using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform;


public class CrearEstimacionTransform
{

    public static CrearEstimacionCommand ToCommand(CrearEstimacionResource resource)
    {
        Console.WriteLine($"DEBUG: Transformando CrearEstimacionResource -> TipoPamId: {resource.TipoPamId}");

        return new CrearEstimacionCommand(
            resource.UsuarioId,
            resource.ProyectoId,
            resource.TipoPamId,
            resource.CodPam,
            resource.FechaEstimacion = DateTime.UtcNow, 
            new Dictionary<int, string>(resource.Valores) 
        );
    }
}