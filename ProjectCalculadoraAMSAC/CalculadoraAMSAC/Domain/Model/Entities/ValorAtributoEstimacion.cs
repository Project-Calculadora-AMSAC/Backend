using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class ValorAtributoEstimacion
{
    public int Id { get; private set; }
    public int EstimacionId { get; private set; }
    public Estimacion Estimacion { get; private set; }

    public int AtributoPamId { get; private set; }
    public AtributosPam AtributoPam { get; private set; }

    public string Valor { get; private set; } // Se almacena como string y se convierte según `AtributoPam.TipoDato`

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