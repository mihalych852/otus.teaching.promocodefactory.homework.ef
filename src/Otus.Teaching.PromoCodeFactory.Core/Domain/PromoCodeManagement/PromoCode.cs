using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime;
using System.Text.Json.Serialization;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class PromoCode
        : BaseEntity
    {
        [MaxLength(30)]
        public string Code { get; set; }

        [MaxLength(300)]
        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(100)]
        public string PartnerName { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid PreferenceId { get; set; }
        public Preference Preference { get; set; }
    }
}