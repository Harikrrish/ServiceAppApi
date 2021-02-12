
using System;

namespace ServiceAppApi.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public virtual PartyRole Customer { get; set; }
        public int CustomerId { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public string ServiceRequestStatus { get; set; }
        public double PaidAmount { get; set; }
        public virtual PartyRole AssignedExecutive { get; set; }
        public int? AssignedExecutiveId { get; set; }
        public DateTime? ScheduledDateTime { get; set; }
    }
}
