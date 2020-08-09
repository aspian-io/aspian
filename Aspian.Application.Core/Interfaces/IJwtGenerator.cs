using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
using Aspian.Domain.UserModel;
using Microsoft.AspNetCore.Identity;

namespace Aspian.Application.Core.Interfaces
{
    public interface IJwtGenerator
    {
        /// <summary>
        /// Creates JWT using <paramref name="user"/> and <paramref name="claim"/>.
        /// </summary>
        /// <returns>
        /// A string containing JWT.
        /// </returns>
        /// <param name="user" >The user of type of User for which we want to issue a JWT.</param>
        /// <param name="claim" >The user's claim of type of Claim for which we want to issue a JWT.</param>
        string CreateToken(User user, Claim claim = null);

        /// <summary>
        /// Creates JWT using <paramref name="user"/> and <paramref name="claimRange"/>.
        /// </summary>
        /// <returns>
        /// A string containing JWT.
        /// </returns>
        /// <param name="user" >The user of type of User for which we want to issue a JWT.</param>
        /// <param name="claimRange" >The list of user's claims of type of Claim for which we want to issue a JWT.</param>
        string CreateToken(User user, List<Claim> claimRange = null);

        /// <summary>
        /// Refreshes JWT using <paramref name="user"/> and <paramref name="token"/>.
        /// </summary>
        /// <returns>
        /// A DTO class containing JWT and new RefreshToken.
        /// </returns>
        /// <param name="user" >The user of type of User for which we want to issue a JWT.</param>
        /// <param name="token" >Previous refresh token.</param>
        RefreshTokenDto RefreshToken(User user, string token, List<Claim> claimRange = null);

        /// <summary>
        /// Revokes refresh token using <paramref name="user"/> and <paramref name="token"/>.
        /// </summary>
        /// <returns>
        /// True of False;
        /// </returns>
        /// <param name="user" >The user of type of User for which we want to revoke a refresh token.</param>
        /// <param name="token" >Refresh token to revoke.</param>
        bool RevokeToken(User user, string token);
    }
}