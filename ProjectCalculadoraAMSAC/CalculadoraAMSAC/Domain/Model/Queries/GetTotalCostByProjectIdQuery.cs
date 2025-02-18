namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

public class GetTotalCostByProjectIdQuery
{
    public int ProyectoId { get; }

    public GetTotalCostByProjectIdQuery(int proyectoId)
    {
        ProyectoId = proyectoId;
    }
}