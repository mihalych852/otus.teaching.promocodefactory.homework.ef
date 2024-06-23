using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;

namespace Otus.Teaching.PromoCodeFactory.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromoCodeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddPromoCodeToCustomerViaPreference(PromoCodeForCreateDto newPromoCode)
        {
            var promoCodeForCreate = _mapper.Map<PromoCode>(newPromoCode);

            var promoCodePreference = await _unitOfWork.PreferencesRepository
                                                            .GetAll()
                                                            .Where(x => x.Name == newPromoCode.PreferenceName)
                                                            .Include(x => x.PromoCodes)
                                                            .FirstOrDefaultAsync();

            if (promoCodePreference is null)
                return false;

            promoCodeForCreate.PreferenceId = promoCodePreference.Id;

            var customers = await _unitOfWork
                                    .CustomerRepository
                                    .GetAll()
                                    .Where(x => x.Preferences.Any(y => y.Name == newPromoCode.PreferenceName))
                                    .Include(x => x.PromoCodes)
                                    .ToListAsync();

            foreach (var customer in customers)
            {
                await _unitOfWork.PromoCodeRepository.AddAsync(new PromoCode
                        {
                            Code = promoCodeForCreate.Code,
                            ServiceInfo = promoCodeForCreate.ServiceInfo,
                            BeginDate = promoCodeForCreate.BeginDate,
                            EndDate = promoCodeForCreate.EndDate,
                            PartnerName = promoCodeForCreate.PartnerName,
                            CustomerId = customer.Id,
                            PreferenceId = promoCodeForCreate.PreferenceId,
                        });
            }
            
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<PromoCode>> GetAll()
        {
            return await _unitOfWork.PromoCodeRepository.GetAll().ToListAsync();
        }
    }
}
