using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.EntityTypeConfigurations
{
    public class CustomerPreferenceTypeConfiguration : IEntityTypeConfiguration<CustomerPreference>
    {
        public void Configure(EntityTypeBuilder<CustomerPreference> builder)
        {
            builder
                .HasAlternateKey(c => c.CustomerId);
            
            builder
                .HasKey(x => new { x.CustomerId, x.PreferenceId });
            
            builder
                .HasOne(x => x.Customer)
                .WithMany(x => x.CustomerPreferences)
                .HasForeignKey(x => x.CustomerId);
            
            builder
                .HasOne(x => x.Preference)
                .WithMany(x => x.CustomerPreferences)
                .HasForeignKey(x => x.PreferenceId);
        }
    }
}