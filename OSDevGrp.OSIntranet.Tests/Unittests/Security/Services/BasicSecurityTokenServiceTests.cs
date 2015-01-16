using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Services;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Services
{
    /// <summary>
    /// Tests the basic security token service.
    /// </summary>
    [TestFixture]
    public class BasicSecurityTokenServiceTests
    {
        /// <summary>
        /// Tests that the basic security token service can be hosted.
        /// </summary>
        [Test]
        public void TestThatBasicSecurityTokenServiceCanBeHosted()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof(BasicSecurityTokenService), new[] { uri });
            try
            {
                host.Open();
                Assert.That(host.State, Is.EqualTo(CommunicationState.Opened));
            }
            finally
            {
                ChannelTools.CloseChannel(host);
            }
        }

        /// <summary>
        /// Tests that the constructor can initialize the basic security token service.
        /// </summary>
        [Test]
        public void TestThatConstructorCanInitializeBasicSecurityTokenService()
        {
            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when the cliams principal is null.
        /// </summary>
        [Test]
        public void TestThatIssueThrowsInvalidRequestExceptionIfClaimsPrincipalIsNull()
        {
            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(null, new RequestSecurityToken()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            // ReSharper disable NotResolvedInText
            Assert.That(exception.Message, Is.EqualTo((new ArgumentNullException("principal")).Message));
            // ReSharper restore NotResolvedInText
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<ArgumentNullException>());
            Assert.That(((ArgumentNullException) exception.InnerException).ParamName, Is.Not.Null);
            Assert.That(((ArgumentNullException) exception.InnerException).ParamName, Is.Not.Empty);
            Assert.That(((ArgumentNullException) exception.InnerException).ParamName, Is.EqualTo("principal"));
            Assert.That(exception.InnerException.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue throws an ArgumentNullException when the request security token is null.
        /// </summary>
        [Test]
        public void TestThatIssueThrowsArgumentNullExceptionIfRequestSecurityTokenIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IClaimsPrincipal>(e => e.FromFactory(() => MockRepository.GenerateMock<IClaimsPrincipal>()));

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => basicSecurityTokenService.Issue(fixture.Create<IClaimsPrincipal>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("request"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when AppliesTo in the request security token is null.
        /// </summary>
        [Test]
        public void TestThatIssueThrowsInvalidRequestExceptionIfAppliesToInRequestSecurityTokenIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IClaimsPrincipal>(e => e.FromFactory(() => MockRepository.GenerateMock<IClaimsPrincipal>()));

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = null
            };

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(fixture.Create<IClaimsPrincipal>(), request));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when AppliesTo in the request security token contains an untrusted relying party.
        /// </summary>
        [Test]
        [TestCase("http://xxx.local")]
        [TestCase("http://yyy.local")]
        [TestCase("http://zzz.local")]
        public void TestThatIssueThrowsInvalidRequestExceptionIfAppliesToInRequestSecurityTokenContainsNoTrustedRelyingParty(string untrustedUri)
        {
            var fixture = new Fixture();
            fixture.Customize<IClaimsPrincipal>(e => e.FromFactory(() => MockRepository.GenerateMock<IClaimsPrincipal>()));

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = new EndpointAddress(new Uri(untrustedUri))
            };

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(fixture.Create<IClaimsPrincipal>(), request));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress, new Uri(untrustedUri))));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue returns when AppliesTo in the request security token contains a trusted relying party.
        /// </summary>
        [Test]
        [TestCase("http://localhost")]
        [TestCase("http://mother")]
        public void TestThatIssueRetursIfAppliesToInRequestSecurityTokenContainsTrustedRelyingParty(string trustedUri)
        {
            var fixture = new Fixture();

            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();
            claimPrincipalMock.Stub(m => m.Identities)
                .Return(null)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = new EndpointAddress(new Uri(trustedUri))
            };

            var response = basicSecurityTokenService.Issue(claimPrincipalMock, request);
            Assert.That(response, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Issue returns a claims identity with appended claims.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com", 2)]
        [TestCase("ole.sorensen@osdevgrp.dk", 1)]
        public void TestThatIssueRetursClaimsIdentityWithAppendedClaims(string mailAddress, int expectedAppendedClaims)
        {
            var fixture = new Fixture();

            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            
            var claimsIdentity = MockRepository.GenerateMock<IClaimsIdentity>();
            var claimsCollection = new ClaimCollection(claimsIdentity)
            {
                new Claim(ClaimTypes.Email, mailAddress)
            };
            claimsIdentity.Stub(m => m.Claims)
                .Return(claimsCollection)
                .Repeat.Any();

            var claimsIdentityCollection = new ClaimsIdentityCollection(new List<IClaimsIdentity> {claimsIdentity});
            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();
            claimPrincipalMock.Stub(m => m.Identities)
                .Return(claimsIdentityCollection)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = new EndpointAddress(new Uri("http://localhost"))
            };

            var response = basicSecurityTokenService.Issue(claimPrincipalMock, request);
            Assert.That(response, Is.Not.Null);

            Assert.That(claimsCollection.Count, Is.EqualTo(1 + expectedAppendedClaims));
        }
    }
}
