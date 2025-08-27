namespace practice_api.Data
{
    public class RefreshTokens
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string UserId { get; set; }
        public AppIdentityUser User { get; set; }

    }
}
