using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for basic functionality used by repositories in the food waste domain.
    /// </summary>
    public interface IDataRepository : IRepository
    {
        /// <summary>
        /// Gets a given identifiable domain object from the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifier">Identifier for the domain object to get.</param>
        /// <returns>The identifiable domain object.</returns>
        TIdentifiable Get<TIdentifiable>(Guid identifier) where TIdentifiable : IIdentifiable;

        /// <summary>
        /// Inserts an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to insert.</param>
        /// <returns>The inserted identifiable domain object.</returns>
        TIdentifiable Insert<TIdentifiable>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable;

        /// <summary>
        /// Updates an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to update.</param>
        /// <returns>The updated identifiable domain object.</returns>
        TIdentifiable Update<TIdentifiable>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable;

        /// <summary>
        /// Deletes an identifiable domain object in the repository.
        /// </summary>
        /// <typeparam name="TIdentifiable">Type of the identifiable domain object.</typeparam>
        /// <param name="identifiable">Identifiable domain object to delete.</param>
        void Delete<TIdentifiable>(TIdentifiable identifiable) where TIdentifiable : IIdentifiable;
    }
}
