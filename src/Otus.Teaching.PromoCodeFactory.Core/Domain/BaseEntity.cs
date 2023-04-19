using System;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}