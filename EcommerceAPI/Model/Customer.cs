namespace EcommerceAPI.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}