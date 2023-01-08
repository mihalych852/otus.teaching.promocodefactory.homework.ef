using System;
using System.Linq;
using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mapping
{
    /// <summary>
    /// AutoMapper profile
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// MappingProfile
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Customer, CustomerShortResponse>();
            CreateMap<Customer, CustomerResponse>();
            CreateMap<CreateOrEditCustomerRequest, Customer>()
                .ForMember(c => c.Preferences, source => source.MapFrom(
                    c => c.PreferenceIds.Select(p => new Preference { Id = p }).ToList()));
            CreateMap<PromoCode, PromoCodeShortResponse>();
            CreateMap<GivePromoCodeRequest, PromoCode>()
                .ForMember(p => p.Code, source => source.MapFrom(
                g => g.PromoCode))
                .ForMember(p => p.Preference, source => source.MapFrom(
                    g => new Preference(){Id = Guid.Parse(g.Preference)}));
            CreateMap<Preference, PreferenceResponse>();

        }
    }
}
