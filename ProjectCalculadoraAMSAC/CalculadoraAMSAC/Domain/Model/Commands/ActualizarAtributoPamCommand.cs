using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarAtributoPamCommand(
    int AtributoPamId,
    int TipoPamId,
    int UnidadDeMedidaId,
    string Nombre,
    string TipoDato
) : IRequest<bool>;