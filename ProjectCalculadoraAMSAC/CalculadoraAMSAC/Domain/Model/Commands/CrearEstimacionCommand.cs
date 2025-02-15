using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearEstimacionCommand(
    Guid UsuarioId,
    int ProyectoId,
    int TipoPamId,
    string CodPam,
    Dictionary<int, string> Valores
) : IRequest<int>;