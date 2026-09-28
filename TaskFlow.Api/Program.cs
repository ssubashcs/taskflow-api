
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskFlow.Api.Data;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Register the dbContext to connect to the SQL Server database.
            // DI manages the creation and delivery of TaskFlowDbContext to classes that depend on it.
            builder.Services.AddDbContext<TaskFlowDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(
                                                                builder.Configuration.GetConnectionString("TaskFlowDatabase")));

            // Register TaskService as the implementation for ITaskService with scoped lifetime.                                                                
            builder.Services.AddScoped<ITaskService, TaskService>();

            // uses dependency injection to provide a password-hashing service wherever needed
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
 
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Configure token validation
            var jwtKey = builder.Configuration["Jwt:Key"] ?? 
                                    throw new InvalidOperationException("JWT signing key is not configured.");

            var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? 
                                        throw new InvalidOperationException("JWT issuer is not configured.");

            var jwtAudience = builder.Configuration["Jwt:Audience"] ?? 
                                            throw new InvalidOperationException("JWT audience is not configured.");

            // Register authentication and authorization
            // Anyone who bears/carries this token can use it to authenticate.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
                                options.TokenValidationParameters = new TokenValidationParameters()
                                {
                                    ValidateIssuer = true, ValidIssuer = jwtIssuer, 

                                    ValidateAudience = true, ValidAudience = jwtAudience,

                                    ValidateIssuerSigningKey = true, 
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

                                    ValidateLifetime = true
                                });

            builder.Services.AddAuthorization();

            // Singleton, because this service is stateless it does not hold request-specific data or a database context.
            // It only reads configuration and creates tokens.
            builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // creates standardized JSON error responses.
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            // catches unhandled exceptions across the application.
            app.UseExceptionHandler();
            // helps generate bodies for status-code responses that otherwise have no body.
            app.UseStatusCodePages();

            app.UseAuthentication();
            // checks whether the user is allowed to access an endpoint
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
