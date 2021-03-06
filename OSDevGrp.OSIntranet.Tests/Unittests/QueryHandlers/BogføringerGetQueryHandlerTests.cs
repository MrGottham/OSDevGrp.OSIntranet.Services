﻿using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
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
    /// Tester QueryHandler til håndtering af forespørgelsen: BogføringerGetQuery.
    /// </summary>
    [TestFixture]
    public class BogføringerGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BogføringerGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Test, at Query henter 3 bogføringslinjer.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBogføringslinjer()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var statusDate = fixture.Create<DateTime>();
            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(3))
            {
                regnskab.TilføjKonto(konto);
                while (konto.Bogføringslinjer.Count() < 25)
                {
                    var bogføringslinje = new Bogføringslinje(fixture.Create<int>(), statusDate.AddDays(random.Next(0, 365) * -1), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), fixture.Create<decimal>());
                    konto.TilføjBogføringslinje(bogføringslinje);
                }
            }
            
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(m => m.Map<Bogføringslinje, BogføringslinjeView>(Arg<Bogføringslinje>.Is.NotNull))
                .Return(fixture.Create<BogføringslinjeView>())
                .Repeat.Any();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BogføringerGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new BogføringerGetQuery
                            {
                                Regnskabsnummer = regnskab.Nummer,
                                StatusDato = statusDate,
                                Linjer = 30
                            };
            var bogføringslinjer = queryHandler.Query(query);
            Assert.That(bogføringslinjer, Is.Not.Null);
            Assert.That(bogføringslinjer.Count(), Is.EqualTo(query.Linjer));

            finansstyringRepository.AssertWasCalled(m => m.RegnskabGet(Arg<int>.Is.Equal(query.Regnskabsnummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull));
            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
        }
    }
}
