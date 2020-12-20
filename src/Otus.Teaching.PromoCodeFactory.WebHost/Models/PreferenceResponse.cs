using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class PreferenceResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public List<CustomerResponse> Customers { get; set; }
    }
}