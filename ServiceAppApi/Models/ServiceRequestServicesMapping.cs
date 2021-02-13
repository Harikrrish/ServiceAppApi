
namespace ServiceAppApi.Models
{
    public class ServiceRequestServicesMapping
    {
        public int ServiceRequestServicesMappingId { get; set; }
        public virtual ServiceRequest ServiceRequest { get; set; }
        public int? ServiceRequestId { get; set; }
        public virtual ProductServiceMapping ProductServiceMapping { get; set; }
        public int ProductServiceMappingId { get; set; }
        public double Cost { get; set; }
    }
}
