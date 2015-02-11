using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Core;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the authorization policy to use when authorizing users who has a mail address as username.
    /// </summary>
    [TestFixture]
    public class UserNameAsMailAddressAuthorizationPolicyTests
    {
        /// <summary>
        /// Tests that the constructor initialize an authorization policy to use when authorizing users who has a mail address as username.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeUserNameAsMailAddressAuthorizationPolicy()
        {
            var primaryIdentityMock = MockRepository.GenerateMock<IIdentity>();

            var userNameAsMailAddressAuthorizationPolicy = new UserNameAsMailAddressAuthorizationPolicy(primaryIdentityMock);
            Assert.That(userNameAsMailAddressAuthorizationPolicy, Is.Not.Null);
            Assert.That(userNameAsMailAddressAuthorizationPolicy.Id, Is.Not.Null);
            Assert.That(userNameAsMailAddressAuthorizationPolicy.Id, Is.Not.Empty);
            Assert.That(userNameAsMailAddressAuthorizationPolicy.Issuer, Is.Not.Null);
            Assert.That(userNameAsMailAddressAuthorizationPolicy.Issuer, Is.EqualTo(ClaimSet.System));
            Assert.That(userNameAsMailAddressAuthorizationPolicy.PrimaryIdentity, Is.Not.Null);
            Assert.That(userNameAsMailAddressAuthorizationPolicy.PrimaryIdentity, Is.EqualTo(primaryIdentityMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the primary identity is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenPrimaryIdentityIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new UserNameAsMailAddressAuthorizationPolicy(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("primaryIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the getter for Id returns a guid.
        /// </summary>
        [Test]
        public void TestThatIdGetterReturnsGuid()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNameAsMailAddressAuthorizationPolicy = new UserNameAsMailAddressAuthorizationPolicy(fixture.Create<IIdentity>());
            Assert.That(userNameAsMailAddressAuthorizationPolicy, Is.Not.Null);

            var guid = Guid.Parse(userNameAsMailAddressAuthorizationPolicy.Id);
            Assert.That(guid, Is.Not.Null);
            Assert.That(guid, Is.Not.EqualTo(Guid.Empty));
        }

        /// <summary>
        /// Tests that Evaluate throws an ArgumentNullException when the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsArgumentNullExceptionWhenEvaluationContextIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNameAsMailAddressAuthorizationPolicy = new UserNameAsMailAddressAuthorizationPolicy(fixture.Create<IIdentity>());
            Assert.That(userNameAsMailAddressAuthorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();

            // ReSharper disable AssignNullToNotNullAttribute
            var exception = Assert.Throws<ArgumentNullException>(() => userNameAsMailAddressAuthorizationPolicy.Evaluate(null, ref state));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("evaluationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an NotSupportedException when the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsNotSupportedExceptionWhenEvaluationContextIsNotNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IIdentity>(e => e.FromFactory(() => MockRepository.GenerateMock<IIdentity>()));

            var userNameAsMailAddressAuthorizationPolicy = new UserNameAsMailAddressAuthorizationPolicy(fixture.Create<IIdentity>());
            Assert.That(userNameAsMailAddressAuthorizationPolicy, Is.Not.Null);

            var evaluationContext = MockRepository.GenerateMock<EvaluationContext>();
            var state = CreateLegalState();

            var exception = Assert.Throws<NotSupportedException>(() => userNameAsMailAddressAuthorizationPolicy.Evaluate(evaluationContext, ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a legal state.
        /// </summary>
        /// <returns>Legal state.</returns>
        private static object CreateLegalState()
        {
            return null;
        }
    }
}
