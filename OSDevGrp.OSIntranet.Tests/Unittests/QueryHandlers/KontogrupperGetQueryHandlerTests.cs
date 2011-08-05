using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KontogrupperGetQuery.
    /// </summary>
    [TestFixture]
    public class KontogrupperGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.CreateAnonymous<KontogrupperGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter kontogrupper.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKontogrupper()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.KontogruppeGetAll())
                .Return(fixture.CreateMany<Kontogruppe>(5));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Kontogruppe>, IEnumerable<KontogruppeView>>(Arg<IEnumerable<Kontogruppe>>.Is.NotNull))
                .Return(fixture.CreateMany<KontogruppeView>(5));

            fixture.Inject(finansstyringRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.CreateAnonymous<KontogrupperGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KontogrupperGetQuery();
            var kontogrupper = queryHandler.Query(query);
            Assert.That(kontogrupper, Is.Not.Null);
            Assert.That(kontogrupper.Count(), Is.EqualTo(5));

            finansstyringRepository.AssertWasCalled(m => m.KontogruppeGetAll());
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Kontogruppe>, IEnumerable<KontogruppeView>>(Arg<IEnumerable<Kontogruppe>>.Is.NotNull));
        }
    }
}
