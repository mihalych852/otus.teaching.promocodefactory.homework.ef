using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement {
    [Table("promo_code")]
    [PrimaryKey(nameof(Id))]
    public class PromoCode
        : BaseEntity {
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PartnerName { get; set; }

        public Guid PreferenceId { get; set; }
        public Guid CustomerId { get; }

        public virtual Preference Preference { get; }
        public virtual Employee PartnerManager { get; }
        public virtual Customer Customer { get; }
    }
}