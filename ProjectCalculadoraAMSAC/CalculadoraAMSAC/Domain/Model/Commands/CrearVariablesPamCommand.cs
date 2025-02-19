using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearVariablesPamCommand(int TipoPamId, string Nombre, decimal Valor) : IRequest<int>;
