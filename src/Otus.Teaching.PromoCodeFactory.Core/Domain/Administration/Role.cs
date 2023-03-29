using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Role
        : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Employee> Employee { get; set; }
    }
}