using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Services
{
    public interface IPromoCodeService
    {
        Task<List<PromoCode>> GetAll();
        Task<Guid> AddPromoCodeToCustomerViaPreference(PromoCodeForCreateDto newPromoCode);
    }
}
