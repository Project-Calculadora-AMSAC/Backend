using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearTipoPamCommand(
    string Name,
    bool Status
) : IRequest<int>;