namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class VariablesPam
{
    public int Id { get; private set; }
    public int TipoPamId { get; private set; }
    public TipoPam TipoPam { get; private set; }
    
    public string Nombre { get; private set; }
    public decimal Valor { get; private set; }
    
    
    private VariablesPam() { }

    public VariablesPam(int tipoPamId, string nombre, decimal valor)
    {
        TipoPamId = tipoPamId;
        Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        Valor = valor;
    }
}