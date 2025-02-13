using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class TipoPam
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    
    private readonly List<Estimacion> _estimaciones = new();
    public IReadOnlyCollection<Estimacion> Estimaciones => _estimaciones.AsReadOnly();
    private readonly List<VariablesPam> _constantes = new();
    public IReadOnlyCollection<VariablesPam> Variables => _constantes.AsReadOnly();

    private TipoPam() { }

    public TipoPam(string name, bool status = true)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Status = status;
    }
    
    public void AgregarConstante(string nombre, decimal valor)
    {
        if (_constantes.Any(c => c.Nombre == nombre))
            throw new InvalidOperationException($"Ya existe una constante con el nombre '{nombre}' en este TipoPam.");

        _constantes.Add(new VariablesPam(Id, nombre, valor));
    }
}