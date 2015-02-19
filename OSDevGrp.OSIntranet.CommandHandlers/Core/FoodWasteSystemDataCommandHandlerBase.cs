using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basic functionality for command handlers which handles commands for system data in the food waste domain.
    /// </summary>
    public abstract class FoodWasteSystemDataCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly ISystemDataRepository _systemDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;
        private readonly ISpecification _specification;
        private readonly ICommonValidations _commonValidations;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the basic functionality for command handlers which handles commands for system data in the food waste domain.
        /// </summary>
        /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        protected FoodWasteSystemDataCommandHandlerBase(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations)
        {
            _systemDataRepository = systemDataRepository;
            _foodWasteObjectMapper = foodWasteObjectMapper;
            _specification = specification;
            _commonValidations = commonValidations;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository which can access system data for the food waste domain.
        /// </summary>
        protected virtual ISystemDataRepository SystemDataRepository
        {
            get { return _systemDataRepository; }
        }

        /// <summary>
        /// Gets the object mapper which can map objects in the food waste domain.
        /// </summary>
        protected virtual IFoodWasteObjectMapper ObjectMapper { get { return _foodWasteObjectMapper; } }

        /// <summary>
        /// Gets the specification which encapsulates validation rules.
        /// </summary>
        protected virtual ISpecification Specification
        {
            get { return _specification; }
        }

        /// <summary>
        /// Gets the common validations.
        /// </summary>
        protected virtual ICommonValidations CommonValidations
        {
            get { return _commonValidations; }
        }

        #endregion
    }
}
