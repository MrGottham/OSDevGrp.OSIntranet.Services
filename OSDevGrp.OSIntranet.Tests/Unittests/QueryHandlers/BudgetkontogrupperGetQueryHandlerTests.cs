using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
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
    /// Tester QueryHandler til håndtering af forespørgelsen: BudgetkontogrupperGetQuery.
    /// </summary>
    [TestFixture]
    public class BudgetkontogrupperGetQueryHandlerTests
    {
        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();

            fixture.Inject(finansstyringRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BudgetkontogrupperGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }

        /// <summary>
        /// Tester, at Query henter grupper til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBudgetkontogrupper()
        {
            var fixture = new Fixture();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.BudgetkontogruppeGetAll())
                .Return(fixture.CreateMany<Budgetkontogruppe>(7));
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            objectMapper.Expect(
                m =>
                m.Map<IEnumerable<Budgetkontogruppe>, IEnumerable<BudgetkontogruppeView>>(
                    Arg<IEnumerable<Budgetkontogruppe>>.Is.NotNull))
                .Return(fixture.CreateMany<BudgetkontogruppeView>(7));

            fixture.Inject(finansstyringRepository);
            fixture.Inject(objectMapper);
            var queryHandler = fixture.Create<BudgetkontogrupperGetQueryHandler>();
            Assert.That(queryHandler, Is.Not.Null);

            var query = new BudgetkontogrupperGetQuery();
            var budgetkontogrupper = queryHandler.Query(query);
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.EqualTo(7));

            finansstyringRepository.AssertWasCalled(m => m.BudgetkontogruppeGetAll());
            objectMapper.AssertWasCalled(
                m =>
                m.Map<IEnumerable<Budgetkontogruppe>, IEnumerable<BudgetkontogruppeView>>(
                    Arg<IEnumerable<Budgetkontogruppe>>.Is.NotNull));
        }
    }
}
