using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Configuration
{
    public class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.Property(x => x.Code)
                .HasMaxLength(255);

            builder.Property(x => x.ServiceInfo)
                .HasMaxLength(1000);

            builder.Property(x => x.PartnerName)
                .HasMaxLength(200);
        }
    }
}
