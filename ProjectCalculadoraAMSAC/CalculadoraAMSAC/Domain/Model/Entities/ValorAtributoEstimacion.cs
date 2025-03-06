using System.Text.Json.Serialization;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class ValorAtributoEstimacion
{
    public int Id { get;  set; }
    public int EstimacionId { get;  set; }
    [JsonIgnore]

    public Estimacion Estimacion { get;  set; }

    public int AtributoPamId { get;  set; }
    public AtributosPam AtributoPam { get;  set; }

    public string Valor { get;  set; } 

    private ValorAtributoEstimacion() { }

    public ValorAtributoEstimacion(int estimacionId, int atributoPamId, string valor)
    {
        EstimacionId = estimacionId;
        AtributoPamId = atributoPamId;
        Valor = valor ?? throw new ArgumentNullException(nameof(valor));
    }

    public T ObtenerValor<T>()
    {
        return (T)Convert.ChangeType(Valor, typeof(T));
    }
    
    public void ActualizarValor(string nuevoValor)
    {
        Valor = nuevoValor;
    }
}