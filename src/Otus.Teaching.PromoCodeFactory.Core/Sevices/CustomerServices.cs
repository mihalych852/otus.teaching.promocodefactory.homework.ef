using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;

namespace Otus.Teaching.PromoCodeFactory.Core.Sevices
{
    public class CustomerServices : ICustinerServices
    {
        private readonly IEfRepository<Customer> _customerRepository;
        private readonly IEfRepository<PromoCode> _promoCodeRepository;
        private readonly IEfRepository<Preference> _preferenceRepository;

        public CustomerServices(IEfRepository<Customer> customerRepository, IEfRepository<PromoCode> promoCodeRepository, IEfRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _promoCodeRepository = promoCodeRepository;
            _preferenceRepository = preferenceRepository;
        }

        //public async Task<Customer>


    }
}
