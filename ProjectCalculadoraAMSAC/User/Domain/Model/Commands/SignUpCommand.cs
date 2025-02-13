namespace ProjectCalculadoraAMSAC.User.Domain.Model.Commands;

public record SignUpCommand(string Email, string Password,string Name,string PhoneNumber, DateTime DateCreatedAt);