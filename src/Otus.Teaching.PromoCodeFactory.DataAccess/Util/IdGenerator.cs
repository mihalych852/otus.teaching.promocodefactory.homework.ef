using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

using System;
using System.Collections.Generic;
using System.Text;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Util
{
    public class IdGenerator : ValueGenerator<Guid>
    {
        public override Guid Next(EntityEntry entry)
        {
            return Guid.NewGuid();
        }

        public override bool GeneratesTemporaryValues => false;
    }

}
