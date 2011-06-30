using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til finansstyring.
    /// </summary>
    [TestFixture]
    public class FinansstyringCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til finansstyring.
        /// </summary>
        private class MyCommandHandler : FinansstyringCommandHandlerBase
        {
            /// <summary>
            /// Dannere egen klasse til test af basisklasse for en CommandHandler til finansstyring.
            /// </summary>
            /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
                : base(finansstyringRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer FinansstyringCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringCommandHandlerBase()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var commandHandler = new MyCommandHandler(finansstyringRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.Not.Null);
            Assert.That(commandHandler.ObjectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(null, objectMapper));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var finansstyringRepository = MockRepository.GenerateMock<IFinansstyringRepository>();
            Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepository, null));
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
            var commandHandler = new MyCommandHandler(finansstyringRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var kontogruppe = commandHandler.KontogruppeGetByNummer(kontogrupper.ElementAt(1).Nummer);
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
            var commandHandler = new MyCommandHandler(finansstyringRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => commandHandler.KontogruppeGetByNummer(-1));
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
            var commandHandler = new MyCommandHandler(finansstyringRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var budgetkontogruppe = commandHandler.BudgetkontogruppeGetByNummer(budgetkontogrupper.ElementAt(1).Nummer);
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
            var commandHandler = new MyCommandHandler(finansstyringRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => commandHandler.BudgetkontogruppeGetByNummer(-1));
        }
    }
}
