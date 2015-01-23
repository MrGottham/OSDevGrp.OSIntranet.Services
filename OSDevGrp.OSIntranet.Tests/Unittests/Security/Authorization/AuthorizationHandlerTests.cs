using System.Security;
using System.Collections.Generic;
using Microsoft.IdentityModel.Claims;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Authorization;
using OSDevGrp.OSIntranet.Security.Claims;
using ClaimSet=System.IdentityModel.Claims.ClaimSet;

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
        /// Tests that GetCustomClaimSet throws a SecurityException when the claim sets added by authorization policies that have been evaluated is null.
        /// </summary>
        [Test]
        public void TestThatGetCustomClaimSetThrowsSecurityExceptionWhenClaimSetsIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.GetCustomClaimSet(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetCustomClaimSet returns an empty claim set to add.
        /// </summary>
        [Test]
        public void TestThatGetCustomClaimSetReturnsClaimSetWithoutClaims()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var result = authorizationHandler.GetCustomClaimSet(new List<ClaimSet>(0));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that Validate throws a SecurityException when the claims which should be used for validation is null.
        /// </summary>
        [Test]
        public void TestThatValidateThrowsSecurityExceptionWhenClaimsIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.Validate(null, typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Validate throws a SecurityException when the claims which should be used for validation does not contain any claims.
        /// </summary>
        [Test]
        public void TestThatValidateThrowsSecurityExceptionWhenClaimsDoesNotContainAnyClaims()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claimCollection = new List<Claim>(0);

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.Validate(claimCollection, typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Validate returns when the service type is null.
        /// </summary>
        [Test]
        public void TestThatValidateReturnsWhenServiceTypeIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty),
            };

            authorizationHandler.Validate(claimCollection, null);
        }

        /// <summary>
        /// Tests that Validate returns when the service type does not have any required claim type attributes.
        /// </summary>
        [Test]
        public void TestThatValidateReturnsWhenServiceTypeDoesNotHaveAnyRequiredClaimTypeAttribute()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty),
            };

            authorizationHandler.Validate(claimCollection, typeof (MyUnsecuredService));
        }

        /// <summary>
        /// Tests that Validate returns when the service type has required claim type attributes and the identity has these claims.
        /// </summary>
        [Test]
        public void TestThatValidateReturnsWhenServiceTypeHasRequiredClaimTypeAttributeAndClaimsIdentityHasRequiredClaimTypes()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty),
            };

            authorizationHandler.Validate(claimCollection, typeof (MySecuredService));
        }

        /// <summary>
        /// Tests that Validate throws a SecurityException when the service type has required claim type attributes and the identity does not have these claims.
        /// </summary>
        [Test]
        public void TestThatValidateThrowsSecurityExceptionWhenServiceTypeHasRequiredClaimTypeAttributeAndClaimsIdentityDoesNotHaveRequiredClaimTypes()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty),
            };

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.Validate(claimCollection, typeof (MySecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity)));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
