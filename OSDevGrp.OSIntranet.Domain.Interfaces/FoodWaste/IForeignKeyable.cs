using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface which indicates that a domain object in the food waste domain has foreign keys.
    /// </summary>
    public interface IForeignKeyable : IIdentifiable
    {
        /// <summary>
        /// Gets the foreign keys for the domain object.
        /// </summary>
        IEnumerable<IForeignKey> ForeignKeys { get; }

        /// <summary>
        /// Adds a foreign key to the domain object.
        /// </summary>
        /// <param name="foreignKey">Foreign key which should be added to the domain object.</param>
        void ForeignKeyAdd(IForeignKey foreignKey);
    }
}
