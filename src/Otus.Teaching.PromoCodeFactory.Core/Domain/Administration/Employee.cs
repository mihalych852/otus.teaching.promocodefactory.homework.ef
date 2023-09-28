using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration {
    [Table("employee")]
    [PrimaryKey(nameof(Id))]
    public class Employee
        : BaseEntity {
        [MaxLength(100)] public string FirstName { get; set; }
        [MaxLength(100)] public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        [MaxLength(100)] public string Email { get; set; }

        public Role Role { get; set; }

        public int AppliedPromoCodesCount { get; set; }
        public Guid RoleId { get; set; }
        [MaxLength(100)] public string PhoneNumber { get; set; }
    }
}