using System.Collections.Generic;
using System.Security.Claims;
using Aspian.Domain.UserModel;
using Microsoft.AspNetCore.Identity;

namespace Aspian.Application.Core.Interfaces
{
    public interface IJwtGenerator
    {
        /// <summary>
        /// Create JWT using <paramref name="user"/> and <paramref name="claim"/>.
        /// </summary>
        /// <returns>
        /// A string containing JWT.
        /// </returns>
        /// <param name="user" >The user of type of User for which we want to issue a JWT.</param>
        /// <param name="claim" >The user's claim of type of Claim for which we want to issue a JWT.</param>
        string CreateToken(User user, Claim claim = null);

        /// <summary>
        /// Create JWT using <paramref name="user"/> and <paramref name="claimRange"/>.
        /// </summary>
        /// <returns>
        /// A string containing JWT.
        /// </returns>
        /// <param name="user" >The user of type of User for which we want to issue a JWT.</param>
        /// <param name="claimRange" >The list of user's claims of type of Claim for which we want to issue a JWT.</param>
        string CreateToken(User user, List<Claim> claimRange = null);
    }
}