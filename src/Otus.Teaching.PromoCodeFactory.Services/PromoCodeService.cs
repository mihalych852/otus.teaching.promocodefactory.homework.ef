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

        public async Task<Guid> AddPromoCodeToCustomerViaPreference(PromoCodeForCreateDto newPromoCode)
        {
            var promoCodeForCreate = _mapper.Map<PromoCode>(newPromoCode);

            await _unitOfWork.PromoCodeRepository.AddAsync(promoCodeForCreate);
            await _unitOfWork.SaveChangesAsync();

            var promoCodePreference = await _unitOfWork.PreferencesRepository
                                                            .GetAll()
                                                            .Where(x => x.Name == newPromoCode.PreferenceName)
                                                            .ToListAsync();

            promoCodePreference.ForEach(x => x.PromoCode = promoCodeForCreate);
            var customers = await _unitOfWork
                                    .CustomerRepository
                                    .GetAll()
                                    .Where(x => x.Preferences.Any(y => y.Name == newPromoCode.PreferenceName))
                                    .ToListAsync();

            customers.ForEach(x => x.PromoCodes.Append(promoCodeForCreate));
            await _unitOfWork.SaveChangesAsync();

            return promoCodeForCreate.Id;
        }

        public async Task<List<PromoCode>> GetAll()
        {
            return await _unitOfWork.PromoCodeRepository.GetAll().ToListAsync();
        }
    }
}
