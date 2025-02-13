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

    public CostoEstimado(Estimacion estimacion)
    {
        if (estimacion.TipoPam == null || estimacion.TipoPam.Variables == null || !estimacion.TipoPam.Variables.Any())
            throw new InvalidOperationException("No hay constantes asignadas para este Tipo de PAM.");
        
        var constantes = estimacion.TipoPam.Variables.ToDictionary(c => c.Nombre, c => c.Valor);
        var atributos = estimacion.Atributos.ToDictionary(a => a.Name, a => a.Valor);
        
        // ✅ Extraer dinámicamente los valores requeridos
        decimal volumen = atributos.ContainsKey("Volumen") ? Convert.ToDecimal(atributos["Volumen"]) : 0;
        decimal area = atributos.ContainsKey("Área") ? Convert.ToDecimal(atributos["Área"]) : 0;
        decimal distanciaTraslado = atributos.ContainsKey("DistanciaTraslado") ? Convert.ToDecimal(atributos["DistanciaTraslado"]) : 0;
        bool generacionDAR = atributos.ContainsKey("GeneracionDAR") && atributos["GeneracionDAR"] == "true";
        bool cobertura = atributos.ContainsKey("Cobertura") && atributos["Cobertura"] == "true";
        string tipoCobertura = atributos.ContainsKey("TipoCobertura") ? atributos["TipoCobertura"] : "NINGUNA";
        
        switch (estimacion.TipoPam.Name)
        {
            case "Desmonte de Mina":
                CostoDirecto = 
                    (constantes.ContainsKey("FactorCorreccionVolumen") ? volumen * constantes["FactorCorreccionVolumen"] : 0) +
                    (constantes.ContainsKey("FactorCorreccionArea") ? area * constantes["FactorCorreccionArea"] : 0) +
                    (constantes.ContainsKey("CorreccionCapacidad") && generacionDAR ? constantes["CorreccionCapacidad"] : 0) +
                    (constantes.ContainsKey("CorreccionCobertura") && cobertura ? constantes["CorreccionCobertura"] : 0) +
                    (constantes.ContainsKey("CorreccionCoberturaTipoIII") && tipoCobertura == "III" ? constantes["CorreccionCoberturaTipoIII"] : 0) +
                    (constantes.ContainsKey("CorreccionCoberturaTipoIV") && tipoCobertura == "IV" ? constantes["CorreccionCoberturaTipoIV"] : 0) +
                    (constantes.ContainsKey("CorreccionCoberturaNinguna") && tipoCobertura == "NINGUNA" ? constantes["CorreccionCoberturaNinguna"] : 0) +
                    (constantes.ContainsKey("CorreccionTransporte") ? distanciaTraslado * constantes["CorreccionTransporte"] : 0) +
                    (constantes.ContainsKey("CorreccionGlobal") ? constantes["CorreccionGlobal"] : 0);
                break;

            case "Bocamina":
                CostoDirecto = 
                    (constantes.ContainsKey("FactorCorreccionArea") ? area * constantes["FactorCorreccionArea"] : 0) +
                    (constantes.ContainsKey("CorreccionHumedad") ? constantes["CorreccionHumedad"] : 0) +
                    (constantes.ContainsKey("CorreccionIluminacion") ? constantes["CorreccionIluminacion"] : 0) +
                    (constantes.ContainsKey("CorreccionSeguridad") ? constantes["CorreccionSeguridad"] : 0);
                break;

            default:
                throw new InvalidOperationException($"El Tipo de PAM '{estimacion.TipoPam.Name}' no tiene una fórmula de cálculo definida.");
        }
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