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
    /// Tester QueryHandler til håndtering af forespørgelsen: FirmalisteGetQuery.
    /// </summary>
    [TestFixture]
    public class FirmalisteGetQueryHandlerTests
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
            var queryHandler = fixture.Create<FirmalisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter en liste af firmaer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterFirmaliste()
        {
            var fixture = new Fixture();
            fixture.Inject<IEnumerable<TelefonlisteView>>(fixture.CreateMany<TelefonlisteView>(3).ToList());

            var adresser = new List<AdresseBase>();
            adresser.AddRange(fixture.CreateMany<Person>(15).ToList());
            adresser.AddRange(fixture.CreateMany<Firma>(10).ToList());

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(adresser);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m => m.Map<IEnumerable<Firma>, IEnumerable<FirmaView>>(Arg<IEnumerable<Firma>>.Is.NotNull))
                .Return(fixture.CreateMany<FirmaView>(10));

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<FirmalisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new FirmalisteGetQuery();
            var firmaliste = queryHandler.Query(query);
            Assert.That(firmaliste, Is.Not.Null);
            Assert.That(firmaliste.Count(), Is.EqualTo(10));

            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            objectMapper.AssertWasCalled(
                m => m.Map<IEnumerable<Firma>, IEnumerable<FirmaView>>(Arg<IEnumerable<Firma>>.Is.NotNull));
        }
    }
}
