using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Entities;
using ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Model.Queries;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Domain.Services;

public interface IProyectoQueryService
{
    Task<Proyecto> Handle(GetProyectoByIdQuery query);
    Task<List<Proyecto>> Handle(GetAllProyectosQuery query);
}