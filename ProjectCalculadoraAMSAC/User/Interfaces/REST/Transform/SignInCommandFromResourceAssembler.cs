using ProjectCalculadoraAMSAC.User.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.User.Interfaces.REST.Resources;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}