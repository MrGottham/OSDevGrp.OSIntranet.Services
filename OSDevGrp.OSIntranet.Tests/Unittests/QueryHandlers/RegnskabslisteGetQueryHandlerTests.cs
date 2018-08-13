using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: RegnskabslisteGetQuery.
    /// </summary>
    [TestFixture]
    public class RegnskabslisteGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til fælles elementer i domænet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFællesRepositoryErNull()
        {
            IFinansstyringRepository finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            IObjectMapper objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new RegnskabslisteGetQueryHandler(finansstyringRepository, null, objectMapper));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "fællesRepository");
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<RegnskabslisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter regnskaber.
        /// </summary>
        [Test]
        public void TestAtQueryHenterRegnskaber()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.RegnskabslisteGet(Arg<Func<int, Brevhoved>>.Is.NotNull))
                .Return(fixture.CreateMany<Regnskab>(3));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Regnskab>, IEnumerable<RegnskabslisteView>>(Arg<IEnumerable<Regnskab>>.Is.NotNull))
                .Return(fixture.CreateMany<RegnskabslisteView>(3));

            fixture.Inject(finansstyringRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<RegnskabslisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new RegnskabslisteGetQuery();
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            var regnskaber = queryHandler.Query(query);
            Assert.That(regnskaber, Is.Not.Null);
            Assert.That(regnskaber.Count(), Is.EqualTo(3));

            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
            finansstyringRepository.AssertWasCalled(m => m.RegnskabslisteGet(Arg<Func<int, Brevhoved>>.Is.NotNull));
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Regnskab>, IEnumerable<RegnskabslisteView>>(Arg<IEnumerable<Regnskab>>.Is.NotNull));
        }
    }
}
