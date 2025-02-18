using MediatR;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetTipoPamByIdQuery(int Id) : IRequest<TipoPam?>;
