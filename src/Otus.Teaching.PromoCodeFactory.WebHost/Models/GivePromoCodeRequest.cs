using System;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class GivePromoCodeRequest
    {
        public string ServiceInfo { get; set; }

        public string PartnerName { get; set; }

        public string PromoCode { get; set; }

        public Guid PreferenceId { get; set; }
    }
}