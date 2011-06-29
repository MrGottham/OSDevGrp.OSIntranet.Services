using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces;

namespace OSDevGrp.OSIntranet.Domain
{
    /// <summary>
    /// Hjælper til en liste af domæneobjekter.
    /// </summary>
    /// <typeparam name="TDomainObject">Typen på listens domæneobjekter.</typeparam>
    /// <typeparam name="TId">Typen på Id for listens domæneobjekter.</typeparam>
    public abstract class DomainObjectListHelper<TDomainObject, TId> : IDomainObjectListHelper<TDomainObject, TId>
    {
        #region Private variables

        private readonly IEnumerable<TDomainObject> _domainObjects;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af domæneobjekter.
        /// </summary>
        /// <param name="domainObjects">Domæneobjekter.</param>
        protected DomainObjectListHelper(IEnumerable<TDomainObject> domainObjects)
        {
            if (domainObjects == null)
            {
                throw new ArgumentNullException("domainObjects");
            }
            _domainObjects = domainObjects;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Domæneobjekter.
        /// </summary>
        protected IEnumerable<TDomainObject> DomainObjetcs
        {
            get
            {
                return _domainObjects;
            }
        }

        #endregion

        #region IDomainObjectListHelper<TDomainObject,TId> Members

        /// <summary>
        /// Henter og returnerer et givent domæneobjekt fra listen.
        /// </summary>
        /// <param name="id">Unik identifikation af domæneobjektet, der skal hentes fra listen.</param>
        /// <returns>Domæneobjekt.</returns>
        public abstract TDomainObject GetById(TId id);

        #endregion
    }
}
