using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.User.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.User.Domain.Services;
using ProjectCalculadoraAMSAC.User.Interfaces.REST.Transform;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Controllers;

[Authorize]
[ApiController]
[Route("amsac/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthUserController(IAuthUserQueryService authUserQueryService, IAuthUserCommandService userCommandService) : ControllerBase
{
    /**
     * <summary>
     *     Get user by id endpoint. It allows to get a user by id
     * </summary>
     * <param name="authUserId">The user id</param>
     * <returns>The user resource</returns>
     */
    [HttpGet("{authUserId}")]
    public async Task<IActionResult> GetAuthUserById(Guid authUserId)
    {
        var getAuthUserByIdQuery = new GetAuthUserByIdQuery(authUserId);
        var user = await authUserQueryService.Handle(getAuthUserByIdQuery);
        var userResource = AuthUserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    /**
     * <summary>
     *     Get all users endpoint. It allows to get all users
     * </summary>
     * <returns>The user resources</returns>
     */
    [HttpGet]
    public async Task<IActionResult> GetAuthAllUsers()
    {
        var getAuthAllUsersQuery = new GetAllAuthUsersQuery();
        var users = await authUserQueryService.Handle(getAuthAllUsersQuery);
        var userResources = users.Select(AuthUserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
    
  

    
    
}