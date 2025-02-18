using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearAtributoPamCommand(
    int TipoPamId,
    int UnidadDeMedidaId,
    string Nombre,
    string TipoDato
) : IRequest<int>;