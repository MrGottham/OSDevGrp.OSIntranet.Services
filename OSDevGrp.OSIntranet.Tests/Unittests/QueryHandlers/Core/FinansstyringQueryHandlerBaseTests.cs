﻿using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.QueryHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en QueryHandler til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringQueryHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en QueryHandler til finansstyring.
        /// </summary>
        private class MyFinansstyringQueryHandler : FinansstyringQueryHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en QueryHandler til finansstyring.
            /// </summary>
            /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyFinansstyringQueryHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
                : base(finansstyringRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer FinansstyringQueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringQueryHandlerBase()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFinansstyringQueryHandler(finansstyringRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);
            Assert.That(queryHandler.Repository, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(() => new MyFinansstyringQueryHandler(null, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            Assert.Throws<ArgumentNullException>(() => new MyFinansstyringQueryHandler(finansstyringRepository, null));
        }

        /// <summary>
        /// Tester, at KontogruppeGetByNummer henter en given kontogruppe.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetByNummerHenterKontogruppe()
        {
            var fixture = new Fixture();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.KontogruppeGetAll())
                .Return(kontogrupper);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFinansstyringQueryHandler(finansstyringRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var kontogruppe = queryHandler.KontogruppeGetByNummer(kontogrupper.ElementAt(1).Nummer);
            Assert.That(kontogruppe, Is.Not.Null);
            Assert.That(kontogruppe.Nummer, Is.EqualTo(kontogrupper.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at KontogruppeGetByNummer kaster en IntranetRepositoryException, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetByNummerKasterIntranetRepositoryExceptionHvisKontogruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.KontogruppeGetAll())
                .Return(kontogrupper);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFinansstyringQueryHandler(finansstyringRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.KontogruppeGetByNummer(-1));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetByNummer henter en given gruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetByNummerHenterBudgetkontogruppe()
        {
            var fixture = new Fixture();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.BudgetkontogruppeGetAll())
                .Return(budgetkontogrupper);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFinansstyringQueryHandler(finansstyringRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            var budgetkontogruppe = queryHandler.BudgetkontogruppeGetByNummer(budgetkontogrupper.ElementAt(1).Nummer);
            Assert.That(budgetkontogruppe, Is.Not.Null);
            Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(budgetkontogrupper.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetByNummer kaster en IntranetRepositoryException, hvis gruppen til budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetByNummerKasterIntranetRepositoryExceptionHvisBudgetkontogruppeIkkeFindes()
        {
            var fixture = new Fixture();
            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();

            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepository.Expect(m => m.BudgetkontogruppeGetAll())
                .Return(budgetkontogrupper);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var queryHandler = new MyFinansstyringQueryHandler(finansstyringRepository, objectMapper);
            Assert.That(queryHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => queryHandler.BudgetkontogruppeGetByNummer(-1));
        }
    }
}
