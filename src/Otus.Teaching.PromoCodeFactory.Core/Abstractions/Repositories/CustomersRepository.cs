using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomersRepository : IRepository<Customer>
    {
        Task<IEnumerable<Guid>> GetCustomersIdsByPreferenceId(Guid preferenceId);
    }
}