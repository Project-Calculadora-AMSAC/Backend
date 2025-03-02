using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectCalculadoraAMSAC.Shared.Domain.Repositories;

public interface IUnitOfWork
{

    Task CompleteAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

}