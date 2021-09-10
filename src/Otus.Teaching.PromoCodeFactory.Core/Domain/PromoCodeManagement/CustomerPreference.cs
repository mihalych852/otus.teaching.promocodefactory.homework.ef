using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class CustomerPreference
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid PreferenceId { get; set; }
        public Preference Preference { get; set; }
    }
}
