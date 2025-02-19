using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarVariablesPamCommand(int Id, string Nombre, decimal Valor) : IRequest<bool>;
