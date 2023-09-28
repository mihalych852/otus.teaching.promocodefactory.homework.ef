using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement {
    [Table("preference")]
    [PrimaryKey(nameof(Id))]
    public class Preference
        : BaseEntity {
        public string Name { get; set; }
        public virtual PromoCode PromoCode { get; set; }
        public virtual ICollection<Customer> Customers { get; }
    }
}