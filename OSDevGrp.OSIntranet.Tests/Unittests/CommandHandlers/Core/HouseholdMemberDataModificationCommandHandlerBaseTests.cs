﻿using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Ploeh.AutoFixture;
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
            public MyHouseholdMemberDataModificationCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder)
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

            /// <summary>
            /// Generate and returns a command which can be used with this command handler.
            /// </summary>
            /// <returns>Command which can be used with this command handler.</returns>
            public TCommand GenerateCommand()
            {
                return _command ?? (_command = new TCommand());
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

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(null, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, null, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, null, specificationMock, commonValidationsMock, exceptionBuilderMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, null, commonValidationsMock, exceptionBuilderMock));
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commonValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the the builder which can build exceptions is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenExceptionBuilderIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
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

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberDataModificationCommandHandlerBase.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
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

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
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

            var householdMemberDataModificationCommandHandlerBase = new MyHouseholdMemberDataModificationCommandHandler<MyHouseholdMemberDataModificationCommand>(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberDataModificationCommandHandlerBase, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => householdMemberDataModificationCommandHandlerBase.HandleException(householdMemberDataModificationCommandHandlerBase.GenerateCommand(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
