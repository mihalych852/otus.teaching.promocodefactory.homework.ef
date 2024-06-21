using System;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(40)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [EmailAddress]
        public string Email { get; set; }

        public string? Skills { get; set; }

        public int AppliedPromocodesCount { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}