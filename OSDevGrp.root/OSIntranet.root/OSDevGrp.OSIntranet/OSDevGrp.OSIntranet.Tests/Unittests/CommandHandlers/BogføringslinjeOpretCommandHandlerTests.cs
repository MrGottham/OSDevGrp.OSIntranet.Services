using System;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers;
using NUnit.Framework;

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
    }
}
