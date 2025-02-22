﻿using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCalculadoraAMSAC.User.Application.Internal.OutboundServices;
using ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.User.Domain.Model.Commands;
using ProjectCalculadoraAMSAC.User.Domain.Repositories;
using ProjectCalculadoraAMSAC.User.Domain.Services;
using ProjectCalculadoraAMSAC.User.Interfaces.REST.Resources;
using ProjectCalculadoraAMSAC.User.Interfaces.REST.Transform;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Controllers;

[Authorize]
[ApiController]
[Route("amsac/v1/authentication")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(
    IAuthUserCommandService userCommandService,
    IAuthUserRepository authUserRepository,
    ITokenService tokenService) : ControllerBase
{
    /**
     * <summary>
     *     Sign in endpoint. It allows to authenticate a user
     * </summary>
     * <param name="signInResource">The sign in resource containing email and password.</param>
     * <returns>The authenticated user resource, including a JWT token</returns>
     */
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        var user = await authUserRepository.FindByEmailAsync(signInResource.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(signInResource.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var refreshToken = tokenService.GenerateRefreshToken();

        await tokenService.StoreRefreshToken(user.Id, refreshToken);

        // Generar un nuevo token para resource
        var resourceToken = tokenService.GenerateToken(user);
        var resource = AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(user, resourceToken);

        return Ok(new { refreshToken, resource });
    }


    /**
     * <summary>
     *     Sign up endpoint. It allows to create a new user
     * </summary>
     * <param name="signUpResource">The sign up resource containing email and password.</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        var existingUser = await authUserRepository.FindByEmailAsync(signUpResource.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "User already exists" });
        }

        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        await userCommandService.Handle(signUpCommand);
        return Ok(new { message = "User created successfully" });
    }

    /**
     * <summary>
     *     Refresh token endpoint. It generates a new access token using a valid refresh token
     * </summary>
     * <param name="request">The refresh token request.</param>
     * <returns>A new access token and refresh token</returns>
     */
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var user = await authUserRepository.FindByRefreshTokenAsync(request.RefreshToken);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token" });
        }

        var isValid = await tokenService.ValidateRefreshToken(user.Id, request.RefreshToken);
        if (isValid == null)
        {
            return Unauthorized(new { message = "Invalid refresh token" });
        }

        var newAccessToken = tokenService.GenerateToken(user);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        await tokenService.StoreRefreshToken(user.Id, newRefreshToken);

        return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
    }

    /**
     * <summary>
     *     Logout endpoint. It invalidates the refresh token to log out the user.
     * </summary>
     * <param name="request">The logout request containing the refresh token.</param>
     * <returns>A confirmation message on successful logout.</returns>
     */
    [HttpPost("sign-out")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        await tokenService.RevokeRefreshToken(request.RefreshToken);
        return Ok(new { message = "Logout successful" });
    }
}
