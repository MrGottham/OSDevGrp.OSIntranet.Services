using System;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tester CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandHandlerTests : FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository for finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(null, null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository for adressekartoteket er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), null, null));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), null));
        }

        /// <summary>
        /// Tester, at Execute kaster en ArgumentNullException, hvis kommandoen er null.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterArgumentNullExceptionHvisCommandErNull()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            Assert.Throws<ArgumentNullException>(() => commandHandler.Execute(null));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = -1,
                                  Dato = new DateTime(2011, 4, 1),
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
                              };
            Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis bogføringsdato er for gammel.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisBogføringsdatoErForGammel()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepository.Expect(m => m.DageForBogføringsperiode).Return(30);
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = DateTime.Now.AddDays(-31),
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 1000M,
                                  Kredit = 0M,
                                  Adressekonto = 1
                              };
            Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
        }


        /// <summary>
        /// Tester, at HandleException kaster IntranetSystemException.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKasterIntranetSystemException()
        {
            var konfigurationRepository = MockRepository.GenerateMock<IKonfigurationRepository>();
            var commandHandler = new BogføringslinjeOpretCommandHandler(GetFinansstyringRepository(), GetAdresseRepository(), konfigurationRepository);
            Assert.Throws<IntranetSystemException>(() => commandHandler.HandleException(new BogføringslinjeOpretCommand(), new Exception("Test")));
        }
    }
}
