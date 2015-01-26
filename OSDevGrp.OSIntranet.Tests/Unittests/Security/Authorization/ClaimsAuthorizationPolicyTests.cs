using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Authorization;
using OSDevGrp.OSIntranet.Security.Claims;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Authorization
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
            Assert.That(authorizationPolicy.Id, Is.Not.Empty);
            Assert.That(authorizationPolicy.Issuer, Is.Not.Null);
            Assert.That(authorizationPolicy.Issuer, Is.EqualTo(ClaimSet.System));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the functionality which can handle the authorization is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenAuthorizationHandlerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new AuthorizationPolicy(null));
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
            var fixture = new Fixture();
            fixture.Customize<IAuthorizationHandler>(e => e.FromFactory(() => MockRepository.GenerateMock<IAuthorizationHandler>()));

            var authorizationPolicy = new AuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(authorizationPolicy, Is.Not.Null);

            var guid = Guid.Parse(authorizationPolicy.Id);
            Assert.That(guid, Is.Not.Null);
            Assert.That(guid, Is.Not.EqualTo(Guid.Empty));
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
        /// Tests that Evaluate does not call GetCustomClaimSet on the functionality which handles authorization when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallGetCustomClaimSetOnAuthorizationHandlerWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            authorizationPolicy.Evaluate(evaluationContextStub, ref state);

            authorizationHandlerMock.AssertWasNotCalled(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            authorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate returns true when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            var state = CreateLegalState();

            var result = authorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate calls GetCustomClaimSet on the functionality which handles authorization when ClaimSets in the evaluation context is not null.
        /// </summary>
        [Test]
        public void TestThatEvaluateCallsGetCustomClaimSetOnAuthorizationHandlerWhenClaimSetsInEvaluationContextIsNotNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new DefaultClaimSet())
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var claimSets = new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0));
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(claimSets)
                .Repeat.Any();
            var state = CreateLegalState();

            authorizationPolicy.Evaluate(evaluationContextStub, ref state);

            authorizationHandlerMock.AssertWasCalled(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.Equal(claimSets)));
        }

        /// <summary>
        /// Tests that Evaluate throws a FaultException when GetCustomClaimSet on the functionality which handles authorization fails.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenCallsGetCustomClaimSetOnAuthorizationHandlerFails()
        {
            var fixture = new Fixture();

            var error = fixture.Create<Exception>();
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Throw(error)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(evaluationContextStub, ref state));
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
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when GetCustomClaimSet on the functionality which handles authorization returns null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(null)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            authorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate returns true when GetCustomClaimSet on the functionality which handles authorization returns null.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenGetCustomClaimSetOnAuthorizationHandlerReturnsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(null)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = authorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when GetCustomClaimSet on the functionality which handles authorization returns an empty claim set.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsEmptyClaimSet()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new DefaultClaimSet())
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            authorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate returns true when GetCustomClaimSet on the functionality which handles authorization returns an empty claim set.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenGetCustomClaimSetOnAuthorizationHandlerReturnsEmptyClaimSet()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new DefaultClaimSet())
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = authorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate calls AddClaimSet on the evaluation context when GetCustomClaimSet on the functionality which handles authorization returns an claim set with claims.
        /// </summary>
        [Test]
        public void TestThatEvaluateCallsAddClaimSetOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsClaimSetWithClaims()
        {
            var fixture = new Fixture();
            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, fixture.Create<string>()),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, fixture.Create<string>())
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            authorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.AddClaimSet(Arg<AuthorizationPolicy>.Is.TypeOf, Arg<ClaimSet>.Is.Equal(customClaimSet)));
        }

        /// <summary>
        /// Tests that Evaluate throws a FaultException when AddClaimSet on the evaluation context fails.
        /// </summary>
        [Test]
        public void TestThatEvaluateThrowsFaultExceptionWhenCallsAddClaimSetOnEvaluationContextFails()
        {
            var fixture = new Fixture();
            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, fixture.Create<string>()),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, fixture.Create<string>())
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var error = fixture.Create<Exception>();
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.NotNull, Arg<ClaimSet>.Is.NotNull))
                .Throw(error)
                .Repeat.Any();
            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => authorizationPolicy.Evaluate(evaluationContextStub, ref state));
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
        /// Tests that Evaluate returns true when GetCustomClaimSet on the functionality which handles authorization returns an claim set with claims..
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenGetCustomClaimSetOnAuthorizationHandlerReturnsClaimSetWithClaims()
        {
            var fixture = new Fixture();
            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, fixture.Create<string>()),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, fixture.Create<string>())
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var authorizationPolicy = new AuthorizationPolicy(authorizationHandlerMock);
            Assert.That(authorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = authorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
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
