namespace ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;

public class AuthUserRefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } // Relación con el usuario
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; } // Expira en X días
    public bool IsRevoked { get; set; } = false; // Para invalidar tokens
    public AuthUser AuthUser { get; set; } // Relación con AuthUser

}