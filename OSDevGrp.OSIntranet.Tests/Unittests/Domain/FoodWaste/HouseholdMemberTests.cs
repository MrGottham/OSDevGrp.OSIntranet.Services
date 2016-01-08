using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
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
                get { return base.MailAddress; }
                set { base.MailAddress = value; }
            }

            /// <summary>
            /// Membership.
            /// </summary>
            public new Membership Membership
            {
                get { return base.Membership; }
                set { base.Membership = value; }
            }

            /// <summary>
            /// Date and time for when the membership expires.
            /// </summary>
            public new DateTime? MembershipExpireTime
            {
                get { return base.MembershipExpireTime; }
                set { base.MembershipExpireTime = value; }
            }

            /// <summary>
            /// Activation code for the household member.
            /// </summary>
            public new string ActivationCode
            {
                get { return base.ActivationCode; }
                set { base.ActivationCode = value; }
            }

            /// <summary>
            /// Date and time for when the household member was created.
            /// </summary>
            public new DateTime CreationTime
            {
                get { return base.CreationTime; }
                set { base.CreationTime = value; }
            }

            /// <summary>
            /// Households on which the household member has a membership.
            /// </summary>
            public new IEnumerable<IHousehold> Households
            {
                get { return base.Households; }
                set { base.Households = value; }
            }

            #endregion
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
            var householdMember = new HouseholdMember(validMailAddress);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Identifier, Is.Null);
            Assert.That(householdMember.Identifier.HasValue, Is.False);
            Assert.That(householdMember.MailAddress, Is.Not.Null);
            Assert.That(householdMember.MailAddress, Is.Not.Empty);
            Assert.That(householdMember.MailAddress, Is.EqualTo(validMailAddress));
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMember.MembershipExpireTime, Is.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);
            Assert.That(householdMember.ActivationCode, Is.Not.Null);
            Assert.That(householdMember.ActivationCode, Is.Not.Empty);
            Assert.That(householdMember.ActivationTime, Is.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.False);
            Assert.That(householdMember.IsActivated, Is.False);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(householdMember.IsPrivacyPolictyAccepted, Is.False);
            Assert.That(householdMember.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(householdMember.Households, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the mail address is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenMailAddressIsNullOrEmpty(string invalidMailAddress)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMember(invalidMailAddress, MockRepository.GenerateMock<IDomainObjectValidations>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("mailAddress"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the activation code is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenActivationCodeIsNullOrEmpty(string invalidActivationCode)
        {
            var fixture = new Fixture();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMember(fixture.Create<string>(), fixture.Create<Membership>(), DateTime.Today.AddYears(1), invalidActivationCode, DateTime.Now, MockRepository.GenerateMock<IDomainObjectValidations>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("activationCode"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor calls IsMailAddress on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorCallsIsMailAddressOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var mailAddress = fixture.Create<string>();
            var householdMember = new HouseholdMember(mailAddress, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(mailAddress)));
        }

        /// <summary>
        /// Tests that the constructor throws an IntranetSystemException when the mail address is not a valid mail address.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsIntranetSystemExceptionWhenMailAddressIsNotValidMailAddress()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(false)
                .Repeat.Any();

            var invalidMailAddress = fixture.Create<string>();
            var exception = Assert.Throws<IntranetSystemException>(() => new HouseholdMember(invalidMailAddress, domainObjectValidationsMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, invalidMailAddress, "mailAddress")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for MailAddress throws an ArgumentNullException when the mail address is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatMailAddressSetterThrowsArgumentNullExceptionWhenMailAddressIsNullOrEmpty(string invalidMailAddress)
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMember.MailAddress = invalidMailAddress);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for MailAddress calls IsMailAddress on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterCallsIsMailAddressOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var newMailAddress = fixture.Create<string>();
            householdMember.MailAddress = newMailAddress;

            domainObjectValidationsMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(newMailAddress)));
        }

        /// <summary>
        /// Tests that the setter for MailAddress throws an IntranetSystemException when the mail address is not a valid mail address.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterThrowsIntranetSystemExceptionWhenMailAddressIsNotValidMailAddress()
        {
            var fixture = new Fixture();
            
            var validMailAddress = fixture.Create<string>();
            var invalidMailAddress = fixture.Create<string>();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Equal(validMailAddress)))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Equal(invalidMailAddress)))
                .Return(false)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(validMailAddress, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<IntranetSystemException>(() => householdMember.MailAddress = invalidMailAddress);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, invalidMailAddress, "value")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for MailAddress sets the mail address.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterSetsMailAddress()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var newMailAddress = fixture.Create<string>();
            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.MailAddress, Is.Not.Null);
            Assert.That(householdMember.MailAddress, Is.Not.Empty);
            Assert.That(householdMember.MailAddress, Is.Not.EqualTo(newMailAddress));

            householdMember.MailAddress = newMailAddress;
            Assert.That(householdMember.MailAddress, Is.Not.Null);
            Assert.That(householdMember.MailAddress, Is.Not.Empty);
            Assert.That(householdMember.MailAddress, Is.EqualTo(newMailAddress));
        }

        /// <summary>
        /// Tests that the getter for Membership returns Basic membership when the membership expire date and time is null.
        /// </summary>
        [Test]
        public void TestThatMembershipGetterReturnsBasicWhenMembershipExpireTimeIsNull()
        {
            var fixture = new Fixture();

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            foreach (var membershipToTest in Enum.GetValues(typeof (Membership)).Cast<Membership>())
            {
                var householdMember = new MyHouseholdMember(fixture.Create<string>(), membershipToTest, null, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(householdMember.MembershipExpireTime, Is.Null);
                Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);
            }
        }

        /// <summary>
        /// Tests that the getter for Membership returns Basic membership when the membership expire date and time is in the past.
        /// </summary>
        [Test]
        public void TestThatMembershipGetterReturnsBasicWhenMembershipExpireTimeIsInPast()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            foreach (var membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                var householdMember = new MyHouseholdMember(fixture.Create<string>(), membershipToTest, DateTime.Now.AddDays(random.Next(1, 365)*-1), fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
                Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMember.MembershipExpireTime.Value, Is.LessThan(DateTime.Now));
                // ReSharper restore PossibleInvalidOperationException
            }
        }

        /// <summary>
        /// Tests that the getter for Membership returns membership when the membership expire date and time is in the future.
        /// </summary>
        [Test]
        public void TestThatMembershipGetterReturnsBasicWhenMembershipExpireTimeIsInFuture()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            foreach (var membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                var householdMember = new MyHouseholdMember(fixture.Create<string>(), membershipToTest, DateTime.Now.AddDays(random.Next(1, 365)), fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember.Membership, Is.EqualTo(membershipToTest));
                Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
                Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMember.MembershipExpireTime.Value, Is.GreaterThan(DateTime.Now));
                // ReSharper restore PossibleInvalidOperationException
            }
        }

        /// <summary>
        /// Tests that the setter for Membership sets the membership.
        /// </summary>
        [Test]
        public void TestThatMembershipSetterSetsMembership()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            foreach (var membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                var householdMember = new MyHouseholdMember(fixture.Create<string>(), Membership.Basic, DateTime.Now.AddDays(random.Next(1, 365)), fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
                Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMember.MembershipExpireTime.Value, Is.GreaterThan(DateTime.Now));
                // ReSharper restore PossibleInvalidOperationException

                householdMember.Membership = membershipToTest;
                Assert.That(householdMember.Membership, Is.EqualTo(membershipToTest));
            }
        }

        /// <summary>
        /// Tests that the setter for Membership sets the membership exprie date and time to null when the membership is set to basic.
        /// </summary>
        [Test]
        public void TestThatMembershipSetterSetsMembershipExpireTimeToNullWhenMembershipIsSetToBasic()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            foreach (var membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>().Where(m => m != Membership.Basic))
            {
                var membershipExpireTime = DateTime.Now.AddDays(random.Next(1, 365));
                var householdMember = new MyHouseholdMember(fixture.Create<string>(), membershipToTest, membershipExpireTime, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
                Assert.That(householdMember, Is.Not.Null);
                Assert.That(householdMember.Membership, Is.EqualTo(membershipToTest));
                Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
                Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(householdMember.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
                // ReSharper restore PossibleInvalidOperationException

                householdMember.Membership = Membership.Basic;
                Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
                Assert.That(householdMember.MembershipExpireTime, Is.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
            }
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the membership expire date and time not equal to null.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsMembershipExpireTimeNotEqualToNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), fixture.Create<Membership>(), null, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.MembershipExpireTime, Is.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);

            var membershipExpireTime = DateTime.Now.AddDays(random.Next(1, 365));
            householdMember.MembershipExpireTime = membershipExpireTime;
            Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMember.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the membership expire date and time equal to null.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsMembershipExpireTimeEqualToNull()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var membershipExpireTime = DateTime.Now.AddDays(random.Next(1, 365));
            var householdMember = new MyHouseholdMember(fixture.Create<string>(), fixture.Create<Membership>(), membershipExpireTime, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.MembershipExpireTime.Value, Is.EqualTo(membershipExpireTime));
            // ReSharper restore PossibleInvalidOperationException

            householdMember.MembershipExpireTime = null;
            Assert.That(householdMember.MembershipExpireTime, Is.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the setter for ActivationCode throws an ArgumentNullException when the activation code is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatActivationCodeSetterThrowsArgumentNullExceptionWhenActivationCodeIsInValid(string invalidActivationCode)
        {
            var householdMember = new MyHouseholdMember();
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMember.ActivationCode = invalidActivationCode);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for ActivationCode sets the activation code.
        /// </summary>
        [Test]
        public void TestThatActivationCodeSetterSetsActivationCode()
        {
            var fixture = new Fixture();

            var householdMember = new MyHouseholdMember();
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.ActivationCode, Is.Null);

            var newActivationCode = fixture.Create<string>();
            householdMember.ActivationCode = newActivationCode;
            Assert.That(householdMember.ActivationCode, Is.Not.Null);
            Assert.That(householdMember.ActivationCode, Is.Not.Empty);
            Assert.That(householdMember.ActivationCode, Is.EqualTo(newActivationCode));
        }

        /// <summary>
        /// Tests that the setter for ActivationTime sets the activation time not equal to null.
        /// </summary>
        [Test]
        public void TestThatActivationTimeSetterSetsActivationTimeNotEqualToNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.ActivationTime, Is.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.False);

            var newActivationTime = DateTime.Now;
            householdMember.ActivationTime = newActivationTime;
            Assert.That(householdMember.ActivationTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMember.ActivationTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMember.ActivationTime.Value, Is.EqualTo(newActivationTime));
        }

        /// <summary>
        /// Tests that the setter for ActivationTime sets the activation time equal to null.
        /// </summary>
        [Test]
        public void TestThatActivationTimeSetterSetsActivationTimeEqualToNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                ActivationTime = DateTime.Now
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.ActivationTime, Is.Not.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.True);

            householdMember.ActivationTime = null;
            Assert.That(householdMember.ActivationTime, Is.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns false when the activation time is null.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsFalseWhenActivationTimeIsNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new HouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                ActivationTime = null
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.ActivationTime, Is.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.False);
            Assert.That(householdMember.IsActivated, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns false when the activation time is in the future.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsFalseWhenActivationTimeIsInFuture()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new HouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                ActivationTime = DateTime.Now.AddMinutes(random.Next(1, 60))
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.ActivationTime, Is.Not.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.ActivationTime.Value, Is.GreaterThan(DateTime.Now));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdMember.IsActivated, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsActivated returns true when the activation time is in the past.
        /// </summary>
        [Test]
        public void TestThatIsActivatedGetterReturnsTrueWhenActivationTimeIsInPast()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new HouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                ActivationTime = DateTime.Now.AddMinutes(random.Next(1, 60)*-1)
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.ActivationTime, Is.Not.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.ActivationTime.Value, Is.LessThan(DateTime.Now));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdMember.IsActivated, Is.True);
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicyAcceptedTime sets the time for when the household member has accepted our privacy policy not equal to null.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyAcceptedTimeSetterSetsPrivacyPolicyAcceptedTimeNotEqualToNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            var newPrivacyPolicyAcceptedTime = DateTime.Now;
            householdMember.PrivacyPolicyAcceptedTime = newPrivacyPolicyAcceptedTime;
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.Value, Is.EqualTo(newPrivacyPolicyAcceptedTime));
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicyAcceptedTime sets the time for when the household member has accepted our privacy policy equal to null.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyAcceptedTimeSetterSetsPrivacyPolicyAcceptedTimeEqualToNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                PrivacyPolicyAcceptedTime = DateTime.Now
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.True);

            householdMember.PrivacyPolicyAcceptedTime = null;
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolictyAccepted returns false when the time that the household member has accepted our privacy policy is null.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolictyAcceptedGetterReturnsFalseWhenPrivacyPolicyAcceptedTimeIsNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new HouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                PrivacyPolicyAcceptedTime = null
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(householdMember.IsPrivacyPolictyAccepted, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolictyAccepted returns false when the time that the household member has accepted our privacy policy is in the future.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolictyAcceptedGetterReturnsFalseWhenPrivacyPolicyAcceptedTimeIsInFuture()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new HouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                PrivacyPolicyAcceptedTime = DateTime.Now.AddMinutes(random.Next(1, 60))
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.Value, Is.GreaterThan(DateTime.Now));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdMember.IsPrivacyPolictyAccepted, Is.False);
        }

        /// <summary>
        /// Tests that the getter for IsPrivacyPolictyAccepted returns true when the time that the household member has accepted our privacy policy is in the past.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPolictyAcceptedGetterReturnsTrueWhenPrivacyPolicyAcceptedTimeIsInPast()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new HouseholdMember(fixture.Create<string>(), domainObjectValidationsMock)
            {
                PrivacyPolicyAcceptedTime = DateTime.Now.AddMinutes(random.Next(1, 60) * -1)
            };
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.PrivacyPolicyAcceptedTime.Value, Is.LessThan(DateTime.Now));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(householdMember.IsPrivacyPolictyAccepted, Is.True);
        }

        /// <summary>
        /// Tests that the setter for CreationTime sets the creation time.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterSetsCreationTime()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            var newCreationTime = DateTime.Today.AddDays(random.Next(1, 365)*-1);
            householdMember.CreationTime = newCreationTime;
            Assert.That(householdMember.CreationTime, Is.EqualTo(newCreationTime));
        }

        /// <summary>
        /// Tests that the setter for Households throws ArgumentNullException when value is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMember.Households = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Households calls GetHouseholdLimit on the common validations used by domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterCallsGetHouseholdLimitOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.GetHouseholdLimit(Arg<Membership>.Is.Anything))
                .Return(0)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            householdMember.Households = new List<IHousehold>(0);

            domainObjectValidationsMock.AssertWasCalled(m => m.GetHouseholdLimit(Arg<Membership>.Is.Equal(householdMember.Membership)));
        }

        /// <summary>
        /// Tests that the setter for Households throws an IntranetBusinessException when the collection of households contains more households than the limit of households.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterThrowsIntranetBusinessExceptionWhenHouseholdCollectionContainsMoreHouseholdsThanHouseholdLimit()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.GetHouseholdLimit(Arg<Membership>.Is.Anything))
                .Return(0)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMember.Households = new List<IHousehold> {MockRepository.GenerateMock<IHousehold>()});
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdLimitHasBeenReached)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Households sets the households on which the household member has a membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterSetsHouseholds()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.GetHouseholdLimit(Arg<Membership>.Is.Anything))
                .Return(3)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Empty);

            var householdMockCollection = new List<IHousehold>
            {
                MockRepository.GenerateMock<IHousehold>(),
                MockRepository.GenerateMock<IHousehold>(),
                MockRepository.GenerateMock<IHousehold>()
            };
            householdMember.Households = householdMockCollection;
            Assert.That(householdMember.Households, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Not.Empty);
            Assert.That(householdMember.Households, Is.EqualTo(householdMockCollection));
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
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything))
                .Return(fixture.Create<bool>())
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), currentMembership, currentMembership == Membership.Basic ? (DateTime?) null : DateTime.Now.AddYears(1), fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(currentMembership));

            householdMember.HasRequiredMembership(requiredMembership);

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
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasRequiredMembership(Arg<Membership>.Is.Anything, Arg<Membership>.Is.Anything))
                .Return(hasRequiredMembership)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), Membership.Basic, null, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));

            var result = householdMember.HasRequiredMembership(Membership.Basic);
            Assert.That(result, Is.EqualTo(hasRequiredMembership));
        }

        /// <summary>
        /// Tests that HouseholdApply applies the basic membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyAppliesBasicMembership()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), Membership.Deluxe, DateTime.Now.AddYears(1), fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Deluxe));
            Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
            // ReSharper restore PossibleInvalidOperationException

            householdMember.MembershipApply(Membership.Basic);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMember.MembershipExpireTime, Is.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that HouseholdApply applies the deluxe membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyAppliesDeluxeMembership()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), Membership.Basic, null, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMember.MembershipExpireTime, Is.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);

            householdMember.MembershipApply(Membership.Deluxe);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Deluxe));
            Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that HouseholdApply applies the premium membership.
        /// </summary>
        [Test]
        public void TestThatHouseholdApplyAppliesPremiumMembership()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), Membership.Basic, null, fixture.Create<string>(), DateTime.Now, domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(householdMember.MembershipExpireTime, Is.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.False);

            householdMember.MembershipApply(Membership.Premium);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Membership, Is.EqualTo(Membership.Premium));
            Assert.That(householdMember.MembershipExpireTime, Is.Not.Null);
            Assert.That(householdMember.MembershipExpireTime.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(householdMember.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(3).Seconds);
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that HouseholdAdd throws ArgumentNullException when the household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMember.HouseholdAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("household"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdAdd calls HasReachedHouseholdLimit on the common validations used by domain object in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddCallsHasReachedHouseholdLimitOnDomainObjectValidations()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Anything, Arg<int>.Is.Anything))
                .Return(false)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            householdMember.HouseholdAdd(MockRepository.GenerateMock<IHousehold>());

            domainObjectValidationsMock.AssertWasCalled(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Equal(householdMember.Membership), Arg<int>.Is.Equal(householdMember.Households.Count() - 1)));
        }

        /// <summary>
        /// Tests that HouseholdAdd  throws an IntranetBusinessException when the limit of households has been reached.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsIntranetBusinessExceptionWhenHouseholdLimitHasBeenReached()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Anything, Arg<int>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<IntranetBusinessException>(() => householdMember.HouseholdAdd(MockRepository.GenerateMock<IHousehold>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdLimitHasBeenReached)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdAdd adds a household to the household member.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddAddsHousehold()
        {
            var fixture = new Fixture();
            var domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.HasReachedHouseholdLimit(Arg<Membership>.Is.Anything, Arg<int>.Is.Anything))
                .Return(false)
                .Repeat.Any();

            var householdMember = new MyHouseholdMember(fixture.Create<string>(), domainObjectValidationsMock);
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Empty);

            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMember.HouseholdAdd(householdMock);
            Assert.That(householdMember.Households, Is.Not.Null);
            Assert.That(householdMember.Households, Is.Not.Empty);
            Assert.That(householdMember.Households.Count(), Is.EqualTo(1));
            Assert.That(householdMember.Households.Contains(householdMock), Is.EqualTo(true));
        }
    }
}
