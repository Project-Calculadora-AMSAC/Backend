namespace ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

public interface IUnitOfWork
{

    Task CompleteAsync();
}