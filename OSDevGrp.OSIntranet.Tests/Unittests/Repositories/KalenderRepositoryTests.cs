using System;
using OSDevGrp.OSIntranet.Repositories;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester repository til kalenderaftaler under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class KalenderRepositoryTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data provideren til MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisMySqlDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IMySqlDataProvider>(null);

            Assert.Throws<ArgumentNullException>(
                () => new KalenderRepository(fixture.CreateAnonymous<IMySqlDataProvider>()));
        }
    }
}
