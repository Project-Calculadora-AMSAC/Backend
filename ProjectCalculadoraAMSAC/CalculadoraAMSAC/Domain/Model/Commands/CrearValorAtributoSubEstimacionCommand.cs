using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;


public record CrearValorAtributoSubEstimacionCommand(
    int SubEstimacionId,
    int AtributoPamId,
    string Valor
) : IRequest<int>;