using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.ValueObject;

public class CostoEstimado
{
    public decimal CostoDirecto { get; private set; }
    public decimal GastosGenerales { get; private set; }
    public decimal Utilidades { get; private set; }
    public decimal IGV { get; private set; }
    public decimal ExpedienteTecnico { get; private set; }
    public decimal Supervision { get; private set; }
    public decimal GestionProyecto { get; private set; }
    public decimal Capacitacion { get; private set; }
    public decimal Contingencias { get; private set; }
    public decimal SubTotal { get; private set; }
    public decimal SubTotalObras { get; private set; }
    public decimal TotalEstimado { get; private set; }

    private CostoEstimado() { } 

    public CostoEstimado(Estimacion estimacion, ConstantesPam constantes)
    {
        var atributos = estimacion.Atributos.ToDictionary(a => a.Name, a => a.Valor);

        
        // ✅ Extraer dinámicamente los valores requeridos
        decimal volumen = atributos.ContainsKey("Volumen") ? Convert.ToDecimal(atributos["Volumen"]) : 0;
        decimal area = atributos.ContainsKey("Área") ? Convert.ToDecimal(atributos["Área"]) : 0;
        decimal distanciaTraslado = atributos.ContainsKey("DistanciaTraslado") ? Convert.ToDecimal(atributos["DistanciaTraslado"]) : 0;
        
        bool generacionDAR = atributos.ContainsKey("GeneracionDAR") && atributos["GeneracionDAR"] == "true";
        bool cobertura = atributos.ContainsKey("Cobertura") && atributos["Cobertura"] == "true";
        string tipoCobertura = atributos.ContainsKey("TipoCobertura") ? atributos["TipoCobertura"] : "NINGUNA";

        // ✅ Aplicar constantes fijas al cálculo
        CostoDirecto = (volumen * constantes.FactorCorreccionVolumen)
                       + (area * constantes.FactorCorreccionArea)
                       + (generacionDAR ? constantes.CorreccionCapacidad : 0)
                       + (cobertura ? constantes.CorreccionCobertura : 0)
                       + (tipoCobertura == "III" ? constantes.CorreccionCoberturaTipoIII : 0)
                       + (tipoCobertura == "IV" ? constantes.CorreccionCoberturaTipoIV : 0)
                       + (tipoCobertura == "NINGUNA" ? constantes.CorreccionCoberturaNinguna : 0)
                       + (distanciaTraslado * constantes.CorreccionTransporte)
                       + constantes.CorreccionGlobal;

        // ✅ Aplicar Gastos Generales, Utilidades e IGV
        GastosGenerales = CostoDirecto * 0.20m;
        Utilidades = CostoDirecto * 0.09m;
        SubTotal = CostoDirecto + GastosGenerales + Utilidades;
        IGV = SubTotal * 0.18m;
        SubTotalObras = SubTotal + IGV;
        ExpedienteTecnico = CostoDirecto * 0.06m;
        Supervision = CostoDirecto * 0.15m;
        GestionProyecto = CostoDirecto * 0.05m;
        Capacitacion = CostoDirecto * 0.01m;
        Contingencias = CostoDirecto * 0.06m;
        
        TotalEstimado = SubTotalObras + ExpedienteTecnico + Supervision + GestionProyecto + Capacitacion + Contingencias;
    }
}