using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public interface IJwtTokenService
    {
        LoginResponseDto CreateToken(User user);
    }
}
