using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class CostoEstimado
{
    [Key]
    public int Id { get; private set; }

    public int EstimacionId { get; private set; }

    [ForeignKey("EstimacionId")]
    [JsonIgnore]
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

    private decimal ParseDecimal(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return 0;
        return decimal.Parse(input.Replace(",", "."), CultureInfo.InvariantCulture);
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

        CostoDirecto = 0;

        decimal volumen = atributos.ContainsKey("Volumen") ? ParseDecimal(atributos["Volumen"]) : 0;
        decimal area = atributos.ContainsKey("Área") ? ParseDecimal(atributos["Área"]) : 0;
        decimal distanciaTraslado = atributos.ContainsKey("DistanciaTraslado") ? ParseDecimal(atributos["DistanciaTraslado"]) : 0;
        bool generacionDAR = atributos.ContainsKey("GeneracionDAR") && atributos["GeneracionDAR"] == "true";
        bool cobertura = atributos.ContainsKey("Cobertura") && atributos["Cobertura"] == "true";
        string tipoCobertura = atributos.ContainsKey("TipoCobertura") ? atributos["TipoCobertura"] : "NINGUNA";

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

            default:
                throw new InvalidOperationException($"El Tipo de PAM '{estimacion.TipoPam.Name}' no tiene una fórmula de cálculo definida.");
        }

        GastosGenerales = CostoDirecto * 0.2m;
        Utilidades = CostoDirecto * 0.09m;
        SubTotal = CostoDirecto + GastosGenerales + Utilidades;
        IGV = SubTotal * 0.18m;
        SubTotalObras = SubTotal + IGV;
        ExpedienteTecnico = CostoDirecto * 0.061m;
        Supervision = CostoDirecto * 0.148m;
        GestionProyecto = CostoDirecto * 0.047m;
        Capacitacion = CostoDirecto * 0.012m;
        Contingencias = CostoDirecto * 0.059m;
        TotalEstimado = SubTotalObras + ExpedienteTecnico + Supervision + GestionProyecto + Capacitacion + Contingencias;
    }
}
