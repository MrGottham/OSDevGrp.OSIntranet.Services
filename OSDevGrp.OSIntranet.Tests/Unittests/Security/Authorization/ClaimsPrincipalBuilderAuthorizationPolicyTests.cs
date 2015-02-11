using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Authorization;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using ClaimsPrincipal=Microsoft.IdentityModel.Claims.ClaimsPrincipal;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Authorization
{
    /// <summary>
    /// Tests the authorization policy which can build and set a claims principal.
    /// </summary>
    [TestFixture]
    public class ClaimsPrincipalBuilderAuthorizationPolicyTests
    {
        /// <summary>
        /// Tests that the constructor initialize an authorization policy which can build and set a claims principal.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeClaimsPrincipalBuilderAuthorizationPolicy()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy.Id, Is.Not.Null);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy.Id, Is.Not.Empty);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy.Issuer, Is.Not.Null);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy.Issuer, Is.EqualTo(ClaimSet.System));
        }

        /// <summary>
        /// Tests that the getter for Id returns a guid.
        /// </summary>
        [Test]
        public void TestThatIdGetterReturnsGuid()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var guid = Guid.Parse(claimsPrincipalBuilderAuthorizationPolicy.Id);
            Assert.That(guid, Is.Not.Null);
            Assert.That(guid, Is.Not.EqualTo(Guid.Empty));
        }

        /// <summary>
        /// Tests that Evaluate throws an ArgumentNullException when the evalutation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsArgumentNullExceptionWhenEvaluationContextIsNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();

            // ReSharper disable AssignNullToNotNullAttribute
            var exception = Assert.Throws<ArgumentNullException>(() => claimsPrincipalBuilderAuthorizationPolicy.Evaluate(null, ref state));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("evaluationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate sets Principal on evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
        }

        /// <summary>
        /// Tests that Evaluate sets Principal with a claim identity without claims on evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalWithClaimIdentityWithoutClaimsOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>
            {
                {"Principal", null}
            };
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            var claimPrincipal = (ClaimsPrincipal) properties["Principal"];
            Assert.That(claimPrincipal, Is.Not.Null);
            Assert.That(claimPrincipal.Identity, Is.Not.Null);
            Assert.That(claimPrincipal.Identity, Is.TypeOf<ClaimsIdentity>());
            Assert.That(claimPrincipal.Identities, Is.Not.Null);
            Assert.That(claimPrincipal.Identities, Is.Not.Empty);
            Assert.That(claimPrincipal.Identities.Count, Is.EqualTo(1));
            Assert.That(claimPrincipal.Identities.ElementAt(0), Is.Not.Null);
            Assert.That(claimPrincipal.Identities.ElementAt(0).Claims, Is.Not.Null);
            Assert.That(claimPrincipal.Identities.ElementAt(0).Claims, Is.Empty);
        }

        /// <summary>
        /// Tests that Evaluate returns true when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenClaimSetsInEvaluationContextIsNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate sets Principal on evaluation context when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalOnEvaluationContextWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
        }

        /// <summary>
        /// Tests that Evaluate sets Principal with a claim identity containing claims from a X509Certificate on evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        [TestCase("CN=OSDevGrp.OSIntranet.Services")]
        [TestCase("CN=OSDevGrp.OSIntranet.Clients")]
        [TestCase("CN=OSDevGrp.OSIntranet.Tokens")]
        public void TestThatEvaluateSetPrincipalWithClaimIdentityWithClaimsFromX509CertificateOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull(string certificateSubjectName)
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>
            {
                {"Principal", null}
            };
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new ClaimSet[] {new X509CertificateClaimSet(TestHelper.GetCertificate(certificateSubjectName))}))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            var claimPrincipal = (ClaimsPrincipal) properties["Principal"];
            Assert.That(claimPrincipal, Is.Not.Null);
            Assert.That(claimPrincipal.Identity, Is.Not.Null);
            Assert.That(claimPrincipal.Identity, Is.TypeOf<ClaimsIdentity>());
            Assert.That(claimPrincipal.Identities, Is.Not.Null);
            Assert.That(claimPrincipal.Identities, Is.Not.Empty);
            Assert.That(claimPrincipal.Identities.Count, Is.EqualTo(1));
            Assert.That(claimPrincipal.Identities.ElementAt(0), Is.Not.Null);
            Assert.That(claimPrincipal.Identities.ElementAt(0).Claims, Is.Not.Null);
            Assert.That(claimPrincipal.Identities.ElementAt(0).Claims, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that Evaluate returns true when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate throws a FaultException when an exception occurs.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenExceptionOccurs()
        {
            var fixture = new Fixture();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy();
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var error = fixture.Create<Exception>();
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Throw(error)
                .Repeat.Any();
            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, error.Message)));
            Assert.That(exception.Reason, Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Empty);
            Assert.That(exception.Reason.ToString(), Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, error.Message)));
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
