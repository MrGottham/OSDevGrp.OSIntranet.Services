using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service which can access and modify system data in the food waste domain.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.FoodWasteNamespace)]
    public class FoodWasteSystemDataService : IFoodWasteSystemDataService
    {
        #region Private variables

        //private readonly ICommandBus _commandBus;
        //private readonly IQueryBus _queryBus;

        #endregion

        /// <summary>
        /// Creates a service which can access and modify system data in the food waste domain.
        /// </summary>
        /// <param name="commandBus">Implementation of the command bus.</param>
        /// <param name="queryBus">Implementation of the query bus.</param>
        public FoodWasteSystemDataService(ICommandBus commandBus, IQueryBus queryBus)
        {
            
        }

        #region Methods

        /// <summary>
        /// Gets all the translation informations which can be used for translations.
        /// </summary>
        /// <param name="query">Query for getting all the translation informations which can be used for translations.</param>
        /// <returns>Collection of all the translation informations which can be used for translations.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual IEnumerable<TranslationInfoSystemView> TranslationInfoGetAll(TranslationInfoCollectionGetQuery query)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}