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
    /// Tester QueryHandler til håndtering af forespørgelsen: PostnumreGetQuery.
    /// </summary>
    [TestFixture]
    public class PostnumreGetQueryHandlerTests
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
            var queryHandler = fixture.Create<PostnumreGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter postnumre.
        /// </summary>
        [Test]
        public void TestAtQueryHenterAdressegrupper()
        {
            var fixture = new Fixture();

            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.PostnummerGetAll())
                .Return(fixture.CreateMany<Postnummer>(750));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Postnummer>, IEnumerable<PostnummerView>>(Arg<IEnumerable<Postnummer>>.Is.NotNull))
                .Return(fixture.CreateMany<PostnummerView>(750));

            fixture.Inject(adresseRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<PostnumreGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new PostnumreGetQuery();
            var postnumre = queryHandler.Query(query);
            Assert.That(postnumre, Is.Not.Null);
            Assert.That(postnumre.Count(), Is.EqualTo(750));

            adresseRepository.AssertWasCalled(m => m.PostnummerGetAll());
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Postnummer>, IEnumerable<PostnummerView>>(Arg<IEnumerable<Postnummer>>.Is.NotNull));
        }
    }
}
