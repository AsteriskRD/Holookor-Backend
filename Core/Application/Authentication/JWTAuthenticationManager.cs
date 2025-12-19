using HolookorBackend.Core.Application.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HolookorBackend.Core.Application.Authentication
{
        public class JWTAuthenticationManager : IJWTAuthenticationManager
        {
            private readonly IConfiguration _configuration;

            public JWTAuthenticationManager(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(UserDto user)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWTSettings:SecretKey"]);

                var claims = new List<Claim>();

                if (!string.IsNullOrEmpty(user.Email))
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                }

                if (!string.IsNullOrEmpty(user.Id))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                }

                if(!string.IsNullOrEmpty(user.UserProfileId))
                {
                claims.Add(new Claim("userProfileId", user.UserProfileId));
                }

                if (!string.IsNullOrEmpty(user.FirstName))
                {
                    claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
                }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(3),
                Issuer = _configuration["JWTSettings:Issuer"],
                Audience = _configuration["JWTSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                  new SymmetricSecurityKey(key),
                  SecurityAlgorithms.HmacSha256Signature
                           )
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
        }
}
