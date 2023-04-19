using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        :BaseEntity
    {
        public string Name { get; set; }

        public Guid PromoCodesId { get; set; }

        [NotMapped]
        public ICollection<CustomerPreference> CustomerPreferences { get; set; }
        [NotMapped]
        public PromoCode PromoCodes { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}