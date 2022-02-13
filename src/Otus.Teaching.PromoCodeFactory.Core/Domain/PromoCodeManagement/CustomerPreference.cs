using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class CustomerPreference: BaseEntity
    {
        public Customer Customer { get; set; }
        public Preference Preference { get; set; }
    }
}
