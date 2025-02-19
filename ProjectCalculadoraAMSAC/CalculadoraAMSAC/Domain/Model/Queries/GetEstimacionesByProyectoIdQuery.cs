using MediatR;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetEstimacionesByProyectoIdQuery(int ProyectoId) : IRequest<List<Estimacion>>;
