using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform;

public class CrearEstimacionTransform
{
    public static CrearEstimacionCommand ToCommand(CrearEstimacionResource resource)
    {
        return new CrearEstimacionCommand(
            resource.UsuarioId,
            resource.ProyectoId,
            resource.TipoPamId,
            resource.CodPam,
            new Dictionary<int, string>(resource.Valores) 
        );
    }
}