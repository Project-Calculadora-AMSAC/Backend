using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarValorAtributoEstimacionCommand(
    int Id,
    string Valor
) : IRequest<bool>;