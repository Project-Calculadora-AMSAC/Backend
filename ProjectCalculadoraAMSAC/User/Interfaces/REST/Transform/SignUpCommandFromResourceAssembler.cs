using ProjectCalculadoraAMSAC.User.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.User.Interfaces.REST.Resources;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(resource.Email, resource.Password,resource.name,resource.phoneNumber,DateTime.UtcNow);
    }
}