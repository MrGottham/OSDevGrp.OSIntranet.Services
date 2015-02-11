using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Security;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Claims;
using OSDevGrp.OSIntranet.Security.Core;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the functionality which can handle authorization.
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
        [RequiredClaimType(FoodWasteClaimTypes.SystemManagement)]
        [RequiredClaimType(FoodWasteClaimTypes.ValidatedUser)]
        private class MySecuredService
        {
        }

        /// <summary>
        /// Tests that the constructor initialize the functionality which can handle authorization.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeAuthorizationHandler()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Authorize throws an ArgumentNullException when the claims from a claims identity is null.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsArgumentNullExceptionWhenClaimsIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => authorizationHandler.Authorize(null, typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claims"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Authorize throws an ArgumentNullException when the service typeis null.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsArgumentNullExceptionWhenServiceTypeIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => authorizationHandler.Authorize(new List<Claim>(0), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("serviceType"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Authorize throws a SecurityException when the claims from a claims identity is empty.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsSecurityExceptionWhenClaimsIsEmpty()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception =Assert.Throws<SecurityException>(() => authorizationHandler.Authorize(new List<Claim>(0), typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Authorize returns when the service type does not have any required claim type attributes.
        /// </summary>
        [Test]
        public void TestThatAuthorizeReturnWhenServiceTypeDoesNotHaveAnyRequiredClaimTypeAttributes()
        {
            var fixture = new Fixture();

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, fixture.Create<object>(), Rights.PossessProperty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, fixture.Create<object>(), Rights.PossessProperty)
            };

            authorizationHandler.Authorize(claims, typeof (MyUnsecuredService));
        }

        /// <summary>
        /// Tests that Authorize returns when the service type has required claim type attributes and claims from the claims identity has the required claim types.
        /// </summary>
        [Test]
        public void TestThatAuthorizeReturnWhenServiceTypeHasRequiredClaimTypeAttributesAndClaimsFromClaimsIdentityHasRequiredClaimTypes()
        {
            var fixture = new Fixture();

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, fixture.Create<object>(), Rights.PossessProperty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, fixture.Create<object>(), Rights.PossessProperty)
            };

            authorizationHandler.Authorize(claims, typeof (MySecuredService));
        }

        /// <summary>
        /// Tests that Authorize throws a SecurityException when the service type has required claim type attributes and claims from the claims identity does not have the required claim types.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsSecurityExceptionWhenServiceTypeHasRequiredClaimTypeAttributesAndClaimsFromClaimsIdentityDoesNotHaveRequiredClaimTypes()
        {
            var fixture = new Fixture();

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.ValidatedUser, fixture.Create<object>(), Rights.PossessProperty)
            };

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.Authorize(claims, typeof (MySecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity)));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
