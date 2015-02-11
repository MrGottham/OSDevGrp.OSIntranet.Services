using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security;
using System.Security.Principal;
using Microsoft.IdentityModel.Claims;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the security token authenticator which can authenticate users who has a mail address as username.
    /// </summary>
    [TestFixture]
    public class UserNameAsMailAddressSecurityTokenAuthenticatorTests
    {
        /// <summary>
        /// Tests that the constructor initialize a security token authenticator which can authenticate users who has a mail address as username.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeUserNameAsMailAddressSecurityTokenAuthenticator()
        {
            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator();
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the validator which can validate username and password is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenUserNamePasswordValidatorIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentityBuilder>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentityBuilder>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new UserNameAsMailAddressSecurityTokenAuthenticator(null, fixture.Create<IIdentityBuilder>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("validator"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the functionality which can build an identity is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenIdentityBuilderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<UserNamePasswordValidator>(e => e.FromFactory(() => MockRepository.GenerateMock<UserNamePasswordValidator>()));

            var exception = Assert.Throws<ArgumentNullException>(() => new UserNameAsMailAddressSecurityTokenAuthenticator(fixture.Create<UserNamePasswordValidator>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identityBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CanValidateToken returns true when the security token is an username security token.
        /// </summary>
        [Test]
        public void TestThatCanValidateTokenReturnsTrueWhenSecurityTokenIsUserNameSecurityToken()
        {
            var fixture = new Fixture();

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var result = userNameAsMailAddressSecurityTokenAuthenticator.CanValidateToken(userNameSecurityToken);
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that CanValidateToken returns false when the security token is not an username security token.
        /// </summary>
        [Test]
        [TestCase("CN=OSDevGrp.OSIntranet.Services")]
        [TestCase("CN=OSDevGrp.OSIntranet.Clients")]
        [TestCase("CN=OSDevGrp.OSIntranet.Tokens")]
        public void TestThatCanValidateTokenReturnsFalseWhenSecurityTokenIsNotUserNameSecurityToken(string certificateSubjectName)
        {
            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();

            var certificateSecurityToken = new X509SecurityToken(TestHelper.GetCertificate(certificateSubjectName));

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var result = userNameAsMailAddressSecurityTokenAuthenticator.CanValidateToken(certificateSecurityToken);
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that ValidateToken calls Validate on the validator which can validate username and password.
        /// </summary>
        [Test]
        public void TestThatValidateTokenCallsValidateOnUserNamePasswordValidator()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();
            identityBuilderMock.Stub(m => m.Build(Arg<SecurityToken>.Is.NotNull, Arg<IDictionary<string, string>>.Is.Anything))
                .Return(fixture.Create<IIdentity>())
                .Repeat.Any();

            var userName = fixture.Create<string>();
            var password = fixture.Create<string>();
            var userNameSecurityToken = new UserNameSecurityToken(userName, password);

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken);

            userNamePasswordValidatorMock.AssertWasCalled(m => m.Validate(Arg<string>.Is.Equal(userName), Arg<string>.Is.Equal(password)));
        }

        /// <summary>
        /// Tests that ValidateToken returns a set of authorization policies for the authenticated security token.
        /// </summary>
        [Test]
        public void TestThatValidateTokenReturnsReadOnlyCollectionOfAuthorizationPolicies()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();
            identityBuilderMock.Stub(m => m.Build(Arg<SecurityToken>.Is.NotNull, Arg<IDictionary<string, string>>.Is.Anything))
                .Return(fixture.Create<IIdentity>())
                .Repeat.Any();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var result = userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that ValidateToken returns a set of authorization policies containing the authorization policy to use when authorizing users who has a mail address as username.
        /// </summary>
        [Test]
        public void TestThatValidateTokenReturnsReadOnlyCollectionOfAuthorizationPoliciesContainingUserNameAsMailAddressAuthorizationPolicy()
        {
            var fixture = new Fixture();

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var identity = MockRepository.GenerateMock<IIdentity>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();
            identityBuilderMock.Stub(m => m.Build(Arg<SecurityToken>.Is.NotNull, Arg<IDictionary<string, string>>.Is.Anything))
                .Return(identity)
                .Repeat.Any();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var result = userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            var userNameAsMailAddressAuthorizationPolicy = result.OfType<UserNameAsMailAddressAuthorizationPolicy>().SingleOrDefault();
            Assert.That(userNameAsMailAddressAuthorizationPolicy, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(userNameAsMailAddressAuthorizationPolicy.PrimaryIdentity, Is.Not.Null);
            Assert.That(userNameAsMailAddressAuthorizationPolicy.PrimaryIdentity, Is.EqualTo(identity));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tests that ValidateToken throws SecurityException when Validate on the validator which can validate username and password fails with a SecurityException.
        /// </summary>
        [Test]
        public void TestThatValidateTokenThrowsSecurityExceptionWhenValidateOnUserNamePasswordValidatorFailsWithSecurityException()
        {
            var fixture = new Fixture();
            
            var error = new SecurityException(fixture.Create<string>());
            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            userNamePasswordValidatorMock.Stub(m => m.Validate(Arg<string>.Is.Anything, Arg<string>.Is.Anything))
                .Throw(error)
                .Repeat.Any();

            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(error));
        }

        /// <summary>
        /// Tests that ValidateToken throws SecurityException when Validate on the validator which can validate username and password fails with an Exception.
        /// </summary>
        [Test]
        public void TestThatValidateTokenThrowsSecurityExceptionWhenValidateOnUserNamePasswordValidatorFailsWithException()
        {
            var fixture = new Fixture();

            var error = fixture.Create<Exception>();
            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            userNamePasswordValidatorMock.Stub(m => m.Validate(Arg<string>.Is.Anything, Arg<string>.Is.Anything))
                .Throw(error)
                .Repeat.Any();

            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(error));
        }

        /// <summary>
        /// Tests that ValidateToken calls Build on the functionality which can build an identity.
        /// </summary>
        [Test]
        public void TestThatValidateTokenCallsBuildOnIdentityBuilder()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userName = fixture.Create<string>();

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();
            identityBuilderMock.Stub(m => m.Build(Arg<SecurityToken>.Is.NotNull, Arg<IDictionary<string, string>>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var identityProperites = (IDictionary<string, string>) e.Arguments.ElementAt(1);
                    Assert.That(identityProperites, Is.Not.Null);
                    Assert.That(identityProperites.Count, Is.EqualTo(1));
                    Assert.That(identityProperites.ElementAt(0).Key, Is.Not.Null);
                    Assert.That(identityProperites.ElementAt(0).Key, Is.Not.Empty);
                    Assert.That(identityProperites.ElementAt(0).Key, Is.EqualTo(ClaimTypes.Email));
                    Assert.That(identityProperites.ElementAt(0).Value, Is.Not.Null);
                    Assert.That(identityProperites.ElementAt(0).Value, Is.Not.Empty);
                    Assert.That(identityProperites.ElementAt(0).Value, Is.EqualTo(userName));
                })
                .Return(fixture.Create<IIdentity>())
                .Repeat.Any();

            var userNameSecurityToken = new UserNameSecurityToken(userName, fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken);

            identityBuilderMock.AssertWasCalled(m => m.Build(Arg<SecurityToken>.Is.Equal(userNameSecurityToken), Arg<IDictionary<string, string>>.Is.NotNull));
        }

        /// <summary>
        /// Tests that ValidateToken throws SecurityException when Build on the functionality which can build an identity fails with a SecurityException.
        /// </summary>
        [Test]
        public void TestThatValidateTokenThrowsSecurityExceptionWhenBuildOnIdentityBuilderFailsWithSecurityException()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var error = new SecurityException(fixture.Create<string>());
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();
            identityBuilderMock.Stub(m => m.Build(Arg<SecurityToken>.Is.NotNull, Arg<IDictionary<string, string>>.Is.Anything))
                .Throw(error)
                .Repeat.Any();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(error));
        }

        /// <summary>
        /// Tests that ValidateToken throws SecurityException when Build on the functionality which can build an identity fails with an Exception.
        /// </summary>
        [Test]
        public void TestThatValidateTokenThrowsSecurityExceptionWhenBuildOnIdentityBuilderFailsWithException()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNamePasswordValidatorMock = MockRepository.GenerateMock<UserNamePasswordValidator>();
            var error = fixture.Create<Exception>();
            var identityBuilderMock = MockRepository.GenerateMock<IIdentityBuilder>();
            identityBuilderMock.Stub(m => m.Build(Arg<SecurityToken>.Is.NotNull, Arg<IDictionary<string, string>>.Is.Anything))
                .Throw(error)
                .Repeat.Any();

            var userNameSecurityToken = new UserNameSecurityToken(fixture.Create<string>(), fixture.Create<string>());

            var userNameAsMailAddressSecurityTokenAuthenticator = new UserNameAsMailAddressSecurityTokenAuthenticator(userNamePasswordValidatorMock, identityBuilderMock);
            Assert.That(userNameAsMailAddressSecurityTokenAuthenticator, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => userNameAsMailAddressSecurityTokenAuthenticator.ValidateToken(userNameSecurityToken));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.EqualTo(error));
        }
    }
}
