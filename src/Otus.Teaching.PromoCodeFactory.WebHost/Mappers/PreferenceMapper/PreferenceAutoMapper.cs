using System.Collections.Generic;
using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mappers.PreferenceMapper
{
    public class PreferenceAutoMapper:IPreferenceMapper
    {
        public PreferenceResponse ToResponse(Preference preference, IEnumerable<CustomerShortResponse> customers)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Preference, PreferenceResponse>()
                    .ForMember(dst => dst.Customers,
                        opt => opt.Ignore());
                cfg.CreateMap<IEnumerable<CustomerShortResponse>, PreferenceResponse>()
                    .ForMember(dst => dst.Customers,
                        opt =>
                            opt.MapFrom(src => new List<CustomerShortResponse>(customers)));
            });

            var mapper = config.CreateMapper();

            var response = mapper.Map<PreferenceResponse>(preference);
            mapper.Map(customers, response);
            return response;
        }

        public PreferenceShortResponse ToShortResponse(Preference preference)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Preference, PreferenceShortResponse>());

            return config.CreateMapper().Map<PreferenceShortResponse>(preference);
        }
    }
}