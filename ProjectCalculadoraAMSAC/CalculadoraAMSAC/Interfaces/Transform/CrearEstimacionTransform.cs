using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Resources;
using System;
using System.Linq;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Transform
{
    public class CrearEstimacionTransform
    {
        public static CrearEstimacionCommand ToCommand(CrearEstimacionResource resource)
        {
            Console.WriteLine($"DEBUG: Transformando CrearEstimacionResource con {resource.SubEstimaciones.Count} subestimaciones.");

            var subEstimaciones = resource.SubEstimaciones
                .Select(se => SubEstimacionTransform.ToCommand(se))
                .ToList();

            return new CrearEstimacionCommand(
                resource.UsuarioId,
                resource.ProyectoId,
                resource.CodPam,
                DateTime.UtcNow,
                subEstimaciones // ✅ Ahora sí es del tipo correcto: List<CrearSubEstimacionCommand>
            );
        }

    }
}