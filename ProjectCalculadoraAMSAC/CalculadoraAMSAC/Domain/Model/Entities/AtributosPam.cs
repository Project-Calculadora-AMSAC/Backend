using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class AtributosPam
{
    public int AtributoPamId { get; set; } 
    public int TipoPamId { get; set; }
    public TipoPam TipoPam { get; set; }
    public int UnidadDeMedidaId {get; set;}
    public UnidadDeMedida UnidadDeMedida { get; set; }

    public string Nombre { get; set; }
    public string TipoDato { get; set; }
    
    private AtributosPam() { }

    public AtributosPam(int tipoPamId,int unidadDeMedidaId, string nombre, string tipoDato)
    {
        TipoPamId = tipoPamId;
        UnidadDeMedidaId = unidadDeMedidaId;
        Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
        TipoDato = tipoDato ?? throw new ArgumentNullException(nameof(tipoDato));
    }
    
}