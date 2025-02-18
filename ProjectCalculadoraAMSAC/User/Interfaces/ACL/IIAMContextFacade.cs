namespace ProjectCalculadoraAMSAC.User.Interfaces.ACL;

public interface IIamContextFacade
{
    Task<Guid> CreateAuthUser(string email, string password,string registerArea,DateTime datecreatedat);
    Task<Guid> FetchAuthUserIdByEmail(string email);
    Task<string> FetchAuthUsernameByUserId(Guid userId);
}