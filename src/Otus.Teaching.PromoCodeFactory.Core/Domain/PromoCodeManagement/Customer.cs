using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement {
    [Table("customer")]
    [PrimaryKey(nameof(Id))]
    public class Customer
        : BaseEntity {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public virtual ICollection<Preference> Preferences { get; }
        public virtual ICollection<PromoCode> PromoCodes { get; }
    }
}