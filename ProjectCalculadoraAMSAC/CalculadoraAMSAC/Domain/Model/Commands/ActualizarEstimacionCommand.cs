using MediatR;
using System.Collections.Generic;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarEstimacionCommand(
    int EstimacionId,
    List<ActualizarSubEstimacionCommand> SubEstimaciones
) : IRequest<bool>;

public record ActualizarSubEstimacionCommand(
    int SubEstimacionId,
    int TipoPamId,
    int Cantidad,
    Dictionary<int, string> Valores
);