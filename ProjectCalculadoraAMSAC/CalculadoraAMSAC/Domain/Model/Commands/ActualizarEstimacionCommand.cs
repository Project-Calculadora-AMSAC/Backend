using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;


public record ActualizarEstimacionCommand(
    int EstimacionId, 
    IDictionary<int, string> Valores
) : IRequest<bool>;