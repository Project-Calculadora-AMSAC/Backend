using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;
using System.Linq;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform
{
    public class SubEstimacionTransform
    {
        public static CrearSubEstimacionCommand ToCommand(SubEstimacionResource resource)
        {
            return new CrearSubEstimacionCommand(
                0, // El EstimacionId se asignará más adelante
                resource.TipoPamId,
                resource.Cantidad,
                new Dictionary<int, string>(resource.Valores) // ✅ Solo los valores, los costos se calculan después
            );
        }
    }
}
