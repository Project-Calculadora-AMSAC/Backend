using System.Text.Json.Serialization;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class TipoPam
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    
    [JsonIgnore]

    private readonly List<SubEstimacion> _subEstimaciones = new();
    [JsonIgnore]

    public IReadOnlyCollection<SubEstimacion> SubEstimaciones => _subEstimaciones.AsReadOnly();
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
    


}