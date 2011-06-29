using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.QueryHandlers
{
    /// <summary>
    /// Basisklasse for en QueryHandler.
    /// </summary>
    public abstract class QueryHandlerBase
    {
        #region Private variables

        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner Basisklasse for en QueryHandler.
        /// </summary>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        protected QueryHandlerBase(IObjectMapper objectMapper)
        {
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _objectMapper = objectMapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Mapper et domæneobjekt til et view.
        /// </summary>
        /// <typeparam name="TDomainObject">Typen på domæneobjektet, der skal mappes.</typeparam>
        /// <typeparam name="TView">Typen på viewet, der skal mappes til.</typeparam>
        /// <param name="domainObject">Domæneobjektet, der skal mappes.</param>
        /// <returns>View.</returns>
        public TView Map<TDomainObject, TView>(TDomainObject domainObject)
        {
            if (Equals(domainObject, null))
            {
                throw new ArgumentNullException("domainObject");
            }
            return _objectMapper.Map<TDomainObject, TView>(domainObject);
        }

        /// <summary>
        /// Mapper et eller flere domæneobjekter til et eller flere views.
        /// </summary>
        /// <typeparam name="TDomainObject">Typen på domæneobjekter, der skal mappes.</typeparam>
        /// <typeparam name="TView">Typen på views, der skal mappes til.</typeparam>
        /// <param name="domainObjects">Domæneobjekter, der skal mappes.</param>
        /// <returns>Views.</returns>
        public IEnumerable<TView> MapMany<TDomainObject, TView>(IEnumerable<TDomainObject> domainObjects)
        {
            if (Equals(domainObjects, null))
            {
                throw new ArgumentNullException("domainObjects");
            }
            return _objectMapper.Map<IEnumerable<TDomainObject>, IEnumerable<TView>>(domainObjects);
        }

        #endregion
    }
}
