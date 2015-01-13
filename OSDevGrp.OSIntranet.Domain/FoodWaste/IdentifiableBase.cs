using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Basic functionality for an identifiable domain object in the food waste domain.
    /// </summary>
    public abstract class IdentifiableBase : DomainObjectBase, IIdentifiable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the domain object in the food wast domain.
        /// </summary>
        public virtual Guid? Identifier
        {
            get;
            set;
        }

        #endregion
    }
}
