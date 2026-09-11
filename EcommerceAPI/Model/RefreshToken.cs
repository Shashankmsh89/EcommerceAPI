namespace EcommerceAPI.Models
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }

        public int CustomerId { get; set; }

        public string TokenHash { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public DateTime? RevokedOn { get; set; }

        public string? ReplacedByTokenHash { get; set; }
    }
}