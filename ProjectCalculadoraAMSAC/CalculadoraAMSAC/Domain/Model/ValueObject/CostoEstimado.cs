using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class CostoEstimado
{
    [Key]
    public int Id { get; private set; }

    public int EstimacionId { get; private set; }

    [ForeignKey("EstimacionId")]
    public Estimacion Estimacion { get; private set; }

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
        if (estimacion.TipoPam == null)
        {
            Console.WriteLine($"ERROR: `TipoPam` en la estimación {estimacion.EstimacionId} es NULL.");
            throw new InvalidOperationException($"No hay un TipoPam asignado a la estimación con ID {estimacion.EstimacionId}.");
        }

        if (estimacion.TipoPam.Variables == null || !estimacion.TipoPam.Variables.Any())
        {
            Console.WriteLine($"ERROR: TipoPam {estimacion.TipoPam.Id} no tiene variables cargadas.");
            throw new InvalidOperationException($"El TipoPam con ID {estimacion.TipoPam.Id} no tiene variables asignadas.");
        }

        Console.WriteLine($"DEBUG: Creando `CostoEstimado` para estimación ID {estimacion.EstimacionId}");

        EstimacionId = estimacion.EstimacionId;
        CalcularCostos(estimacion);
    }


    public void CalcularCostos(Estimacion estimacion)
    {
        
        if (estimacion.Valores == null || !estimacion.Valores.Any())
        {
            throw new InvalidOperationException($"No hay valores asignados a la estimación ID {estimacion.EstimacionId}.");
        }
        
        var variables = estimacion.TipoPam.Variables.ToDictionary(v => v.Nombre, v => v.Valor);
        var atributos = estimacion.Valores.ToDictionary(v => v.AtributoPam.Nombre, v => v.Valor);
        Console.WriteLine($"DEBUG: CostoEstimado -> Estimación ID: {estimacion.EstimacionId}, Valores: {atributos.Count}");

        // ✅ Inicializar costo directo
        CostoDirecto = 0;

        // ✅ Extraer valores de la estimación
        decimal volumen = atributos.ContainsKey("Volumen") ? Convert.ToDecimal(atributos["Volumen"]) : 0;
        decimal area = atributos.ContainsKey("Área") ? Convert.ToDecimal(atributos["Área"]) : 0;
        decimal distanciaTraslado = atributos.ContainsKey("DistanciaTraslado") ? Convert.ToDecimal(atributos["DistanciaTraslado"]) : 0;
        bool generacionDAR = atributos.ContainsKey("GeneracionDAR") && atributos["GeneracionDAR"] == "true";
        bool cobertura = atributos.ContainsKey("Cobertura") && atributos["Cobertura"] == "true";
        string tipoCobertura = atributos.ContainsKey("TipoCobertura") ? atributos["TipoCobertura"] : "NINGUNA";

        // ✅ Aplicar lógica de cálculo según el Tipo de PAM
        switch (estimacion.TipoPam.Name)
        {
            case "Desmonte de Mina":
                CostoDirecto =
                    (variables.ContainsKey("FactorCorreccionVolumen") ? volumen * variables["FactorCorreccionVolumen"] : 0) +
                    (variables.ContainsKey("FactorCorreccionArea") ? area * variables["FactorCorreccionArea"] : 0) +
                    (variables.ContainsKey("CorreccionCapacidad") && generacionDAR ? variables["CorreccionCapacidad"] : 0) +
                    (variables.ContainsKey("CorreccionCobertura") && cobertura ? variables["CorreccionCobertura"] : 0) +
                    (variables.ContainsKey("CorreccionCoberturaTipoIII") && tipoCobertura == "III" ? variables["CorreccionCoberturaTipoIII"] : 0) +
                    (variables.ContainsKey("CorreccionCoberturaTipoIV") && tipoCobertura == "IV" ? variables["CorreccionCoberturaTipoIV"] : 0) +
                    (variables.ContainsKey("CorreccionCoberturaNinguna") && tipoCobertura == "NINGUNA" ? variables["CorreccionCoberturaNinguna"] : 0) +
                    (variables.ContainsKey("CorreccionTransporte") ? distanciaTraslado * variables["CorreccionTransporte"] : 0) +
                    (variables.ContainsKey("CorreccionGlobal") ? variables["CorreccionGlobal"] : 0);
                break;

            case "Bocamina":
                CostoDirecto =
                    (variables.ContainsKey("FactorCorreccionArea") ? area * variables["FactorCorreccionArea"] : 0) +
                    (variables.ContainsKey("CorreccionHumedad") ? variables["CorreccionHumedad"] : 0) +
                    (variables.ContainsKey("CorreccionIluminacion") ? variables["CorreccionIluminacion"] : 0) +
                    (variables.ContainsKey("CorreccionSeguridad") ? variables["CorreccionSeguridad"] : 0);
                break;

            default:
                throw new InvalidOperationException($"El Tipo de PAM '{estimacion.TipoPam.Name}' no tiene una fórmula de cálculo definida.");
        }

        // ✅ Aplicar Gastos Generales, Utilidades e IGV
        GastosGenerales = CostoDirecto * (variables.ContainsKey("FactorGastosGenerales") ? variables["FactorGastosGenerales"] : 0.2m);
        Utilidades = CostoDirecto * (variables.ContainsKey("FactorUtilidades") ? variables["FactorUtilidades"] : 0.09m);
        SubTotal = CostoDirecto + GastosGenerales + Utilidades;
        IGV = SubTotal * (variables.ContainsKey("FactorIGV") ? variables["FactorIGV"] : 0.18m);
        SubTotalObras = SubTotal + IGV;
        ExpedienteTecnico = CostoDirecto * (variables.ContainsKey("FactorExpedienteTecnico") ? variables["FactorExpedienteTecnico"] : 0.061m);
        Supervision = CostoDirecto * (variables.ContainsKey("FactorSupervision") ? variables["FactorSupervision"] : 0.148m);
        GestionProyecto = CostoDirecto * (variables.ContainsKey("FactorGestionProyecto") ? variables["FactorGestionProyecto"] : 0.047m);
        Capacitacion = CostoDirecto * (variables.ContainsKey("FactorCapacitacion") ? variables["FactorCapacitacion"] : 0.012m);
        Contingencias = CostoDirecto * (variables.ContainsKey("FactorContingencias") ? variables["FactorContingencias"] : 0.059m);

        TotalEstimado = SubTotalObras + ExpedienteTecnico + Supervision + GestionProyecto + Capacitacion + Contingencias;
    }
}
