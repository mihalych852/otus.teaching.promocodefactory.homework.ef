using System;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class CustomerShortDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public CustomerShortDTO()
        {
            
        }

        public CustomerShortDTO(Customer customer)
        {
            Id = customer.Id;
            Email = customer.Email;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
        }
    }
}