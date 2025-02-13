namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Resources;

public record AuthenticatedUserResource(Guid Id, string Email, string Token);