using System;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tester CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(null, null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository for adressekartoteket er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), null));
        }

        /// <summary>
        /// Tester, at HandleException kaster IntranetSystemException.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKasterIntranetSystemException()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            Assert.Throws<IntranetSystemException>(() => commandHandler.HandleException(new BogføringslinjeOpretCommand(), new Exception("Test")));
        }
    }
}
