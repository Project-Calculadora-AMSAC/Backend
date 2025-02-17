using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearProyectoCommand(string Name, string Descripcion) : IRequest<int>;
