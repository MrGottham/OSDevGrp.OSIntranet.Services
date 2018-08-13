using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: KontoGetQuery.
    /// </summary>
    [TestFixture]
    public class KontoGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
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
            var queryHandler = fixture.Create<KontoGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Test, at Query henter en konto.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKonto()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var knt in fixture.CreateMany<Konto>(3))
            {
                regnskab.TilføjKonto(knt);
                var tempDate = DateTime.Now.AddMonths(12);
                for (var i = 0; i < 24; i++)
                {
                    knt.TilføjKreditoplysninger(new Kreditoplysninger(tempDate.Year, tempDate.Month,
                                                                      fixture.Create<decimal>()));
                    tempDate = tempDate.AddMonths(-1);
                }
                foreach (var bogføringslinje in fixture.CreateMany<Bogføringslinje>(250))
                {
                    knt.TilføjBogføringslinje(bogføringslinje);
                }
            }

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            fixture.Inject(fixture.CreateMany<KreditoplysningerView>(24));
            objectMapper.Expect(m => m.Map<Konto, KontoView>(Arg<Konto>.Is.NotNull))
                .Return(fixture.Create<KontoView>());

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<KontoGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KontoGetQuery
                            {
                                Regnskabsnummer = regnskab.Nummer,
                                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                                StatusDato = fixture.Create<DateTime>()
                            };
            Assert.That(query, Is.Not.Null);
            var konto = queryHandler.Query(query);
            Assert.That(konto, Is.Not.Null);

            finansstyringRepository.AssertWasCalled(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(query.Regnskabsnummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull));
            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
            objectMapper.AssertWasCalled(m => m.Map<Konto, KontoView>(Arg<Konto>.Is.NotNull));
        }
    }
}
