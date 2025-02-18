using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarUnidadDeMedidaCommand(int Id, string Nombre, string Simbolo) : IRequest<bool>;
