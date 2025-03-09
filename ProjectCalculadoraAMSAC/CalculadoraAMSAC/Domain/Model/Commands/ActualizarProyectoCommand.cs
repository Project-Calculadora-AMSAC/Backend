using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record ActualizarProyectoCommand(int ProyectoId, string Name, string Descripcion, bool Estado) : IRequest<bool>;
