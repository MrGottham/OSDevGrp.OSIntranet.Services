using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
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
            /// <param name="activationCode">Activation code for the household member.</param>
            /// <param name="creationTime">Date and time for when the household member was created.</param>
            /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
            public MyHouseholdMember(string mailAddress, string activationCode, DateTime creationTime, IDomainObjectValidations domainObjectValidations = null)
                : base(mailAddress, activationCode, creationTime, domainObjectValidations)
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
            Assert.That(householdMember.ActivationCode, Is.Not.Null);
            Assert.That(householdMember.ActivationCode, Is.Not.Empty);
            Assert.That(householdMember.ActivationTime, Is.Null);
            Assert.That(householdMember.ActivationTime.HasValue, Is.False);
            Assert.That(householdMember.IsActivated, Is.False);
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

            var exception = Assert.Throws<ArgumentNullException>(() => new MyHouseholdMember(fixture.Create<string>(), invalidActivationCode, DateTime.Now, MockRepository.GenerateMock<IDomainObjectValidations>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("activationCode"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor calls IsMailAddress on common validations used by domain objects in the food waste domain.
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
        /// Tests that the setter for MailAddress calls IsMailAddress on common validations used by domain objects in the food waste domain.
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
