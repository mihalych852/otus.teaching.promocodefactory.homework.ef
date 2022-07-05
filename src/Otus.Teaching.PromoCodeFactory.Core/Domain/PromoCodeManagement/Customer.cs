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

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(250)]
        public string Email { get; set; }

        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; }
        
        public virtual ICollection<PromoCode> Promocodes { get; set; }
    }
}