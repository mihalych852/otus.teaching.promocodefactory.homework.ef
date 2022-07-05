using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(250)]
        public string Email { get; set; }

        public int AppliedPromocodesCount { get; set; }

        public Guid RoleId { get; set; }
        
        public virtual Role Role { get; set; }

        public virtual ICollection<PromoCode> Promocodes { get; set; }
    }
}