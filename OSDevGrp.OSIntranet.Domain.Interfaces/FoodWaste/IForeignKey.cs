using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a foreign key to a domain object in the food waste domain.
    /// </summary>
    public interface IForeignKey : IIdentifiable
    {
        /// <summary>
        /// Gets the data provider who own the foreign key.
        /// </summary>
        IDataProvider DataProvider { get; }

        /// <summary>
        /// Gets the identifier for the domain object which has this foreign key.
        /// </summary>
        Guid ForeignKeyForIdentifier { get; }

        /// <summary>
        /// Gets the types which has this foreign key.
        /// </summary>
        IEnumerable<Type> ForeignKeyForTypes { get; }

        /// <summary>
        /// Gets the value of the foreign key.
        /// </summary>
        string ForeignKeyValue { get; set; }
    }
}
