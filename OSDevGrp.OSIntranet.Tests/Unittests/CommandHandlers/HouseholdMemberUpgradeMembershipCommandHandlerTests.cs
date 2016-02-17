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
    }
}
