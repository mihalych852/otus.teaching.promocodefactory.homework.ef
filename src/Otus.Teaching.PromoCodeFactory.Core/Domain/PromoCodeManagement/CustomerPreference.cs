using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

[Table("customer_preference")]
[PrimaryKey(nameof(Id))]
public class CustomerPreference: BaseEntity {
    public Guid CustomerId { get; set; }
    public Guid PreferenceId { get; set; }
}