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
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: AdressekontoGetQuery.
    /// </summary>
    [TestFixture]
    public class AdressekontoGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster en ArgumentNullException, hvis query er null.
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
            var queryHandler = fixture.Create<AdressekontoGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter adressekonti.
        /// </summary>
        [Test]
        public void TestAtQueryHenterAdressekonti()
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
            objectMapper.Expect(m => m.Map<AdresseBase, AdressekontoView>(Arg<AdresseBase>.Is.NotNull))
                .Return(fixture.Create<AdressekontoView>());

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<AdressekontoGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new AdressekontoGetQuery
                            {
                                Regnskabsnummer = fixture.Create<int>(),
                                StatusDato = fixture.Create<DateTime>(),
                                Nummer = personer.ElementAt(3).Nummer
                            };
            var adressekonto = queryHandler.Query(query);
            Assert.That(adressekonto, Is.Not.Null);

            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
            objectMapper.AssertWasCalled(m => m.Map<AdresseBase, AdressekontoView>(Arg<AdresseBase>.Is.NotNull));
        }
    }
}
