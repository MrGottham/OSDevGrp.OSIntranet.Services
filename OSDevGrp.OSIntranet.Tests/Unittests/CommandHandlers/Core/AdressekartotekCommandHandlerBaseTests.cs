using System;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til adressekartoteket.
    /// </summary>
    [TestFixture]
    public class AdressekartotekCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til adressekartoteket.
        /// </summary>
        private class MyCommandHandler : AdressekartotekCommandHandlerBase
        {
            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til adressekartoteket.
            /// </summary>
            /// <param name="adresseRepository">Implementering af repository til adresser.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            public MyCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
                : base(adresseRepository, objectMapper)
            {
            }
        }

        /// <summary>
        /// Tester, at konstruktøren initierer AdressekartotekCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererAdressekartotekCommandHandlerBase()
        {
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapper = MockRepository.GenerateMock<IObjectMapper>();
            var commandHandler = new MyCommandHandler(adresseRepository, objectMapper);
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
            var adresseRepository = MockRepository.GenerateMock<IAdresseRepository>();
            Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(adresseRepository, null));
        }
    }
}
