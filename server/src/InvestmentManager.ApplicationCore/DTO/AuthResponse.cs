namespace InvestmentManager.ApplicationCore.DTO
{
    public class AuthResponse
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string AccessToken { get; set; }
    }
}
