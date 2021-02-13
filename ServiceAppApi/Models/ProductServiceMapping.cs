
namespace ServiceAppApi.Models
{
    public class ProductServiceMapping
    {
        public int ProductServiceMappingId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual Service Service { get; set; }
        public int ServiceId { get; set; }
        public double Cost { get; set; }
        public bool IsActive { get; set; }
    }
}
