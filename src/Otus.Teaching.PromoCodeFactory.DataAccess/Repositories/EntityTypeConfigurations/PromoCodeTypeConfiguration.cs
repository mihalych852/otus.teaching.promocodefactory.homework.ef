using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.EntityTypeConfigurations
{
    public class PromoCodeTypeConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder
                .HasKey(pc => pc.Id);

            builder
                .Property(pc => pc.Code)
                .HasMaxLength(32);

            builder
                .Property(pc => pc.ServiceInfo)
                .HasMaxLength(128);

            builder
                .Property(pc => pc.PartnerName)
                .HasMaxLength(128);
        }
    }
}