using MediatR;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetAllValoresAtributoEstimacionQuery(int EstimacionId) : IRequest<IEnumerable<ValorAtributoEstimacion>>;
