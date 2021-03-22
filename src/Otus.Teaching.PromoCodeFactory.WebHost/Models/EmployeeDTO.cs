using System;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public RoleItemDTO Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}