﻿using System.Text.Json.Serialization;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class TipoPam
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    
    [JsonIgnore]

    private readonly List<Estimacion> _estimaciones = new();
    [JsonIgnore]

    public IReadOnlyCollection<Estimacion> Estimaciones => _estimaciones.AsReadOnly();
    private readonly List<AtributosPam> _atributos = new();
    public IReadOnlyCollection<AtributosPam> Atributos => _atributos.AsReadOnly();
    private readonly List<VariablesPam> _variables = new();
    public IReadOnlyCollection<VariablesPam> Variables => _variables.AsReadOnly();
   
    private TipoPam() { }
    public TipoPam(string name, bool status = true)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Status = status;
    }

    public void AgregarAtributo(int unidadMedida,string nombre, string tipoDato)
    {
        if (_atributos.Any(a => a.Nombre == nombre))
            throw new InvalidOperationException($"El atributo '{nombre}' ya existe en este TipoPam.");

        _atributos.Add(new AtributosPam(Id,unidadMedida, nombre, tipoDato));
    }

    public void AgregarVariable(string nombre, decimal valor)
    {
        if (_variables.Any(v => v.Nombre == nombre))
            throw new InvalidOperationException($"Ya existe una variable con el nombre '{nombre}' en este TipoPam.");

        _variables.Add(new VariablesPam(Id, nombre, valor));
    }

    public void AgregarEstimacion(Estimacion estimacion)
    {
        if (estimacion.TipoPamId != Id)
            throw new InvalidOperationException("La estimación no corresponde a este TipoPam.");

        _estimaciones.Add(estimacion);
    }
}