using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Responses;
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
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis CommandBus er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisCommandBusErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FinansstyringService(null));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kalder CommandBus.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKalderCommandBus()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            var service = new FinansstyringService(commandBus);
            var command = new BogføringslinjeOpretCommand();
            service.BogføringslinjeOpret(command);
            commandBus.AssertWasCalled(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetRepositoryFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new IntranetRepositoryException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetRepositoryFault>>(() => service.BogføringslinjeOpret(command));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetBusinessFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new IntranetBusinessException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetBusinessFault>>(() => service.BogføringslinjeOpret(command));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetSystemFault()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new IntranetSystemException("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringslinjeOpret(command));
        }

        /// <summary>
        /// Tester, at BogføringslinjeOpret kaster en IntranetSystemFault ved en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjeOpretKasterIntranetSystemFaultVedUnhandledException()
        {
            var commandBus = MockRepository.GenerateMock<ICommandBus>();
            commandBus.Expect(m => m.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(Arg<BogføringslinjeOpretCommand>.Is.Anything))
                .Throw(new Exception("Test", new Exception("Test")));
            var service = new FinansstyringService(commandBus);
            var command = new BogføringslinjeOpretCommand();
            Assert.Throws<FaultException<IntranetSystemFault>>(() => service.BogføringslinjeOpret(command));
        }
    }
}