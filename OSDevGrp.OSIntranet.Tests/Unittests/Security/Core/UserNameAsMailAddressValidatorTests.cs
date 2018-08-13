using System;
using System.Security;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Core;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the validator which can validate whether an username is a mail address.
    /// </summary>
    [TestFixture]
    public class UserNameAsMailAddressValidatorTests
    {
        /// <summary>
        /// Tests that the constructor initialize the validator which can validate whether an username is a mail address.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeUserNameAsMailAddressValidator()
        {
            var userNameAsMailAddressValidator = new UserNameAsMailAddressValidator();
            Assert.That(userNameAsMailAddressValidator, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCommonValidationIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new UserNameAsMailAddressValidator(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("commonValidation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that Validate calls IsMailAddress on the common validations with invalid user name.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatValidateCallsIsMailAddressOnCommonValidationWithInvalidUserName(string invalidUserName)
        {
            var fixture = new Fixture();

            var commonValidationMock = MockRepository.GenerateMock<ICommonValidation>();
            commonValidationMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var userNameAsMailAddressValidator = new UserNameAsMailAddressValidator(commonValidationMock);
            Assert.That(userNameAsMailAddressValidator, Is.Not.Null);

            userNameAsMailAddressValidator.Validate(invalidUserName, fixture.Create<string>());

            commonValidationMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(invalidUserName)));
        }

        /// <summary>
        /// Test that Validate calls IsMailAddress on the common validations with invalid user name.
        /// </summary>
        [Test]
        public void TestThatValidateCallsIsMailAddressOnCommonValidation()
        {
            var fixture = new Fixture();

            var commonValidationMock = MockRepository.GenerateMock<ICommonValidation>();
            commonValidationMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var userName = fixture.Create<string>();

            var userNameAsMailAddressValidator = new UserNameAsMailAddressValidator(commonValidationMock);
            Assert.That(userNameAsMailAddressValidator, Is.Not.Null);

            userNameAsMailAddressValidator.Validate(userName, fixture.Create<string>());

            commonValidationMock.AssertWasCalled(m => m.IsMailAddress(Arg<string>.Is.Equal(userName)));
        }

        /// <summary>
        /// Test that Validate returns when IsMailAddress on the common validations returns true.
        /// </summary>
        [Test]
        public void TestThatValidatReturnsWhenIsMailAddressOnCommonValidationReturnsTrue()
        {
            var fixture = new Fixture();

            var commonValidationMock = MockRepository.GenerateMock<ICommonValidation>();
            commonValidationMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            var userNameAsMailAddressValidator = new UserNameAsMailAddressValidator(commonValidationMock);
            Assert.That(userNameAsMailAddressValidator, Is.Not.Null);

            userNameAsMailAddressValidator.Validate(fixture.Create<string>(), fixture.Create<string>());
        }

        /// <summary>
        /// Test that Validate throws SecurityException when IsMailAddress on the common validations returns false.
        /// </summary>
        [Test]
        public void TestThatValidatThrowsSecurityExceptionWhenIsMailAddressOnCommonValidationReturnsFalse()
        {
            var fixture = new Fixture();

            var commonValidationMock = MockRepository.GenerateMock<ICommonValidation>();
            commonValidationMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Return(false)
                .Repeat.Any();

            var userNameAsMailAddressValidator = new UserNameAsMailAddressValidator(commonValidationMock);
            Assert.That(userNameAsMailAddressValidator, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => userNameAsMailAddressValidator.Validate(fixture.Create<string>(), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.UserNameAndPasswordCouldNotBeValidated)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that Validate throws SecurityException when IsMailAddress on the common validations fails.
        /// </summary>
        [Test]
        public void TestThatValidatThrowsSecurityExceptionWhenIsMailAddressOnCommonValidationFails()
        {
            var fixture = new Fixture();

            var error = fixture.Create<Exception>();
            var commonValidationMock = MockRepository.GenerateMock<ICommonValidation>();
            commonValidationMock.Stub(m => m.IsMailAddress(Arg<string>.Is.Anything))
                .Throw(error)
                .Repeat.Any();

            var userNameAsMailAddressValidator = new UserNameAsMailAddressValidator(commonValidationMock);
            Assert.That(userNameAsMailAddressValidator, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => userNameAsMailAddressValidator.Validate(fixture.Create<string>(), fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.UserNameAndPasswordCouldNotBeValidated)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(error));
        }
    }
}
