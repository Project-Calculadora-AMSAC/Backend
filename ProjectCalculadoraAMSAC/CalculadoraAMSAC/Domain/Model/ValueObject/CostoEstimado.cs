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

    public int SubEstimacionId { get; private set; }

    [ForeignKey("SubEstimacionId")]
    [JsonIgnore]
    public SubEstimacion SubEstimacion { get; private set; }

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

    public CostoEstimado(SubEstimacion subEstimacion)
    {
        if (subEstimacion == null)
        {
            Console.WriteLine("ERROR: `subEstimacion` es NULL.");
            throw new ArgumentNullException(nameof(subEstimacion), "La subestimación no puede ser null.");
        }

        if (subEstimacion.TipoPam == null)
        {
            Console.WriteLine($"ERROR: `TipoPam` en la subestimación {subEstimacion.Id} es NULL.");
            throw new InvalidOperationException($"No hay un TipoPam asignado a la subestimación con ID {subEstimacion.Id}.");
        }

        if (subEstimacion.TipoPam.Variables == null || !subEstimacion.TipoPam.Variables.Any())
        {
            Console.WriteLine($"ERROR: TipoPam {subEstimacion.TipoPam.Id} no tiene variables cargadas.");
            throw new InvalidOperationException($"El TipoPam con ID {subEstimacion.TipoPam.Id} no tiene variables asignadas.");
        }

        Console.WriteLine($"DEBUG: Creando `CostoEstimado` para subestimación ID {subEstimacion.Id}");

        SubEstimacionId = subEstimacion.Id;
        CalcularCostos(subEstimacion);
    }


    private decimal ParseDecimal(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return 0;
        return decimal.Parse(input.Replace(",", "."), CultureInfo.InvariantCulture);
    }

    public void CalcularCostos(SubEstimacion subEstimacion)
    {
    
        if (subEstimacion == null)
        {
            Console.WriteLine("ERROR: `subEstimacion` en `CalcularCostos` es NULL.");
            throw new ArgumentNullException(nameof(subEstimacion), "La subestimación no puede ser null.");
        }

        if (subEstimacion.Valores == null || !subEstimacion.Valores.Any())
        {
            Console.WriteLine($"ERROR: `Valores` en subestimación ID {subEstimacion.Id} es NULL o vacío.");
            throw new InvalidOperationException($"No hay valores en la subestimación ID {subEstimacion.Id}.");
        }

        if (subEstimacion.TipoPam == null)
        {
            Console.WriteLine($"ERROR: `TipoPam` en subestimación ID {subEstimacion.Id} es NULL.");
            throw new InvalidOperationException($"No hay TipoPam en la subestimación ID {subEstimacion.Id}.");
        }

        Console.WriteLine($"DEBUG: Calculando costos para SubEstimacion ID {subEstimacion.Id}");

        var variables = subEstimacion.TipoPam.Variables.ToDictionary(v => v.Nombre, v => v.Valor);
        var atributos = subEstimacion.Valores.ToDictionary(v => v.AtributoPam.Nombre, v => v.Valor);

        Console.WriteLine($"DEBUG: CostoEstimado -> SubEstimacion ID: {subEstimacion.Id}, Valores: {atributos.Count}");

        CostoDirecto = 0;

        decimal volumen = atributos.ContainsKey("Volumen") ? ParseDecimal(atributos["Volumen"]) : 0;
        decimal area = atributos.ContainsKey("Área") ? ParseDecimal(atributos["Área"]) : 0;
        decimal distanciaTraslado = atributos.ContainsKey("DistanciaTraslado") ? ParseDecimal(atributos["DistanciaTraslado"]) : 0;
        bool generacionDAR = atributos.ContainsKey("GeneracionDAR") && atributos["GeneracionDAR"] == "true";
        bool cobertura = atributos.ContainsKey("Cobertura") && atributos["Cobertura"] == "true";
        string tipoCobertura = atributos.ContainsKey("TipoCobertura") ? atributos["TipoCobertura"] : "NINGUNA";

        switch (subEstimacion.TipoPam.Name)
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
                throw new InvalidOperationException($"El Tipo de PAM '{subEstimacion.TipoPam.Name}' no tiene una fórmula de cálculo definida.");
        }

        GastosGenerales = CostoDirecto * 0.200989288758199m;
        Utilidades = CostoDirecto * 0.0900000000203m;
        SubTotal = CostoDirecto + GastosGenerales + Utilidades;
        IGV = SubTotal * 0.18m;
        SubTotalObras = SubTotal + IGV;
        ExpedienteTecnico = CostoDirecto * 0.0617399364388341m;
        Supervision = CostoDirecto * 0.147885283929561m;
        GestionProyecto = CostoDirecto * 0.047200000143723m;
        Capacitacion = CostoDirecto * 0.0120887931329303m;
        Contingencias = CostoDirecto * 0.0589999998802309m;
        TotalEstimado = SubTotalObras + ExpedienteTecnico + Supervision + GestionProyecto + Capacitacion + Contingencias;
    }
}
