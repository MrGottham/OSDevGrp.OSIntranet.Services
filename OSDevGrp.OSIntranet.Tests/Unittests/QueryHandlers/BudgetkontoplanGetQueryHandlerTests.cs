using System;
using System.Linq;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: BudgetplanGetQuery.
    /// </summary>
    [TestFixture]
    public class BudgetplanGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BudgetkontoplanGetQueryHandler(null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapper er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BudgetkontoplanGetQueryHandler(GetFinansstyringRepository(), null));
        }

        /// <summary>
        /// Test, at Query henter budgetplan.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBudgetplan()
        {
            var queryHandler = new BudgetkontoplanGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontoplanGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            var budgetplan = queryHandler.Query(query);
            Assert.That(budgetplan, Is.Not.Null);
            Assert.That(budgetplan.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Test, at Query kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var queryHandler = new BudgetkontoplanGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontoplanGetQuery
                            {
                                Regnskabsnummer = -1,
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            Assert.Throws<IntranetRepositoryException>(() => queryHandler.Query(query));
        }

        /// <summary>
        /// Tester, at Query kaster ArgumentNullException, hvis Query er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterArgumentNullExceptionHvisQueryErNull()
        {
            var queryHandler = new BudgetkontoplanGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            Assert.That(queryHandler, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
