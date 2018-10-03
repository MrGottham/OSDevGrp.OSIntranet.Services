using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Services;
using AutoFixture;
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
            var host = new ServiceHost(typeof (BasicSecurityTokenService), new[] {uri});
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
        /// Tests that Issue throws an InvalidRequestException when identity on the claims principal is null.
        /// </summary>
        [Test]
        public void TestThatIssueThrowsInvalidRequestExceptionIfIdentityOnClaimsPrincipalIsNull()
        {
            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(null)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(claimPrincipalMock, new RequestSecurityToken()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo((new AuthenticationException()).Message));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<AuthenticationException>());
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when IsAuthenticated from the identity on the claims principal is false.
        /// </summary>
        [Test]
        public void TestThatIssueThrowsInvalidRequestExceptionIfIsAuthenticatedFromIdentityOnClaimsPrincipalIsFalse()
        {
            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(false)
                .Repeat.Any();

            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(claimPrincipalMock, new RequestSecurityToken()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo((new AuthenticationException()).Message));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<AuthenticationException>());
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when AppliesTo in the request security token is null.
        /// </summary>
        [Test]
        public void TestThatIssueThrowsInvalidRequestExceptionIfAppliesToInRequestSecurityTokenIsNull()
        {
            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();

            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = null
            };

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(claimPrincipalMock, request));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when Identity on AppliesTo in the request security token is null.
        /// </summary>
        [Test]
        [TestCase("http://localhost")]
        [TestCase("http://mother")]
        public void TestThatIssueThrowsInvalidRequestExceptionIfIdentityOnAppliesToInRequestSecurityTokenIsNull(string trustedUri)
        {
            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();

            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = new EndpointAddress(new Uri(trustedUri), null, new AddressHeaderCollection())
            };

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(claimPrincipalMock, request));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustHaveX509CertificateEndpointIdentity)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when Identity on AppliesTo in the request security token is not type of X509CertificateEndpointIdentity.
        /// </summary>
        [Test]
        [TestCase("http://localhost")]
        [TestCase("http://mother")]
        public void TestThatIssueThrowsInvalidRequestExceptionIfIdentityOnAppliesToInRequestSecurityTokenIsNotTypeOfX509CertificateEndpointIdentity(string trustedUri)
        {
            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();

            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = new EndpointAddress(new Uri(trustedUri), new DnsEndpointIdentity(string.Empty), new AddressHeaderCollection())
            };

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(claimPrincipalMock, request));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustHaveX509CertificateEndpointIdentity)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Issue throws an InvalidRequestException when AppliesTo in the request security token contains an untrusted relying party.
        /// </summary>
        [Test]
        [TestCase("http://xxx.local", "CN=OSDevGrp.OSIntranet.Tokens")]
        [TestCase("http://yyy.local", "CN=OSDevGrp.OSIntranet.Tokens")]
        [TestCase("http://zzz.local", "CN=OSDevGrp.OSIntranet.Tokens")]
        public void TestThatIssueThrowsInvalidRequestExceptionIfAppliesToInRequestSecurityTokenContainsNoTrustedRelyingParty(string untrustedUri, string identityCertificate)
        {
            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();

            var claimPrincipalMock = MockRepository.GenerateMock<IClaimsPrincipal>();
            claimPrincipalMock.Stub(m => m.Identity)
                .Return(identifyMock)
                .Repeat.Any();

            var basicSecurityTokenService = new BasicSecurityTokenService(new BasicSecurityTokenServiceConfiguration());
            Assert.That(basicSecurityTokenService, Is.Not.Null);

            var request = new RequestSecurityToken
            {
                AppliesTo = new EndpointAddress(new Uri(untrustedUri), new X509CertificateEndpointIdentity(TestHelper.GetCertificate(identityCertificate)), new AddressHeaderCollection())
            };

            var exception = Assert.Throws<InvalidRequestException>(() => basicSecurityTokenService.Issue(claimPrincipalMock, request));
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
        [TestCase("http://localhost", "CN=OSDevGrp.OSIntranet.Tokens")]
        [TestCase("http://services.osdevgrp.local", "CN=OSDevGrp.OSIntranet.Tokens")]
        public void TestThatIssueRetursIfAppliesToInRequestSecurityTokenContainsTrustedRelyingParty(string trustedUri, string identityCertificate)
        {
            var fixture = new Fixture();

            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(true)
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
                AppliesTo = new EndpointAddress(new Uri(trustedUri), new X509CertificateEndpointIdentity(TestHelper.GetCertificate(identityCertificate)), new AddressHeaderCollection())
            };

            var response = basicSecurityTokenService.Issue(claimPrincipalMock, request);
            Assert.That(response, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Issue appends claims to the calling claims principal.
        /// </summary>
        [Test]
        [TestCase("http://localhost", "CN=OSDevGrp.OSIntranet.Tokens", "mrgottham@gmail.com", 3)]
        [TestCase("http://localhost", "CN=OSDevGrp.OSIntranet.Tokens", "ole.sorensen@osdevgrp.dk", 2)]
        [TestCase("http://services.osdevgrp.local", "CN=OSDevGrp.OSIntranet.Tokens", "mrgottham@gmail.com", 3)]
        [TestCase("http://services.osdevgrp.local", "CN=OSDevGrp.OSIntranet.Tokens", "ole.sorensen@osdevgrp.dk", 2)]
        public void TestThatIssueAppendsClaimsToCallingClaimsPrincipal(string trustedUri, string identityCertificate, string mailAddress, int expectedAppendedClaims)
        {
            var fixture = new Fixture();

            var identifyMock = MockRepository.GenerateMock<IIdentity>();
            identifyMock.Expect(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            identifyMock.Expect(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();
            
            var claimsIdentityMock = MockRepository.GenerateMock<IClaimsIdentity>();
            var claimsCollection = new ClaimCollection(claimsIdentityMock)
            {
                new Claim(ClaimTypes.Email, mailAddress)
            };
            claimsIdentityMock.Stub(m => m.Claims)
                .Return(claimsCollection)
                .Repeat.Any();

            var claimsIdentityCollection = new ClaimsIdentityCollection(new List<IClaimsIdentity> {claimsIdentityMock});
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
                AppliesTo = new EndpointAddress(new Uri(trustedUri), new X509CertificateEndpointIdentity(TestHelper.GetCertificate(identityCertificate)), new AddressHeaderCollection())
            };

            var response = basicSecurityTokenService.Issue(claimPrincipalMock, request);
            Assert.That(response, Is.Not.Null);

            Assert.That(claimsCollection.Count, Is.EqualTo(1 + expectedAppendedClaims));
        }
    }
}
