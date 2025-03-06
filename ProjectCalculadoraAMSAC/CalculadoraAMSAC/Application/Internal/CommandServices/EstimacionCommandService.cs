using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices
{
    public class EstimacionCommandService : IEstimacionCommandService
    {
        private readonly IEstimacionRepository _estimacionRepository;
        private readonly ICostoEstimadoRepository _costoEstimadoRepository;
        private readonly ITipoPamRepository _tipoPamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EstimacionCommandService(
            IEstimacionRepository estimacionRepository,
            ITipoPamRepository tipoPamRepository,
            ICostoEstimadoRepository costoEstimadoRepository,
            IUnitOfWork unitOfWork)
        {
            _estimacionRepository = estimacionRepository;
            _tipoPamRepository = tipoPamRepository;
            _costoEstimadoRepository = costoEstimadoRepository;
            _unitOfWork = unitOfWork;
        }
        
        private async Task<string> GenerarNuevoCodPam()
        {
            int intento = 0;
            while (intento < 5) 
            {
                try
                {
                    var ultimoCodPam = await _estimacionRepository.GetUltimoCodPamAsync();
                    int nuevoNumero = 1;

                    if (ultimoCodPam != null)
                    {
                        string numeroStr = ultimoCodPam.Replace("SN-", "");
                        if (int.TryParse(numeroStr, out int numero))
                        {
                            nuevoNumero = numero + 1;
                        }
                    }

                    string nuevoCodPam = $"SN-{nuevoNumero:D4}"; 

                    bool exists = await _estimacionRepository.ExistsByCodPamAsync(nuevoCodPam);

                    if (!exists)
                    {
                        return nuevoCodPam;
                    }

                    Console.WriteLine($"DEBUG: CodPam {nuevoCodPam} ya existe, intentando con otro.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR: {e.Message}");
                }

                intento++;

                await Task.Delay(100); 
            }

            throw new Exception("No se pudo generar un CodPam único después de varios intentos.");
        }


        public async Task<int> Handle(CrearEstimacionCommand command)
        {
            if (command == null)
                throw new ArgumentException("Invalid estimation data.");

            var tipoPam = await _tipoPamRepository.GetByIdWithVariablesAsync(command.TipoPamId);

            if (tipoPam == null)
                throw new InvalidOperationException($"No se encontró el TipoPam con ID {command.TipoPamId}.");
            string codPam = command.CodPam;
            if (string.IsNullOrWhiteSpace(codPam) || codPam == "0")
            {
                codPam = await GenerarNuevoCodPam();
                Console.WriteLine($"DEBUG: CodPam generado -> {codPam}");
            }
            if (tipoPam.Variables == null || !tipoPam.Variables.Any())
            {
                Console.WriteLine($"ERROR: TipoPam {command.TipoPamId} no tiene variables asignadas.");
                throw new InvalidOperationException($"El TipoPam con ID {command.TipoPamId} no tiene variables asignadas.");
            }

            var nuevaEstimacion = new Estimacion(
                command.UsuarioId,
                command.ProyectoId,
                tipoPam.Id,
                codPam,
                command.Valores
            );

            try
            {
                await _estimacionRepository.AddAsync(nuevaEstimacion);
                await _unitOfWork.CompleteAsync();  

                Console.WriteLine($"DEBUG: Estimación guardada con ID {nuevaEstimacion.EstimacionId}");

                var estimacionGuardada = await _estimacionRepository.GetByIdAsync(nuevaEstimacion.EstimacionId);
                if (estimacionGuardada == null)
                    throw new Exception($"No se pudo recuperar la estimación con ID {nuevaEstimacion.EstimacionId}.");

                Console.WriteLine($"DEBUG: Estimación recuperada con ID {estimacionGuardada.EstimacionId} y TipoPam {estimacionGuardada.TipoPam?.Name}");

                var costoEstimado = new CostoEstimado(estimacionGuardada);
                await _costoEstimadoRepository.AddAsync(costoEstimado);
                await _unitOfWork.CompleteAsync();

                return estimacionGuardada.EstimacionId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                throw new Exception($"An error occurred while creating the estimation: {e.Message}");
            }
        }

        public async Task<bool> Handle(ActualizarEstimacionCommand command)
        {
            try
            {
                var estimacion = await _estimacionRepository.GetByIdAsync(command.EstimacionId);
                if (estimacion == null)
                    throw new Exception("Estimation not found.");

                estimacion.ActualizarValores(new Dictionary<int, string>(command.Valores));

                var costoEstimado = await _costoEstimadoRepository.GetByEstimacionId(command.EstimacionId);
                if (costoEstimado != null)
                {
                    costoEstimado.CalcularCostos(estimacion);
                    await _unitOfWork.CompleteAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
                throw new Exception($"An error occurred while updating the estimation: {e.Message}");
            }
        }

        public async Task<bool> Handle(EliminarEstimacionCommand command)
        {
            var estimacion = await _estimacionRepository.FindByIdAsync(command.EstimacionId);
            if (estimacion == null)
                throw new Exception("Estimacion not found");

            await _estimacionRepository.DeleteAsync(estimacion.EstimacionId);
            await _unitOfWork.CompleteAsync();
            return true; 
        }
    }
}
