using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class CustomerPreference
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int PreferenceId { get; set; }
        public Preference Preference { get; set; }
    }
}
