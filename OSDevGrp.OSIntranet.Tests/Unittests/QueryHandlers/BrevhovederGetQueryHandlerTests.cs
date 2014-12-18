using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Tester QueryHandler til håndtering af forespørgelsen: BrevhovderGetQuery.
    /// </summary>
    [TestFixture]
    public class BrevhovederGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BrevhovederGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter brevhoveder.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBrevhoveder()
        {
            var fixture = new Fixture();

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(2));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m => m.Map<IEnumerable<Brevhoved>, IEnumerable<BrevhovedView>>(Arg<IEnumerable<Brevhoved>>.Is.NotNull))
                .Return(fixture.CreateMany<BrevhovedView>(2));

            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BrevhovederGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new BrevhovederGetQuery();
            var brevhoveder = queryHandler.Query(query);
            Assert.That(brevhoveder, Is.Not.Null);
            Assert.That(brevhoveder.Count(), Is.EqualTo(2));

            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
            objectMapper.AssertWasCalled(
                m => m.Map<IEnumerable<Brevhoved>, IEnumerable<BrevhovedView>>(Arg<IEnumerable<Brevhoved>>.Is.NotNull));
        }
    }
}
