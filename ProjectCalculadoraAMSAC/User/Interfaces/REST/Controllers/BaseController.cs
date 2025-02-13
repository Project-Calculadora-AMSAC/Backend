﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ProjectCalculadoraAMSAC.User.Interfaces.REST.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Obtiene el ID del usuario autenticado desde el token JWT.
    /// </summary>
    /// <returns>El ID del usuario autenticado como un GUID.</returns>
    /// <exception cref="UnauthorizedAccessException">Si el usuario no está autenticado.</exception>
    protected Guid GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) 
                          ?? User.FindFirst(ClaimTypes.Sid) // Usar SID si NameIdentifier no está
                          ?? throw new UnauthorizedAccessException("Token inválido o expirado.");

        return Guid.Parse(userIdClaim.Value);
    }
}