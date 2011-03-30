using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: BudgetkontogrupperGetQuery.
    /// </summary>
    [TestFixture]
    public class BudgetkontogrupperGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BudgetkontogrupperGetQueryHandler(null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BudgetkontogrupperGetQueryHandler(GetFinansstyringRepository(), null));
        }

        /// <summary>
        /// Tester, at Query henter grupper til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBudgetkontogrupper()
        {
            var queryHandler = new BudgetkontogrupperGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontogrupperGetQuery();
            var budgetkontogrupper = queryHandler.Query(query);
            Assert.That(budgetkontogrupper, Is.Not.Null);
            Assert.That(budgetkontogrupper.Count(), Is.EqualTo(2));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new BudgetkontogrupperGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
