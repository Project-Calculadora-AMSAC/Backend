using System.Text.Json.Serialization;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class ValorAtributoEstimacion
{
    public int Id { get;  set; }
    public int SubEstimacionId { get;  set; }
    [JsonIgnore]

    public SubEstimacion SubEstimacion { get;  set; }

    public int AtributoPamId { get;  set; }
    public AtributosPam AtributoPam { get;  set; }

    public string Valor { get;  set; } // Se almacena como string y se convierte según `AtributoPam.TipoDato`

    private ValorAtributoEstimacion() { }

    public ValorAtributoEstimacion(int subEstimacionId, int atributoPamId, string valor)
    {
        SubEstimacionId = subEstimacionId;
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