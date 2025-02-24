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

    private readonly List<ValorAtributoEstimacion> _valores  = new(); 
    public IReadOnlyCollection<ValorAtributoEstimacion> Valores => _valores.AsReadOnly();

    public CostoEstimado CostoEstimado { get; private set; }


    private Estimacion() { }
    
    public Estimacion(Guid usuarioId, int proyectoId, int tipoPamId, string codPam, Dictionary<int, string> valores)
    {
        UsuarioId = usuarioId;
        ProyectoId = proyectoId;
        TipoPamId = tipoPamId;
        CodPam = codPam ?? throw new ArgumentNullException(nameof(codPam));
        FechaEstimacion = DateTime.UtcNow;

        if (valores == null)
            throw new ArgumentNullException(nameof(valores), "Los valores no pueden ser null.");

        foreach (var (atributoPamId, valor) in valores)
        {
            _valores.Add(new ValorAtributoEstimacion(EstimacionId, atributoPamId, valor));
        }

        Console.WriteLine($"DEBUG: Creando Estimacion con ID (antes de guardar en DB): {EstimacionId}");

    }



    public void SetTipoPam(TipoPam tipoPam)
    {
        if (tipoPam == null)
            throw new ArgumentNullException(nameof(tipoPam), "TipoPam no puede ser null.");

        Console.WriteLine($"DEBUG: Asignando TipoPam a Estimacion -> TipoPamId: {tipoPam.Id}, Nombre: {tipoPam.Name}");
        TipoPam = tipoPam;
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