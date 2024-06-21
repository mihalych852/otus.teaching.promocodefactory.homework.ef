using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        :BaseEntity
    {
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(40)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<Preference> Preferences { get; set; }
        public IEnumerable<PromoCode> PromoCodes { get; set; }
    }
}