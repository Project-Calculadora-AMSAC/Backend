using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;

public class SubEstimacion
{
    public int Id { get; set; }
    public int EstimacionId { get; set; }
    public Estimacion Estimacion { get; set; }
    public int TipoPamId { get; set; }
    public TipoPam TipoPam { get; set; }
    public int Cantidad { get; set; }
    
    private readonly List<ValorAtributoEstimacion> _valorAtributoEstimacion = new();
    public IReadOnlyCollection<ValorAtributoEstimacion> Valores => _valorAtributoEstimacion.AsReadOnly();
    public CostoEstimado CostoEstimado { get;  set; }

    private SubEstimacion() { }

    public SubEstimacion(int tipoPamId, int cantidad, Dictionary<int, string> valores)
    {
        if (tipoPamId == null)
            throw new ArgumentNullException(nameof(tipoPamId), "TipoPam no puede ser null.");

        if (valores == null || !valores.Any())
            throw new ArgumentException("Los valores de la subestimación no pueden ser null o vacíos.");

        TipoPamId = tipoPamId; // Se almacena el ID
        Cantidad = cantidad > 0 ? cantidad : throw new ArgumentException("Cantidad debe ser mayor a 0");

        if (valores == null)
            throw new ArgumentNullException(nameof(valores), "Los valores no pueden ser null.");

        foreach (var (atributoPamId, valor) in valores)
        {
            _valorAtributoEstimacion.Add(new ValorAtributoEstimacion(Id, atributoPamId, valor));
        }
        
    }
    
    public void ActualizarCantidad(int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");

        Cantidad = nuevaCantidad;
    }
    
    public void SetEstimacion(Estimacion estimacion)
    {
        if (estimacion == null) throw new ArgumentNullException(nameof(estimacion));
        Estimacion = estimacion;
        EstimacionId = estimacion.EstimacionId;
    }
    public void ActualizarValores(Dictionary<int, string> nuevosValores)
    {
        if (nuevosValores == null || !nuevosValores.Any())
            throw new ArgumentException("Los valores de la subestimación no pueden estar vacíos.");

        foreach (var (atributoPamId, valor) in nuevosValores)
        {
            var atributoExistente = Valores.FirstOrDefault(v => v.AtributoPamId == atributoPamId);
            if (atributoExistente != null)
            {
                atributoExistente.ActualizarValor(valor);
            }
            else
            {
                throw new KeyNotFoundException($"El atributo con ID {atributoPamId} no existe en esta subestimación.");
            }
        }
    }
    public void AsignarCostoEstimado(CostoEstimado costo)
    {
        if (costo == null) throw new ArgumentNullException(nameof(costo));
        CostoEstimado = costo;
    }
   
}