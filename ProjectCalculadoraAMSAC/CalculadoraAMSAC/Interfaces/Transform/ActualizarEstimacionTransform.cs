using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform;

public class ActualizarEstimacionTransform
{
    public static ActualizarEstimacionCommand ToCommand(ActualizarEstimacionResource resource)
    {
        var subEstimaciones = resource.SubEstimaciones
            .Select(se => new ActualizarSubEstimacionCommand(se.SubEstimacionId,se.TipoPamId ,se.Cantidad, se.Valores))
            .ToList();

        return new ActualizarEstimacionCommand(
            resource.EstimacionId,
            subEstimaciones // ✅ Ahora se pasa la lista de subestimaciones
        );
    }
}