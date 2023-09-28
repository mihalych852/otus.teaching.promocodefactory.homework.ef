using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration {
    [Table("employee")]
    [PrimaryKey(nameof(Id))]
    public class Employee
        : BaseEntity {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public Role Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
        public Guid RoleId { get; set; }

    }
}