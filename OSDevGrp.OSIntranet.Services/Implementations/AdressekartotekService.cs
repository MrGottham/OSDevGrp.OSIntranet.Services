using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service til adressekartotek.
    /// </summary>
    public class AdressekartotekService : IntranetServiceBase, IAdressekartotekService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til adressekartotek.
        /// </summary>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public AdressekartotekService(IQueryBus queryBus)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IAdressekartotekService Members

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle postnumre.</param>
        /// <returns>Liste af postnumre.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<PostnummerView> PostnumreGet(PostnumreGetQuery query)
        {
            try
            {
                return _queryBus.Query<PostnumreGetQuery, IEnumerable<PostnummerView>>(query);
            }
            catch (IntranetRepositoryException ex)
            {
                throw CreateIntranetRepositoryFault(ex);
            }
            catch (IntranetBusinessException ex)
            {
                throw CreateIntranetBusinessFault(ex);
            }
            catch (IntranetSystemException ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
            catch (Exception ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
        }

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="query">Foresprøgelse efter alle adressegrupper.</param>
        /// <returns>Liste af adressegrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<AdressegruppeView> AdressegrupperGet(AdressegrupperGetQuery query)
        {
            try
            {
                return _queryBus.Query<AdressegrupperGetQuery, IEnumerable<AdressegruppeView>>(query);
            }
            catch (IntranetRepositoryException ex)
            {
                throw CreateIntranetRepositoryFault(ex);
            }
            catch (IntranetBusinessException ex)
            {
                throw CreateIntranetBusinessFault(ex);
            }
            catch (IntranetSystemException ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
            catch (Exception ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
        }

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="query">Foresprøgelse efter alle betalingsbetingelser.</param>
        /// <returns>Liste af betalingsbetingelser.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BetalingsbetingelseView> BetalingsbetingelserGet(BetalingsbetingelserGetQuery query)
        {
            try
            {
                return _queryBus.Query<BetalingsbetingelserGetQuery, IEnumerable<BetalingsbetingelseView>>(query);
            }
            catch (IntranetRepositoryException ex)
            {
                throw CreateIntranetRepositoryFault(ex);
            }
            catch (IntranetBusinessException ex)
            {
                throw CreateIntranetBusinessFault(ex);
            }
            catch (IntranetSystemException ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
            catch (Exception ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
        }

        #endregion
    }
}