using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;
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
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til fælles elementer i domænet, såsom brevhoveder.
            /// </summary>
            /// <param name="fællesRepository">Implementering af repository til finansstyring.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            /// <param name="exceptionBuilder">Implementering af en builder, der kan bygge exceptions.</param>
            public MyCommandHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
                : base(fællesRepository, objectMapper, exceptionBuilder)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer FællesElementCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererFællesElementCommandHandlerBase()
        {
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            
            var commandHandler = new MyCommandHandler(fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.EqualTo(fællesRepositoryMock));
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
            Assert.That(exception.ParamName, Is.EqualTo("fællesRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objectmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(fællesRepositoryMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("objectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis builderen, der kan bygge exceptions, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionBuilderErNull()
        {
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(fællesRepositoryMock, objectMapperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BrevhovedGetByNummer henter en given brevhoved.
        /// </summary>
        [Test]
        public void TestAtBrevhovedGetByNummerHenterBrevhoved()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(brevhoveder)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
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
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var brevhoveder = fixture.CreateMany<Brevhoved>(3).ToList();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(brevhoveder)
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.BrevhovedGetByNummer(-1));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Brevhoved).Name, -1)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
