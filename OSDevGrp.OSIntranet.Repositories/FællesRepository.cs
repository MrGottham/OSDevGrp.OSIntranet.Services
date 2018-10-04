using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.Fælles;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Repository til fælles elementer i domænet.
    /// </summary>
    public class FællesRepository : IFællesRepository
    {
        #region Private constants

        private const string EndpointConfigurationName = "FællesRepositoryService";

        #endregion

        #region Private variables

        private readonly IChannelFactory _channelFactory;
        private readonly IMySqlDataProvider _mySqlDataProvider;
        private readonly IDomainObjectBuilder _domainObjectBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til fælles elementer i domænet.
        /// </summary>
        /// <param name="channelFactory">Implementering af en ChannelFactory.</param>
        /// <param name="mySqlDataProvider">Implementering af data provider til MySql.</param>
        /// <param name="domainObjectBuilder">Implementering af domæneobjekt bygger.</param>
        public FællesRepository(IChannelFactory channelFactory, IMySqlDataProvider mySqlDataProvider, IDomainObjectBuilder domainObjectBuilder)
        {
            ArgumentNullGuard.NotNull(channelFactory, nameof(channelFactory))
                .NotNull(mySqlDataProvider, nameof(mySqlDataProvider))
                .NotNull(domainObjectBuilder, nameof(domainObjectBuilder));

            _channelFactory = channelFactory;
            _mySqlDataProvider = mySqlDataProvider;
            _domainObjectBuilder = domainObjectBuilder;
        }

        #endregion

        #region IFællesRepository Members

        /// <summary>
        /// Henter alle brevhoveder.
        /// </summary>
        /// <returns>Liste af brevhoveder.</returns>
        public IEnumerable<Brevhoved> BrevhovedGetAll()
        {
            IFællesRepositoryService channel = _channelFactory.CreateChannel<IFællesRepositoryService>(EndpointConfigurationName);
            try
            {
                BrevhovedGetAllQuery query = new BrevhovedGetAllQuery();
                IEnumerable<BrevhovedView> brevhovedViews = channel.BrevhovedGetAll(query);
                return _domainObjectBuilder.BuildMany<BrevhovedView, Brevhoved>(brevhovedViews);
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
        /// Henter alle systemer under OSWEBDB.
        /// </summary>
        /// <returns>Liste af systemer under OSWEBDB.</returns>
        public IEnumerable<ISystem> SystemGetAll()
        {
            try
            {
                MySqlCommand command = new CommonCommandBuilder("SELECT SystemNo,Title,Properties FROM Systems ORDER BY SystemNo").Build();
                return _mySqlDataProvider.GetCollection<SystemProxy>(command);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        #endregion
    }
}
