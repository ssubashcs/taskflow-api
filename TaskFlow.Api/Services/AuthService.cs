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

        public AuthService(TaskFlowDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
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
    }
}
