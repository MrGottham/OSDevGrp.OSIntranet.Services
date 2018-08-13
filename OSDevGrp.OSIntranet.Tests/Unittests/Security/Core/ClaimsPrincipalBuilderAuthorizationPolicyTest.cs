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
using OSDevGrp.OSIntranet.Security.Core;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Tests the authorization policy which can build and set a claims principal.
    /// </summary>
    [TestFixture]
    public class ClaimsPrincipalBuilderAuthorizationPolicyTest
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
        /// Tests that the constructor throws an ArgumentNullException when the functionality which can handle authorization is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenAuthorizationHandlerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClaimsPrincipalBuilderAuthorizationPolicy(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("authorizationHandler"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the getter for Id returns a guid.
        /// </summary>
        [Test]
        public void TestThatIdGetterReturnsGuid()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var guid = Guid.Parse(claimsPrincipalBuilderAuthorizationPolicy.Id);
            Assert.That(guid, Is.Not.Null);
            Assert.That(guid, Is.Not.EqualTo(Guid.Empty));
        }

        /// <summary>
        /// Tests that Evaluate throws an ArgumentNullException when the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsArgumentNullExceptionWhenEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
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
        /// Tests that Evaluate adds a principal to properties on the evaluation context when properties does not contains a principal.
        /// </summary>
        [Test]
        public void TestThatEvaluateAddsPrincipalToPropertiesOnEvaluationContextWhenPropertiesDoesNotContainsPrincipal()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>(0);
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            Assert.That(properties.ContainsKey("Principal"), Is.False);
            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(properties.ContainsKey("Principal"), Is.True);
        }

        /// <summary>
        /// Tests that Evaluate returns true when properties on evaluation context does not contains a principal.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenPropertiesOnEvaluationContextDoesNotContainsPrincipal()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate returns true when properties on evaluation context contains a claims principal.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenPropertiesOnEvaluationContextContainsClaimsPrincipal()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>
            {
                {"Principal", new ClaimsPrincipal()}
            };
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate returns false when properties on evaluation context contains a principal which are not a claims principal
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsFalseWhenPropertiesOnEvaluationContextContainsPrincipalWhichAreNotClaimsPrincipal()
        {
            var fixture = new Fixture();
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>
            {
                {"Principal", fixture.Create<object>()}
            };
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate does not call GetTrustedClaimSets on the functionality which can handle authorization when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallGetTrustedClaimSetsOnAuthorizationHandlerWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            authorizationHandlerMock.AssertWasNotCalled(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate sets a principal in properties on evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetsPrincipalInPropertiesOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
        }

        /// <summary>
        /// Tests that Evaluate sets a principal with a claim identity without claims in properties on evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetsPrincipalWithClaimIdentityWithoutClaimsInPropertiesOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>(0);
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
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
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
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
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new List<ClaimSet>(0))
                .Repeat.Any();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate calls GetTrustedClaimSets on the functionality which can handle authorization when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateCallsGetTrustedClaimSetsOnAuthorizationHandlerWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new List<ClaimSet>(0))
                .Repeat.Any();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var claimSets = new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0));
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(claimSets)
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            authorizationHandlerMock.AssertWasCalled(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.Equal(claimSets)));
        }

        /// <summary>
        /// Tests that Evaluate sets a principal in properties on evaluation context when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetsPrincipalInPropertiesOnEvaluationContextWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new List<ClaimSet>(0))
                .Repeat.Any();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsPrincipalBuilderAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
        }

        /// <summary>
        /// Tests that Evaluate sets a principal with a claim identity containing claims from the trusted claim sets in properties on evaluation context when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        [TestCase("CN=OSDevGrp.OSIntranet.Services")]
        [TestCase("CN=OSDevGrp.OSIntranet.Clients")]
        [TestCase("CN=OSDevGrp.OSIntranet.Tokens")]
        public void TestThatEvaluateSetsPrincipalWithClaimIdentityWithTrustedClaimSetsInPropertiesOnEvaluationContextWhenClaimSetsInEvaluationContextIsNotNull(string certificateSubjectName)
        {
            var certificateClaimSet = new X509CertificateClaimSet(TestHelper.GetCertificate(certificateSubjectName));

            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new List<ClaimSet> {certificateClaimSet})
                .Repeat.Any();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>(0);
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(properties)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet> {MockRepository.GenerateStub<ClaimSet>()}))
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
            Assert.That(claimPrincipal.Identities.ElementAt(0).Claims.Count, Is.EqualTo(certificateClaimSet.Count(claim => string.Compare(claim.Right, Rights.PossessProperty, StringComparison.Ordinal) == 0)));
        }

        /// <summary>
        /// Tests that Evaluate returns true when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetTrustedClaimSets(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new List<ClaimSet>(0))
                .Repeat.Any();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
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
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsPrincipalBuilderAuthorizationPolicy = new ClaimsPrincipalBuilderAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsPrincipalBuilderAuthorizationPolicy, Is.Not.Null);

            var error = fixture.Create<Exception>();
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
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
