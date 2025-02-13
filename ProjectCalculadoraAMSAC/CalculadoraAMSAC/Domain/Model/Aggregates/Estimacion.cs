using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

public class Estimacion
{
    public int EstimacionId { get; private set; }
    public int UsuarioId { get; private set; }
    public int ProyectoId { get; private set; }
    public Proyecto Proyecto { get; private set; }

    public int TipoPamId { get; private set; }
    public TipoPam TipoPam { get; private set; }

    public string CodPam { get; private set; } // Código único por estimación
    public DateTime FechaEstimacion { get; private set; }

    // ✅ Relación con `AtributoEstimacion`
    private readonly List<AtributoEstimacion> _atributos = new();
    public IReadOnlyCollection<AtributoEstimacion> Atributos => _atributos.AsReadOnly();


    private Estimacion() { }

    public Estimacion(int usuarioId, int proyectoId, int tipoPamId, string codPam, Dictionary<string, object> valores)
    {
        UsuarioId = usuarioId;
        ProyectoId = proyectoId;
        TipoPamId = tipoPamId;
        CodPam = codPam ?? throw new ArgumentNullException(nameof(codPam));
        FechaEstimacion = DateTime.UtcNow;

        // ✅ Guardar los valores dinámicos en `AtributoEstimacion`
        foreach (var (key, value) in valores)
        {
            _atributos.Add(new AtributoEstimacion(EstimacionId, key, value?.ToString() ?? string.Empty));
        }
    }

    public T ObtenerAtributo<T>(string nombre)
    {
        var atributo = _atributos.FirstOrDefault(a => a.Name == nombre);
        if (atributo == null)
            throw new KeyNotFoundException($"El atributo '{nombre}' no existe.");

        return atributo.ObtenerValor<T>();
    }
}