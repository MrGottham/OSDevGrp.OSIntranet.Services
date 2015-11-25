using System;
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
//            Assert.That(householdMember.ActivationTime, Is.Null);
//            Assert.That(householdMember.ActivationTime.HasValue, Is.False);
//            Assert.That(householdMember.IsActivated, Is.False);
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
        /// Tests that the setter for MailAddress throws an ArgumentNullException when the mail address is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatMailAddressSetterThrowsArgumentNullExceptionWhenMailAddressIsInValid(string invalidMailAddress)
        {
            var householdMember = new MyHouseholdMember();
            Assert.That(householdMember, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMember.MailAddress = invalidMailAddress);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for MailAddress throws an IntranetSystemException when the mail address is not a valid mail address.
        /// </summary>
        [Test]
        [TestCase("123")]
        [TestCase("XYZ")]
        [TestCase("XXX.YYY")]
        [TestCase("XXX.YYY.COM")]
        public void TestThatMailAddressSetterThrowsIntranetSystemExceptionWhenMailAddressIsNotValidMailAddress(string invalidMailAddress)
        {
            var householdMember = new MyHouseholdMember();
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

            var householdMember = new MyHouseholdMember();
            Assert.That(householdMember, Is.Not.Null);
            Assert.That(householdMember.MailAddress, Is.Null);

            var newMailAddress = string.Format("test.{0}@osdevgrp.dk", fixture.Create<string>());
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
        public void TestThatActivationCodeSetterThrowsArgumentNullExceptionWheActivationCodeIsInValid(string invalidActivationCode)
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
    }
}
