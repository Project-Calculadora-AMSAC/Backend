using System.Threading.Tasks;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services
{
    public interface ISubEstimacionCommandService
    {
        Task<int> Handle(CrearSubEstimacionCommand command);
    }
}