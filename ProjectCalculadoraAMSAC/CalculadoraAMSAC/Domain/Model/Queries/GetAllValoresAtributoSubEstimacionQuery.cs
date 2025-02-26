using MediatR;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetAllValoresAtributoSubEstimacionQuery(int SubEstimacionId) : IRequest<IEnumerable<ValorAtributoEstimacion>>;
