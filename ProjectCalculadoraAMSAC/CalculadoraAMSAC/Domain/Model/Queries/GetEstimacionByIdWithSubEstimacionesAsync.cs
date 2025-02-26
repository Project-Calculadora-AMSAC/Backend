using MediatR;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public record GetEstimacionByIdWithSubEstimacionesAsync(int EstimacionId);