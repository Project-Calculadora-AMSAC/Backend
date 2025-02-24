using System.Runtime.InteropServices.JavaScript;
using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

public record CrearEstimacionCommand(
    Guid UsuarioId,
    int ProyectoId,
    int TipoPamId,
    string CodPam,
    DateTime FechaEstimacion, 
    Dictionary<int, string> Valores
) : IRequest<int>;