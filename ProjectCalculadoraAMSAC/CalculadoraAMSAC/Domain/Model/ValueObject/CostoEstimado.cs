using System;
using System.Collections.Generic;
using System.Linq;
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
        if (estimacion.TipoPam == null || !estimacion.TipoPam.Variables.Any())
            throw new InvalidOperationException("No hay variables asignadas para este Tipo de PAM.");

        // ✅ Mapeamos las constantes del TipoPam en un diccionario
        var variables = estimacion.TipoPam.Variables.ToDictionary(v => v.Nombre, v => v.Valor);

        // ✅ Convertimos los valores de estimación en un diccionario uniendo con `AtributosPam`
        var atributos = estimacion.Valores
            .ToDictionary(
                v => v.AtributoPam.Nombre,   // Clave: nombre del atributo
                v => v.Valor                 // Valor: el valor en string
            );

        // ✅ Inicializamos el costo directo
        CostoDirecto = 0;

        // ✅ Extraemos dinámicamente los valores requeridos
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
        GastosGenerales = CostoDirecto * 0.200989288758199m;
        Utilidades = CostoDirecto * 0.0900000000203m;
        SubTotal = CostoDirecto + GastosGenerales + Utilidades;
        IGV = SubTotal * 0.18m;
        SubTotalObras = SubTotal + IGV;
        ExpedienteTecnico = CostoDirecto * 0.0617399364388341m;
        Supervision = CostoDirecto * 0.147885283929561m;
        GestionProyecto = CostoDirecto * 0.047200000143723m;
        Capacitacion = CostoDirecto * 0.0120887931329303m;
        Contingencias = CostoDirecto * 0.058999999880230m;

        TotalEstimado = SubTotalObras + ExpedienteTecnico + Supervision + GestionProyecto + Capacitacion + Contingencias;
    }
}
