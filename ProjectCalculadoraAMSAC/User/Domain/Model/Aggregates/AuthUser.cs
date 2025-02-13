using Newtonsoft.Json;

namespace ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;

public class AuthUser(string email, string passwordHash,string name,string phone,DateTime registeredAt)

{

    public AuthUser(): this(string.Empty, string.Empty,string.Empty,string.Empty,DateTime.UtcNow){}
    
    public Guid Id { get; }
    
    public string Email { get; private set; } = email;
    
    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;

    public string RegisterArea { get; set; } = name; // Nombre completo, ra
    
    public DateTime RegisteredAt { get; set; } = registeredAt; // Fecha de registro

    public AuthUser updateEmail(string email)
    {
        Email = email;
        return this;
    }

    public AuthUser updatePassword(string password)
    {
        PasswordHash = password;
        return this;
    }
    
    

    
}