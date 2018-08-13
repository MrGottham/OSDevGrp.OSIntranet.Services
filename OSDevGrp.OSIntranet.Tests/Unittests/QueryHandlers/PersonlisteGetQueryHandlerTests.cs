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
    /// Tester QueryHandler til håndtering af forespørgelsen: PersonlisteGetQuery.
    /// </summary>
    [TestFixture]
    public class PersonlisteGetQueryHandlerTests
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
            var queryHandler = fixture.Create<PersonlisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter en liste af personer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterPersonliste()
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
                m => m.Map<IEnumerable<Person>, IEnumerable<PersonView>>(Arg<IEnumerable<Person>>.Is.NotNull))
                .Return(fixture.CreateMany<PersonView>(15));

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<PersonlisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new PersonlisteGetQuery();
            var personliste = queryHandler.Query(query);
            Assert.That(personliste, Is.Not.Null);
            Assert.That(personliste.Count(), Is.EqualTo(15));

            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            objectMapper.AssertWasCalled(
                m => m.Map<IEnumerable<Person>, IEnumerable<PersonView>>(Arg<IEnumerable<Person>>.Is.NotNull));
        }
    }
}
