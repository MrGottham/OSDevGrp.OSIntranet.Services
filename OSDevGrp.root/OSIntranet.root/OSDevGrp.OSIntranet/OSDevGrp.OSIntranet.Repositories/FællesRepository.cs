using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
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
        private readonly IDomainObjectBuilder _domainObjectBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til fælles elementer i domænet.
        /// </summary>
        /// <param name="channelFactory">Implementering af en ChannelFactory.</param>
        /// <param name="domainObjectBuilder">Implementering af domæneobjekt bygger.</param>
        public FællesRepository(IChannelFactory channelFactory, IDomainObjectBuilder domainObjectBuilder)
        {
            if (channelFactory == null)
            {
                throw new ArgumentNullException("channelFactory");
            }
            if (domainObjectBuilder == null)
            {
                throw new ArgumentNullException("channelFactory");
            }
            _channelFactory = channelFactory;
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
            var channel = _channelFactory.CreateChannel<IFællesRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new BrevhovedGetAllQuery();
                var brevhovedViews = channel.BrevhovedGetAll(query);
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
        /// Henter alle systemer under OSWEBDB.
        /// </summary>
        /// <returns>Liste af systemer under OSWEBDB.</returns>
        public IEnumerable<ISystem> SystemerGetAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
