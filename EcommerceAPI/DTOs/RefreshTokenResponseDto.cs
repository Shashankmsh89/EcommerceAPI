namespace EcommerceAPI.DTOs
{
    public class RefreshTokenResponseDto
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}