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
    /// Tester QueryHandler til håndtering af forespørgelsen: BetalingsbetingelserGetQuery.
    /// </summary>
    [TestFixture]
    public class BetalingsbetingelserGetQueryHandlerTests
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
            var queryHandler = fixture.Create<BetalingsbetingelserGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter betalingsbetingelser.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBetalingsbetingelser()
        {
            var fixture = new Fixture();

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.BetalingsbetingelseGetAll())
                .Return(fixture.CreateMany<Betalingsbetingelse>(2));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Betalingsbetingelse>, IEnumerable<BetalingsbetingelseView>>(
                    Arg<IEnumerable<Betalingsbetingelse>>.Is.NotNull))
                .Return(fixture.CreateMany<BetalingsbetingelseView>(2));

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BetalingsbetingelserGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new BetalingsbetingelserGetQuery();
            var betalingsbetingelser = queryHandler.Query(query);
            Assert.That(betalingsbetingelser, Is.Not.Null);
            Assert.That(betalingsbetingelser.Count(), Is.EqualTo(2));

            adresseRepository.AssertWasCalled(m => m.BetalingsbetingelseGetAll());
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Betalingsbetingelse>, IEnumerable<BetalingsbetingelseView>>(
                    Arg<IEnumerable<Betalingsbetingelse>>.Is.NotNull));
        }
    }
}
