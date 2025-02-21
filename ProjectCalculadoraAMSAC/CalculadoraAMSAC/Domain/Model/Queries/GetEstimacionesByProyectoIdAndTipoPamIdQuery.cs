using MediatR;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetEstimacionesByProyectoIdAndTipoPamIdQuery(int ProyectoId, int TipoPamId) : IRequest<List<Estimacion>>;
