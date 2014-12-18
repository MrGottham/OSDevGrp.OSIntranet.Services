using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: SystemerGetQuery.
    /// </summary>
    [TestFixture]
    public class SystemerGetQueryHandlerTests
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
            var queryHandler = fixture.Create<SystemerGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter systemer under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtQueryHenterSystemer()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<ISystem>());

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.SystemGetAll())
                .Return(fixture.CreateMany<ISystem>(4));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m => m.Map<IEnumerable<ISystem>, IEnumerable<SystemView>>(Arg<IEnumerable<ISystem>>.Is.NotNull))
                .Return(fixture.CreateMany<SystemView>(4));

            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<SystemerGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new SystemerGetQuery();
            var systemer = queryHandler.Query(query);
            Assert.That(systemer, Is.Not.Null);
            Assert.That(systemer.Count(), Is.EqualTo(4));

            fællesRepository.AssertWasCalled(m => m.SystemGetAll());
            objectMapper.AssertWasCalled(
                m => m.Map<IEnumerable<ISystem>, IEnumerable<SystemView>>(Arg<IEnumerable<ISystem>>.Is.NotNull));
        }
    }
}
