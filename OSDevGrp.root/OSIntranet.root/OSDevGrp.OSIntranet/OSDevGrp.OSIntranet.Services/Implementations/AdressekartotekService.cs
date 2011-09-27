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
        /// Henter en telefonliste.
        /// </summary>
        /// <param name="query">Forespørgelse efter en telefonliste.</param>
        /// <returns>Telefonliste.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<TelefonlisteView> TelefonlisteGet(TelefonlisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<TelefonlisteGetQuery, IEnumerable<TelefonlisteView>>(query);
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
        /// Henter en liste af personer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af personer.</param>
        /// <returns>Liste af personer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<PersonView> PersonlisteGet(PersonlisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<PersonlisteGetQuery, IEnumerable<PersonView>>(query);
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
        /// Henter en given person.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given person.</param>
        /// <returns>Person.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public PersonView PersonGet(PersonGetQuery query)
        {
            try
            {
                return _queryBus.Query<PersonGetQuery, PersonView>(query);
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
        /// Henter en liste af firmaer.
        /// </summary>
        /// <param name="query">Forespørgelse efter en liste af firmaer.</param>
        /// <returns>Liste af firmaer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<FirmaView> FirmalisteGet(FirmalisteGetQuery query)
        {
            try
            {
                return _queryBus.Query<FirmalisteGetQuery, IEnumerable<FirmaView>>(query);
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
        /// Henter et givent firma.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public FirmaView FirmaGet(FirmaGetQuery query)
        {
            try
            {
                return _queryBus.Query<FirmaGetQuery, FirmaView>(query);
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