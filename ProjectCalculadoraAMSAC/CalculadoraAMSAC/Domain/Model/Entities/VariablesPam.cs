using System.Text.Json.Serialization;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class VariablesPam
{
    public int Id { get;  set; }
    public int TipoPamId { get;  set; }
    [JsonIgnore]

    public TipoPam TipoPam { get;  set; }
    
    public string Nombre { get;  set; }
    public decimal Valor { get;  set; }
    
    
    private VariablesPam() { }

    public VariablesPam(int tipoPamId, string nombre, decimal valor)
    {
        TipoPamId = tipoPamId;
        Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        Valor = valor;
    }
}