using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform;

public class ActualizarEstimacionTransform
{
    public static ActualizarEstimacionCommand ToCommand(int estimacionId, ActualizarEstimacionResource resource)
    {
        return new ActualizarEstimacionCommand(estimacionId, resource.Valores);
    }
}