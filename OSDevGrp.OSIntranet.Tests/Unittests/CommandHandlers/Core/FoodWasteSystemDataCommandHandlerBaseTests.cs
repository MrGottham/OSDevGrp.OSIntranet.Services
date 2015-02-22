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

        /// <summary>
        /// Tests that the constructor initialize the basic functionality for command handlers which handles commands for system data in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodWasteSystemDataCommandHandler()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var systemDataCommandHandlerBase = new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, commonValidationsMock);
            Assert.That(systemDataCommandHandlerBase, Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetSystemDataRepository(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetSystemDataRepository(), Is.EqualTo(systemDataRepositoryMock));
            Assert.That(systemDataCommandHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetObjectMapper(), Is.EqualTo(foodWasteObjectMapperMock));
            Assert.That(systemDataCommandHandlerBase.GetSpecification(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetSpecification(), Is.EqualTo(specificationMock));
            Assert.That(systemDataCommandHandlerBase.GetCommonValidations(), Is.Not.Null);
            Assert.That(systemDataCommandHandlerBase.GetCommonValidations(), Is.EqualTo(commonValidationsMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access system data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSystemDataRepositoryIsNull()
        {
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(null, foodWasteObjectMapperMock, specificationMock, commonValidationsMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("systemDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, null, specificationMock, commonValidationsMock));
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
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, null, commonValidationsMock));
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
            var systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            var foodWasteObjectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyFoodWasteSystemDataCommandHandler(systemDataRepositoryMock, foodWasteObjectMapperMock, specificationMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commonValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
