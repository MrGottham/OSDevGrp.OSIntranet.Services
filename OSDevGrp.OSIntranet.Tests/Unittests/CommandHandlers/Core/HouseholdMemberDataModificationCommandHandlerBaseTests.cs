using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tests the basic functionality which can handle a command for modifying some data on a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberDataModificationCommandHandlerBaseTests
    {
        /// <summary>
        /// Private class for a command which can modify some data on a household member.
        /// </summary>
        private class MyHouseholdMemberDataModificationCommand : HouseholdMemberDataModificationCommandBase
        {
        }

        /// <summary>
        /// Private class for testing the basic functionality which can handle a command for modifying some data on a household member.
        /// </summary>
        /// <typeparam name="TCommand">Type of the command for modifying some data on a household member.</typeparam>
        private class MyHouseholdMemberDataModificationCommandHandler<TCommand> : HouseholdMemberDataModificationCommandHandlerBase<TCommand> where TCommand : HouseholdMemberDataModificationCommandBase, new()
        {
            #region Private variables

            private TCommand _command;
            private IIdentifiable _identifiableMock;
            private readonly IHouseholdMember _householdMember;
            private readonly bool? _shouldBeActivated;
            private readonly bool? _shouldHaveAcceptedPrivacyPolicy;
            private readonly Membership? _requiredMembership;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates an instanse of the private class for testing the basic functionality which can handle a command for modifying some data on a household member.
            /// </summary>
            /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
            /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
            /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
            /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
            /// <param name="commonValidations">Implementation of a common validations.</param>
            /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
            /// <param name="householdMember">Implementation of the household member which this command handler should modify data on.</param>
            /// <param name="shouldBeActivated">Overrides whether the household member should be activated to execute the command handled by this command handler.</param>
            /// <param name="shouldHaveAcceptedPrivacyPolicy">Overrides whether the household member should have accepted the privacy policy to execute the command handled by this command handler.</param>
            /// <param name="requiredMembership">Overrides the requeired membership which the household member should have to execute the command handled by this command handler.</param>
            public MyHouseholdMemberDataModificationCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder, IHouseholdMember householdMember, bool? shouldBeActivated = null, bool? shouldHaveAcceptedPrivacyPolicy = null, Membership? requiredMembership = null)
                : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
            {
                if (householdMember == null)
                {
                    throw new ArgumentNullException("householdMember");
                }
                _householdMember = householdMember;
                _shouldBeActivated = shouldBeActivated;
                _shouldHaveAcceptedPrivacyPolicy = shouldHaveAcceptedPrivacyPolicy;
                _requiredMembership = requiredMembership;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets whether the household member should be activated to execute the command handled by this command handler.
            /// </summary>
            public override bool ShouldBeActivated
            {
                get { return _shouldBeActivated.HasValue ? _shouldBeActivated.Value : base.ShouldBeActivated; }
            }

            /// <summary>
            /// Gets whether the household member should have accepted the privacy policy to execute the command handled by this command handler.
            /// </summary>
            public override bool ShouldHaveAcceptedPrivacyPolicy
            {
                get { return _shouldHaveAcceptedPrivacyPolicy.HasValue ? _shouldHaveAcceptedPrivacyPolicy.Value : base.ShouldHaveAcceptedPrivacyPolicy; }
            }

            /// <summary>
            /// Gets the requeired membership which the household member should have to execute the command handled by this command handler.
            /// </summary>
            public override Membership RequiredMembership
            {
                get { return _requiredMembership.HasValue ? _requiredMembership.Value : base.RequiredMembership; }
            }

            /// <summary>
            /// Gets whether AddValidationRules has been called.
            /// </summary>
            public bool AddValidationRulesWasCalled { get; private set; }

            /// <summary>
            /// Gets whether ModifyData has been called.
            /// </summary>
            public bool ModifyDataWasCalled { get; private set; }

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

            /// <summary>
            /// Generates and returns a command which can be used with this command handler.
            /// </summary>
            /// <returns>Command which can be used with this command handler.</returns>
            public TCommand GenerateCommand()
            {
                return _command ?? (_command = new TCommand());
            }

            /// <summary>
            /// Generates and returns a mockup of an identifiable domain object in the food waste domain.
            /// </summary>
            /// <returns>Mockup of an identifiable domain object in the food waste domain.</returns>
            public IIdentifiable GenerateIdentifiableMock()
            {
                return _identifiableMock ?? (_identifiableMock = MockRepository.GenerateMock<IIdentifiable>());
            }

            /// <summary>
            /// Adds validation rules to the specification which encapsulates validation rules.
            /// </summary>
            /// <param name="householdMember">Household member for which to modify data.</param>
            /// <param name="command">Command for modifying some data on a household member.</param>
            /// <param name="specification">Specification which encapsulates validation rules.</param>
            public override void AddValidationRules(IHouseholdMember householdMember, TCommand command, ISpecification specification)
            {
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember, Is.EqualTo(_householdMember));
                Assert.That(command, Is.Not.Null);
                Assert.That(command, Is.EqualTo(GenerateCommand()));
                Assert.That(specification, Is.Not.Null);
                Assert.That(specification, Is.EqualTo(Specification));

                AddValidationRulesWasCalled = true;
            }

            /// <summary>
            /// Modifies the data.
            /// </summary>
            /// <param name="householdMember">Household member for which to modify data.</param>
            /// <param name="command">Command for modifying some data on a household member.</param>
            /// <returns>An identifiable domain object in the food waste domain.</returns>
            public override IIdentifiable ModifyData(IHouseholdMember householdMember, TCommand command)
            {
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember, Is.EqualTo(_householdMember));
                Assert.That(command, Is.Not.Null);
                Assert.That(command, Is.EqualTo(GenerateCommand()));

                ModifyDataWasCalled = true;

                return GenerateIdentifiableMock();
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize the basic functionality which can handle a command for modifying some data on a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberDataModificationCommandHandlerBase()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ShouldBeActivated, Is.True);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdMemberDataModificationCommandHandlerBase.RequiredMembership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetHouseholdDataRepository(), Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetHouseholdDataRepository(), Is.EqualTo(householdDataRepositoryMock));
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetClaimValueProvider(), Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetClaimValueProvider(), Is.EqualTo(claimValueProviderMock));
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetObjectMapper(), Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetObjectMapper(), Is.EqualTo(objectMapperMock));
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetSpecification(), Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetSpecification(), Is.EqualTo(specificationMock));
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetCommonValidations(), Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetCommonValidations(), Is.EqualTo(commonValidationsMock));
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetExceptionBuilder(), Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.GetExceptionBuilder(), Is.EqualTo(exceptionBuilderMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(null, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, null, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, null, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, null, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, null, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null, DomainObjectMockBuilder.BuildHouseholdMemberMock()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the household member which this command handler should modify data on is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute throws an ArgumentNullException when the command for modifying some data on a household member is null.
        /// </summary>
        [Test]
        public void TestThatExecuteThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberDataModificationCommandHandlerBase.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Execute calls MailAddress on the provider which can resolve values from the current users claims.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMailAddressOnClaimValueProvider()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            claimValueProviderMock.AssertWasCalled(m => m.MailAddress);
        }

        /// <summary>
        /// Tests that Execute calls HouseholdMemberGetByMailAddress on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsHouseholdMemberGetByMailAddressOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var mailAddress = fixture.Create<string>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            householdDataRepositoryMock.AssertWasCalled(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that Execute calls IsNotNull with household member on the common validations.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsNotNullWithHouseholdMemberOnCommonValidations()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
        }

        /// <summary>
        /// Tests that Execute does not call IsActivated on the household member when the household member does not need to be activated.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallIsActivatedOnHouseholdMemberWhenHouseholdMemberDoesNotNeedToBeActivated()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock, false);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ShouldBeActivated, Is.False);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            householdMemberMock.AssertWasNotCalled(m => m.IsActivated);
        }

        /// <summary>
        /// Tests that Execute calls IsActivated on the household member when the household member should be activated.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsActivatedOnHouseholdMemberWhenHouseholdMemberShouldBeActivated()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ShouldBeActivated, Is.True);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            householdMemberMock.AssertWasCalled(m => m.IsActivated);
        }

        /// <summary>
        /// Tests that Execute does not call IsPrivacyPolicyAccepted on the household member when the household member does not need to have accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatExecuteDoesNotCallIsPrivacyPolicyAcceptedOnHouseholdMemberWhenHouseholdMemberDoesNotNeedToHaveAcceptedPrivacyPolicy()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock, shouldHaveAcceptedPrivacyPolicy: false);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.False);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            householdMemberMock.AssertWasNotCalled(m => m.IsPrivacyPolicyAccepted);
        }

        /// <summary>
        /// Tests that Execute calls IsPrivacyPolicyAccepted on the household member when the household member should have accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsPrivacyPolicyAcceptedOnHouseholdMemberWhenHouseholdMemberShouldHaveAcceptedPrivacyPolicy()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ShouldHaveAcceptedPrivacyPolicy, Is.True);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            householdMemberMock.AssertWasCalled(m => m.IsPrivacyPolicyAccepted);
        }

        /// <summary>
        /// Tests that Execute calls HasRequiredMembership on the household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatExecuteCallsHasRequiredMembershipOnHouseholdMember(Membership requiredMembership)
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .WhenCalled(e =>
                {
                    var func = (Func<bool>) e.Arguments.ElementAt(0);
                    func.Invoke();
                })
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock, requiredMembership: requiredMembership);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.RequiredMembership, Is.EqualTo(requiredMembership));

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            householdMemberMock.AssertWasCalled(m => m.HasRequiredMembership(Arg<Membership>.Is.Equal(requiredMembership)));
        }

        /// <summary>
        /// Tests that Execute calls IsSatisfiedBy on the specification which encapsulates validation rules 4 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsIsSatisfiedByOnSpecification4Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(4));
        }

        /// <summary>
        /// Tests that Execute calls AddValidationRules to add validation rules to the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsAddValidationRules()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.AddValidationRulesWasCalled, Is.False);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            Assert.That(householdMemberDataModificationCommandHandlerBase.AddValidationRulesWasCalled, Is.True);
        }

        /// <summary>
        /// Tests that Execute calls Evaluate on the specification which encapsulates validation rules 2 times.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecification2Times()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            specificationMock.AssertWasCalled(m => m.Evaluate(), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that Execute calls Evaluate on the specification which encapsulates validation once before the call to method which add validation rules and once after the call to method which add validation rules.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecificationOnceBeforeCallToAddValidationRulesAndOnceAfterCallToAddValidationRules()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var numberOfCalls = 0;
            specificationMock.Stub(m => m.Evaluate())
                .WhenCalled(e =>
                {
                    numberOfCalls += 1;
                    switch (numberOfCalls)
                    {
                        case 1:
                            Assert.That(householdMemberDataModificationCommandHandlerBase.AddValidationRulesWasCalled, Is.False);
                            break;

                        case 2:
                            Assert.That(householdMemberDataModificationCommandHandlerBase.AddValidationRulesWasCalled, Is.True);
                            break;
                    }
                })
                .Repeat.Any();

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            specificationMock.AssertWasCalled(m => m.Evaluate(), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that Execute calls Evaluate on the specification which encapsulates validation rules before call to the method which modifies the data.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsEvaluateOnSpecificationBeforeCallToModifyData()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ModifyDataWasCalled, Is.False);

            specificationMock.Stub(m => m.Evaluate())
                .WhenCalled(e => Assert.That(householdMemberDataModificationCommandHandlerBase.ModifyDataWasCalled, Is.False))
                .Repeat.Any();

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that Execute calls ModifyData to modify the data.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsModifyData()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);
            Assert.That(householdMemberDataModificationCommandHandlerBase.ModifyDataWasCalled, Is.False);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            Assert.That(householdMemberDataModificationCommandHandlerBase.ModifyDataWasCalled, Is.True);
        }

        /// <summary>
        /// Tests that Execute calls Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteCallsMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(fixture.Create<ServiceReceiptResponse>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());

            objectMapperMock.AssertWasCalled(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Equal(householdMemberDataModificationCommandHandlerBase.GenerateIdentifiableMock()), Arg<CultureInfo>.Is.Null));
        }

        /// <summary>
        /// Tests that Execute returns the result from Map on the object mapper which can map objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatExecuteReturnsResultFromMapOnFoodWasteObjectMapper()
        {
            var fixture = new Fixture();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.HouseholdMemberGetByMailAddress(Arg<string>.Is.Anything))
                .Return(householdMemberMock)
                .Repeat.Any();

            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            claimValueProviderMock.Stub(m => m.MailAddress)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var serviceReceipt = fixture.Create<ServiceReceiptResponse>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            objectMapperMock.Stub(m => m.Map<IIdentifiable, ServiceReceiptResponse>(Arg<IIdentifiable>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(serviceReceipt)
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, householdMemberMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var result = householdMemberDataModificationCommandHandlerBase.Execute(householdMemberDataModificationCommandHandlerBase.GenerateCommand());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(serviceReceipt));
        }

        /// <summary>
        /// Tests that HandleException calls Build on the builder which can build exceptions.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionCallsBuildOnExceptionBuilder()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var methodBase = (MethodBase) e.Arguments.ElementAt(1);
                    Assert.That(methodBase, Is.Not.Null);
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (HouseholdMemberDataModificationCommandHandlerBase<>).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => householdMemberDataModificationCommandHandlerBase.HandleException(householdMemberDataModificationCommandHandlerBase.GenerateCommand(), exception));

            exceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tests that HandleException throws the created exception from the builder which can build exceptions.
        /// </summary>
        [Test]
        public void TestThatHandleExceptionThrowsCreatedExceptionFromExceptionBuilder()
        {
            var fixture = new Fixture();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exceptionToThrow = fixture.Create<Exception>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(exceptionToThrow)
                .Repeat.Any();

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock, DomainObjectMockBuilder.BuildHouseholdMemberMock());
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => householdMemberDataModificationCommandHandlerBase.HandleException(householdMemberDataModificationCommandHandlerBase.GenerateCommand(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
