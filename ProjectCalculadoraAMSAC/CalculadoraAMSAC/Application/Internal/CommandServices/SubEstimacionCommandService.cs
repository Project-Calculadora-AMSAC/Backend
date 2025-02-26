using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using System;
using System.Threading.Tasks;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices
{
    public class SubEstimacionCommandService : ISubEstimacionCommandService
    {
        private readonly ISubEstimacionRepository _subEstimacionRepository;
        private readonly ITipoPamRepository _tipoPamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubEstimacionCommandService(
            ISubEstimacionRepository subEstimacionRepository,
            ITipoPamRepository tipoPamRepository,
            IUnitOfWork unitOfWork)
        {
            _subEstimacionRepository = subEstimacionRepository;
            _tipoPamRepository = tipoPamRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CrearSubEstimacionCommand command)
        {
            if (command == null)
                throw new ArgumentException("Invalid sub-estimacion data.");

            Console.WriteLine($"DEBUG: Creando SubEstimacion -> TipoPamId: {command.TipoPamId}, Cantidad: {command.Cantidad}");

            var tipoPam = await _tipoPamRepository.GetByIdWithVariablesAsync(command.TipoPamId);
            if (tipoPam == null)
                throw new InvalidOperationException($"No se encontró el TipoPam con ID {command.TipoPamId}.");

            var subEstimacion = new SubEstimacion(tipoPam.Id, command.Cantidad, command.Valores);
            await _subEstimacionRepository.AddAsync(subEstimacion);
            await _unitOfWork.CompleteAsync();

            Console.WriteLine($"DEBUG: SubEstimacion creada con ID {subEstimacion.Id}");

            return subEstimacion.Id;
        }
    }
}