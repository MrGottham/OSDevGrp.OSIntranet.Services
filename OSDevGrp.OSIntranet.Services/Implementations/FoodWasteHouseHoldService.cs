using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Claims;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.FoodWasteNamespace)]
    [RequiredClaimType(FoodWasteClaimTypes.HouseHoldManagement)]
    [RequiredClaimType(FoodWasteClaimTypes.ValidatedUser)]
    public class FoodWasteHouseHoldService : IFoodWasteHouseHoldService
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IFaultExceptionBuilder<FoodWasteFault> _foodWasteFaultExceptionBuilder; 

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a service which can access and modify data on a house hold in the food waste domain.
        /// </summary>
        /// <param name="commandBus">Implementation of the command bus.</param>
        /// <param name="queryBus">Implementation of the query bus.</param>
        /// <param name="foodWasteFaultExceptionBuilder">Implementation of builder which can generate faults.</param>
        public FoodWasteHouseHoldService(ICommandBus commandBus, IQueryBus queryBus, IFaultExceptionBuilder<FoodWasteFault> foodWasteFaultExceptionBuilder)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            if (foodWasteFaultExceptionBuilder == null)
            {
                throw new ArgumentNullException("foodWasteFaultExceptionBuilder");
            }
            _commandBus = commandBus;
            _queryBus = queryBus;
            _foodWasteFaultExceptionBuilder = foodWasteFaultExceptionBuilder;
        }

        #endregion

        #region Methods

        [OperationBehavior(TransactionScopeRequired = false)]
        public virtual void Test()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}