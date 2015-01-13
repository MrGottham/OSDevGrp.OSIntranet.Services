using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for an identifiable domain object in the food waste domain.
    /// </summary>
    public interface IIdentifiable : IDomainObject
    {
        /// <summary>
        /// Get or sets the identfier for the domain object in the food wast domain.
        /// </summary>
        Guid? Identifier { get; set; }
    }
}
