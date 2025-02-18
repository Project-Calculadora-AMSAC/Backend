using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarTipoPamCommand(
    int Id,
    string Name,
    bool Status
) : IRequest<bool>;