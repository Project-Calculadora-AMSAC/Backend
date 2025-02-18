namespace ProjectCalculadoraAMSAC.User.Domain.Model.Commands;

public record SignUpCommand(string Email, string Password,string RegisterArea, DateTime DateCreatedAt);