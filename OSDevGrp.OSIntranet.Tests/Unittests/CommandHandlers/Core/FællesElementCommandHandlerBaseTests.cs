using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
    /// </summary>
    [TestFixture]
    public class FællesElementCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
        /// </summary>
        private class MyCommandHandler : FællesElementCommandHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
            /// </summary>
            /// <param name="fællesRepository">Implementering af repository til finansstyring.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyCommandHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper)
                : base(fællesRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer FællesElementCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFællesElementCommandHandlerBase()
        {
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var commandHandler = new MyCommandHandler(fællesRepository, objectMapper);
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
            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(fællesRepository, null));
        }

        /// <summary>
        /// Tester, at BrevhovedGetByNummer henter en given brevhoved.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetByNummerHenterBrevhoved()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(brevhoveder);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var commandHandler = new MyCommandHandler(fællesRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            var brevhoved = commandHandler.BrevhovedGetByNummer(brevhoveder.ElementAt(1).Nummer);
            Assert.That(brevhoved, Is.Not.Null);
            Assert.That(brevhoved.Nummer, Is.EqualTo(brevhoveder.ElementAt(1).Nummer));
        }

        /// <summary>
        /// Tester, at BrevhovedGetByNummer kaster en IntranetRepositoryException, hvis brevhoved ikke findes.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetByNummerKasterIntranetRepositoryExceptionHvisBrevhovedIkkeFindes()
        {
            var fixture = new Fixture();
            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();

            var fællesRepository = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepository.Expect(m => m.BrevhovedGetAll())
                .Return(brevhoveder);
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var commandHandler = new MyCommandHandler(fællesRepository, objectMapper);
            Assert.That(commandHandler, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(() => commandHandler.BrevhovedGetByNummer(-1));
        }
    }
}
