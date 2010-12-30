using System;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tester QueryHandler til håndtering af forespørgelsen: BudgetkontoGetQuery.
    /// </summary>
    [TestFixture]
    public class BudgetkontoGetQueryHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BudgetkontoGetQueryHandler(null, null));
        }

        /// <summary>
        /// Test, at konstruktøren kaster en ArgumentNullException, hvis objectmapper er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BudgetkontoGetQueryHandler(GetFinansstyringRepository(), null));
        }

        /// <summary>
        /// Test, at Query henter en budgetkonto.
        /// </summary>
        [Test]
        public void TestAtQueryHenterBudgetkonto()
        {
            var queryHandler = new BudgetkontoGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontoGetQuery()
                            {
                                Regnskabsnummer = 1,
                                Kontonummer = "1000",
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            var budgetkonto = queryHandler.Query(query);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));
        }

        /// <summary>
        /// Test, at Query kaster en IntranetSystemException, hvis kontonummer er null.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetSystemExceptionHvisKontonummerErNull()
        {
            var queryHandler = new BudgetkontoGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                Kontonummer = null,
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            Assert.Throws<IntranetSystemException>(() => queryHandler.Query(query));
        }

        /// <summary>
        /// Test, at Query kaster en IntranetSystemException, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetSystemExceptionHvisBudgetkontoIkkeFindes()
        {
            var queryHandler = new BudgetkontoGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                Kontonummer = "XYZ",
                                StatusDato = new DateTime(2010, 10, 31)
                            };
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(query, Is.Not.Null);
            Assert.Throws<IntranetSystemException>(() => queryHandler.Query(query));
        }

        /// <summary>
        /// Test, at Query kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtQueryKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var queryHandler = new BudgetkontoGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = -1,
                                Kontonummer = "1000",
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
            var queryHandler = new BudgetkontoGetQueryHandler(GetFinansstyringRepository(), GetObjectMapper());
            Assert.That(queryHandler, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => queryHandler.Query(null));
        }
    }
}
