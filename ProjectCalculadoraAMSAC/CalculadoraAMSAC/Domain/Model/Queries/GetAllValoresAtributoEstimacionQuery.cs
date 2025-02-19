using MediatR;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetAllValoresAtributoEstimacionQuery() : IRequest<IEnumerable<ValorAtributoEstimacion>>;
