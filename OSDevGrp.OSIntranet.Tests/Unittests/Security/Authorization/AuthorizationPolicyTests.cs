using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Authorization;
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
        /// Private class for an evaluation context which are used for testing purpose.
        /// </summary>
        private class MyEvaluationContext : EvaluationContext
        {
            #region Private variables

            private const int GenerationValue = 0;
            private readonly ReadOnlyCollection<ClaimSet> _claimSets = new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>());
            private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

            #endregion

            #region Constructor

            /// <summary>
            /// Creates an evaluation context which are used for testing purpose.
            /// </summary>
            /// <param name="properties">Collection of non-claim properties which should be associated with this evaluation context.</param>
            public MyEvaluationContext(IDictionary<string, object> properties)
            {
                _properties = properties;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets a read-only collection of ClaimSet objects that contains the claims added by authorization policies that have been evaluated.
            /// </summary>
            public override ReadOnlyCollection<ClaimSet> ClaimSets
            {
                get { return _claimSets; }
            }

            /// <summary>
            /// Gets the number of times that claims have been added to the evaluation context.
            /// </summary>
            public override int Generation
            {
                get { return GenerationValue; }
            }

            /// <summary>
            /// Gets a collection of non-claim properties associated with this evaluation context.
            /// </summary>
            public override IDictionary<string, object> Properties
            {
                get { return _properties; }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Adds a set of claims to the evaluation context.
            /// </summary>
            /// <param name="policy">Authorization policy which is adding claims to the evaluation context.</param>
            /// <param name="claimSet">Set of claims which should be added.</param>
            public override void AddClaimSet(IAuthorizationPolicy policy, ClaimSet claimSet)
            {
            }

            /// <summary>
            /// Sets the date and time at which this EvaluationContext is no longer valid.
            /// </summary>
            /// <param name="expirationTime">Date and time that indicates when this authorization context object is no longer valid.</param>
            public override void RecordExpirationTime(DateTime expirationTime)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize an authorization policy which can be used by WCF services.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeAuthorizationPolicy()
        {
            var authorizationPolicy = new AuthorizationPolicy();
            Assert.That(authorizationPolicy, Is.Not.Null);
            Assert.That(authorizationPolicy.Id, Is.Not.Null);
            Assert.That(authorizationPolicy.Id, Is.Not.Empty);
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
            Assert.That(exception.ParamName, Is.Not.Empty);
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
            fixture.Customize<IAuthorizationHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();

            // ReSharper disable AssignNullToNotNullAttribute
            var exception = Assert.Throws<ArgumentNullException>(() => authorizationPolicy.Evaluate(null, ref state));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("evaluationContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an FaultException when Properties in evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenPropertiesInEvaluationContextIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContext = new MyEvaluationContext(null);

            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(evaluationContext, ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.Reason, Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Empty);
            Assert.That(exception.Reason.ToString(), Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an FaultException when evaluation properties does not contain any identities.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenEvaluationContextDoesNotContainAnyIdentities()
        {
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContext = new MyEvaluationContext(new Dictionary<string, object>(0));

            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(evaluationContext, ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.Reason, Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Empty);
            Assert.That(exception.Reason.ToString(), Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an FaultException when evaluation properties does not contain an object of claim based identities.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenEvaluationContextDoesNotContainObjectOfClaimBasedIdentities()
        {
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>(1)
            {
                {"Identities", fixture.CreateMany<string>().ToList()}
            };
            var evaluationContext = new MyEvaluationContext(properties);

            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(evaluationContext, ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.Reason, Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Empty);
            Assert.That(exception.Reason.ToString(), Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate throws an FaultException when evaluation properties does not contain any claim based identities.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenEvaluationContextDoesNotContainAnyClaimBasedIdentities()
        {
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var properties = new Dictionary<string, object>(1)
            {
                {"Identities", new List<IIdentity>(0)}
            };
            var evaluationContext = new MyEvaluationContext(properties);

            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(evaluationContext, ref state));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.Reason, Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Null);
            Assert.That(exception.Reason.ToString(), Is.Not.Empty);
            Assert.That(exception.Reason.ToString(), Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, Resource.GetExceptionMessage(ExceptionMessage.NoIdentityWasFound))));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Evaluate calls GetCustomPrincipal on the functionality which can handle the authorization policy.
        /// </summary>
        [Test]
        public void TestThatEvaluateCallsGetCustomPrincipalOnAuthorizationPolicyHandler()
        {
            var fixture = new Fixture();
            fixture.Customize<IClaimsPrincipal>(e => e.FromFactory(() => MockRepository.GenerateMock<IClaimsPrincipal>()));

            var authorizationPolicyHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationPolicyHandlerMock.Stub(m => m.GetCustomPrincipal(Arg<IEnumerable<IClaimsIdentity>>.Is.NotNull, Arg<Type>.Is.Anything))
                .Return(fixture.Create<IClaimsPrincipal>())
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationPolicyHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();
            authorizationPolicy.Evaluate(CreateLegalEvaluationContext(), ref state);

            authorizationPolicyHandlerMock.AssertWasCalled(m => m.GetCustomPrincipal(Arg<IEnumerable<IClaimsIdentity>>.Is.NotNull, Arg<Type>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate throws an FaultException when GetCustomPrincipal on the functionality which can handle the authorization policy fails.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenGetCustomPrincipalOnAuthorizationPolicyHandlerFails()
        {
            var fixture = new Fixture();

            var error = fixture.Create<Exception>();
            var authorizationPolicyHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationPolicyHandlerMock.Stub(m => m.GetCustomPrincipal(Arg<IEnumerable<IClaimsIdentity>>.Is.NotNull, Arg<Type>.Is.Anything))
                .Throw(error)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationPolicyHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(CreateLegalEvaluationContext(), ref state));
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
        /// Tests that Evaluate updates Principal in the evaluation context.
        /// </summary>
        [Test]
        public void TestThatEvaluateUpdatesPrincipalInEvaluationContext()
        {
            var claimsPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            var authorizationPolicyHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationPolicyHandlerMock.Stub(m => m.GetCustomPrincipal(Arg<IEnumerable<IClaimsIdentity>>.Is.NotNull, Arg<Type>.Is.Anything))
                .Return(claimsPrincipalMock)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationPolicyHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContext = CreateLegalEvaluationContext();
            var state = CreateLegalState();

            Assert.That(evaluationContext.Properties["Principal"], Is.Null);
            authorizationPolicy.Evaluate(evaluationContext, ref state);
            Assert.That(evaluationContext.Properties["Principal"], Is.Not.Null);
            Assert.That(evaluationContext.Properties["Principal"], Is.EqualTo(claimsPrincipalMock));
        }

        /// <summary>
        /// Tests that Evaluate returns true if no exception occurs.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnTrueIfNoExceptionOccurs()
        {
            var fixture = new Fixture();
            fixture.Customize<IClaimsPrincipal>(e => e.FromFactory(() => MockRepository.GenerateMock<IClaimsPrincipal>()));

            var authorizationPolicyHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationPolicyHandlerMock.Stub(m => m.GetCustomPrincipal(Arg<IEnumerable<IClaimsIdentity>>.Is.NotNull, Arg<Type>.Is.Anything))
                .Return(fixture.Create<IClaimsPrincipal>())
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationPolicyHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();
            
            var result = authorizationPolicy.Evaluate(CreateLegalEvaluationContext(), ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Creates a legal evaluation context which can be used for testing purpose.
        /// </summary>
        /// <returns>Legal evaluation context which can be used for testing purpose.</returns>
        private static EvaluationContext CreateLegalEvaluationContext()
        {
            var claimsIdentity = MockRepository.GenerateMock<IClaimsIdentity>();
            var properties = new Dictionary<string, object>(2)
            {
                {"Identities", new[] {claimsIdentity}},
                {"Principal", null}
            };
            return new MyEvaluationContext(properties);
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
