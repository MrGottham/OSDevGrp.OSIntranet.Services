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
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: AdressegrupperGetQuery.
    /// </summary>
    [TestFixture]
    public class AdressegrupperGetQueryHandlerTests
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
            var queryHandler = fixture.Create<AdressegrupperGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter adressegrupper.
        /// </summary>
        [Test]
        public void TestAtQueryHenterAdressegrupper()
        {
            var fixture = new Fixture();

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdressegruppeGetAll())
                .Return(fixture.CreateMany<Adressegruppe>(5));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Adressegruppe>, IEnumerable<AdressegruppeView>>(
                    Arg<IEnumerable<Adressegruppe>>.Is.NotNull))
                .Return(fixture.CreateMany<AdressegruppeView>(5));

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<AdressegrupperGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new AdressegrupperGetQuery();
            var adressegrupper = queryHandler.Query(query);
            Assert.That(adressegrupper, Is.Not.Null);
            Assert.That(adressegrupper.Count(), Is.EqualTo(5));

            adresseRepository.AssertWasCalled(m => m.AdressegruppeGetAll());
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Adressegruppe>, IEnumerable<AdressegruppeView>>(
                    Arg<IEnumerable<Adressegruppe>>.Is.NotNull));
        }
    }
}
