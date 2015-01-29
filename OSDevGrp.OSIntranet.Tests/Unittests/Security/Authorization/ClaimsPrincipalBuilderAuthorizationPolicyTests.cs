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
using ClaimsPrincipal=Microsoft.IdentityModel.Claims.ClaimsPrincipal;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Authorization
{
    /// <summary>
    /// Tests the authorization policy which can be used by WCF services using claims based security.
    /// </summary>
    [TestFixture]
    public class ClaimsAuthorizationPolicyTests
    {
        /// <summary>
        /// Tests that the constructor initialize an authorization policy which can be used by WCF services using claims based security.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeClaimsAuthorizationPolicy()
        {
            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy();
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);
            Assert.That(claimsAuthorizationPolicy.Id, Is.Not.Null);
            Assert.That(claimsAuthorizationPolicy.Id, Is.Not.Empty);
            Assert.That(claimsAuthorizationPolicy.Issuer, Is.Not.Null);
            Assert.That(claimsAuthorizationPolicy.Issuer, Is.EqualTo(ClaimSet.System));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the functionality which can handle the authorization is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenAuthorizationHandlerIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClaimsAuthorizationPolicy(null));
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var guid = Guid.Parse(claimsAuthorizationPolicy.Id);
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(fixture.Create<IAuthorizationHandler>());
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var state = CreateLegalState();

            // ReSharper disable AssignNullToNotNullAttribute
            var exception = Assert.Throws<ArgumentNullException>(() => claimsAuthorizationPolicy.Evaluate(null, ref state));
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            authorizationHandlerMock.AssertWasNotCalled(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate does not call AddClaimSet on the evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateDoesNotCallAddClaimSetOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate sets Principal on evaluation context when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalOnEvaluationContextWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
        }

        /// <summary>
        /// Tests that Evaluate returns true when ClaimSets in the evaluation context is null.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenClaimSetsInEvaluationContextIsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(null)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var claimSets = new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0));
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(claimSets)
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state));
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate sets Principal on evaluation context when GetCustomClaimSet on the functionality which handles authorization returns null.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsNull()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(null)
                .Repeat.Any();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasNotCalled(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.Anything, Arg<ClaimSet>.Is.Anything));
        }

        /// <summary>
        /// Tests that Evaluate sets Principal on evaluation context when GetCustomClaimSet on the functionality which handles authorization returns an empty claim set.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsEmptyClaimSet()
        {
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(new DefaultClaimSet())
                .Repeat.Any();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
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

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that Evaluate calls AddClaimSet on the evaluation context when GetCustomClaimSet on the functionality which handles authorization returns an claim set with claims.
        /// </summary>
        [Test]
        public void TestThatEvaluateCallsAddClaimSetOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsClaimSetWithClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, Rights.PossessProperty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, Rights.PossessProperty)
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.AddClaimSet(Arg<ClaimsAuthorizationPolicy>.Is.TypeOf, Arg<ClaimSet>.Is.Equal(customClaimSet)));
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
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, Rights.PossessProperty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, Rights.PossessProperty)
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var error = fixture.Create<Exception>();
            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.AddClaimSet(Arg<IAuthorizationPolicy>.Is.NotNull, Arg<ClaimSet>.Is.NotNull))
                .Throw(error)
                .Repeat.Any();
            var state = CreateLegalState();

            var exception = Assert.Throws<FaultException>(() => claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state));
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
        /// Tests that Evaluate sets Principal on evaluation context when GetCustomClaimSet on the functionality which handles authorization returns an claim set with claims.
        /// </summary>
        [Test]
        public void TestThatEvaluateSetPrincipalOnEvaluationContextWhenGetCustomClaimSetOnAuthorizationHandlerReturnsClaimSetWithClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, Rights.PossessProperty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, Rights.PossessProperty)
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);

            evaluationContextStub.AssertWasCalled(m => m.Properties[Arg<string>.Is.Equal("Principal")] = Arg<ClaimsPrincipal>.Is.TypeOf);
        }

        /// <summary>
        /// Tests that Evaluate returns true when GetCustomClaimSet on the functionality which handles authorization returns an claim set with claims.
        /// </summary>
        [Test]
        public void TestThatEvaluateReturnsTrueWhenGetCustomClaimSetOnAuthorizationHandlerReturnsClaimSetWithClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(FoodWasteClaimTypes.SystemManagement, string.Empty, Rights.PossessProperty),
                new Claim(FoodWasteClaimTypes.ValidatedUser, string.Empty, Rights.PossessProperty)
            };
            var customClaimSet = new DefaultClaimSet(claims);
            var authorizationHandlerMock = MockRepository.GenerateMock<IAuthorizationHandler>();
            authorizationHandlerMock.Stub(m => m.GetCustomClaimSet(Arg<IEnumerable<ClaimSet>>.Is.NotNull))
                .Return(customClaimSet)
                .Repeat.Any();

            var claimsAuthorizationPolicy = new ClaimsAuthorizationPolicy(authorizationHandlerMock);
            Assert.That(claimsAuthorizationPolicy, Is.Not.Null);

            var evaluationContextStub = MockRepository.GenerateStub<EvaluationContext>();
            evaluationContextStub.Stub(m => m.ClaimSets)
                .Return(new ReadOnlyCollection<ClaimSet>(new List<ClaimSet>(0)))
                .Repeat.Any();
            evaluationContextStub.Stub(m => m.Properties)
                .Return(new Dictionary<string, object>(0))
                .Repeat.Any();
            var state = CreateLegalState();

            var result = claimsAuthorizationPolicy.Evaluate(evaluationContextStub, ref state);
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
