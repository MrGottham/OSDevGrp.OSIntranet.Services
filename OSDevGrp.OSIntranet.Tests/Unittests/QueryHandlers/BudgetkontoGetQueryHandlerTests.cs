using System;
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
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: BudgetkontoGetQuery.
    /// </summary>
    [TestFixture]
    public class BudgetkontoGetQueryHandlerTests
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
            var queryHandler = fixture.Create<BudgetkontoGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Test, at Query henter en budgetkonto.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBudgetkonto()
        {
            var fixture = new Fixture();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var budgetknt in fixture.CreateMany<Budgetkonto>(3))
            {
                regnskab.TilføjKonto(budgetknt);
                var tempDate = DateTime.Now.AddMonths(12);
                for (var i = 0; i < 24; i++)
                {
                    budgetknt.TilføjBudgetoplysninger(new Budgetoplysninger(tempDate.Year, tempDate.Month,
                                                                            fixture.Create<decimal>(),
                                                                            fixture.Create<decimal>()));
                    tempDate = tempDate.AddMonths(-1);
                }
                foreach (var bogføringslinje in fixture.CreateMany<Bogføringslinje>(250))
                {
                    budgetknt.TilføjBogføringslinje(bogføringslinje);
                }
            }

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(
                m =>
                m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab);
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepository.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25));
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            fixture.Inject(fixture.CreateMany<BudgetoplysningerView>(24));
            objectMapper.Expect(m => m.Map<Budgetkonto, BudgetkontoView>(Arg<Budgetkonto>.Is.NotNull))
                .Return(fixture.Create<BudgetkontoView>());

            fixture.Inject(finansstyringRepository);
            fixture.Inject(adresseRepository);
            fixture.Inject(fællesRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BudgetkontoGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = regnskab.Nummer,
                                Kontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                                StatusDato = fixture.Create<DateTime>()
                            };
            Assert.That(query, Is.Not.Null);
            var budgetkonto = queryHandler.Query(query);
            Assert.That(budgetkonto, Is.Not.Null);

            finansstyringRepository.AssertWasCalled(
                m =>
                m.RegnskabGet(Arg<int>.Is.Equal(query.Regnskabsnummer), Arg<Func<int, Brevhoved>>.Is.NotNull,
                              Arg<Func<int, AdresseBase>>.Is.NotNull));
            adresseRepository.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepository.AssertWasCalled(m => m.BrevhovedGetAll());
            objectMapper.AssertWasCalled(m => m.Map<Budgetkonto, BudgetkontoView>(Arg<Budgetkonto>.Is.NotNull));
        }
    }
}
