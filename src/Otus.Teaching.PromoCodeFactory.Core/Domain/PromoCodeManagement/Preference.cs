using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        :BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        public Guid PromoCodeId { get; set; }
        public PromoCode PromoCode { get; set; }
    }
}