
namespace ServiceAppApi.Models
{
    public class PartyRole
    {
        public int PartyRoleId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public virtual Role Role { get; set; }
        public int RoleId { get; set; }
        public virtual Address Address { get; set; }
        public int AddressId { get; set; }
    }
}
