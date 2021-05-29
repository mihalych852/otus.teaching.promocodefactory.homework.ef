using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class PreferenceRepository: EfRepository<Preference>
    {
        public PreferenceRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public override async Task<Preference> GetByIdAsync(Guid id)
        {
            return await DataContext.Preferences
                .Include(p => p.CustomerPreferences)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}