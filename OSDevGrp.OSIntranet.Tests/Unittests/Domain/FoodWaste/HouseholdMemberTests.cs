using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberTests
    {
        /// <summary>
        /// Private class for testing the household member.
        /// </summary>
        private class MyHouseholdMember : HouseholdMember
        {
            #region Constructors

            /// <summary>
            /// Creates a private class for testing the household member.
            /// </summary>
            public MyHouseholdMember()
            {
            }

            /// <summary>
            /// Creates a private class for testing the household member.
            /// </summary>
            /// <param name="mailAddress">Mail address for the household member.</param>
            /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
            public MyHouseholdMember(string mailAddress, IDomainObjectValidations domainObjectValidations = null)
                : base(mailAddress, domainObjectValidations)
            {
            }

            /// <summary>
            /// Creates a private class for testing the household member.
            /// </summary>
            /// <param name="mailAddress">Mail address for the household member.</param>
            /// <param name="membership">Membership.</param>
            /// <param name="membershipExpireTime">Date and time for when the membership expires.</param>
            /// <param name="activationCode">Activation code for the household member.</param>
            /// <param name="creationTime">Date and time for when the household member was created.</param>
            /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
            public MyHouseholdMember(string mailAddress, Membership membership, DateTime? membershipExpireTime, string activationCode, DateTime creationTime, IDomainObjectValidations domainObjectValidations = null)
                : base(mailAddress, membership, membershipExpireTime, activationCode, creationTime, domainObjectValidations)
            {
            }

            #endregion

            #region Properties
            
            /// <summary>
            /// Mail address for the household member.
            /// </summary>
            public new string MailAddress
            {
                get => base.MailAddress;
                set => base.MailAddress = value;
            }

            /// <summary>
            /// Membership.
            /// </summary>
            public new Membership Membership
            {
                get => base.Membership;
                set => base.Membership = value;
            }

            /// <summary>
            /// Date and time for when the membership expires.
            /// </summary>
            public new DateTime? MembershipExpireTime
            {
                get => base.MembershipExpireTime;
                set => base.MembershipExpireTime = value;
            }

            /// <summary>
            /// Activation code for the household member.
            /// </summary>
            public new string ActivationCode
            {
                get => base.ActivationCode;
                set => base.ActivationCode = value;
            }

            /// <summary>
            /// Date and time for when the household member was created.
            /// </summary>
            public new DateTime CreationTime
            {
                get => base.CreationTime;
                set => base.CreationTime = value;
            }

            /// <summary>
            /// Households on which the household member has a membership.
            /// </summary>
            public new IEnumerable<IHousehold> Households
            {
                get => base.Households;
                set => base.Households = value;
            }

            /// <summary>
            /// Payments made by the household member.
            /// </summary>
            public new IEnumerable<IPayment> Payments
            {
                get => base.Payments;
                set => base.Payments = value;
            }

            #endregion
        }

        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a household member.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com")]
        [TestCase("test@osdevgrp.dk")]
        [TestCase("ole.sorensen@gmail.com")]
        public void TestThatConstructorInitializeHouseholdMember(string validMailAddress)
        {
            IHouseholdMember sut = new HouseholdMember(validMailAddress);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(sut.MailAddress, Is.Not.Null);
            Assert.That(sut.MailAddress, Is.Not.Empty);
            Assert.That(sut.MailAddress, Is.EqualTo(validMailAddress));
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);
            Assert.That(sut.CanRenewMembership, Is.False);
            Assert.That(sut.CanUpgradeMembership, Is.True);
            Assert.That(sut.ActivationCode, Is.Not.Null);
            Assert.That(sut.ActivationCode, Is.Not.Empty);
            Assert.That(sut.ActivationTime, Is.Null);
            Assert.That(sut.ActivationTime.HasValue, Is.False);
            Assert.That(sut.IsActivated, Is.False);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(sut.IsPrivacyPolicyAccepted, Is.False);
            Assert.That(sut.HasReachedHouseholdLimit, Is.False);
            Assert.That(sut.CanCreateStorage, Is.False);
            Assert.That(sut.CanUpdateStorage, Is.True);
            Assert.That(sut.CanDeleteStorage, Is.False);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(sut.UpgradeableMemberships, Is.Not.Null);
            Assert.That(sut.UpgradeableMemberships, Is.Not.Empty);
            Assert.That(sut.UpgradeableMemberships.Count(), Is.EqualTo(2));
            Assert.That(sut.UpgradeableMemberships.Contains(Membership.Basic), Is.False);
            Assert.That(sut.UpgradeableMemberships.Contains(Membership.Deluxe), Is.True);
            Assert.That(sut.UpgradeableMemberships.Contains(Membership.Premium), Is.True);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the mail address is null, empty or white space.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("  ")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenMailAddressIsNullEmptyOrWhiteSpace(string invalidMailAddress)
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => CreateSut(invalidMailAddress, domainObjectValidations: domainObjectValidationsMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "mailAddress");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the activation code is null, empty or white space.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("  ")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenActivationCodeIsNullEmptyOrWhiteSpace(string invalidActivationCode)
        {
            Fixture fixture = new Fixture();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => CreateMySut(fixture.Create<string>(), fixture.Create<Membership>(), DateTime.Today.AddYears(1), invalidActivationCode, DateTime.Now, domainObjectValidationsMock));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "activationCode");
        }

        /// <summary>
        /// Tests that the constructor calls IsMailAddress on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorCallsIsMailAddressOnDomainObjectValidations()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            string mailAddress = _fixture.Create<string>();
            IHouseholdMember sut = CreateSut(mailAddress, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that the constructor throws an IntranetSystemException when the mail address is not a valid mail address.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsIntranetSystemExceptionWhenMailAddressIsNotValidMailAddress()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(false);

            string invalidMailAddress = _fixture.Create<string>();
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => CreateSut(invalidMailAddress, domainObjectValidations: domainObjectValidationsMock));

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, invalidMailAddress, "mailAddress");
        }

        /// <summary>
        /// Tests that the setter for MailAddress throws an ArgumentNullException when the mail address is null, empty or white space.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("  ")]
        public void TestThatMailAddressSetterThrowsArgumentNullExceptionWhenMailAddressIsNullEmptyOrWhiteSpace(string invalidMailAddress)
        {
           IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MailAddress = invalidMailAddress);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for MailAddress calls IsMailAddress on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterCallsIsMailAddressOnDomainObjectValidations()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            string newMailAddress = _fixture.Create<string>();
            sut.MailAddress = newMailAddress;

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(newMailAddress)));
        }

        /// <summary>
        /// Tests that the setter for MailAddress throws an IntranetSystemException when the mail address is not a valid mail address.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterThrowsIntranetSystemExceptionWhenMailAddressIsNotValidMailAddress()
        {
            string validMailAddress = _fixture.Create<string>();
            string invalidMailAddress = _fixture.Create<string>();
            IDomainObjectValidations domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Equal(validMailAddress)))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Equal(invalidMailAddress)))
                .Return(false)
                .Repeat.Any();

            MyHouseholdMember sut = CreateMySut(validMailAddress, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.MailAddress = invalidMailAddress);

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, invalidMailAddress, "value");
        }

        /// <summary>
        /// Tests that the setter for MailAddress sets the mail address.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterSetsMailAddress()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            string newMailAddress = _fixture.Create<string>();
            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.MailAddress, Is.Not.Null);
            Assert.That(sut.MailAddress, Is.Not.Empty);
            Assert.That(sut.MailAddress, Is.Not.EqualTo(newMailAddress));

            sut.MailAddress = newMailAddress;
            Assert.That(sut.MailAddress, Is.Not.Null);
            Assert.That(sut.MailAddress, Is.Not.Empty);
            Assert.That(sut.MailAddress, Is.EqualTo(newMailAddress));
        }

        /// <summary>
        /// Tests that the getter for Membership returns Basic membership when the membership expire date and time is null.
        /// </summary>
        [Test]
        public void TestThatMembershipGetterReturnsBasicWhenMembershipExpireTimeIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            foreach (Membership membershipToTest in Enum.GetValues(typeof (Membership)).Cast<Membership>())
            {
                MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membershipToTest, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(sut.MembershipExpireTime, Is.Null);
                Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
                Assert.That(sut.MembershipHasExpired, Is.True);
            }
        }

        /// <summary>
        /// Tests that the getter for Membership returns Basic membership when the membership expire date and time is in the past.
        /// </summary>
        [Test]
        public void TestThatMembershipGetterReturnsBasicWhenMembershipExpireTimeIsInPast()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            foreach (Membership membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membershipToTest, DateTime.Now.AddDays(_random.Next(1, 365)*-1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(sut.MembershipExpireTime, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.Value, Is.LessThan(DateTime.Now));
                Assert.That(sut.MembershipHasExpired, Is.True);
            }
        }

        /// <summary>
        /// Tests that the getter for Membership returns membership when the membership expire date and time is in the future.
        /// </summary>
        [Test]
        public void TestThatMembershipGetterReturnsBasicWhenMembershipExpireTimeIsInFuture()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            foreach (Membership membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membershipToTest, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut.Membership, Is.EqualTo(membershipToTest));
                Assert.That(sut.MembershipExpireTime, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.Value, Is.GreaterThan(DateTime.Now));
                Assert.That(sut.MembershipHasExpired, Is.False);
            }
        }

        /// <summary>
        /// Tests that the setter for Membership sets the membership.
        /// </summary>
        [Test]
        public void TestThatMembershipSetterSetsMembership()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            foreach (Membership membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(sut.MembershipExpireTime, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.Value, Is.GreaterThan(DateTime.Now));
                Assert.That(sut.MembershipHasExpired, Is.False);

                sut.Membership = membershipToTest;
                Assert.That(sut.Membership, Is.EqualTo(membershipToTest));
            }
        }

        /// <summary>
        /// Tests that the setter for Membership sets the membership expire date and time to null when the membership is set to basic.
        /// </summary>
        [Test]
        public void TestThatMembershipSetterSetsMembershipExpireTimeToNullWhenMembershipIsSetToBasic()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            foreach (Membership membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m != Membership.Basic))
            {
                DateTime membershipExpireTime = DateTime.Now.AddDays(_random.Next(1, 365));
                MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membershipToTest, membershipExpireTime, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(sut, Is.Not.Null);
                Assert.That(sut.Membership, Is.EqualTo(membershipToTest));
                Assert.That(sut.MembershipExpireTime, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
                Assert.That(sut.MembershipHasExpired, Is.False);

                sut.Membership = Membership.Basic;
                Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(sut.MembershipExpireTime, Is.Null);
                Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
                Assert.That(sut.MembershipHasExpired, Is.True);
            }
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the membership expire date and time in the past.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsMembershipExpireTimeInPast()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), _fixture.Create<Membership>(), null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);

            DateTime membershipExpireTime = DateTime.Now.AddDays(_random.Next(1, 365) * -1);
            sut.MembershipExpireTime = membershipExpireTime;
            Assert.That(sut.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
            Assert.That(sut.MembershipHasExpired, Is.True);
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the membership expire date and time in the future.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsMembershipExpireTimeInFuture()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), _fixture.Create<Membership>(), null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);

            DateTime membershipExpireTime = DateTime.Now.AddDays(_random.Next(1, 365));
            sut.MembershipExpireTime = membershipExpireTime;
            Assert.That(sut.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
            Assert.That(sut.MembershipHasExpired, Is.False);
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the membership expire date and time equal to null.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsMembershipExpireTimeEqualToNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime membershipExpireTime = DateTime.Now.AddDays(_random.Next(1, 365));
            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), _fixture.Create<Membership>(), membershipExpireTime, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
            Assert.That(sut.MembershipHasExpired, Is.False);

            sut.MembershipExpireTime = null;
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership does not call CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterDoesNotCallCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsBasic()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.TypeOf<bool>());

            domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything));
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership returns false when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterReturnsFalseWhenCurrentMembershipIsBasic()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership calls CanUpgradeMembership with Deluxe as the membership to upgrade to on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterCallsCanUpgradeMembershipWithDeluxeAsMembershipToUpgradeToOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.TypeOf<bool>());

            domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Deluxe), Arg<Membership>.Is.Equal(Membership.Deluxe)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership does not call CanUpgradeMembership with any other memberships then Deluxe as the membership to upgrade to on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterDoesNotCallCanUpgradeMembershipWithAnyOtherMembershipsThenDeluxeAsMembershipToUpgradeToOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.TypeOf<bool>());

            foreach (Membership otherMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m != Membership.Deluxe))
            {
                domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Deluxe), Arg<Membership>.Is.Equal(otherMembership)));
            }
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership returns the result from CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterReturnsResultFromCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            bool canRenewMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canRenewMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.EqualTo(canRenewMembership));
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership calls CanUpgradeMembership with Premium as the membership to upgrade to on the common validations used by domain objects in the food waste domain when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterCallsCanUpgradeMembershipWithPremiumAsMembershipToUpgradeToOnDomainObjectValidationsWhenCurrentMembershipIsPremium()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.TypeOf<bool>());

            domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Premium), Arg<Membership>.Is.Equal(Membership.Premium)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership does not call CanUpgradeMembership with any other memberships then Premium as the membership to upgrade to on the common validations used by domain objects in the food waste domain when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterDoesNotCallCanUpgradeMembershipWithAnyOtherMembershipsThenPremiumAsMembershipToUpgradeToOnDomainObjectValidationsWhenCurrentMembershipIsPremium()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.TypeOf<bool>());

            foreach (Membership otherMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m != Membership.Premium))
            {
                domainObjectValidationsMock.AssertWasNotCalled(m =>m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Premium), Arg<Membership>.Is.Equal(otherMembership)));
            }
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership returns the result from CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterReturnsResultFromCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsPremium()
        {
            bool canRenewMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canRenewMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            bool result = sut.CanRenewMembership;
            Assert.That(result, Is.EqualTo(canRenewMembership));
        }

        /// <summary>
        /// Tests that the getter for CanRenewMembership throws an IntranetSystemException when the current membership is illegal.
        /// </summary>
        [Test]
        public void TestThatCanRenewMembershipGetterThrowIntranetSystemExceptionWhenCurrentMembershipIsIllegal()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), 0, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo((Membership) 0));

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.CanRenewMembership.ToString());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.UnhandledSwitchValue, 0, "Membership", "get_CanRenewMembership");
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership calls CanUpgradeMembership with all higher memberships on the common validations used by domain objects in the food waste domain when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterCallsCanUpgradeMembershipWithAllHigherMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsBasic()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.TypeOf<bool>());

            foreach (Membership higherMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m > Membership.Basic))
            {
                domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Basic), Arg<Membership>.Is.Equal(higherMembership)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership does not call CanUpgradeMembership with any lower memberships on the common validations used by domain objects in the food waste domain when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterDoesNotCallCanUpgradeMembershipWithAnyLowerMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsBasic()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.TypeOf<bool>());

            foreach (Membership lowerMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m <= Membership.Basic))
            {
                domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Basic), Arg<Membership>.Is.Equal(lowerMembership)));
            }
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership returns the result from CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterReturnsResultFromCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsBasic()
        {
            bool canUpgradeMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canUpgradeMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.EqualTo(canUpgradeMembership));
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership calls CanUpgradeMembership with all higher memberships on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterCallsCanUpgradeMembershipWithAllHigherMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.TypeOf<bool>());

            foreach (Membership higherMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m > Membership.Deluxe))
            {
                domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Deluxe), Arg<Membership>.Is.Equal(higherMembership)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership does not call CanUpgradeMembership with any lower memberships on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterDoesNotCallCanUpgradeMembershipWithAnyLowerMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.TypeOf<bool>());

            foreach (Membership lowerMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m <= Membership.Deluxe))
            {
                domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Deluxe), Arg<Membership>.Is.Equal(lowerMembership)));
            }
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership returns the result from CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterReturnsResultFromCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            bool canUpgradeMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canUpgradeMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.EqualTo(canUpgradeMembership));
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership does not call CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterDoesNotCallCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsPremium()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.TypeOf<bool>());

            domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything));
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership returns false when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterReturnsFalseWhenCurrentMembershipIsPremium()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            bool result = sut.CanUpgradeMembership;
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that the getter for CanUpgradeMembership throws an IntranetSystemException when the current membership is illegal.
        /// </summary>
        [Test]
        public void TestThatCanUpgradeMembershipGetterThrowIntranetSystemExceptionWhenCurrentMembershipIsIllegal()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), 0, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo((Membership) 0));

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.CanUpgradeMembership.ToString());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.UnhandledSwitchValue, 0, "Membership", "get_CanUpgradeMembership");
        }

        /// <summary>
        /// Tests that the setter for ActivationCode throws an ArgumentNullException when the activation code is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatActivationCodeSetterThrowsArgumentNullExceptionWhenActivationCodeIsInValid(string invalidActivationCode)
        {
            MyHouseholdMember sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ActivationCode = invalidActivationCode);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for ActivationCode sets the activation code.
        /// </summary>
        [Test]
        public void TestThatActivationCodeSetterSetsActivationCode()
        {
            MyHouseholdMember sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationCode, Is.Null);

            string newActivationCode = _fixture.Create<string>();
            sut.ActivationCode = newActivationCode;
            Assert.That(sut.ActivationCode, Is.Not.Null);
            Assert.That(sut.ActivationCode, Is.Not.Empty);
            Assert.That(sut.ActivationCode, Is.EqualTo(newActivationCode));
        }

        /// <summary>
        /// Tests that the setter for ActivationTime sets the activation time not equal to null.
        /// </summary>
        [Test]
        public void TestThatActivationTimeSetterSetsActivationTimeNotEqualToNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationTime, Is.Null);
            Assert.That(sut.ActivationTime.HasValue, Is.False);

            DateTime newActivationTime = DateTime.Now;
            sut.ActivationTime = newActivationTime;
            Assert.That(sut.ActivationTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.Value, Is.EqualTo(newActivationTime));
        }

        /// <summary>
        /// Tests that the setter for ActivationTime sets the activation time equal to null.
        /// </summary>
        [Test]
        public void TestThatActivationTimeSetterSetsActivationTimeEqualToNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime activationTime = DateTime.Now;
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), activationTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            sut.ActivationTime = null;
            Assert.That(sut.ActivationTime, Is.Null);
            Assert.That(sut.ActivationTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns false when the activation time is null.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsFalseWhenActivationTimeIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationTime, Is.Null);
            Assert.That(sut.ActivationTime.HasValue, Is.False);
            Assert.That(sut.IsActivated, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns false when the activation time is date in the future.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsFalseWhenActivationTimeIsDateInFuture()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime activationTime = DateTime.Now.AddDays(_random.Next(1, 30));
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), activationTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.Value, Is.GreaterThan(DateTime.Now));
            Assert.That(sut.IsActivated, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns true when the activation time is today.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsTrueWhenActivationTimeIsToday()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime activationTime = DateTime.Now.AddMinutes(_random.Next(-120, 120));
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), activationTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.Value.Date, Is.EqualTo(DateTime.Today));
            Assert.That(sut.IsActivated, Is.True);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns true when the activation time is date in the past.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsTrueWhenActivationTimeIsDateInPast()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime activationTime = DateTime.Now.AddDays(_random.Next(1, 30) * -1);
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), activationTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.ActivationTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.ActivationTime.Value, Is.LessThan(DateTime.Now));
            Assert.That(sut.IsActivated, Is.True);
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicyAcceptedTime sets the time for when the household member has accepted our privacy policy not equal to null.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyAcceptedTimeSetterSetsPrivacyPolicyAcceptedTimeNotEqualToNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            DateTime newPrivacyPolicyAcceptedTime = DateTime.Now;
            sut.PrivacyPolicyAcceptedTime = newPrivacyPolicyAcceptedTime;
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.Value, Is.EqualTo(newPrivacyPolicyAcceptedTime));
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicyAcceptedTime sets the time for when the household member has accepted our privacy policy equal to null.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyAcceptedTimeSetterSetsPrivacyPolicyAcceptedTimeEqualToNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime privacyPolicyAcceptedTime = DateTime.Now;
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), privacyPolicyAcceptedTime: privacyPolicyAcceptedTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            sut.PrivacyPolicyAcceptedTime = null;
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolicyAccepted returns false when the time that the household member has accepted our privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolicyAcceptedGetterReturnsFalseWhenPrivacyPolicyAcceptedTimeIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(sut.IsPrivacyPolicyAccepted, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolicyAccepted returns false when the time that the household member has accepted our privacy policy is date in the future.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolicyAcceptedGetterReturnsFalseWhenPrivacyPolicyAcceptedTimeIsDateInFuture()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime privacyPolicyAcceptedTime = DateTime.Now.AddDays(_random.Next(1, 30));
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), privacyPolicyAcceptedTime: privacyPolicyAcceptedTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.Value, Is.GreaterThan(DateTime.Now));
            Assert.That(sut.IsPrivacyPolicyAccepted, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolicyAccepted returns true when the time that the household member has accepted our privacy policy is today.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolicyAcceptedGetterReturnsTrueWhenPrivacyPolicyAcceptedTimeIsToday()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime privacyPolicyAcceptedTime = DateTime.Now.AddMinutes(_random.Next(-120, 120));
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), privacyPolicyAcceptedTime: privacyPolicyAcceptedTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.Value.Date, Is.EqualTo(DateTime.Today));
            Assert.That(sut.IsPrivacyPolicyAccepted, Is.True);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolicyAccepted returns true when the time that the household member has accepted our privacy policy is date in the past.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolicyAcceptedGetterReturnsTrueWhenPrivacyPolicyAcceptedTimeIsDateInPast()
        {
           IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            DateTime privacyPolicyAcceptedTime = DateTime.Now.AddDays(_random.Next(1, 30) * -1);
            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), privacyPolicyAcceptedTime: privacyPolicyAcceptedTime, domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.PrivacyPolicyAcceptedTime.Value, Is.LessThan(DateTime.Now));
            Assert.That(sut.IsPrivacyPolicyAccepted, Is.True);
        }

        /// <summary>
        /// Tests that the getter for HasReachedHouseholdLimit calls HasReachedHouseholdLimit on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatHasReachedHouseholdLimitGetterCallsHasReachedHouseholdLimitOnDomainObjectValidations(Membership membership)
        {
           IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHousehold[] householdMockCollection = DomainObjectMockBuilder.BuildHouseholdMockCollection(membership).ToArray();
            Assert.That(householdMockCollection, Is.Not.Null);
            Assert.That(householdMockCollection, Is.Not.Empty);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membership, membership == Membership.Basic ? (DateTime?) null : DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            sut.Households = householdMockCollection;
            Assert.That(sut.Membership, Is.EqualTo(membership));
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households, Is.EqualTo(householdMockCollection));

            bool result = sut.HasReachedHouseholdLimit;
            Assert.That(result, Is.TypeOf<bool>());

            domainObjectValidationsMock.AssertWasCalled(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Equal(membership), Arg<int>.Is.Equal(householdMockCollection.Length)));
        }

        /// <summary>
        /// Tests that the getter for HasReachedHouseholdLimit returns the result from HasReachedHouseholdLimit on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatHasReachedHouseholdLimitGetterReturnsResultFromHasReachedHouseholdLimitOnDomainObjectValidations(Membership membership)
        {
            bool hasReachedHouseholdLimit = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasReachedHouseholdLimit: hasReachedHouseholdLimit);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membership, membership == Membership.Basic ? (DateTime?) null : DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(membership));

            bool result = sut.HasReachedHouseholdLimit;
            Assert.That(result, Is.EqualTo(hasReachedHouseholdLimit));
        }

        /// <summary>
        /// Tests that the getter for CanCreateStorage calls HasRequiredMembership on the common validations used by domain objects in the food waste domain..
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatCanCreateStorageGetterCallsHasRequiredMembershipOnDomainObjectValidations(Membership membership)
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membership, membership == Membership.Basic ? (DateTime?) null : DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(membership));
            Assert.That(sut.CanCreateStorage, Is.AnyOf(true, false));

            domainObjectValidationsMock.AssertWasCalled(m => m.HasRequiredMembership(Arg<Membership>.Is.Equal(membership), Arg<Membership>.Is.Equal(Membership.Deluxe)));
        }

        /// <summary>
        /// Tests that the getter for CanCreateStorage returns the result of HasRequiredMembership from the common validations used by domain objects in the food waste domain..
        /// </summary>
        [Test]
        public void TestThatCanCreateStorageGetterReturnResultOfHasRequiredMembershipFromDomainObjectValidations()
        {
            bool hasRequiredMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasRequiredMembership: hasRequiredMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.CanCreateStorage, Is.EqualTo(hasRequiredMembership));
        }

        /// <summary>
        /// Tests that the getter for CanUpdateStorage calls HasRequiredMembership on the common validations used by domain objects in the food waste domain..
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatCanUpdateStorageGetterCallsHasRequiredMembershipOnDomainObjectValidations(Membership membership)
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membership, membership == Membership.Basic ? (DateTime?)null : DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(membership));
            Assert.That(sut.CanUpdateStorage, Is.AnyOf(true, false));

            domainObjectValidationsMock.AssertWasCalled(m => m.HasRequiredMembership(Arg<Membership>.Is.Equal(membership), Arg<Membership>.Is.Equal(Membership.Basic)));
        }

        /// <summary>
        /// Tests that the getter for CanUpdateStorage returns the result of HasRequiredMembership from the common validations used by domain objects in the food waste domain..
        /// </summary>
        [Test]
        public void TestThatCanUpdateStorageGetterReturnResultOfHasRequiredMembershipFromDomainObjectValidations()
        {
            bool hasRequiredMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasRequiredMembership: hasRequiredMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.CanUpdateStorage, Is.EqualTo(hasRequiredMembership));
        }

        /// <summary>
        /// Tests that the getter for CanDeleteStorage calls HasRequiredMembership on the common validations used by domain objects in the food waste domain..
        /// </summary>
        [Test]
        [TestCase(Membership.Basic)]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatCanDeleteStorageGetterCallsHasRequiredMembershipOnDomainObjectValidations(Membership membership)
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), membership, membership == Membership.Basic ? (DateTime?)null : DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(membership));
            Assert.That(sut.CanDeleteStorage, Is.AnyOf(true, false));

            domainObjectValidationsMock.AssertWasCalled(m => m.HasRequiredMembership(Arg<Membership>.Is.Equal(membership), Arg<Membership>.Is.Equal(Membership.Deluxe)));
        }

        /// <summary>
        /// Tests that the getter for CanDeleteStorage returns the result of HasRequiredMembership from the common validations used by domain objects in the food waste domain..
        /// </summary>
        [Test]
        public void TestThatCanDeleteStorageGetterReturnResultOfHasRequiredMembershipFromDomainObjectValidations()
        {
            bool hasRequiredMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasRequiredMembership: hasRequiredMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.CanDeleteStorage, Is.EqualTo(hasRequiredMembership));
        }

        /// <summary>
        /// Tests that the setter for CreationTime sets the creation time.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterSetsCreationTime()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            DateTime newCreationTime = DateTime.Today.AddDays(_random.Next(1, 365) * -1);
            sut.CreationTime = newCreationTime;
            Assert.That(sut.CreationTime, Is.EqualTo(newCreationTime));
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships calls CanUpgradeMembership with all higher memberships on the common validations used by domain objects in the food waste domain when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterCallsCanUpgradeMembershipWithAllHigherMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsBasic()
        {
            bool canUpgradeMembership = _fixture.Create<bool>(); 
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canUpgradeMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            IEnumerable<Membership> result = sut.UpgradeableMemberships;
            Assert.That(result, Is.Not.Null);

            foreach (Membership higherMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m > Membership.Basic))
            {
                domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Basic), Arg<Membership>.Is.Equal(higherMembership)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships does not call CanUpgradeMembership with any lower memberships on the common validations used by domain objects in the food waste domain when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterDoesNotCallCanUpgradeMembershipWithAnyLowerMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsBasic()
        {
            bool canUpgradeMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canUpgradeMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            IEnumerable<Membership> result = sut.UpgradeableMemberships;
            Assert.That(result, Is.Not.Null);

            foreach (Membership lowerMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m <= Membership.Basic))
            {
                domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Basic), Arg<Membership>.Is.Equal(lowerMembership)));
            }
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships return the memberships which the household member can upgrade to when the current membership is Basic.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterReturnsUpgradeableMembershipsWhenCurrentMembershipIsBasic()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            Membership[] result = sut.UpgradeableMemberships.ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result.Contains(Membership.Deluxe), Is.True);
            Assert.That(result.Contains(Membership.Premium), Is.True);
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships calls CanUpgradeMembership with all higher memberships on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterCallsCanUpgradeMembershipWithAllHigherMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            IEnumerable<Membership> result = sut.UpgradeableMemberships;
            Assert.That(result, Is.Not.Null);

            foreach (Membership higherMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m > Membership.Deluxe))
            {
                domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Deluxe), Arg<Membership>.Is.Equal(higherMembership)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships does not call CanUpgradeMembership with any lower memberships on the common validations used by domain objects in the food waste domain when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterDoesNotCallCanUpgradeMembershipWithAnyLowerMembershipsOnDomainObjectValidationsWhenCurrentMembershipIsDeluxe()
        {
            bool canUpgradeMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canUpgradeMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            IEnumerable<Membership> result = sut.UpgradeableMemberships;
            Assert.That(result, Is.Not.Null);

            foreach (Membership lowerMembership in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m <= Membership.Deluxe))
            {
                domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(Membership.Deluxe), Arg<Membership>.Is.Equal(lowerMembership)));
            }
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships return the memberships which the household member can upgrade to when the current membership is Deluxe.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterReturnsUpgradeableMembershipsWhenCurrentMembershipIsDeluxe()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));

            Membership[] result = sut.UpgradeableMemberships.ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result.Contains(Membership.Premium), Is.True);
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships does not call CanUpgradeMembership on the common validations used by domain objects in the food waste domain when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterDoesNotCallCanUpgradeMembershipOnDomainObjectValidationsWhenCurrentMembershipIsPremium()
        {
            bool canUpgradeMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: canUpgradeMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            IEnumerable<Membership> result = sut.UpgradeableMemberships;
            Assert.That(result, Is.Not.Null);

            domainObjectValidationsMock.AssertWasNotCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything));
        }

        /// <summary>
        /// Tests that the getter for UpgradeableMemberships return the memberships which the household member can upgrade to when the current membership is Premium.
        /// </summary>
        [Test]
        public void TestThatUpgradeableMembershipsGetterReturnsUpgradeableMembershipsWhenCurrentMembershipIsPremium()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Premium, DateTime.Now.AddDays(_random.Next(1, 365)), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));

            Membership[] result = sut.UpgradeableMemberships.ToArray();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Tests that the setter for Households throws ArgumentNullException when value is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Households = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for Households calls GetHouseholdLimit on the common validations used by domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterCallsGetHouseholdLimitOnDomainObjectValidations()
        {
            const int householdLimit = 0;
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(householdLimit: householdLimit);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            sut.Households = new List<IHousehold>(0);

            domainObjectValidationsMock.AssertWasCalled(m => m.GetHouseholdLimit(Arg<Membership>.Is.Equal(sut.Membership)));
        }

        /// <summary>
        /// Tests that the setter for Households throws an IntranetBusinessException when the collection of households contains more households than the limit of households.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterThrowsIntranetBusinessExceptionWhenHouseholdCollectionContainsMoreHouseholdsThanHouseholdLimit()
        {
            const int householdLimit = 0;
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(householdLimit: householdLimit);

            MyHouseholdMember householdMember = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => householdMember.Households = new List<IHousehold> {CreateHouseholdMock()});

            TestHelper.AssertIntranetBusinessExceptionIsValid(result, ExceptionMessage.HouseholdLimitHasBeenReached);
        }

        /// <summary>
        /// Tests that the setter for Households sets the households on which the household member has a membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterSetsHouseholds()
        {
            const int householdLimit = 3;
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(householdLimit: householdLimit);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            IList<IHousehold> householdMockCollection = new List<IHousehold>
            {
                CreateHouseholdMock(),
                CreateHouseholdMock(),
                CreateHouseholdMock()
            };
            sut.Households = householdMockCollection;
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households, Is.EqualTo(householdMockCollection));
        }

        /// <summary>
        /// Tests that the setter for Payments throws ArgumentNullException when value is null.
        /// </summary>
        [Test]
        public void TestThatPaymentsSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Payments = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for Payments sets the payments made by the household member.
        /// </summary>
        [Test]
        public void TestThatPaymentsSetterSetsPayments()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Empty);

            IList<IPayment> paymentMockCollection = new List<IPayment>
            {
                MockRepository.GenerateMock<IPayment>(),
                MockRepository.GenerateMock<IPayment>(),
                MockRepository.GenerateMock<IPayment>()
            };
            sut.Payments = paymentMockCollection;
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Empty);
            Assert.That(sut.Payments, Is.EqualTo(paymentMockCollection));
        }

        /// <summary>
        /// Tests that HasRequiredMembership calls HasRequiredMembership on the common validations used by domain objects in the food waste domain.
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
        public void TestThatHasRequiredMembershipCallsHasRequiredMembershipOnDomainObjectValidations(Membership currentMembership, Membership requiredMembership)
        {
            bool hasRequiredMembership = _fixture.Create<bool>();
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasRequiredMembership: hasRequiredMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), currentMembership, currentMembership == Membership.Basic ? (DateTime?) null : DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(currentMembership));

            sut.HasRequiredMembership(requiredMembership);

            domainObjectValidationsMock.AssertWasCalled(m => m.HasRequiredMembership(Arg<Membership>.Is.Equal(currentMembership), Arg<Membership>.Is.Equal(requiredMembership)));
        }

        /// <summary>
        /// Tests that HasRequiredMembership returns the result from HasRequiredMembership on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatHasRequiredMembershipReturnsResultFromHasRequiredMembershipOnDomainObjectValidations(bool hasRequiredMembership)
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasRequiredMembership: hasRequiredMembership);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));

            bool result = sut.HasRequiredMembership(Membership.Basic);
            Assert.That(result, Is.EqualTo(hasRequiredMembership));
        }

        /// <summary>
        /// Tests that HouseholdApply calls CanUpgradeMembership on the common validations used by domain objects in the food waste domain.
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
        public void TestThatHouseholdApplyCallsCanUpgradeMembershipOnDomainObjectValidations(Membership currentMembership, Membership upgradeToMembership)
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), currentMembership, currentMembership == Membership.Basic ? null : (DateTime?) DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(currentMembership));

            sut.MembershipApply(upgradeToMembership);

            domainObjectValidationsMock.AssertWasCalled(m => m.CanUpgradeMembership(Arg<Membership>.Is.Equal(currentMembership), Arg<Membership>.Is.Equal(upgradeToMembership)));
        }

        /// <summary>
        /// Tests that HouseholdApply throws an IntranetBusinessException when CanUpgradeMembership on the common validations used by domain objects in the food waste domain returns false.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyThrowsIntranetBusinessExceptionWhenCanUpgradeMembershipOnDomainObjectValidationsReturnsFalse()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(canUpgradeMembership: false);

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.MembershipApply(_fixture.Create<Membership>()));

            TestHelper.AssertIntranetBusinessExceptionIsValid(result, ExceptionMessage.MembershipCannotDowngrade);
        }

        /// <summary>
        /// Tests that HouseholdApply applies the basic membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyAppliesBasicMembership()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddYears(1), _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));
            Assert.That(sut.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
            Assert.That(sut.MembershipHasExpired, Is.False);

            sut.MembershipApply(Membership.Basic);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);
        }

        /// <summary>
        /// Tests that HouseholdApply applies the deluxe membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyAppliesDeluxeMembership()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);

            sut.MembershipApply(Membership.Deluxe);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Deluxe));
            Assert.That(sut.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
            Assert.That(sut.MembershipHasExpired, Is.False);
        }

        /// <summary>
        /// Tests that HouseholdApply applies the premium membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyAppliesPremiumMembership()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            MyHouseholdMember sut = CreateMySut(_fixture.Create<string>(), Membership.Basic, null, _fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);

            sut.MembershipApply(Membership.Premium);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Premium));
            Assert.That(sut.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
            Assert.That(sut.MembershipHasExpired, Is.False);
        }

        /// <summary>
        /// Tests that HouseholdAdd throws ArgumentNullException when the household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that HouseholdAdd calls HasReachedHouseholdLimit on the common validations used by domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCallsHasReachedHouseholdLimitOnDomainObjectValidations()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = CreateHouseholdMock();

            sut.HouseholdAdd(householdMock);

            domainObjectValidationsMock.AssertWasCalled(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Equal(sut.Membership), Arg<int>.Is.Equal(sut.Households.Count() - 1)));
        }

        /// <summary>
        /// Tests that HouseholdAdd throws an IntranetBusinessException when the limit of households has been reached.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsIntranetBusinessExceptionWhenHouseholdLimitHasBeenReached()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock(hasReachedHouseholdLimit: true);

            IHouseholdMember householdMember = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => householdMember.HouseholdAdd(CreateHouseholdMock()));

            TestHelper.AssertIntranetBusinessExceptionIsValid(result, ExceptionMessage.HouseholdLimitHasBeenReached);
        }

        /// <summary>
        /// Tests that HouseholdAdd adds a household to the household member.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddAddsHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            IHousehold householdMock = CreateHouseholdMock();

            sut.HouseholdAdd(householdMock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(1));
            Assert.That(sut.Households.Contains(householdMock), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests that HouseholdAdd calls HouseholdMembers on the household which should be added to the household member.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCallsHouseholdMembersOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = CreateHouseholdMock();

            sut.HouseholdAdd(householdMock);

            householdMock.AssertWasCalled(m => m.HouseholdMembers);
        }

        /// <summary>
        /// Tests that HouseholdAdd calls HouseholdMemberAdd on the household which should be added to the household member when the household member is not a member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCallsHouseholdMemberAddOnHouseholdWhenHouseholdMemberIsNotMemberOfHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = CreateHouseholdMock();

            sut.HouseholdAdd(householdMock);

            householdMock.AssertWasCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Equal(sut)));
        }

        /// <summary>
        /// Tests that HouseholdAdd does not call HouseholdMemberAdd on the household which should be added to the household member when the household member is a member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddDoesNotCallHouseholdMemberAddOnHouseholdWhenHouseholdMemberIsMemberOfHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IHousehold householdMock = CreateHouseholdMock(sut);

            sut.HouseholdAdd(householdMock);

            householdMock.AssertWasNotCalled(m => m.HouseholdMemberAdd(Arg<IHouseholdMember>.Is.Anything));
        }

        /// <summary>
        /// Tests that HouseholdRemove throws ArgumentNullException when the household where the membership for the household member should be removed is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdRemove(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that HouseholdRemove returns null when the household where the membership for the household member should be removed does not exist on the household member.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveReturnsNullWhenHouseholdMemberDoesNotExistOnHouseholdMember()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHousehold household1Mock = CreateHouseholdMock();
            IHousehold household2Mock = CreateHouseholdMock();
            IHousehold household3Mock = CreateHouseholdMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            sut.HouseholdAdd(household1Mock);
            sut.HouseholdAdd(household2Mock);
            sut.HouseholdAdd(household3Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(3));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.True);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);

            IHousehold result = sut.HouseholdRemove(CreateHouseholdMock());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdRemove removes the household where the membership for the household member should be removed.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveRemovesHouseholdMember()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHousehold household1Mock = CreateHouseholdMock();
            IHousehold household2Mock = CreateHouseholdMock();
            IHousehold household3Mock = CreateHouseholdMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            sut.HouseholdAdd(household1Mock);
            sut.HouseholdAdd(household2Mock);
            sut.HouseholdAdd(household3Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(3));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.True);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);

            sut.HouseholdRemove(household2Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(2));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.False);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);
        }

        /// <summary>
        /// Tests that HouseholdRemove calls HouseholdMembers on the household where the membership for the household member should be removed.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveCallsHouseholdMembersOnHouseholdMember()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHousehold household1Mock = CreateHouseholdMock();
            IHousehold household2Mock = CreateHouseholdMock();
            IHousehold household3Mock = CreateHouseholdMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            sut.HouseholdAdd(household1Mock);
            sut.HouseholdAdd(household2Mock);
            sut.HouseholdAdd(household3Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(3));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.True);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);

            sut.HouseholdRemove(household2Mock);

            household2Mock.AssertWasCalled(m => m.HouseholdMembers, opt => opt.Repeat.Times(2)); // One time when added and one time when removed.
        }

        /// <summary>
        /// Tests that HouseholdRemove calls HouseholdMemberRemove on the household where the membership for the household member should be removed when the household member is member of the household where the membership for the household member should be removed.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveCallsHouseholdMemberRemoveOnHouseholdMemberWhenHouseholdMemberExistsOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            IHousehold household1Mock = CreateHouseholdMock(sut);
            IHousehold household2Mock = CreateHouseholdMock(sut);
            IHousehold household3Mock = CreateHouseholdMock(sut);

            sut.HouseholdAdd(household1Mock);
            sut.HouseholdAdd(household2Mock);
            sut.HouseholdAdd(household3Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(3));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.True);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);

            sut.HouseholdRemove(household2Mock);

            household2Mock.AssertWasCalled(m => m.HouseholdMemberRemove(Arg<IHouseholdMember>.Is.Equal(sut)));
        }

        /// <summary>
        /// Tests that HouseholdRemove does not call HouseholdMemberRemove on the household where the membership for the household member should be removed when the household member is not a member of the household where the membership for the household member should be removed.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveDoesNotCallHouseholdMemberRemoveOnHouseholdMemberWhenHouseholdMemberDoesNotExistOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHousehold household1Mock = CreateHouseholdMock();
            IHousehold household2Mock = CreateHouseholdMock();
            IHousehold household3Mock = CreateHouseholdMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            sut.HouseholdAdd(household1Mock);
            sut.HouseholdAdd(household2Mock);
            sut.HouseholdAdd(household3Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(3));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.True);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);

            sut.HouseholdRemove(household2Mock);

            household2Mock.AssertWasNotCalled(m => m.HouseholdMemberRemove(Arg<IHouseholdMember>.Is.Anything));
        }

        /// <summary>
        /// Tests that HouseholdRemove returns the household where the membership for the household member has been removed.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveReturnsRemovedHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHousehold household1Mock = CreateHouseholdMock();
            IHousehold household2Mock = CreateHouseholdMock();
            IHousehold household3Mock = CreateHouseholdMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            sut.HouseholdAdd(household1Mock);
            sut.HouseholdAdd(household2Mock);
            sut.HouseholdAdd(household3Mock);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(3));
            Assert.That(sut.Households.Contains(household1Mock), Is.True);
            Assert.That(sut.Households.Contains(household2Mock), Is.True);
            Assert.That(sut.Households.Contains(household3Mock), Is.True);

            IHousehold result = sut.HouseholdRemove(household2Mock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(household2Mock));
        }

        /// <summary>
        /// Tests that PaymentAdd throws ArgumentNullException when the payment made by the household member is null.
        /// </summary>
        [Test]
        public void TestThatPaymentAddThrowsArgumentNullExceptionWhenPaymentIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.PaymentAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "payment");
        }

        /// <summary>
        /// Tests that PaymentAdd adds a payment made by the household member to the household member.
        /// </summary>
        [Test]
        public void TestThatPaymentAddAddsPayment()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Empty);

            IPayment paymentMock = MockRepository.GenerateMock<IPayment>();
            sut.PaymentAdd(paymentMock);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Empty);
            Assert.That(sut.Payments.Count(), Is.EqualTo(1));
            Assert.That(sut.Payments.Contains(paymentMock), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests that Translate throws an ArgumentNullException when the culture information which are used for translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslateThrowsArgumentNullExceptionWhenTranslationCultureIsNull()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Translate(null, _fixture.Create<bool>(), _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "translationCulture");
        }

        /// <summary>
        /// Tests that Translate calls Translate for each household on which the household member has a membership when those should be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachHouseholdWhenTranslateHouseholdsIsTrue()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            int numberOfHouseholds = _random.Next(1, 5);
            while (sut.Households.Count() < numberOfHouseholds)
            {
                sut.HouseholdAdd(CreateHouseholdMock());
            }
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(numberOfHouseholds));

            CultureInfo translationCulture = CultureInfo.CurrentCulture;
            sut.Translate(translationCulture, true, _fixture.Create<bool>());

            foreach (IHousehold householdMock in sut.Households)
            {
                householdMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture), Arg<bool>.Is.Equal(false), Arg<bool>.Is.Equal(true)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that Translate does not call Translate on any household on which the household member has a membership when those should not be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnAnyHouseholdWhenTranslateHouseholdsIsFalse()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            int numberOfHouseholds = _random.Next(1, 5);
            while (sut.Households.Count() < numberOfHouseholds)
            {
                sut.HouseholdAdd(CreateHouseholdMock());
            }
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Count(), Is.EqualTo(numberOfHouseholds));

            sut.Translate(CultureInfo.CurrentCulture, false, _fixture.Create<bool>());

            foreach (IHousehold householdMock in sut.Households)
            {
                householdMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything, Arg<bool>.Is.Anything, Arg<bool>.Is.Anything));
            }
        }

        /// <summary>
        /// Tests that Translate calls Translate for each payment made by the household member when those should be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachPaymentWhenTranslatePaymentsIsTrue()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Empty);

            int numberOfPayments = _random.Next(1, 5);
            while (sut.Payments.Count() < numberOfPayments)
            {
                sut.PaymentAdd(MockRepository.GenerateMock<IPayment>());
            }
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Empty);
            Assert.That(sut.Payments.Count(), Is.EqualTo(numberOfPayments));

            CultureInfo translationCulture = CultureInfo.CurrentCulture;
            sut.Translate(translationCulture, _fixture.Create<bool>());

            foreach (IPayment paymentMock in sut.Payments)
            {
                paymentMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture)));
            }
        }

        /// <summary>
        /// Tests that Translate does not call Translate on any payment made by the household member when those should not be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnAnyPaymentWhenTranslatePaymentsIsFalse()
        {
            IDomainObjectValidations domainObjectValidationsMock = CreateDomainObjectValidationsMock();

            IHouseholdMember sut = CreateSut(_fixture.Create<string>(), domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Empty);

            var numberOfPayments = _random.Next(1, 5);
            while (sut.Payments.Count() < numberOfPayments)
            {
                sut.PaymentAdd(MockRepository.GenerateMock<IPayment>());
            }
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Empty);
            Assert.That(sut.Payments.Count(), Is.EqualTo(numberOfPayments));

            sut.Translate(CultureInfo.CurrentCulture, _fixture.Create<bool>(), false);

            foreach (IPayment paymentMock in sut.Payments)
            {
                paymentMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything));
            }
        }

        /// <summary>
        /// Creates an instance of a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="activationTime">Date and time for when the household member was activated.</param>
        /// <param name="privacyPolicyAcceptedTime">Date and time for when the household member has accepted our privacy policy.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        /// <returns>Instance of a household member.</returns>
        private IHouseholdMember CreateSut(string mailAddress, DateTime? activationTime = null, DateTime? privacyPolicyAcceptedTime = null, IDomainObjectValidations domainObjectValidations = null)
        {
            return new HouseholdMember(mailAddress, domainObjectValidations)
            {
                ActivationTime = activationTime,
                PrivacyPolicyAcceptedTime = privacyPolicyAcceptedTime
            };
        }

        /// <summary>
        /// Creates an instance of the private class for testing the household member.
        /// </summary>
        /// <returns>Instance of the private class for testing the household member.</returns>
        private MyHouseholdMember CreateMySut()
        {
            return new MyHouseholdMember();
        }

        /// <summary>
        /// Creates an instance of the private class for testing the household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        /// <returns>Instance of the private class for testing the household member.</returns>
        private MyHouseholdMember CreateMySut(string mailAddress, IDomainObjectValidations domainObjectValidations = null)
        {
            return new MyHouseholdMember(mailAddress, domainObjectValidations);
        }

        /// <summary>
        /// Creates an instance of the private class for testing the household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="membership">Membership.</param>
        /// <param name="membershipExpireTime">Date and time for when the membership expires.</param>
        /// <param name="activationCode">Activation code for the household member.</param>
        /// <param name="creationTime">Date and time for when the household member was created.</param>
        /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
        /// <returns>Instance of the private class for testing the household member.</returns>
        private MyHouseholdMember CreateMySut(string mailAddress, Membership membership, DateTime? membershipExpireTime, string activationCode, DateTime creationTime, IDomainObjectValidations domainObjectValidations = null)
        {
            return new MyHouseholdMember(mailAddress, membership, membershipExpireTime, activationCode, creationTime, domainObjectValidations);
        }

        /// <summary>
        /// Creates a mockup for common validations used by domain objects in the food waste domain.
        /// </summary>
        /// <param name="isMailAddress">Indicates whether the value is a mail address.</param>
        /// <param name="canUpgradeMembership">Indicates whether the membership can be upgraded.</param>
        /// <param name="householdLimit">Limit of households according to a given membership.</param>
        /// <param name="hasReachedHouseholdLimit">Indicates whether the limit of households has been reached.</param>
        /// <param name="hasRequiredMembership">Indicates whether a given membership matches the required membership.</param>
        /// <returns>Mockup for common validations used by domain objects in the food waste domain.</returns>
        private IDomainObjectValidations CreateDomainObjectValidationsMock(bool isMailAddress = true, bool canUpgradeMembership = true, int householdLimit = int.MaxValue, bool hasReachedHouseholdLimit = false, bool hasRequiredMembership = true)
        {
            IDomainObjectValidations domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(isMailAddress)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.CanUpgradeMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything))
                .Return(canUpgradeMembership)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.GetHouseholdLimit(Arg<Membership>.Is.Anything))
                .Return(householdLimit)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Anything, Arg<int>.Is.Anything))
                .Return(hasReachedHouseholdLimit)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything))
                .Return(hasRequiredMembership)
                .Repeat.Any();
            return domainObjectValidationsMock;
        }

        /// <summary>
        /// Creates a mockup for a household.
        /// </summary>
        /// <returns>Mockup for a household.</returns>
        private IHousehold CreateHouseholdMock()
        {
            return CreateHouseholdMock(new IHouseholdMember[0]);
        }

        /// <summary>
        /// Creates a mockup for a household.
        /// </summary>
        /// <param name="householdMemberCollection">Collection of household members in the household.</param>
        /// <returns>Mockup for a household.</returns>
        private IHousehold CreateHouseholdMock(params IHouseholdMember[] householdMemberCollection)
        {
            if (householdMemberCollection == null)
            {
                throw new ArgumentNullException(nameof(householdMemberCollection));
            }

            IHousehold householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.HouseholdMembers)
                .Return(householdMemberCollection)
                .Repeat.Any();
            return householdMock;
        }
    }
}
