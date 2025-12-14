using HolookorBackend.Core.Application.DTOs;

namespace HolookorBackend.Core.Application.Authentication
{
    public interface IJWTAuthenticationManager
    {
        public string GenerateToken(UserDto user);

    }
}
