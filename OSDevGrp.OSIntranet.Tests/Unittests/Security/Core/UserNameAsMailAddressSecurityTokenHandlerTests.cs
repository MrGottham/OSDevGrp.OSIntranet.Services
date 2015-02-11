using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Core;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the security token handler for authentication of users who has a mail address as username.
    /// </summary>
    [TestFixture]
    public class UserNameAsMailAddressSecurityTokenHandlerTests
    {
        /// <summary>
        /// Tests that the constructor initialize a security token handler for authentication of users who has a mail address as username.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeUserNameAsMailAddressSecurityTokenHandler()
        {
            var userNameAsMailAddressSecurityTokenHandler = new UserNameAsMailAddressSecurityTokenHandler();
            Assert.That(userNameAsMailAddressSecurityTokenHandler, Is.Not.Null);
            Assert.That(userNameAsMailAddressSecurityTokenHandler.TokenType, Is.Not.Null);
            Assert.That(userNameAsMailAddressSecurityTokenHandler.TokenType.Name, Is.Not.Null);
            Assert.That(userNameAsMailAddressSecurityTokenHandler.TokenType.Name, Is.Not.Empty);
            Assert.That(userNameAsMailAddressSecurityTokenHandler.TokenType.Name, Is.EqualTo(typeof (UserNameSecurityToken).Name));
            Assert.That(userNameAsMailAddressSecurityTokenHandler.CanValidateToken, Is.True);
            Assert.That(userNameAsMailAddressSecurityTokenHandler.CanWriteToken, Is.True);
        }

        /// <summary>
        /// Tests that the constructor throw an ArgumentNullException when the security token authenticator to use in this security token handler is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSecurityTokenAuthenticatorIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new UserNameAsMailAddressSecurityTokenHandler(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("securityTokenAuthenticator"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ValidateToken throws an ArgumentNullException when the security token to validate is null.
        /// </summary>
        [Test]
        public void TestThatValidateTokenThrowsArgumentNullExceptionWhenSecurityTokenIsNull()
        {
            var userNameAsMailAddressSecurityTokenHandler = new UserNameAsMailAddressSecurityTokenHandler(new UserNameAsMailAddressSecurityTokenAuthenticator());
            Assert.That(userNameAsMailAddressSecurityTokenHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => userNameAsMailAddressSecurityTokenHandler.ValidateToken(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("token"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ValidateToken validates a legal mail address.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com")]
        [TestCase("test@osdevgrp.dk")]
        public void TestThatValidateTokenValidatesLegalMailAddress(string legalMailAddress)
        {
            var userNameAsMailAddressSecurityTokenHandler = new UserNameAsMailAddressSecurityTokenHandler(new UserNameAsMailAddressSecurityTokenAuthenticator());
            Assert.That(userNameAsMailAddressSecurityTokenHandler, Is.Not.Null);

            var securityToken = new UserNameSecurityToken(legalMailAddress, null);

            userNameAsMailAddressSecurityTokenHandler.ValidateToken(securityToken);
        }

        /// <summary>
        /// Tests that ValidateToken returns collection of claims identities for a legal mail address.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com")]
        [TestCase("test@osdevgrp.dk")]
        public void TestThatValidateTokenReturnsClaimsIdentityCollectionForLegalMailAddress(string legalMailAddress)
        {
            var userNameAsMailAddressSecurityTokenHandler = new UserNameAsMailAddressSecurityTokenHandler(new UserNameAsMailAddressSecurityTokenAuthenticator());
            Assert.That(userNameAsMailAddressSecurityTokenHandler, Is.Not.Null);

            var securityToken = new UserNameSecurityToken(legalMailAddress, null);

            var result = userNameAsMailAddressSecurityTokenHandler.ValidateToken(securityToken);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(1));

            var claimsIdentity = result.First();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.Name, Is.Not.Null);
            Assert.That(claimsIdentity.Name, Is.Not.Empty);
            Assert.That(claimsIdentity.Name, Is.EqualTo(legalMailAddress));

            var mailAddressClaim = claimsIdentity.Claims.Single(m => string.Compare(m.ClaimType, ClaimTypes.Email, StringComparison.Ordinal) == 0);
            Assert.That(mailAddressClaim, Is.Not.Null);
            Assert.That(mailAddressClaim.Value, Is.Not.Null);
            Assert.That(mailAddressClaim.Value, Is.Not.Empty);
            Assert.That(mailAddressClaim.Value, Is.EqualTo(legalMailAddress));
        }
    }
}
