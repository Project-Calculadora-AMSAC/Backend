using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Repositories;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Application.Internal.QueryServices;

public class ProyectoQueryService : IProyectoQueryService
    {
        private readonly IProyectoRepository _proyectoRepository;

        public ProyectoQueryService(IProyectoRepository proyectoRepository)
        {
            _proyectoRepository = proyectoRepository;
        }

        public async Task<Proyecto> Handle(GetProyectoByIdQuery query)
        {
            return await _proyectoRepository.GetByIdAsync(query.ProyectoId);
        }

        public async Task<List<Proyecto>> Handle(GetAllProyectosQuery query)
        {
            return await _proyectoRepository.GetAllAsync();
        }
    }