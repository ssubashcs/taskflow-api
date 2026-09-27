namespace TaskFlow.Api.Dtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;

        // Stores the expiration time in Coordinated Universal Time (UTC)
        public DateTime ExpiresAtUtc { get; set; }
    }
}
