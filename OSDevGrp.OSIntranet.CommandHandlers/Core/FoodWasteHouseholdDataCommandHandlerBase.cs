using System;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basic functionality for command handlers which handles commands for household data in the food waste domain.
    /// </summary>
    public abstract class FoodWasteHouseholdDataCommandHandlerBase : CommandHandlerTransactionalBase
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;
        private readonly IFoodWasteObjectMapper _foodWasteObjectMapper;
        private readonly ISpecification _specification;
        private readonly ICommonValidations _commonValidations;
        private readonly IExceptionBuilder _exceptionBuilder;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the basic functionality for command handlers which handles commands for household data in the food waste domain.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of the repository which can access household data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        /// <param name="exceptionBuilder">Implementation of the builder which can build exceptions.</param>
        protected FoodWasteHouseholdDataCommandHandlerBase(IHouseholdDataRepository householdDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException("householdDataRepository");
            }
            if (foodWasteObjectMapper == null)
            {
                throw new ArgumentNullException("foodWasteObjectMapper");
            }
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }
            if (commonValidations == null)
            {
                throw new ArgumentNullException("commonValidations");
            }
            if (exceptionBuilder == null)
            {
                throw new ArgumentNullException("exceptionBuilder");
            }
            _householdDataRepository = householdDataRepository;
            _foodWasteObjectMapper = foodWasteObjectMapper;
            _specification = specification;
            _commonValidations = commonValidations;
            _exceptionBuilder = exceptionBuilder;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository which can access household data for the food waste domain.
        /// </summary>
        protected virtual IHouseholdDataRepository HouseholdDataRepository
        {
            get { return _householdDataRepository; }
        }

        /// <summary>
        /// Gets the object mapper which can map objects in the food waste domain.
        /// </summary>
        protected virtual IFoodWasteObjectMapper ObjectMapper
        {
            get { return _foodWasteObjectMapper; }
        }

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

        /// <summary>
        /// Gets the builder which can build exceptions.
        /// </summary>
        protected virtual IExceptionBuilder ExceptionBuilder
        {
            get { return _exceptionBuilder; }
        }

        #endregion
    }
}
