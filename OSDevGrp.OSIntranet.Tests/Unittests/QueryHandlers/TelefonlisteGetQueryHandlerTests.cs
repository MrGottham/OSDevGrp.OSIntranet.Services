using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
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
    /// Tester QueryHandler til håndtering af forespørgelsen: TelefonlisteGetQuery.
    /// </summary>
    [TestFixture]
    public class TelefonlisteGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<TelefonlisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter en telefonliste.
        /// </summary>
        [Test]
        public void TestAtQueryHenterTelefonliste()
        {
            var fixture = new Fixture();

            var adresser = new List<AdresseBase>();
            adresser.AddRange(fixture.CreateMany<Person>(15).ToList());
            adresser.AddRange(fixture.CreateMany<Firma>(10).ToList());

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<AdresseBase>, IEnumerable<TelefonlisteView>>(Arg<IEnumerable<AdresseBase>>.Is.NotNull))
                .Return(fixture.CreateMany<TelefonlisteView>(25));

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<TelefonlisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new TelefonlisteGetQuery();
            var telefonliste = queryHandler.Query(query);
            Assert.That(telefonliste, Is.Not.Null);
            Assert.That(telefonliste.Count(), Is.EqualTo(25));

            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<AdresseBase>, IEnumerable<TelefonlisteView>>(Arg<IEnumerable<AdresseBase>>.Is.NotNull));
        }
    }
}
