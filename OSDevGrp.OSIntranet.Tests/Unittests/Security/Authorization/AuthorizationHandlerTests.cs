using System.Security;
using Microsoft.IdentityModel.Claims;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Authorization;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Security.Claims;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Authorization
{
    /// <summary>
    /// Tests the functionality which can handle the authorization.
    /// </summary>
    [TestFixture]
    public class AuthorizationHandlerTests
    {
        /// <summary>
        /// Private class for an unsecured service used for testing purpose.
        /// </summary>
        private class MyUnsecuredService
        {
        }

        /// <summary>
        /// Private class for a secured service used for testing purpose.
        /// </summary>
        [RequiredClaimTypeAttribute("urn://osdevgrp/foodwaste/security/systemmanagement")]
        [RequiredClaimTypeAttribute("urn://osdevgrp/foodwaste/security/user")]
        private class MySecuredService
        {
        }

        /// <summary>
        /// Tests that the constructor initalize functionality which can handle the authorization.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeAuthorizationHandler()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that GetCustomPrincipal throws a SecurityException when the collection of claim based identities is null.
        /// </summary>
        [Test]
        public void TestThatGetCustomPrincipalThrowsSecurityExceptionWhenClaimsIdentitiesIsNull()
        {
            var authorizationPolicyHandler = new AuthorizationHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => authorizationPolicyHandler.GetCustomPrincipal(null, typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetCustomPrincipal throws a SecurityException when the collection of claim based identities does not contain any claim based identities.
        /// </summary>
        [Test]
        public void TestThatGetCustomPrincipalThrowsSecurityExceptionWhenClaimsIdentitiesDoesNotContainAnyClaimsIdentities()
        {
            var authorizationPolicyHandler = new AuthorizationHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);

            var claimsIdentitiesMockCollection = new List<IClaimsIdentity>(0);

            var exception = Assert.Throws<SecurityException>(() => authorizationPolicyHandler.GetCustomPrincipal(claimsIdentitiesMockCollection, typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetCustomPrincipal returns a custom claim based principal when the service type is null.
        /// </summary>
        [Test]
        public void TestThatGetCustomPrincipalReturnsClaimsPrincipalWhenServiceTypeIsNull()
        {
            var authorizationPolicyHandler = new AuthorizationHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);

            var claimsIdentityMock = MockRepository.GenerateMock<IClaimsIdentity>();

            var claimsIdentitiesMockCollection = new List<IClaimsIdentity>
            {
                claimsIdentityMock
            };

            var claimsPrincipal = authorizationPolicyHandler.GetCustomPrincipal(claimsIdentitiesMockCollection, null);
            Assert.That(claimsPrincipal, Is.Not.Null);
            Assert.That(claimsPrincipal.Identity, Is.Not.Null);
            Assert.That(claimsPrincipal.Identity, Is.EqualTo(claimsIdentityMock));
            Assert.That(claimsPrincipal.Identities, Is.Not.Null);
            Assert.That(claimsPrincipal.Identities, Is.Not.Empty);
            Assert.That(claimsPrincipal.Identities, Is.EqualTo(claimsIdentitiesMockCollection));
        }

        /// <summary>
        /// Tests that GetCustomPrincipal returns a custom claim based principal when the service type does not have any required claim type attributes.
        /// </summary>
        [Test]
        public void TestThatGetCustomPrincipalReturnsClaimsPrincipalWhenServiceTypeDoesNotHaveAnyRequiredClaimTypeAttribute()
        {
            var authorizationPolicyHandler = new AuthorizationHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);

            var claimsIdentityMock = MockRepository.GenerateMock<IClaimsIdentity>();

            var claimsIdentitiesMockCollection = new List<IClaimsIdentity>
            {
                claimsIdentityMock
            };

            var claimsPrincipal = authorizationPolicyHandler.GetCustomPrincipal(claimsIdentitiesMockCollection, typeof (MyUnsecuredService));
            Assert.That(claimsPrincipal, Is.Not.Null);
            Assert.That(claimsPrincipal.Identity, Is.Not.Null);
            Assert.That(claimsPrincipal.Identity, Is.EqualTo(claimsIdentityMock));
            Assert.That(claimsPrincipal.Identities, Is.Not.Null);
            Assert.That(claimsPrincipal.Identities, Is.Not.Empty);
            Assert.That(claimsPrincipal.Identities, Is.EqualTo(claimsIdentitiesMockCollection));
        }

        /// <summary>
        /// Tests that GetCustomPrincipal returns a custom claim based principal when the service type has required claim type attributes and the identity has these claims.
        /// </summary>
        [Test]
        public void TestThatGetCustomPrincipalReturnsClaimsPrincipalWhenServiceTypeHasRequiredClaimTypeAttributeAndClaimsIdentityHasRequiredClaimTypes()
        {
            var authorizationPolicyHandler = new AuthorizationHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);

            var claimsIdentityMock = MockRepository.GenerateMock<IClaimsIdentity>();
            claimsIdentityMock.Stub(m => m.Claims)
                .Return(new ClaimCollection(claimsIdentityMock) {new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty), new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty)})
                .Repeat.Any();

            var claimsIdentitiesMockCollection = new List<IClaimsIdentity>
            {
                claimsIdentityMock
            };

            var claimsPrincipal = authorizationPolicyHandler.GetCustomPrincipal(claimsIdentitiesMockCollection, typeof (MySecuredService));
            Assert.That(claimsPrincipal, Is.Not.Null);
            Assert.That(claimsPrincipal.Identity, Is.Not.Null);
            Assert.That(claimsPrincipal.Identity, Is.EqualTo(claimsIdentityMock));
            Assert.That(claimsPrincipal.Identities, Is.Not.Null);
            Assert.That(claimsPrincipal.Identities, Is.Not.Empty);
            Assert.That(claimsPrincipal.Identities, Is.EqualTo(claimsIdentitiesMockCollection));
        }

        /// <summary>
        /// Tests that GetCustomPrincipal throws a SecurityException when the service type has required claim type attributes and the identity does not have these claims.
        /// </summary>
        [Test]
        public void TestThatGetCustomPrincipalThrowsSecurityExceptionWhenServiceTypeHasRequiredClaimTypeAttributeAndClaimsIdentityDoesNotHaveRequiredClaimTypes()
        {
            var authorizationPolicyHandler = new AuthorizationHandler();
            Assert.That(authorizationPolicyHandler, Is.Not.Null);

            var claimsIdentityMock = MockRepository.GenerateMock<IClaimsIdentity>();
            claimsIdentityMock.Stub(m => m.Claims)
                .Return(new ClaimCollection(claimsIdentityMock) {new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty)})
                .Repeat.Any();

            var claimsIdentitiesMockCollection = new List<IClaimsIdentity>
            {
                claimsIdentityMock
            };

            var exception = Assert.Throws<SecurityException>(() => authorizationPolicyHandler.GetCustomPrincipal(claimsIdentitiesMockCollection, typeof (MySecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity)));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
