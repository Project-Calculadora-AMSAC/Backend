namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class ConstantesPam
{
    public int Id { get; private set; }
    public decimal FactorCorreccionVolumen { get; private set; }
    public decimal FactorCorreccionArea { get; private set; }
    public decimal CorreccionCapacidad { get; private set; }
    public decimal CorreccionCobertura { get; private set; }
    public decimal CorreccionCoberturaTipoIII { get; private set; }
    public decimal CorreccionCoberturaTipoIV { get; private set; }
    public decimal CorreccionCoberturaNinguna { get; private set; }
    public decimal CorreccionTransporte { get; private set; }
    public decimal CorreccionGlobal { get; private set; }
    
    private ConstantesPam() { }

    public ConstantesPam(decimal factorVolumen, decimal factorArea, decimal correccionCapacidad,
        decimal correccionCobertura, decimal correccionCoberturaTipoIII, decimal correccionCoberturaTipoIV,
        decimal correccionCoberturaNinguna, decimal correccionTransporte, decimal correccionGlobal)
    {
        FactorCorreccionVolumen = factorVolumen;
        FactorCorreccionArea = factorArea;
        CorreccionCapacidad = correccionCapacidad;
        CorreccionCobertura = correccionCobertura;
        CorreccionCoberturaTipoIII = correccionCoberturaTipoIII;
        CorreccionCoberturaTipoIV = correccionCoberturaTipoIV;
        CorreccionCoberturaNinguna = correccionCoberturaNinguna;
        CorreccionTransporte = correccionTransporte;
        CorreccionGlobal = correccionGlobal;
    }
}