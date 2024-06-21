using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class CreateOrEditCustomerRequest
    {
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(40)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<Guid> PreferenceIds { get; set; }
    }
}