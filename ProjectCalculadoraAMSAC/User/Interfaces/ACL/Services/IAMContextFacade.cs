using ProjectCalculadoraAMSAC.User.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.User.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.User.Domain.Services;

namespace ProjectCalculadoraAMSAC.User.Interfaces.ACL.Services;

public class IamContextFacade(IAuthUserCommandService userCommandService, IAuthUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<Guid> CreateAuthUser(string email, string password,string registerArea,DateTime dateCreated)
    {
        var signUpCommand = new SignUpCommand(email, password,registerArea,dateCreated);
        await userCommandService.Handle(signUpCommand);
        var getUserByUsernameQuery = new GetAuthUserByEmailQuery(email);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? Guid.Empty;

    }

    public async Task<Guid> FetchAuthUserIdByEmail(string email)
    {
        var getAuthUserByUsernameQuery = new GetAuthUserByEmailQuery(email);
        var result = await userQueryService.Handle(getAuthUserByUsernameQuery);
        return result?.Id ?? Guid.Empty;
    }

    public async Task<string> FetchAuthUsernameByUserId(Guid userId)
    {
        var getAuthUserByIdQuery = new GetAuthUserByIdQuery(userId);
        var result = await userQueryService.Handle(getAuthUserByIdQuery);
        return result?.Email ?? string.Empty;
    }
}