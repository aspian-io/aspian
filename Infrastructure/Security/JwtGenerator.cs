using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtGenerator(IConfiguration config, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        //
        public async Task<string> CreateTokenAsync(User user, Claim claim = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
            };

            claims.Add(claim);

            // Generate signing credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            var newRefreshToken = await generateRefreshTokenAsync(user);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            return tokenHandler.WriteToken(token);
        }

        //
        public async Task<string> CreateTokenAsync(User user, List<Claim> claimRange = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
            };

            claims.AddRange(claimRange);

            // Generate signing credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            var newRefreshToken = await generateRefreshTokenAsync(user);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            return tokenHandler.WriteToken(token);
        }

        //
        public async Task<RefreshTokenDto> RefreshTokenAsync(User user, string token, List<Claim> claimRange = null)
        {
            var refreshToken = await _context.Tokens.SingleOrDefaultAsync(ut => ut.RefreshToken == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = await generateRefreshTokenAsync(user);
            if (newRefreshToken == null)
                throw new Exception("Problem generating new refresh token!");

            // generate new jwt
            var jwtToken = generateJWT(user, newRefreshToken, claimRange);

            if (jwtToken != null)
            {
                refreshToken.RevokedAt = DateTime.UtcNow.AddMinutes(5);
                refreshToken.ReplacedByToken = newRefreshToken;
            }

            await _context.SaveChangesAsync();

            return new RefreshTokenDto
            {
                JWT = jwtToken,
                RefreshToken = newRefreshToken
            };
        }

        //
        public bool RevokeToken(User user, string token)
        {
            var refreshToken = user.Tokens.SingleOrDefault(x => x.RefreshToken == token);

            if (!refreshToken.IsActive) return false;

            refreshToken.RevokedAt = DateTime.UtcNow;

            var succeeded = _context.SaveChanges() > 0;

            if (succeeded) return true;

            throw new Exception("Problem revoking the token!");
        }

        // 
        private string generateJWT(User user, string newRefreshToken, List<Claim> claimRange = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
            };

            claims.AddRange(claimRange);

            // Generate signing credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

            return tokenHandler.WriteToken(token);
        }

        //
        private async Task<string> generateRefreshTokenAsync(User user)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                var refreshToken = Convert.ToBase64String(randomBytes);
                user.Tokens.Add(new UserToken
                {
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                });

                var succeeded = await _context.SaveChangesAsync() > 0;

                return refreshToken;
            }
        }

    }
}