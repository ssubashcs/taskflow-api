using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly TaskFlowDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(TaskFlowDbContext dbContext, IPasswordHasher<User> passwordHasher, IJwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();
            bool emailExists = await _dbContext.Users.AnyAsync(user => user.Email == normalizedEmail);

            if (emailExists) return null;

            User user = new() { Email = normalizedEmail };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();

            User? user = await _dbContext.Users.FirstOrDefaultAsync(existingUser => existingUser.Email == normalizedEmail);

            if (user is null) return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed) return null;

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

                await _dbContext.SaveChangesAsync();
            }

            return _jwtTokenService.CreateToken(user);
        }
    }
}
