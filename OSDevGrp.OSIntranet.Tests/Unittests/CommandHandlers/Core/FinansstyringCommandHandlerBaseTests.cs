using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
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
            /// <param name="exceptionBuilder">Implementering af builderen, der kan bygge exceptions.</param>
            public MyCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
                : base(finansstyringRepository, objectMapper, exceptionBuilder)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer FinansstyringCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFinansstyringCommandHandlerBase()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.EqualTo(finansstyringRepositoryMock));
            Assert.That(commandHandler.ObjectMapper, Is.Not.Null);
            Assert.That(commandHandler.ObjectMapper, Is.EqualTo(objectMapperMock));
            Assert.That(commandHandler.ExceptionBuilder, Is.Not.Null);
            Assert.That(commandHandler.ExceptionBuilder, Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(null, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepositoryMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("objectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionBuilderErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepositoryMock, objectMapperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KontogruppeGetByNummer henter en given kontogruppe.
        /// </summary>
        [Test]
        public void TestAtKontogruppeGetByNummerHenterKontogruppe()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontogruppeGetAll())
                .Return(kontogrupper)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, objectMapperMock, exceptionBuilderMock);
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var kontogrupper = fixture.CreateMany<Kontogruppe>(3).ToList();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.KontogruppeGetAll())
                .Return(kontogrupper)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            const int kontogruppeNummer = -1;
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.KontogruppeGetByNummer(kontogruppeNummer));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Kontogruppe).Name, kontogruppeNummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at BudgetkontogruppeGetByNummer henter en given gruppe til budgetkonti.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeGetByNummerHenterBudgetkontogruppe()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontogruppeGetAll())
                .Return(budgetkontogrupper)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, objectMapperMock, exceptionBuilderMock);
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var budgetkontogrupper = fixture.CreateMany<Budgetkontogruppe>(3).ToList();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.BudgetkontogruppeGetAll())
                .Return(budgetkontogrupper)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            const int budgetkontogruppeNummer = -1;
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.BudgetkontogruppeGetByNummer(budgetkontogruppeNummer));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message,Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkontogruppe).Name, budgetkontogruppeNummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
