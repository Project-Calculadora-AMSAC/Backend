using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ProjectCalculadoraAMSAC.User.Application.Internal.OutboundServices;
using ProjectCalculadoraAMSAC.User.Domain.Model.Aggregates;
using ProjectCalculadoraAMSAC.User.Domain.Repositories;
using ProjectCalculadoraAMSAC.User.Infraestructure.Tokens.JWT.Configurations;

namespace ProjectCalculadoraAMSAC.User.Infraestructure.Tokens.JWT.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;
        private readonly IAuthUserRefreshTokenRepository _refreshTokenRepository;

        public TokenService(
            IOptions<TokenSettings> tokenSettings,
            IAuthUserRefreshTokenRepository refreshTokenRepository) 
        {
            _tokenSettings = tokenSettings?.Value ?? throw new ArgumentNullException(nameof(tokenSettings));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        }

        /// <summary>
        /// Generate a new JWT token
        /// </summary>
        public string GenerateToken(AuthUser user)
        {
            var secret = _tokenSettings.Secret;
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60), // Expira en 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JsonWebTokenHandler();
            return tokenHandler.CreateToken(tokenDescriptor);
        }
        
        /// <summary>
        /// Validate the JWT token
        /// </summary>
        public async Task<Guid?> ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            try
            {
                var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                });

                var jwtToken = (JsonWebToken)tokenValidationResult.SecurityToken;
                var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Generate a secure Refresh Token
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Validate Refresh Token
        /// </summary>
        public async Task<Guid?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return null; // Token no válido o expirado
            }

            return storedToken.UserId;
        }

        /// <summary>
        /// Store Refresh Token
        /// </summary>
        public async Task StoreRefreshToken(Guid userId, string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetByUserIdAsync(userId);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                existingToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
            }
            else
            {
                var newRefreshToken = new AuthUserRefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Token = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };
                await _refreshTokenRepository.AddAsync(newRefreshToken);
            }
        }

        /// <summary>
        /// Revoke Refresh Token (Logout)
        /// </summary>
        public async Task RevokeRefreshToken(string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingToken != null)
            {
                await _refreshTokenRepository.RevokeAsync(existingToken);
            }
        }
    }
}
