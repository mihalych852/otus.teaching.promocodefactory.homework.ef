using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode
        : BaseEntity
    {
        [Required, MaxLength(30)]
        public string Code { get; set; }

        [MaxLength(20)]
        public string ServiceInfo { get; set; }

        [Required]
        public DateTime BeginDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, MaxLength(20)]
        public string PartnerName { get; set; }

        [Required]
        public Employee PartnerManager { get; set; }

        [Required]
        public Preference Preference { get; set; }
    }
}