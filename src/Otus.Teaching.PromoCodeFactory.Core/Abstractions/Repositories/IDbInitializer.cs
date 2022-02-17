using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IDbInitializer
    {
        public void InitializeDB();
        //Task<T> RegisterNew();
        //Task<T> RegisterDiry();
        //Task<T> RegisterClean();
        //Task<T> RegisterDeleted();
        //Task Commit();
        //Task Rolback();

    }
}
