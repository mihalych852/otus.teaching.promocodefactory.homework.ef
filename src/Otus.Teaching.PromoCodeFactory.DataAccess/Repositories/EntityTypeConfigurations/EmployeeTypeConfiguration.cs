using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.EntityTypeConfigurations
{
    public class EmployeeTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.FirstName)
                .HasMaxLength(128);

            builder
                .Property(e => e.LastName)
                .HasMaxLength(128);

            builder
                .Property(e => e.Email)
                .HasMaxLength(128);
        }
    }
}