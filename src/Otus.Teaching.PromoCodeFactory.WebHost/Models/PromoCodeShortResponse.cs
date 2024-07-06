using System;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class PromoCodeShortResponse
    {
        public PromoCodeShortResponse() { }

        public PromoCodeShortResponse(PromoCode promoCode) 
        {
            Id = promoCode.Id;
            Code = promoCode.Code;
            ServiceInfo = promoCode.ServiceInfo;
            BeginDate = promoCode.BeginDate.ToString();
            EndDate = promoCode.EndDate.ToString();
            PartnerName = promoCode.PartnerName;
        }

        public Guid Id { get; set; }
        
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public string PartnerName { get; set; }
    }
}