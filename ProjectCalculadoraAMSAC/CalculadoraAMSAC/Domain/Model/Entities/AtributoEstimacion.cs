using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class AtributoEstimacion
{
    public int AtributoEstimacionId { get; set; }
    public int EstimacionId { get; set; }
    public Estimacion Estimacion { get; private set; }

    public string Name { get; set; }
    public string Valor { get; set; }
    
    private AtributoEstimacion() { }

    public AtributoEstimacion(int estimacionId, string name, string valor)
    {
        EstimacionId = estimacionId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Valor = valor ?? throw new ArgumentNullException(nameof(valor));
    }

    public T ObtenerValor<T>()
    {
        return (T)Convert.ChangeType(Valor, typeof(T));
    }
}