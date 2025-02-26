﻿using Microsoft.EntityFrameworkCore;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;
using ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.CommandServices;

public class EstimacionCommandService : IEstimacionCommandService
{
    private readonly IEstimacionRepository _estimacionRepository;
    private readonly ISubEstimacionRepository _subEstimacionRepository;
    private readonly ICostoEstimadoRepository _costoEstimadoRepository;
    private readonly ITipoPamRepository _tipoPamRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EstimacionCommandService(
        IEstimacionRepository estimacionRepository,
        ISubEstimacionRepository subEstimacionRepository,
        ITipoPamRepository tipoPamRepository,
        IUnitOfWork unitOfWork)
    {
        _estimacionRepository = estimacionRepository;
        _subEstimacionRepository = subEstimacionRepository;
        _tipoPamRepository = tipoPamRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CrearEstimacionCommand command)
    {
        if (command == null)
            throw new ArgumentException("Invalid estimation data.");

        var nuevaEstimacion = new Estimacion(
            command.UsuarioId,
            command.ProyectoId,
            command.CodPam,
            DateTime.UtcNow
        );

        var subEstimaciones = new List<SubEstimacion>();

        foreach (var subCommand in command.SubEstimaciones)
        {
            var tipoPam = await _tipoPamRepository.GetByIdWithVariablesAsync(subCommand.TipoPamId);
            if (tipoPam == null)
                throw new InvalidOperationException($"No se encontró el TipoPam con ID {subCommand.TipoPamId}.");

            var subEstimacion = new SubEstimacion(tipoPam.Id, subCommand.Cantidad, subCommand.Valores);
            subEstimaciones.Add(subEstimacion);
        }

        // ✅ Guardar la estimación antes de asignar `SubEstimacion`
        await _estimacionRepository.AddAsync(nuevaEstimacion);
        await _unitOfWork.CompleteAsync();

        // ✅ Ahora que `nuevaEstimacion` tiene ID, se asignan las `SubEstimaciones`
        foreach (var subEstimacion in subEstimaciones)
        {
            subEstimacion.SetEstimacion(nuevaEstimacion);
            await _subEstimacionRepository.AddAsync(subEstimacion);
        }
    
        await _unitOfWork.CompleteAsync();

        // ✅ Ahora asignar `CostoEstimado` porque `SubEstimacion` ya tiene ID
        foreach (var subEstimacion in subEstimaciones)
        {
            var costoEstimado = new CostoEstimado(subEstimacion);
            await _costoEstimadoRepository.AddAsync(costoEstimado);
        }

        await _unitOfWork.CompleteAsync();

        return nuevaEstimacion.EstimacionId;
    }




    public async Task<bool> Handle(ActualizarEstimacionCommand command)
    {
        try
        {
            var estimacion = await _estimacionRepository.GetByIdAsync(command.EstimacionId);
            if (estimacion == null)
                throw new Exception("Estimation not found.");

            // Actualizar subestimaciones
            foreach (var subCommand in command.SubEstimaciones)
            {
                var subEstimacion = estimacion.SubEstimaciones.FirstOrDefault(se => se.Id == subCommand.SubEstimacionId);
                if (subEstimacion != null)
                {
                    subEstimacion.Cantidad = subCommand.Cantidad;
                }
            }

            await _unitOfWork.CompleteAsync();
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
