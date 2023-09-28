using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration {
    [Table("role")]
    [PrimaryKey(nameof(Id))]
    public class Role
        : BaseEntity {
        public string Name { get; set; }

        public string Description { get; set; }
        public virtual Employee Employee { get; }
    }
}