using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class GivePromoCodeRequest
    {
        [MaxLength(300)]
        public string ServiceInfo { get; set; }

        [MaxLength(100)]
        public string PartnerName { get; set; }

        [MaxLength(30)]
        public string PromoCode { get; set; }

        public string Preference { get; set; }
    }
}