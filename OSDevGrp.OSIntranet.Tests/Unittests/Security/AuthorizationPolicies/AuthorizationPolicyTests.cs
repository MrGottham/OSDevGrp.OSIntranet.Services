using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.AuthorizationPolicies;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.AuthorizationPolicies
{
    /// <summary>
    /// Tests the authorization policy which can be used by WCF services.
    /// </summary>
    [TestFixture]
    public class AuthorizationPolicyTests
    {
        /// <summary>
        /// Tests that the constructor initialize an authorization policy which can be used by WCF services.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeAuthorizationPolicy()
        {
            var authorizationPolicy = new AuthorizationPolicy();
            Assert.That(authorizationPolicy, Is.Not.Null);
            Assert.That(authorizationPolicy.Id, Is.Not.Null);
            Assert.That(authorizationPolicy.Id, Is.Not.Null);
            Assert.That(authorizationPolicy.Id, Is.EqualTo("{C4579216-DD42-4755-BB96-968EF1E54F5C}"));
            Assert.That(authorizationPolicy.Issuer, Is.Not.Null);
            Assert.That(authorizationPolicy.Issuer, Is.EqualTo(ClaimSet.System));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the functionality which can handle the authorization policy is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenAuthorizationPolicyHandlerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new AuthorizationPolicy(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.EqualTo("authorizationPolicyHandler"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an ArgumentNullException when the evalutation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsArgumentNullExceptionWhenEvaluationContextIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationPolicyHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationPolicyHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationPolicyHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var state = fixture.Create<object>();

            // ReSharper disable AssignNullToNotNullAttribute
            var exception = Assert.Throws<ArgumentNullException>(() => authorizationPolicy.Evaluate(null, ref state));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.EqualTo("evaluationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an ArgumentNullException when the state is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsArgumentNullExceptionWhenStateIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationPolicyHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationPolicyHandler>()));
            fixture.Customize<EvaluationContext>(e => e.FromFactory(() => MockRepository.GenerateMock<EvaluationContext>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationPolicyHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            object state = null;

            var exception = Assert.Throws<ArgumentNullException>(() => authorizationPolicy.Evaluate(fixture.Create<EvaluationContext>(), ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.EqualTo("state"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
