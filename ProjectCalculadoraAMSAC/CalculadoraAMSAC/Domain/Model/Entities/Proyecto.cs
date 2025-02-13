using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

public class Proyecto
{
    public int ProyectoId { get; set; }
    public string Name { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    private readonly List<Estimacion> _estimaciones = new();
    public IReadOnlyCollection<Estimacion> Estimaciones => _estimaciones.AsReadOnly();
    
    private Proyecto() { }

    public Proyecto(string nombre, string descripcion)
    {
        Name = nombre ?? throw new ArgumentNullException(nameof(nombre));
        Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
        FechaRegistro = DateTime.UtcNow;
    }
}