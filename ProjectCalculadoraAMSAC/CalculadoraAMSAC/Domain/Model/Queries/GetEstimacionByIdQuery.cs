namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public class GetEstimacionByIdQuery
{
    public int EstimacionId { get; }

    public GetEstimacionByIdQuery(int estimacionId)
    {
        EstimacionId = estimacionId;
    }
}