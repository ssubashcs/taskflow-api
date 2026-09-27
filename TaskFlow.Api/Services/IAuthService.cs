using TaskFlow.Api.Dtos;

namespace TaskFlow.Api.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto?> RegisterAsync(RegisterRequestDto request);

        Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    }
}
