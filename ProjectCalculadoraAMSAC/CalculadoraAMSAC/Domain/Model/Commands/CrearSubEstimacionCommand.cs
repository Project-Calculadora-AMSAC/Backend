
namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands
{
    public class CrearSubEstimacionCommand
    {
        public int EstimacionId { get; }
        public int TipoPamId { get; }
        public int Cantidad { get; }
        public Dictionary<int, string> Valores { get; }

        public CrearSubEstimacionCommand(int estimacionId, int tipoPamId, int cantidad, Dictionary<int, string> valores)
        {
            EstimacionId = estimacionId;
            TipoPamId = tipoPamId;
            Cantidad = cantidad;
            Valores = valores;
        }
    }
}