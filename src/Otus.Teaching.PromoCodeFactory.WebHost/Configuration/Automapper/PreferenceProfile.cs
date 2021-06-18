using AutoMapper;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Configuration.Automapper
{
    public class PreferenceProfile : Profile
    {
        public PreferenceProfile()
        {
            CreateMap<Preference, PreferenceResponse>();
        }
    }
}