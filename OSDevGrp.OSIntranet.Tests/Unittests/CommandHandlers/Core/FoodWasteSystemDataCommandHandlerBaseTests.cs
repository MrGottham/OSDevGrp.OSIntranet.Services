using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality for command handlers which handles commands for system data in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteSystemDataCommandHandlerBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality for command handlers which handles commands for system data in the food waste domain.
        /// </summary>
        private class MyFoodWasteSystemDataCommandHandler : FoodWasteSystemDataCommandHandlerBase
        {
            #region Constructor

            /// <summary>
            /// Creates a private class for testing the basic functionality for command handlers which handles commands for system data in the food waste domain.
            /// </summary>
            /// <param name="systemDataRepository">Implementation of the repository which can access system data for the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
            /// <param name="commonValidations">Implementation of the common validations.</param>
            public MyFoodWasteSystemDataCommandHandler(ISystemDataRepository systemDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations)
                : base(systemDataRepository, foodWasteObjectMapper, specification, commonValidations)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access system data for the food waste domain.
            /// </summary>
            public ISystemDataRepository GetSystemDataRepository()
            {
                return SystemDataRepository;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return ObjectMapper;
            }

            /// <summary>
            /// Gets the specification which encapsulates validation rules.
            /// </summary>
            public ISpecification GetSpecification()
            {
                return Specification;
            }

            /// <summary>
            /// Gets the common validations.
            /// </summary>
            public ICommonValidations GetCommonValidations()
            {
                return CommonValidations;
            }
            
            #endregion
        }
    }
}
