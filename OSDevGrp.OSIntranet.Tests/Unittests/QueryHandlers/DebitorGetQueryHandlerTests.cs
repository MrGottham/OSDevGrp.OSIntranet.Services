using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
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
    /// Tester QueryHandler til håndtering af forespørgelsen: DebitorGetQueryHandler.
    /// </summary>
    [TestFixture]
    public class DebitorGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster en ArgumentNullExcpetion, hvis query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.CreateAnonymous<DebitorGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter en debitor.
        /// </summary>
        [Test]
        public void TestAtQueryHenterDebitor()
        {
            var fixture = new Fixture();
            var personer = fixture.CreateMany<Person>(25).ToList();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(personer);
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(m => m.Map<AdresseBase, DebitorView>(Arg<AdresseBase>.Is.NotNull))
                .Return(fixture.CreateAnonymous<DebitorView>());

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.CreateAnonymous<DebitorGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new DebitorGetQuery
                            {
                                Regnskabsnummer = fixture.CreateAnonymous<int>(),
                                StatusDato = fixture.CreateAnonymous<DateTime>(),
                                Nummer = personer.ElementAt(3).Nummer
                            };
            var debitor = queryHandler.Query(query);
            Assert.That(debitor, Is.Not.Null);
        }
    }
}
