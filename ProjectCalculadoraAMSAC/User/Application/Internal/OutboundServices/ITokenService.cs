﻿namespace ProjectCalculadoraAMSAC.User.Application.Internal.OutboundServices;

public interface ITokenService
{
    /**
     * <summary>
     *     Generate a JWT token
     * </summary>
     * <param name="user">The user to generate the token for</param>
     * <returns>The generated token</returns>
     */
    string GenerateToken(Domain.Model.Aggregates.AuthUser user);

    /**
     * <summary>
     *     Validate a JWT token
     * </summary>
     * <param name="token">The token to validate</param>
     * <returns>The user id if the token is valid, null otherwise</returns>
     */
    Task<Guid?> ValidateToken(string token);
}