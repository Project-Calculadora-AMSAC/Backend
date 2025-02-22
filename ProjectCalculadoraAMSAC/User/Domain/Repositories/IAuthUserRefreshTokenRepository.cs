using ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;

namespace ProjectCalculadoraAMSAC.User.Domain.Repositories;

public interface IAuthUserRefreshTokenRepository
{
    Task<AuthUserRefreshToken?> GetByTokenAsync(string token);
    Task<AuthUserRefreshToken?> GetByUserIdAsync(Guid userId);
    Task AddAsync(AuthUserRefreshToken refreshToken);
    Task RevokeAsync(AuthUserRefreshToken refreshToken);
}