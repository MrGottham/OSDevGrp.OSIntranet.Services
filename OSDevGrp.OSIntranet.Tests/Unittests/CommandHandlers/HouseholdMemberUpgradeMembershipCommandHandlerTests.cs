using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
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

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Test the command handler which can handles the command for upgrading the membership on the current users household member account.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberUpgradeMembershipCommandHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize the command handler which can handles the command for upgrading the membership on the current users household member account.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberUpgradeMembershipCommandHandler()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);
            Assert.That(householdMemberUpgradeMembershipCommandHandler.ShouldBeActivated, Is.True);
            Assert.That(householdMemberUpgradeMembershipCommandHandler.ShouldHaveAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdMemberUpgradeMembershipCommandHandler.RequiredMembership, Is.EqualTo(Membership.Basic));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("domainObjectValidations"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the household member on which the membership should be upgraded is null.
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
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberUpgradeMembershipCommandHandler.AddValidationRules(null, fixture.Create<HouseholdMemberUpgradeMembershipCommand>(), specificationMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules throws an ArgumentNullException when the command for upgrading the membership on the current users household member account is null.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesThrowsArgumentNullExceptionWhenCommandIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null, specificationMock));
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
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), fixture.Create<HouseholdMemberUpgradeMembershipCommand>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("specification"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the membership which the household member should be upgraded to on the common validations.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatAddValidationRulesCallsHasValueWithMembershipOnCommonValidations(Membership upgradeToMembership)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var upgradeToMembershipAsString = upgradeToMembership.ToString();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, upgradeToMembershipAsString)
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(upgradeToMembershipAsString)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLegalEnumValue with the membership which the household member should be upgraded to on the common validations.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatAddValidationRulesCallsIsLegalEnumValueWithMembershipOnCommonValidations(Membership upgradeToMembership)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var upgradeToMembershipAsString = upgradeToMembership.ToString();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, upgradeToMembershipAsString)
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLegalEnumValue(Arg<string>.Is.Equal(upgradeToMembershipAsString), Arg<IEnumerable<Membership>>.Is.Equal(new List<Membership> {Membership.Deluxe, Membership.Premium})));
        }

        /// <summary>
        /// Tests that AddValidationRules calls CanUpgradeMembership to on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, Membership.Basic)]
        [TestCase(Membership.Basic, Membership.Deluxe)]
        [TestCase(Membership.Basic, Membership.Premium)]
        [TestCase(Membership.Deluxe, Membership.Basic)]
        [TestCase(Membership.Deluxe, Membership.Deluxe)]
        [TestCase(Membership.Deluxe, Membership.Premium)]
        [TestCase(Membership.Premium, Membership.Basic)]
        [TestCase(Membership.Premium, Membership.Deluxe)]
        [TestCase(Membership.Premium, Membership.Premium)]
        public void TestThatAddValidationRulesCallsCanUpgradeMembershipOnDomainObjectValidations(Membership currentMembership, Membership upgradeToMembership)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock(currentMembership);

            var upgradeToMembershipAsString = upgradeToMembership.ToString();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, upgradeToMembershipAsString)
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(householdMemberMock, householdMemberUpgradeMembershipCommand, specificationMock);

            domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(currentMembership), Arg<Membership>.Is.Equal(upgradeToMembership)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsDateTimeInPast with the date and time for the payment for the upgrade on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsDateTimeInPastWithPaymentTimeOnCommonValidations()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, paymentTime)
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsDateTimeInPast(Arg<DateTime>.Is.Equal(paymentTime)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls HasValue with the payment reference from the data provider who handled the payment for the upgrade on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsHasValueWithPaymentReferenceOnCommonValidations()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var paymentReference = fixture.Create<string>();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, paymentReference)
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.HasValue(Arg<string>.Is.Equal(paymentReference)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsLengthValid with the payment reference from the data provider who handled the payment for the upgrade on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsLengthValidWithPaymentReferenceOnCommonValidations()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var paymentReference = fixture.Create<string>();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, paymentReference)
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.IsLengthValid(Arg<string>.Is.Equal(paymentReference), Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(128)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls ContainsIllegalChar with the payment reference from the data provider who handled the payment for the upgrade on the common validations.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsContainsIllegalCharWithPaymentReferenceOnCommonValidations()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var paymentReference = fixture.Create<string>();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, paymentReference)
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            commonValidationsMock.AssertWasCalled(m => m.ContainsIllegalChar(Arg<string>.Is.Equal(paymentReference)));
        }

        /// <summary>
        /// Tests that AddValidationRules calls IsSatisfiedBy on the specification which encapsulates validation rules 7 times.
        /// </summary>
        [Test]
        public void TestThatAddValidationRulesCallsIsSatisfiedByOnSpecification7Times()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.AddValidationRules(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand, specificationMock);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(7));
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the household member on which the membership should be upgraded is null.
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
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberUpgradeMembershipCommandHandler.ModifyData(null, fixture.Create<HouseholdMemberUpgradeMembershipCommand>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData throws an ArgumentNullException when the command for upgrading the membership on the current users household member account is null.
        /// </summary>
        [Test]
        public void TestThatModifyDataThrowsArgumentNullExceptionWhenCommandrIsNull()
        {
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ModifyData calls Get with the identifier for the data provider who handled the payment for the upgrade on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsGetWithDataProviderIdentifierOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var dataProviderIdentifier = Guid.NewGuid();
            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, dataProviderIdentifier)
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Get<IDataProvider>(Arg<Guid>.Is.Equal(dataProviderIdentifier)));
        }

        /// <summary>
        /// Tests that ModifyData calls IsNotNull with the data provider who handled the payment for the upgrade on the common validations.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsNotNullWithDataProviderOnCommonValidations()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand);

            commonValidationsMock.AssertWasCalled(m => m.IsNotNull(Arg<IDataProvider>.Is.Equal(dataProviderMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls HandlesPayments on the data provider who handled the payment for the upgrade.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsHandlesPaymentsOnDataProvider()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
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

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand);

            dataProviderMock.AssertWasCalled(m => m.HandlesPayments);
        }

        /// <summary>
        /// Tests that ModifyData calls IsSatisfiedBy on the specification which encapsulates validation rules 2 times.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsIsSatisfiedByOnSpecification2Times()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand);

            specificationMock.AssertWasCalled(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.NotNull, Arg<IntranetBusinessException>.Is.TypeOf), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that ModifyData calls Evaluate on the specification which encapsulates validation rules.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsEvaluateOnSpecification()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand);

            specificationMock.AssertWasCalled(m => m.Evaluate());
        }

        /// <summary>
        /// Tests that ModifyData calls Insert with a payment for the upgrade of the membership on the repository which can access household data for the food waste domain.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatModifyDataCallsInsertWithPaymentOnHouseholdDataRepository(bool hasPaymentReceipt)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var dataProviderMock = DomainObjectMockBuilder.BuildDataProviderMock(true);
            var paymentTime = DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            var paymentReference = fixture.Create<string>();
            var paymentReceipt = hasPaymentReceipt ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : null;

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(dataProviderMock)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var payment = (IPayment) e.Arguments.ElementAt(0);
                    Assert.That(payment, Is.Not.Null);
                    Assert.That(payment.Identifier, Is.Null);
                    Assert.That(payment.Identifier.HasValue, Is.False);
                    Assert.That(payment.Stakeholder, Is.Not.Null);
                    Assert.That(payment.Stakeholder, Is.EqualTo(householdMemberMock));
                    Assert.That(payment.DataProvider, Is.Not.Null);
                    Assert.That(payment.DataProvider, Is.EqualTo(dataProviderMock));
                    Assert.That(payment.PaymentTime, Is.EqualTo(paymentTime));
                    Assert.That(payment.PaymentReference, Is.Not.Null);
                    Assert.That(payment.PaymentReference, Is.Not.Empty);
                    Assert.That(payment.PaymentReference, Is.EqualTo(paymentReference));
                    Assert.That(payment.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                    if (hasPaymentReceipt == false || paymentReceipt == null)
                    {
                        Assert.That(payment.PaymentReceipt, Is.Null);
                        return;
                    }
                    Assert.That(payment.PaymentReceipt, Is.Not.Null);
                    Assert.That(payment.PaymentReceipt, Is.Not.Empty);
                    Assert.That(payment.PaymentReceipt.Count(), Is.EqualTo(paymentReceipt.Length));
                    for (var byteNo = 0; byteNo < paymentReceipt.Length; byteNo++)
                    {
                        Assert.That(payment.PaymentReceipt.ElementAt(byteNo), Is.EqualTo(paymentReceipt.ElementAt(byteNo)));
                    }
                })
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, paymentTime)
                .With(m => m.PaymentReference, paymentReference)
                .With(m => m.PaymentReceipt, hasPaymentReceipt && paymentReceipt != null ? Convert.ToBase64String(paymentReceipt) : null)
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(householdMemberMock, householdMemberUpgradeMembershipCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Insert(Arg<IPayment>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ModifyData calls PaymentAdd with the inserted payment for the upgrade on the household member.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsPaymentAddWithInsertedPaymentOnHouseholdMember()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedPayment = DomainObjectMockBuilder.BuildPaymentMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(insertedPayment)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(householdMemberMock, householdMemberUpgradeMembershipCommand);

            householdMemberMock.AssertWasCalled(m => m.PaymentAdd(Arg<IPayment>.Is.Equal(insertedPayment)));
        }

        /// <summary>
        /// Tests that ModifyData calls Delete with the inserted payment for the upgrade on the repository which can access household data for the food waste domain when PaymentAdd throws an exception.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsDeleteWithInsertedPaymentOnHouseholdDataRepositoryWhenPaymentAddThrowsException()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedPayment = DomainObjectMockBuilder.BuildPaymentMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(insertedPayment)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.PaymentAdd(Arg<IPayment>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            Assert.Throws<Exception>(() => householdMemberUpgradeMembershipCommandHandler.ModifyData(householdMemberMock, householdMemberUpgradeMembershipCommand));

            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IPayment>.Is.Equal(insertedPayment)));
        }

        /// <summary>
        /// Tests that ModifyData calls MembershipApply with the membership which the household member account should be upgrade to on the household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatModifyDataCallsMembershipApplyWithMembershipOnHouseholdMember(Membership upgradeToMembership)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, upgradeToMembership.ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(householdMemberMock, householdMemberUpgradeMembershipCommand);

            householdMemberMock.AssertWasCalled(m => m.MembershipApply(Arg<Membership>.Is.Equal(upgradeToMembership)));
        }

        /// <summary>
        /// Tests that ModifyData calls Delete with the inserted payment for the upgrade on the repository which can access household data for the food waste domain when MembershipApply throws an exception.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsDeleteWithInsertedPaymentOnHouseholdDataRepositoryWhenMembershipApplyThrowsException()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedPayment = DomainObjectMockBuilder.BuildPaymentMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(insertedPayment)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            householdMemberMock.Stub(m => m.MembershipApply(Arg<Membership>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            Assert.Throws<Exception>(() => householdMemberUpgradeMembershipCommandHandler.ModifyData(householdMemberMock, householdMemberUpgradeMembershipCommand));

            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IPayment>.Is.Equal(insertedPayment)));
        }

        /// <summary>
        /// Tests that ModifyData calls Update with the membership which the upgraded household member on the repository which can access household data for the food waste domain..
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsUpdateWithUpgradedHouseholdMemberOnHouseholdDataRepository()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildHouseholdMemberMock())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberMock = DomainObjectMockBuilder.BuildHouseholdMemberMock();

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            householdMemberUpgradeMembershipCommandHandler.ModifyData(householdMemberMock, householdMemberUpgradeMembershipCommand);

            householdDataRepositoryMock.AssertWasCalled(m => m.Update(Arg<IHouseholdMember>.Is.Equal(householdMemberMock)));
        }

        /// <summary>
        /// Tests that ModifyData calls Delete with the inserted payment for the upgrade on the repository which can access household data for the food waste domain when Update for the upgraded household member throws an exception.
        /// </summary>
        [Test]
        public void TestThatModifyDataCallsDeleteWithInsertedPaymentOnHouseholdDataRepositoryWhenUpdateForUpgradedHouseholdMemberThrowsException()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var insertedPayment = DomainObjectMockBuilder.BuildPaymentMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(insertedPayment)
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Throw(fixture.Create<Exception>())
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            Assert.Throws<Exception>(() =>householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand));

            householdDataRepositoryMock.AssertWasCalled(m => m.Delete(Arg<IPayment>.Is.Equal(insertedPayment)));
        }

        /// <summary>
        /// Tests that ModifyData return the household member who's membership has been upgraded.
        /// </summary>
        [Test]
        public void TestThatModifyDataReturnsUpgradedHouseholdMember()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            var objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();
            var commonValidationsMock = MockRepository.GenerateMock<ICommonValidations>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var upgradedHouseholdMember = DomainObjectMockBuilder.BuildHouseholdMemberMock();
            var householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            householdDataRepositoryMock.Stub(m => m.Get<IDataProvider>(Arg<Guid>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildDataProviderMock(true))
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Insert(Arg<IPayment>.Is.Anything))
                .Return(DomainObjectMockBuilder.BuildPaymentMock())
                .Repeat.Any();
            householdDataRepositoryMock.Stub(m => m.Update(Arg<IHouseholdMember>.Is.Anything))
                .Return(upgradedHouseholdMember)
                .Repeat.Any();

            var specificationMock = MockRepository.GenerateMock<ISpecification>();
            specificationMock.Stub(m => m.IsSatisfiedBy(Arg<Func<bool>>.Is.Anything, Arg<Exception>.Is.Anything))
                .Return(specificationMock)
                .Repeat.Any();

            var householdMemberUpgradeMembershipCommandHandler = new HouseholdMemberUpgradeMembershipCommandHandler(householdDataRepositoryMock, claimValueProviderMock, objectMapperMock, specificationMock, commonValidationsMock, domainObjectValidationsMock, exceptionBuilderMock);
            Assert.That(householdMemberUpgradeMembershipCommandHandler, Is.Not.Null);

            var householdMemberUpgradeMembershipCommand = fixture.Build<HouseholdMemberUpgradeMembershipCommand>()
                .With(m => m.Membership, fixture.Create<Membership>().ToString())
                .With(m => m.DataProviderIdentifier, Guid.NewGuid())
                .With(m => m.PaymentTime, DateTime.Now.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240)))
                .With(m => m.PaymentReference, fixture.Create<string>())
                .With(m => m.PaymentReceipt, Convert.ToBase64String(fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray()))
                .Create();

            var result = householdMemberUpgradeMembershipCommandHandler.ModifyData(DomainObjectMockBuilder.BuildHouseholdMemberMock(), householdMemberUpgradeMembershipCommand);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(upgradedHouseholdMember));
        }
    }
}
