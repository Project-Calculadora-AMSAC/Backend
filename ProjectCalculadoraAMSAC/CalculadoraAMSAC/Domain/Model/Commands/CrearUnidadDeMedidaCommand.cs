using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearUnidadDeMedidaCommand(string Nombre, string Simbolo) : IRequest<int>;