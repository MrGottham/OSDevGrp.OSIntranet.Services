using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which handles a command for importing a food group from a given data provider.
    /// </summary>
    public class ImportFoodGroupFromDataProviderCommandHandler : FoodWasteSystemDataCommandHandlerBase
    {
        #region Private variables

        private readonly ILogicExecutor _logicExecutor;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for importing a food group from a given data provider.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        /// <param name="logicExecutor">Implementation of the logic executor which can execute basic logic.</param>
        public ImportFoodGroupFromDataProviderCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, ILogicExecutor logicExecutor)
            : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations)
        {
            if (logicExecutor == null)
            {
                throw new ArgumentNullException("logicExecutor");
            }
            _logicExecutor = logicExecutor;
        }

        #endregion
    }
}
