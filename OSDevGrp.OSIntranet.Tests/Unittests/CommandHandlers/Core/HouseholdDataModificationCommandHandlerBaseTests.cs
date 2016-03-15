using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Test the basic functionality which can handle a command for modifying some data on a given household on the current household member.
    /// </summary>
    [TestFixture]
    public class HouseholdDataModificationCommandHandlerBaseTests
    {
        /// <summary>
        /// Private class for a command for modifying some data on a given household on the current household member.
        /// </summary>
        private class MyHouseholdDataModificationCommand : HouseholdDataModificationCommandBase
        {
        }

        /// <summary>
        /// Private class for testing tasic functionality which can handle a command for modifying some data on a given household on the current household member.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command for modifying some data on a given household on the current household member.</typeparam>
        private class MyHouseholdDataModificationCommandHandler<TCommand> : HouseholdDataModificationCommandHandlerBase<TCommand> where TCommand : HouseholdDataModificationCommandBase
        {
            #region Constructor

            /// <summary>
            /// Create an instance of the private class for testing tasic functionality which can handle a command for modifying some data on a given household on the current household member.
            /// </summary>
            /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
            /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
            /// <param name="commonValidations">Implementation of a common validations.</param>
            /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
            public MyHouseholdDataModificationCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
                : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
            {
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets the repository which can access household data for the food waste domain.
            /// </summary>
            public IHouseholdDataRepository GetHouseholdDataRepository()
            {
                return base.HouseholdDataRepository;
            }

            /// <summary>
            /// Gets the provider which can resolve values from the current users claims.
            /// </summary>
            public IClaimValueProvider GetClaimValueProvider()
            {
                return base.ClaimValueProvider;
            }

            /// <summary>
            /// Gets the object mapper which can map objects in the food waste domain.
            /// </summary>
            public IFoodWasteObjectMapper GetObjectMapper()
            {
                return base.ObjectMapper;
            }

            /// <summary>
            /// Gets the specification which encapsulates validation rules.
            /// </summary>
            public ISpecification GetSpecification()
            {
                return base.Specification;
            }

            /// <summary>
            /// Gets the common validations.
            /// </summary>
            public ICommonValidations GetCommonValidations()
            {
                return base.CommonValidations;
            }

            /// <summary>
            /// Gets the builder which can build exceptions.
            /// </summary>
            public IExceptionBuilder GetExceptionBuilder()
            {
                return base.ExceptionBuilder;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality which can handle a command for modifying some data on a given household on the current household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataModificationCommandHandlerBase()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.ShouldBeActivated, Is.True);
            Assert.That(householdDataModificationCommandHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdDataModificationCommandHandlerBase.RequiredMembership, Is.EqualTo(Membership.Basic));
            Assert.That(householdDataModificationCommandHandlerBase.GetHouseholdDataRepository(), Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.GetHouseholdDataRepository(), Is.EqualTo(householdDataRepositoryMock));
            Assert.That(householdDataModificationCommandHandlerBase.GetClaimValueProvider(), Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.GetClaimValueProvider(), Is.EqualTo(claimValueProviderMock));
            Assert.That(householdDataModificationCommandHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.GetObjectMapper(), Is.EqualTo(objectMapperMock));
            Assert.That(householdDataModificationCommandHandlerBase.GetSpecification(), Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.GetSpecification(), Is.EqualTo(specificationMock));
            Assert.That(householdDataModificationCommandHandlerBase.GetCommonValidations(), Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.GetCommonValidations(), Is.EqualTo(commonValidationsMock));
            Assert.That(householdDataModificationCommandHandlerBase.GetExceptionBuilder(), Is.Not.Null);
            Assert.That(householdDataModificationCommandHandlerBase.GetExceptionBuilder(), Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data for the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(null, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can resolve values from the current users claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, null, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the object mapper which can map objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenFoodWasteObjectMapperIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, null, specificationMock, commonValidationsMock, exceptionBuilderMock));
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
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, null, commonValidationsMock, exceptionBuilderMock));
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
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, null, exceptionBuilderMock));
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
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household member for which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataModificationCommandHandlerBase.AddValidationRules(null, fixture.Create<MyHouseholdDataModificationCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for modifying some data on a household member is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataModificationCommandHandlerBase.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the specification which encapsulates validation rules is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenSpecificationIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataModificationCommandHandlerBase.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<MyHouseholdDataModificationCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules calls Households on the the household member for which to modify data.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHouseholdsOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.AddValidationRules(householdMemberMock, householdDataModificationCommandBase, specificationMock);

            householdMemberMock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(4)); // Tree times in the test and one time in AddValidationRules.
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsNotNull with the household on which to modify data on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsNotNullWithHouseholdOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.IsNotNull(householdMock);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.AddValidationRules(householdMemberMock, householdDataModificationCommandBase, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHousehold>.Is.Equal(householdMock)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification1Time()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.AddValidationRules(householdMemberMock, householdDataModificationCommandBase, specificationMock);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.NotNull), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that AddValidationRules calls Evaluate on the specification which encapsulates validation.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.AddValidationRules(householdMemberMock, householdDataModificationCommandBase, specificationMock);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household member for which to modify data is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataModificationCommandHandlerBase.ModifyData(null, fixture.Create<MyHouseholdDataModificationCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for modifying some data on a household member is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataModificationCommandHandlerBase.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData calls Households on the the household member for which to modify data.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHouseholdsOnHouseholdMember()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.ModifyData(householdMemberMock, householdDataModificationCommandBase);

            householdMemberMock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(4)); // Tree times in the test and one time in AddValidationRules.
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the household on which to modify data on the common validations.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithHouseholdOnCommonValidations()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdMock = householdMemberMock.Households.FirstOrDefault();
            Assert.IsNotNull(householdMock);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMock.Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.ModifyData(householdMemberMock, householdDataModificationCommandBase);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHousehold>.Is.Equal(householdMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 1 time.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification1Time()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.ModifyData(householdMemberMock, householdDataModificationCommandBase);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.NotNull), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that ModifyData calls Evaluate on the specification which encapsulates validation.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdDataModificationCommandHandlerBase = new MyHouseholdDataModificationCommandHandler<MyHouseholdDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdDataModificationCommandHandlerBase, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            Assert.That(householdMemberMock, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Null);
            Assert.That(householdMemberMock.Households, Is.Not.Empty);

            var householdDataModificationCommandBase = fixture.Build<MyHouseholdDataModificationCommand>()
                // ReSharper disable PossibleInvalidOperationException
                .With(m => m.HouseholdIdentifier, householdMemberMock.Households.First().Identifier.Value)
                // ReSharper restore PossibleInvalidOperationException
                .Create();

            householdDataModificationCommandHandlerBase.ModifyData(householdMemberMock, householdDataModificationCommandBase);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }
    }
}
