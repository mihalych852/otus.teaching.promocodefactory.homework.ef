using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Exceptions
{
    public class DataInsertionConflictException 
        : Exception
    {
        public IEnumerable<string> ConflictingIdentifiers { get; }

        public DataInsertionConflictException(string message, Exception innerException, params string[] conflictingIdentifiers)
            : base(message ?? "Conflict occurred while inserting a record(s)", innerException)
        {
            ConflictingIdentifiers = conflictingIdentifiers;
        }

        public DataInsertionConflictException(params string[] conflictingIdentifiers)
            : this(null, null, conflictingIdentifiers) { }
    }
}