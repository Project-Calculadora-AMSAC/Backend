using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

public class Estimacion
{
    public int EstimacionId { get; private set; }
    public Guid UsuarioId { get; private set; }
    public AuthUser AuthUser { get; private set; }
    public int ProyectoId { get; private set; }
    public Proyecto Proyecto { get; private set; }
    public int TipoPamId { get; private set; }
    public TipoPam TipoPam { get; private set; }

    public string CodPam { get; private set; } // Código único por estimación
    public DateTime FechaEstimacion { get; private set; }

    // ✅ Relación con `AtributoEstimacion`
    private readonly List<ValorAtributoEstimacion> _valores = new();
    public IReadOnlyCollection<ValorAtributoEstimacion> Valores => _valores.AsReadOnly();


    private Estimacion() { }

    public Estimacion(Guid usuarioId, int proyectoId, int tipoPamId, string codPam, Dictionary<int, string> valores)
    {
        UsuarioId = usuarioId;
        ProyectoId = proyectoId;
        TipoPamId = tipoPamId;
        CodPam = codPam ?? throw new ArgumentNullException(nameof(codPam));
        FechaEstimacion = DateTime.UtcNow;

        // ✅ Guardar los valores dinámicos en `AtributoEstimacion`
        foreach (var (atributoPamId, valor) in valores)
        {
            _valores.Add(new ValorAtributoEstimacion(EstimacionId, atributoPamId, valor));
        }
    }

    public void AsignarValor(int atributoPamId, string valor)
    {
        var valorExistente = _valores.FirstOrDefault(v => v.AtributoPamId == atributoPamId);
        if (valorExistente != null)
        {
            valorExistente.ActualizarValor(valor);
        }
        else
        {
            throw new KeyNotFoundException($"El atributo con ID {atributoPamId} no existe en esta estimación.");
        }
    }
    public void ActualizarValores(Dictionary<int, string> nuevosValores)
    {
        if (nuevosValores == null || !nuevosValores.Any())
            throw new ArgumentException("Los valores de la estimación no pueden estar vacíos.");

        foreach (var (atributoPamId, valor) in nuevosValores)
        {
            var atributoExistente = _valores.FirstOrDefault(v => v.AtributoPamId == atributoPamId);
            if (atributoExistente != null)
            {
                atributoExistente.ActualizarValor(valor); 
            }
            else
            {
                throw new KeyNotFoundException($"El atributo con ID {atributoPamId} no existe en esta estimación.");
            }
        }
    }
}