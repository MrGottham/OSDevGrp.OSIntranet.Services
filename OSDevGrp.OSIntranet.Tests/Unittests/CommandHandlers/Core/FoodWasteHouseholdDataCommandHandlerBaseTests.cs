using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality for command handlers which handles commands for household data in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteHouseholdDataCommandHandlerBaseTests
    {
        /// <summary>
        /// Private class for testing the basic functionality for command handlers which handles commands for household data in the food waste domain.
        /// </summary>
        private class MyFoodWasteHouseholdDataCommandHandler : FoodWasteHouseholdDataCommandHandlerBase
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the basic functionality for command handlers which handles commands for household data in the food waste domain.
            /// </summary>
            /// <param name="householdDataRepository">Implementation of the repository which can access household data for the food waste domain.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
            /// <param name="commonValidations">Implementation of the common validations.</param>
            /// <param name="exceptionBuilder">Implementation of the builder which can build exceptions.</param>
            public MyFoodWasteHouseholdDataCommandHandler(IHouseholdDataRepository householdDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
                : base(householdDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access household data for the food waste domain.
            /// </summary>
            public IHouseholdDataRepository GetHouseholdDataRepository()
            {
                return HouseholdDataRepository;
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

            /// <summary>
            /// Gets the builder which can build exceptions.
            /// </summary>
            public IExceptionBuilder GetExceptionBuilder()
            {
                return ExceptionBuilder;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality for command handlers which handles commands for household data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodWasteHouseholdDataCommandHandlerBase()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataCommandHandlerBase = new MyFoodWasteHouseholdDataCommandHandler(householdDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataCommandHandlerBase, Is.Not.Null);
            Assert.That(householdDataCommandHandlerBase.GetHouseholdDataRepository(), Is.Not.Null);
            Assert.That(householdDataCommandHandlerBase.GetHouseholdDataRepository(), Is.EqualTo(householdDataRepositoryMock));
            Assert.That(householdDataCommandHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(householdDataCommandHandlerBase.GetObjectMapper(), Is.EqualTo(foodWasteObjectMapperMock));
            Assert.That(householdDataCommandHandlerBase.GetSpecification(), Is.Not.Null);
            Assert.That(householdDataCommandHandlerBase.GetSpecification(), Is.EqualTo(specificationMock));
            Assert.That(householdDataCommandHandlerBase.GetCommonValidations(), Is.Not.Null);
            Assert.That(householdDataCommandHandlerBase.GetCommonValidations(), Is.EqualTo(commonValidationsMock));
            Assert.That(householdDataCommandHandlerBase.GetExceptionBuilder(), Is.Not.Null);
            Assert.That(householdDataCommandHandlerBase.GetExceptionBuilder(), Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteHouseholdDataCommandHandler(null, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteHouseholdDataCommandHandler(householdDataRepositoryMock, null, specificationMock, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("foodWasteObjectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteHouseholdDataCommandHandler(householdDataRepositoryMock, foodWasteObjectMapperMock, null, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommonValidationsIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteHouseholdDataCommandHandler(householdDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commonValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the builder which can build exceptions is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenExceptionBuilderIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteHouseholdDataCommandHandler(householdDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
