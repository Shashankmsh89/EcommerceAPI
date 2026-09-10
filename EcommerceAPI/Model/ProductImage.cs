namespace EcommerceAPI.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public int ProductId { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public byte[] ImageData { get; set; }

        public DateTime UploadedOn { get; set; }
    }
}