using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Configuration
{
    public class CustomerPreferenceConfiguration : IEntityTypeConfiguration<CustomerPreference>
    {
        public void Configure(EntityTypeBuilder<CustomerPreference> builder)
        {
            builder.HasKey(bc => new { bc.CustomerId, bc.PreferenceId });

            builder.HasOne(bc => bc.Customer)
                .WithMany(b => b.CustomerPreferences)
                .HasForeignKey(bc => bc.CustomerId);

            builder.HasOne(bc => bc.Preference)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(bc => bc.PreferenceId);
        }
    }
}
