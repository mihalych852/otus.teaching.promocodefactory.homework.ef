using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        :BaseEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(201)]
        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(100)]
        public string Email { get; set; }
        
        public virtual ICollection<PromoCode> PromoCodes { get; set; }

        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; }
    }
}