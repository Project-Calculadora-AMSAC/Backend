using ProjectCalculadoraAMSAC.User.Interfaces.REST.Resources;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(
        Domain.Model.Aggregates.AuthUser user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.Email, token);
    }
}