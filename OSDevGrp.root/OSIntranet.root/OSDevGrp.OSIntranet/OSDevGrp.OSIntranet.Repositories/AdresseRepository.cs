using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Repository til adressekartoteket.
    /// </summary>
    public class AdresseRepository : IAdresseRepository
    {
        #region Private constants

        private const string EndpointConfigurationName = "AdresseRepositoryService";

        #endregion

        #region Private variables

        private readonly IChannelFactory _channelFactory;
        private readonly IDomainObjectBuilder _domainObjectBuilder;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til adressekartoteket.
        /// </summary>
        /// <param name="channelFactory">Implementering af en ChannelFactory.</param>
        /// <param name="domainObjectBuilder">Implementering af domæneobjekt bygger.</param>
        public AdresseRepository(IChannelFactory channelFactory, IDomainObjectBuilder domainObjectBuilder)
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

        #region IAdresseRepository Members

        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <returns>Liste af adresser.</returns>
        public IEnumerable<AdresseBase> AdresseGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                // Henter alle adressegrupper.
                var adressegruppeQuery = new AdressegruppeGetAllQuery();
                var adressegruppeViews = channel.AdressegruppeGetAll(adressegruppeQuery);
                // Henter alle betalingsbetingelser.
                var betalingsbetingelseQuery = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelseViews = channel.BetalingsbetingelseGetAll(betalingsbetingelseQuery);
                // Henter alle firmaadresser.
                var firmaQuery = new FirmaGetAllQuery();
                var firmaViews = channel.FirmaGetAll(firmaQuery);
                // Henter alle personadresser.
                var personQuery = new PersonGetAllQuery();
                var personViews = channel.PersonGetAll(personQuery);
                // Mapper views til adresser.
                lock (SyncRoot)
                {
                    var adresser = new List<AdresseBase>();
                    var adressegruppelisteHelper = new AdressegruppelisteHelper(_domainObjectBuilder.BuildMany<AdressegruppeView, Adressegruppe>(adressegruppeViews));
                    var betalingsbetingelselisteHelper = new BetalingsbetingelselisteHelper(_domainObjectBuilder.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(betalingsbetingelseViews));
                    var adresselisteHelper = new AdresselisteHelper(adresser);
                    _domainObjectBuilder.GetAdressegruppeCallback = adressegruppelisteHelper.GetById;
                    _domainObjectBuilder.GetBetalingsbetingelseCallback = betalingsbetingelselisteHelper.GetById;
                    _domainObjectBuilder.GetAdresseBaseCallback = adresselisteHelper.GetById;
                    adresser.AddRange(_domainObjectBuilder.BuildMany<FirmaView, AdresseBase>(firmaViews));
                    adresser.AddRange(_domainObjectBuilder.BuildMany<PersonView, AdresseBase>(personViews));
                    return adresser.OrderBy(m => m, new AdresseComparer()).ToList();
                }
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
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <returns>Liste af postnumre.</returns>
        public IEnumerable<Postnummer> PostnummerGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new PostnummerGetAllQuery();
                var postnummerViews = channel.PostnummerGetAll(query);
                return _domainObjectBuilder.BuildMany<PostnummerView, Postnummer>(postnummerViews);
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
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste af adressegrupper.</returns>
        public IEnumerable<Adressegruppe> AdressegruppeGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new AdressegruppeGetAllQuery();
                var adressegruppeViews = channel.AdressegruppeGetAll(query);
                return _domainObjectBuilder.BuildMany<AdressegruppeView, Adressegruppe>(adressegruppeViews);
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
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <returns>Liste af betalingsbetingelser.</returns>
        public IEnumerable<Betalingsbetingelse> BetalingsbetingelseGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelseViews = channel.BetalingsbetingelseGetAll(query);
                return
                    _domainObjectBuilder.BuildMany<BetalingsbetingelseView, Betalingsbetingelse>(
                        betalingsbetingelseViews);
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
