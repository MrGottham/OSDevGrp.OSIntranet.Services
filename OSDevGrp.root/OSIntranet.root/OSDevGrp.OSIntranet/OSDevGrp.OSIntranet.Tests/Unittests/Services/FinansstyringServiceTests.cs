using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Services.Implementations;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Tester service til finansstyring.
    /// </summary>
    public class FinansstyringServiceTests
    {
        /// <summary>
        /// Tester, at service til finansstyring kan hostes.
        /// </summary>
        [Test]
        public void TestAtFinansstyringServiceKanHostes()
        {
            var uri = new Uri("http://localhost:7000/OSIntranet/");
            var host = new ServiceHost(typeof (FinansstyringService), new [] {uri});
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
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis QueryBus er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisQueryBusErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringService(null));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kalder QueryBus.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKalderQueryBus()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            service.RegnskabslisteGet(query);
            queryBus.AssertWasCalled(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetRepositoryFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetBusinessFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetSystemFault()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.RegnskabslisteGet(query));
        }

        /// <summary>
        /// Tester, at RegnskabslisteGet kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtRegnskabslisteGetKasterIntranetSystemFaultVedUnhandledException()
        {
            var queryBus = MockRepository.GenerateMock<IQueryBus>();
            queryBus.Expect(
                m =>
                m.Query<RegnskabslisteGetQuery, IEnumerable<RegnskabslisteView>>(Arg<RegnskabslisteGetQuery>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(queryBus);
            var query = new RegnskabslisteGetQuery();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.RegnskabslisteGet(query));
        }
    }
}
