using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security;
using Microsoft.IdentityModel.Claims;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Configuration;
using OSDevGrp.OSIntranet.Security.Core;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the functionality which can build an identity.
    /// </summary>
    [TestFixture]
    public class IdentityBuilderTests
    {
        /// <summary>
        /// Tests that the constructor initialize the functionality which can build an identity.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeIdentityBuilder()
        {
            var identityBuilder = new IdentityBuilder();
            Assert.That(identityBuilder, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Build throws an ArgumentNullException when the security token is not null.
        /// </summary>
        [Test]
        public void TestThatBuildThrowsArgumentNullExceptionWhenSecurityTokenIsNull()
        {
            var identityBuilder = new IdentityBuilder();
            Assert.That(identityBuilder, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => identityBuilder.Build(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("securityToken"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build throws an SecurityException when the security token is not an UserNameSecurityToken.
        /// </summary>
        [Test]
        public void TestThatBuildThrowsSecurityExceptionWhenSecurityTokenIsNotTypeOfUserNameSecurityToken()
        {
            var securityToken = MockRepository.GenerateMock<SecurityToken>();

            var identityBuilder = new IdentityBuilder();
            Assert.That(identityBuilder, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => identityBuilder.Build(securityToken));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, securityToken, "securityToken")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Build builds identity for an UserNameSecurityToken without identity properties.
        /// </summary>
        [Test]
        public void TestThatBuildBuildsIdentityForUserNameSecurityTokenWithoutIdentityProperties()
        {
            var fixture = new Fixture();

            var userName = fixture.Create<string>();
            var password = fixture.Create<string>();
            var userNameSecurityToken = new UserNameSecurityToken(userName, password);

            var identityBuilder = new IdentityBuilder();
            Assert.That(identityBuilder, Is.Not.Null);

            var identity = identityBuilder.Build(userNameSecurityToken);
            Assert.That(identity, Is.Not.Null);
            Assert.That(identity, Is.TypeOf<ClaimsIdentity>());

            var claimsIdentity = (ClaimsIdentity) identity;
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.Name, Is.Not.Null);
            Assert.That(claimsIdentity.Name, Is.Not.Empty);
            Assert.That(claimsIdentity.Name, Is.EqualTo(userName));
            Assert.That(claimsIdentity.NameClaimType, Is.Not.Null);
            Assert.That(claimsIdentity.NameClaimType, Is.Not.Empty);
            Assert.That(claimsIdentity.NameClaimType, Is.EqualTo(ClaimTypes.Name));
            Assert.That(claimsIdentity.RoleClaimType, Is.Not.Null);
            Assert.That(claimsIdentity.RoleClaimType, Is.Not.Empty);
            Assert.That(claimsIdentity.RoleClaimType, Is.EqualTo(ClaimTypes.Role));
            Assert.That(claimsIdentity.IsAuthenticated, Is.True);
            Assert.That(claimsIdentity.AuthenticationType, Is.Not.Null);
            Assert.That(claimsIdentity.AuthenticationType, Is.Not.Empty);
            Assert.That(claimsIdentity.AuthenticationType, Is.EqualTo(AuthenticationTypes.Password));
            Assert.That(claimsIdentity.Label, Is.Not.Null);
            Assert.That(claimsIdentity.Label, Is.Not.Empty);
            Assert.That(claimsIdentity.Label, Is.EqualTo(userName));
            Assert.That(claimsIdentity.BootstrapToken, Is.Not.Null);
            Assert.That(claimsIdentity.BootstrapToken, Is.EqualTo(userNameSecurityToken));
            Assert.That(claimsIdentity.Actor, Is.Null);
            Assert.That(claimsIdentity.Claims, Is.Not.Null);
            Assert.That(claimsIdentity.Claims, Is.Not.Empty);
            Assert.That(claimsIdentity.Claims.Count, Is.EqualTo(1));

            ValidateClaim(claimsIdentity.Claims.Single(claim => string.Compare(claim.ClaimType, ClaimTypes.Name, StringComparison.Ordinal) == 0), userName, ClaimValueTypes.String);
        }

        /// <summary>
        /// Tests that Build builds identity for an UserNameSecurityToken with identity properties.
        /// </summary>
        [Test]
        public void TestThatBuildBuildsIdentityForUserNameSecurityTokenWithIdentityProperties()
        {
            var fixture = new Fixture();

            var userName = fixture.Create<string>();
            var password = fixture.Create<string>();
            var userNameSecurityToken = new UserNameSecurityToken(userName, password);

            var identityProperties = new Dictionary<string, string>
            {
                {ClaimTypes.Email, fixture.Create<string>()},
                {ClaimTypes.MobilePhone, fixture.Create<string>()},
                {ClaimTypes.OtherPhone, fixture.Create<string>()}
            };

            var identityBuilder = new IdentityBuilder();
            Assert.That(identityBuilder, Is.Not.Null);

            var identity = identityBuilder.Build(userNameSecurityToken, identityProperties);
            Assert.That(identity, Is.Not.Null);
            Assert.That(identity, Is.TypeOf<ClaimsIdentity>());

            var claimsIdentity = (ClaimsIdentity) identity;
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.Name, Is.Not.Null);
            Assert.That(claimsIdentity.Name, Is.Not.Empty);
            Assert.That(claimsIdentity.Name, Is.EqualTo(userName));
            Assert.That(claimsIdentity.NameClaimType, Is.Not.Null);
            Assert.That(claimsIdentity.NameClaimType, Is.Not.Empty);
            Assert.That(claimsIdentity.NameClaimType, Is.EqualTo(ClaimTypes.Name));
            Assert.That(claimsIdentity.RoleClaimType, Is.Not.Null);
            Assert.That(claimsIdentity.RoleClaimType, Is.Not.Empty);
            Assert.That(claimsIdentity.RoleClaimType, Is.EqualTo(ClaimTypes.Role));
            Assert.That(claimsIdentity.IsAuthenticated, Is.True);
            Assert.That(claimsIdentity.AuthenticationType, Is.Not.Null);
            Assert.That(claimsIdentity.AuthenticationType, Is.Not.Empty);
            Assert.That(claimsIdentity.AuthenticationType, Is.EqualTo(AuthenticationTypes.Password));
            Assert.That(claimsIdentity.Label, Is.Not.Null);
            Assert.That(claimsIdentity.Label, Is.Not.Empty);
            Assert.That(claimsIdentity.Label, Is.EqualTo(userName));
            Assert.That(claimsIdentity.BootstrapToken, Is.Not.Null);
            Assert.That(claimsIdentity.BootstrapToken, Is.EqualTo(userNameSecurityToken));
            Assert.That(claimsIdentity.Actor, Is.Null);
            Assert.That(claimsIdentity.Claims, Is.Not.Null);
            Assert.That(claimsIdentity.Claims, Is.Not.Empty);
            Assert.That(claimsIdentity.Claims.Count, Is.EqualTo(1 + identityProperties.Count));

            ValidateClaim(claimsIdentity.Claims.Single(claim => string.Compare(claim.ClaimType, ClaimTypes.Name, StringComparison.Ordinal) == 0), userName, ClaimValueTypes.String);
            foreach (var keyValuePair in identityProperties)
            {
                ValidateClaim(claimsIdentity.Claims.Single(claim => string.Compare(claim.ClaimType, keyValuePair.Key, StringComparison.Ordinal) == 0), keyValuePair.Value, ClaimValueTypes.String);
            }
        }

        /// <summary>
        /// Validates a created claim.
        /// </summary>
        /// <param name="claim">The claim to validate.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="expectedValueType">The expected value type.</param>
        private static void ValidateClaim(Claim claim, string expectedValue, string expectedValueType)
        {
            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(expectedValue));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(expectedValueType));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(ConfigurationProvider.Instance.IssuerTokenName.Address));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(ConfigurationProvider.Instance.IssuerTokenName.Address));
            Assert.That(claim.Properties, Is.Not.Null);
            Assert.That(claim.Properties, Is.Empty);
        }
    }
}
