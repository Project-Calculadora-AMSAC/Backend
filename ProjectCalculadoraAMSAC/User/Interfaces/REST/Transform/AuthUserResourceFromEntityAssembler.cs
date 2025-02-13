using ProjectCalculadoraAMSAC.User.Interfaces.REST.Resources;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Transform;

public static class AuthUserResourceFromEntityAssembler
{
    public static AuthUserResource ToResourceFromEntity(Domain.Model.Aggregates.AuthUser user)
    {
        return new AuthUserResource(user.Id, user.Email);
    }
}