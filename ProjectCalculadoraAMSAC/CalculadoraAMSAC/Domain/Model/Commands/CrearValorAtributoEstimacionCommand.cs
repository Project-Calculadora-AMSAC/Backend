using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearValorAtributoEstimacionCommand(
    int EstimacionId,
    int AtributoPamId,
    string Valor
) : IRequest<int>;