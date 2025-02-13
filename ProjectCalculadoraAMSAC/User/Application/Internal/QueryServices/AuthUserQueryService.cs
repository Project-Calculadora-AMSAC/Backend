﻿
using ProjectCalculadoraAMSAC.User.Domain.Model.Queries;
using ProjectCalculadoraAMSAC.User.Domain.Repositories;
using ProjectCalculadoraAMSAC.User.Domain.Services;

namespace ProjectCalculadoraAMSAC.User.Application.Internal.QueryServices;


public class AuthUserQueryService(IAuthUserRepository userRepository) : IAuthUserQueryService
{
    /**
     * <summary>
     *     Handle get user by id query
     * </summary>
     * <param name="query">The query object containing the user id to search</param>
     * <returns>The user</returns>
     */
    public async Task<Domain.Model.Aggregates.AuthUser?> Handle(GetAuthUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.Id);
    }

    /**
     * <summary>
     *     Handle get user by username query
     * </summary>
     * <param name="query">The query object for getting all users</param>
     * <returns>The user</returns>
     */
    public async Task<IEnumerable<Domain.Model.Aggregates.AuthUser>> Handle(GetAllAuthUsersQuery query)
    {
        return await userRepository.ListAsync();
    }

    /**
     * <summary>
     *     Handle get user by username query
     * </summary>
     * <param name="query">The query object containing the username to search</param>
     * <returns>The user</returns>
     */
    public async Task<Domain.Model.Aggregates.AuthUser?> Handle(GetAuthUserByEmailQuery query)
    {
        return await userRepository.FindByEmailAsync(query.Email);
    }
  



}