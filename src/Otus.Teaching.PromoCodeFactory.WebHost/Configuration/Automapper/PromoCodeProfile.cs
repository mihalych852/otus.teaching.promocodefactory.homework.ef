using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Configuration.Automapper
{
    public class PromoCodeProfile : Profile
    {
        public PromoCodeProfile()
        {
            CreateMap<GivePromoCodeRequest, PromoCode>()
                .ForMember(pr => pr.Code, opt =>
                    opt.MapFrom(g => g.PromoCode))
                .ForSourceMember(s => s.Preference, opt => opt.DoNotValidate())
                .ForMember(pr => pr.Preference, opt => opt.Ignore());
            
            CreateMap<PromoCode, PromoCodeShortResponse>()
                .ForMember(pr => pr.BeginDate, opt =>
                    opt.MapFrom(p => p.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(pr => pr.EndDate, opt =>
                    opt.MapFrom(p => p.EndDate.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}