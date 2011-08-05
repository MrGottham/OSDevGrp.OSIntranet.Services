using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Repository til finansstyring.
    /// </summary>
    public class FinansstyringRepository : IFinansstyringRepository
    {
        #region Private constants

        private const string EndpointConfigurationName = "FinansstyringRepositoryService";

        #endregion

        #region Private variables

        private readonly IChannelFactory _channelFactory;
        private readonly IDomainObjectBuilder _domainObjectBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til finansstyring.
        /// </summary>
        /// <param name="channelFactory">Implementering af en ChannelFactory.</param>
        /// <param name="domainObjectBuilder">Implementering af domæneobjekt bygger.</param>
        public FinansstyringRepository(IChannelFactory channelFactory, IDomainObjectBuilder domainObjectBuilder)
        {
            if (channelFactory == null)
            {
                throw new ArgumentNullException("channelFactory");
            }
            if (domainObjectBuilder == null)
            {
                throw new ArgumentNullException("domainObjectBuilder");
            }
            _channelFactory = channelFactory;
            _domainObjectBuilder = domainObjectBuilder;
        }

        #endregion

        #region IFinansstyringRepository Members

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <param name="getBrevhovedCallback">Callbackmetode til at hente et givent brevhoved.</param>
        /// <returns>Liste af regnskaber.</returns>
        public IEnumerable<Regnskab> RegnskabslisteGet(Func<int, Brevhoved> getBrevhovedCallback)
        {
            if (getBrevhovedCallback == null)
            {
                throw new ArgumentNullException("getBrevhovedCallback");
            }
            var channel = _channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                // Hent alle regnskaber.
                var regnskabQuery = new RegnskabGetAllQuery();
                var regnskabViews = channel.RegnskabGetAll(regnskabQuery);
                // Mapning af regnskaber.
                _domainObjectBuilder.GetBrevhovedCallback = getBrevhovedCallback;
                return _domainObjectBuilder.BuildMany<RegnskabListeView, Regnskab>(regnskabViews);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <param name="getBrevhovedCallback">Callbackmetode til at hente et givent brevhoved.</param>
        /// <param name="getAdresseCallback">Callbackmetode til at hente en given adresse.</param>
        /// <returns>Regnskab.</returns>
        public Regnskab RegnskabGet(int nummer, Func<int, Brevhoved> getBrevhovedCallback, Func<int, AdresseBase> getAdresseCallback)
        {
            if (getBrevhovedCallback == null)
            {
                throw new ArgumentNullException("getBrevhovedCallback");
            }
            if (getAdresseCallback == null)
            {
                throw new ArgumentNullException("getAdresseCallback");
            }
            var channel = _channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                // Hent regnskabet.
                var regnskabQuery = new RegnskabGetByNummerQuery
                                        {
                                            Regnskabsnummer = nummer
                                        };
                var regnskabView = channel.RegnskabGetByNummer(regnskabQuery);
                // Hent alle kontogrupper.
                var kontogruppeQuery = new KontogruppeGetAllQuery();
                var kontogruppeViews = channel.KontogruppeGetAll(kontogruppeQuery);
                // Hent alle budgetkontogrupper.
                var budgetkontogruppeQuery = new BudgetkontogruppeGetAllQuery();
                var budgetkontogruppeViews = channel.BudgetkontogruppeGetAll(budgetkontogruppeQuery);
                // Mapning af regnskab.
                var kontogruppelisteHelper =
                    new KontogruppelisteHelper(
                        _domainObjectBuilder.BuildMany<KontogruppeView, Kontogruppe>(kontogruppeViews));
                var budgetkontogruppelisteHelper =
                    new BudgetkontogruppelisteHelper(
                        _domainObjectBuilder.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(budgetkontogruppeViews));
                _domainObjectBuilder.GetKontogruppeCallback = kontogruppelisteHelper.GetById;
                _domainObjectBuilder.GetBudgetkontogruppeCallback = budgetkontogruppelisteHelper.GetById;
                _domainObjectBuilder.GetBrevhovedCallback = getBrevhovedCallback;
                _domainObjectBuilder.GetAdresseBaseCallback = getAdresseCallback;
                return _domainObjectBuilder.Build<RegnskabView, Regnskab>(regnskabView);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste af kontogrupper.</returns>
        public IEnumerable<Kontogruppe> KontogruppeGetAll()
        {
            var channel = _channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new KontogruppeGetAllQuery();
                var kontogruppeViews = channel.KontogruppeGetAll(query);
                return _domainObjectBuilder.BuildMany<KontogruppeView, Kontogruppe>(kontogruppeViews);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter alle grupper til budgetkonti.
        /// </summary>
        /// <returns>Liste af grupper til budgetkonti.</returns>
        public IEnumerable<Budgetkontogruppe> BudgetkontogruppeGetAll()
        {
            var channel = _channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new BudgetkontogruppeGetAllQuery();
                var budgetkontogruppeViews = channel.BudgetkontogruppeGetAll(query);
                return _domainObjectBuilder.BuildMany<BudgetkontogruppeView, Budgetkontogruppe>(budgetkontogruppeViews);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tilføjer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringstidspunkt">Bogføringstidspunkt.</param>
        /// <param name="bilag">Bilag.</param>
        /// <param name="konto">Konto.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkonto">Budgetkonto.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adressekonto">Adressekonto.</param>
        public void BogføringslinjeAdd(DateTime bogføringstidspunkt, string bilag, Konto konto, string tekst, Budgetkonto budgetkonto, decimal debit, decimal kredit, AdresseBase adressekonto)
        {
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            if (string.IsNullOrEmpty(tekst))
            {
                throw new ArgumentNullException("tekst");
            }
            var channel = _channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                // Udførelse af kommando.
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = konto.Regnskab.Nummer,
                                      Bogføringsdato = bogføringstidspunkt,
                                      Bilag = bilag,
                                      Kontonummer = konto.Kontonummer,
                                      Tekst = tekst,
                                      Budgetkontonummer = budgetkonto == null ? null : budgetkonto.Kontonummer,
                                      Debit = debit,
                                      Kredit = kredit,
                                      AdresseId = adressekonto == null ? 0 : adressekonto.Nummer
                                  };
                var result = channel.BogføringslinjeAdd(command);
                // Behandling af resultat.
                var bogføringslinje = new Bogføringslinje(result.Løbenummer, result.Dato, result.Bilag, result.Tekst,
                                                          result.Debit, result.Kredit);
                konto.TilføjBogføringslinje(bogføringslinje);
                if (budgetkonto != null)
                {
                     budgetkonto.TilføjBogføringslinje(bogføringslinje);
                }
                if (adressekonto != null)
                {
                    adressekonto.TilføjBogføringslinje(bogføringslinje);
                }

                // TODO: Find ud af calculering af bogføringslinje.
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        #endregion
    }
}
