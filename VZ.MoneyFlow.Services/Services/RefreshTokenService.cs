using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Data;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Extensions;
using VZ.MoneyFlow.Models.Models.Configuration;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.Services.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public RefreshTokenService(AppDbContext context, IOptionsMonitor<JwtConfig> options, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, TokenValidationParameters tokenValidationParameters)
        {
            _context = context;
            _jwtConfig = options.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseAuthResultDto> VerifyAndGenerateToken(RequestTokenDto tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var newTokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _tokenValidationParameters.IssuerSigningKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, newTokenValidationParameters, out var validatedToken);
                if (validatedToken is JwtSecurityToken JwtSecurityToken)
                {
                    var result = JwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        return null;
                    }
                }

                var utcExpiryTime = long.Parse(tokenInVerification.Claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryTime = DateTimeConverter.ConvertToDateTime(utcExpiryTime);
                if (expiryTime > DateTime.UtcNow)
                {
                    return new ResponseAuthResultDto(false, "Token is still valid");
                }

                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenRequest.RefreshToken);
                if (storedToken == null)
                {
                    return new ResponseAuthResultDto(false, "Token dose not exist");
                }

                if (storedToken.IsUsed)
                {
                    return new ResponseAuthResultDto(false, "Token has been used");
                }

                if (storedToken.IsRevoked)
                {
                    return new ResponseAuthResultDto(false, "Token has been revoked");
                }

                var jti = tokenInVerification.Claims.FirstOrDefault(j => j.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return new ResponseAuthResultDto(false, "Token does not match");
                }

                storedToken.IsUsed = true;

                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return new ResponseAuthResultDto(false, "Token has been expired please re-login");
                }
                else
                {
                    return new ResponseAuthResultDto(false, "Something went wrong.");
                }
            }
        }

        public async Task<ResponseAuthResultDto> GenerateJwtToken(AppUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetAllClaims(user);

            var tokenDesctiptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtConfig.DurationInMinutesForT)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtHandler.CreateToken(tokenDesctiptor);
            var jwtToken = jwtHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                AddTime = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtConfig.DurationInMinutesForRT)),
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new ResponseAuthResultDto(true, null, jwtToken, refreshToken.Token);
        }

        private async Task<List<Claim>> GetAllClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
