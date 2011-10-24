using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KalenderbrugerAftaleGetQuery.
    /// </summary>
    [TestFixture]
    public class KalenderbrugerAftaleGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster en ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<KalenderbrugerAftaleGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter en given kalenderaftale til en kalenderbruger med et givent sæt initialer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKalenderbrugerAftale()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IKalenderRepository>());
            fixture.Inject(MockRepository.GenerateMock<IFællesRepository>());
            fixture.Inject(MockRepository.GenerateMock<IObjectMapper>());

            var queryHandler = fixture.CreateAnonymous<KalenderbrugerAftaleGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KalenderbrugerAftaleGetQuery
                            {
                                System = 0,
                                AftaleId = 0,
                                Initialer = string.Empty
                            };
            var kalenderaftale = queryHandler.Query(query);
            Assert.That(kalenderaftale, Is.Not.Null);
        }
    }
}
