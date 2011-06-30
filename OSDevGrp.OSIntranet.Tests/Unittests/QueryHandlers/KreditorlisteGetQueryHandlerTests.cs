﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
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
    /// Tester QueryHandler til håndtering af forespørgelsen: KreditorlisteGetQuery.
    /// </summary>
    [TestFixture]
    public class KreditorlisteGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepositoryet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject<IKonfigurationRepository>(null);
            fixture.Inject(objectMapper);
            Assert.That(
                Assert.Throws<TargetInvocationException>(() => fixture.CreateAnonymous<KreditorlisteGetQueryHandler>()).
                    InnerException, Is.TypeOf(typeof (ArgumentNullException)));
        }

        /// <summary>
        /// Tester, at Query kaster en ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.CreateAnonymous<KreditorlisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter kreditorer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterKreditorer()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2011, 6, 30));
            var debitorer = fixture.CreateMany<Person>(15).ToList();
            debitorer.ForEach(
                m =>
                m.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<decimal>(), 0M)));
            var kreditorer = fixture.CreateMany<Person>(15).ToList();
            kreditorer.ForEach(
                m =>
                m.TilføjBogføringslinje(new Bogføringslinje(fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<DateTime>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(), 0M,
                                                            fixture.CreateAnonymous<decimal>())));
            var personer = new List<Person>();
            personer.AddRange(debitorer);
            personer.AddRange(kreditorer);

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(personer);
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.KreditorSaldoOverNul)
                .Return(false);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<AdresseBase>, IEnumerable<KreditorlisteView>>(Arg<IEnumerable<AdresseBase>>.Is.NotNull))
                .Return(fixture.CreateMany<KreditorlisteView>(kreditorer.Count));

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(konfigurationRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.CreateAnonymous<KreditorlisteGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new KreditorlisteGetQuery
                            {
                                Regnskabsnummer = fixture.CreateAnonymous<int>(),
                                StatusDato = fixture.CreateAnonymous<DateTime>()
                            };
            var result = queryHandler.Query(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(kreditorer.Count));

            konfigurationRepository.AssertWasCalled(m => m.KreditorSaldoOverNul);
        }
    }
}
