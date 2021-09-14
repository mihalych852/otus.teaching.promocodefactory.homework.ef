using AutoMapper;

using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mapping
{
    public class PromoCodeMapping : Profile
    {
        public PromoCodeMapping()
        {
            CreateCustomerMapping();
            CreateEmployeeMapping();
            CreateRoleMapping();
            CreatePreferenceMapping();
            CreatePromoCodeMapping();
        }

        private void CreatePromoCodeMapping()
        {
            var dateTimeFormat = "MM/dd/yyyy hh:mm:sszzz";
            CreateMap<PromoCode, PromoCodeShortResponse>()
                .ForMember(e => e.BeginDate,
                    opt => opt.MapFrom(src => src.BeginDate.ToString(dateTimeFormat)))
                .ForMember(e => e.EndDate,
                    opt => opt.MapFrom(src => src.EndDate.ToString(dateTimeFormat)));
        }

        private void CreatePreferenceMapping()
        {
            CreateMap<Preference, CustomerPreferenceResponse>();
        }

        private void CreateRoleMapping()
        {
            CreateMap<Role, RoleResponse>();
        }

        private void CreateEmployeeMapping()
        {
            CreateMap<Employee, EmployeeShortResponse>()
                .ForMember(e => e.FullName,
                    opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            CreateMap<Employee, EmployeeResponse>();
        }

        private void CreateCustomerMapping()
        {
            CreateMap<Customer, CustomerShortResponse>();

            CreateMap<Customer, CustomerResponse>();

            CreateMap<CreateOrEditCustomerRequest, Customer>()
                .AfterMap((src, dest) =>
                {
                    dest.Preferences ??= new List<Preference>();
                });
        }
    }

}
