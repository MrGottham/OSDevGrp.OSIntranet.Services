using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Attributes;
using OSDevGrp.OSIntranet.Security.Claims;
using OSDevGrp.OSIntranet.Security.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

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
        /// Tests that GetTrustedClaimSets throws an ArgumentNullException when claim sets is null.
        /// </summary>
        [Test]
        public void TestThatGetTrustedClaimSetsThrowsArgumentNullExceptionWhenClaimSetsIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => authorizationHandler.GetTrustedClaimSets(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimSets"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetTrustedClaimSets returns trusted claim sets.
        /// </summary>
        [Test]
        [TestCase("CN=OSDevGrp.OSIntranet.Tokens", "CN=OSDevGrp.OSIntranet.Services")]
        [TestCase("CN=OSDevGrp.OSIntranet.Tokens", "CN=OSDevGrp.OSIntranet.Clients")]
        public void TestThatGetTrustedClaimSetsReturnsTrustedClaimSets(string trustedCertificateSubjectName, string untrustedCertificateSubjectName)
        {
            var trustedClaimSet1Stub = MockRepository.GenerateStub<ClaimSet>();
            trustedClaimSet1Stub.Stub(m => m.Issuer)
                .Return(new X509CertificateClaimSet(TestHelper.GetCertificate(trustedCertificateSubjectName)))
                .Repeat.Any();

            var trustedClaimSet2Stub = MockRepository.GenerateStub<ClaimSet>();
            trustedClaimSet2Stub.Stub(m => m.Issuer)
                .Return(new X509CertificateClaimSet(TestHelper.GetCertificate(trustedCertificateSubjectName)))
                .Repeat.Any();

            var untrustedClaimSet1Stub = MockRepository.GenerateStub<ClaimSet>();
            untrustedClaimSet1Stub.Stub(m => m.Issuer)
                .Return(new X509CertificateClaimSet(TestHelper.GetCertificate(untrustedCertificateSubjectName)))
                .Repeat.Any();

            var untrustedClaimSet2Stub = MockRepository.GenerateStub<ClaimSet>();
            untrustedClaimSet2Stub.Stub(m => m.Issuer)
                .Return(null)
                .Repeat.Any();

            var untrustedClaimSet3Stub = MockRepository.GenerateStub<ClaimSet>();
            untrustedClaimSet3Stub.Stub(m => m.Issuer)
                .Return(MockRepository.GenerateStub<ClaimSet>())
                .Repeat.Any();

            var claimSets = new List<ClaimSet>
            {
                trustedClaimSet1Stub,
                trustedClaimSet2Stub,
                untrustedClaimSet1Stub,
                untrustedClaimSet2Stub,
                untrustedClaimSet3Stub
            };

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var trustedClaimSets = authorizationHandler.GetTrustedClaimSets(claimSets).ToList();
            Assert.That(trustedClaimSets, Is.Not.Null);
            Assert.That(trustedClaimSets, Is.Not.Empty);
            Assert.That(trustedClaimSets.Count, Is.EqualTo(2));
            Assert.That(trustedClaimSets.Contains(trustedClaimSet1Stub), Is.True);
            Assert.That(trustedClaimSets.Contains(trustedClaimSet2Stub), Is.True);
        }

        /// <summary>
        /// Tests that Authorize throws an ArgumentNullException when the collection of trusted claims sets is null.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsArgumentNullExceptionWhenClaimSetsIsNull()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => authorizationHandler.Authorize(null, typeof (MyUnsecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimSets"));
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

            var exception = Assert.Throws<ArgumentNullException>(() => authorizationHandler.Authorize(new List<ClaimSet>(0), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("serviceType"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Authorize throws a SecurityException when the collection of trusted claims sets is empty.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsSecurityExceptionWhenClaimSetIsEmpty()
        {
            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.Authorize(new List<ClaimSet>(0), typeof (MyUnsecuredService)));
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
            var claimSetStub = MockRepository.GenerateStub<ClaimSet>();

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            authorizationHandler.Authorize(new List<ClaimSet> {claimSetStub}, typeof (MyUnsecuredService));
        }

        /// <summary>
        /// Tests that Authorize returns when the service type has required claim type attributes and the collection of trusted claim sets has the required claim types.
        /// </summary>
        [Test]
        public void TestThatAuthorizeReturnWhenServiceTypeHasRequiredClaimTypeAttributesAndClaimSetsHasRequiredClaimTypes()
        {
            var fixture = new Fixture();
            var claimSetStub = MockRepository.GenerateStub<ClaimSet>();
            claimSetStub.Stub(m => m.FindClaims(Arg<string>.Is.Equal(FoodWasteClaimTypes.SystemManagement), Arg<string>.Is.Anything))
                .Return(new List<Claim> {new Claim(FoodWasteClaimTypes.SystemManagement, fixture.Create<object>(), Rights.PossessProperty)})
                .Repeat.Any();
            claimSetStub.Stub(m => m.FindClaims(Arg<string>.Is.Equal(FoodWasteClaimTypes.ValidatedUser), Arg<string>.Is.Anything))
                .Return(new List<Claim> {new Claim(FoodWasteClaimTypes.ValidatedUser, fixture.Create<object>(), Rights.PossessProperty)})
                .Repeat.Any();

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            authorizationHandler.Authorize(new List<ClaimSet> {claimSetStub}, typeof (MySecuredService));

            claimSetStub.AssertWasCalled(m => m.FindClaims(Arg<string>.Is.Equal(FoodWasteClaimTypes.SystemManagement), Arg<string>.Is.Equal(Rights.PossessProperty)));
            claimSetStub.AssertWasCalled(m => m.FindClaims(Arg<string>.Is.Equal(FoodWasteClaimTypes.ValidatedUser), Arg<string>.Is.Equal(Rights.PossessProperty)));
        }

        /// <summary>
        /// Tests that Authorize throws a SecurityException when the service type has required claim type attributes and the collection of trusted claim sets does not have the required claim types.
        /// </summary>
        [Test]
        public void TestThatAuthorizeThrowsSecurityExceptionWhenServiceTypeHasRequiredClaimTypeAttributesAndClaimSetsDoesNotHaveRequiredClaimTypes()
        {
            var fixture = new Fixture();
            var claimSetStub = MockRepository.GenerateStub<ClaimSet>();
            claimSetStub.Stub(m => m.FindClaims(Arg<string>.Is.Equal(FoodWasteClaimTypes.SystemManagement), Arg<string>.Is.Anything))
                .Return(new List<Claim>(0))
                .Repeat.Any();
            claimSetStub.Stub(m => m.FindClaims(Arg<string>.Is.Equal(FoodWasteClaimTypes.ValidatedUser), Arg<string>.Is.Anything))
                .Return(new List<Claim> {new Claim(FoodWasteClaimTypes.ValidatedUser, fixture.Create<object>(), Rights.PossessProperty)})
                .Repeat.Any();

            var authorizationHandler = new AuthorizationHandler();
            Assert.That(authorizationHandler, Is.Not.Null);

            var exception = Assert.Throws<SecurityException>(() => authorizationHandler.Authorize(new List<ClaimSet> {claimSetStub}, typeof (MySecuredService)));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity)));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
