using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til kalenderdelen under OSWEBDB.
    /// </summary>
    [TestFixture]
    public class KalenderCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til kalenderdelen under OSWEBDB.
        /// </summary>
        private class MyKalenderCommandHandler : KalenderCommandHandlerBase
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til kalenderdelen under OSWEBDB.
            /// </summary>
            /// <param name="kalenderRepository">Implementering af repository til kalenderdelen under OSWEBDB.</param>
            /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet, såsom systemer under OSWEBDB.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            /// <param name="exceptionBuilder">Implementering af en builder, der kan bygge exceptions.</param>
            public MyKalenderCommandHandler(IKalenderRepository kalenderRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
                : base(kalenderRepository, fællesRepository, objectMapper, exceptionBuilder)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer KalenderCommandHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererKalenderCommandHandlerBase()
        {
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.KalenderRepository, Is.Not.Null);
            Assert.That(commandHandler.KalenderRepository, Is.EqualTo(kalenderRepositoryMock));
            Assert.That(commandHandler.FællesRepository, Is.Not.Null);
            Assert.That(commandHandler.FællesRepository, Is.EqualTo(fællesRepositoryMock));
            Assert.That(commandHandler.ObjectMapper, Is.Not.Null);
            Assert.That(commandHandler.ObjectMapper, Is.EqualTo(objectMapperMock));
            Assert.That(commandHandler.ExceptionBuilder, Is.Not.Null);
            Assert.That(commandHandler.ExceptionBuilder, Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til kalenderdelen under OSWEBDB er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKalenderRepositoryErNull()
        {
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKalenderCommandHandler(null, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kalenderRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til fælles elementer i domænet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFællesRepositoryErNull()
        {
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKalenderCommandHandler(kalenderRepositoryMock, null, objectMapperMock, exceptionBuilderMock));
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
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, null, exceptionBuilderMock));
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
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at SystemGetByNummer henter og returnerer et givent system under OSWEBDB.
        /// </summary>
        [Test]
        public void TestAtSystemGetByNummerHenterSystem()
        {
            var fixture = new Fixture();
            fixture.Customize<ISystem>(e => e.FromFactory(() =>
            {
                var systemMock = MockRepository.GenerateMock<ISystem>();
                systemMock.Stub(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return systemMock;
            }));
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemer = fixture.CreateMany<ISystem>(4).ToList();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.SystemGetAll())
                .Return(systemer)
                .Repeat.Any();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.SystemGetByNummer(systemer.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Nummer, Is.EqualTo(systemer.ElementAt(1).Nummer));

            fællesRepositoryMock.AssertWasCalled(m => m.SystemGetAll());
        }

        /// <summary>
        /// Tester, at SystemGetByNummer kaster en IntranetRepositoryException, hvis systemet ikke findes.
        /// </summary>
        [Test]
        public void TestAtSystemGetByNummerKasterIntranetRepositoryExceptionHvisSystemIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Customize<ISystem>(e => e.FromFactory(() =>
            {
                var systemMock = MockRepository.GenerateMock<ISystem>();
                systemMock.Stub(m => m.Nummer)
                    .Return(fixture.Create<int>())
                    .Repeat.Any();
                return systemMock;
            }));
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemer = fixture.CreateMany<ISystem>(4).ToList();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.SystemGetAll())
                .Return(systemer)
                .Repeat.Any();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var systemNummer = fixture.Create<int>();
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.SystemGetByNummer(systemNummer));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (ISystem).Name, systemNummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());

            fællesRepositoryMock.AssertWasCalled(m => m.SystemGetAll());
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en ArgumentNullException, hvis systemet, hvorfra brugere skal hentes, er null.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterArgumentNullExceptionHvisSystemErNull()
        {
            var fixture = new Fixture();
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.BrugerlisteGetBySystemAndInitialer(null, fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("system"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en ArgumentNullException, initialer for brugerne er null.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterArgumentNullExceptionHvisInitialerErNull()
        {
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.BrugerlisteGetBySystemAndInitialer(MockRepository.GenerateMock<ISystem>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("initialer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en ArgumentNullException, initialer for brugerne er tom.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterArgumentNullExceptionHvisInitialerErEmpty()
        {
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.BrugerlisteGetBySystemAndInitialer(MockRepository.GenerateMock<ISystem>(), string.Empty));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("initialer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer henter en liste af brugere med et givet sæt initialer.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerHenterBrugerlister()
        {
            var fixture = new Fixture();
            fixture.Customize<IBruger>(e => e.FromFactory(() =>
            {
                var brugerMock = MockRepository.GenerateMock<IBruger>();
                brugerMock.Stub(m => m.Initialer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                return brugerMock;
            }));
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemMock = MockRepository.GenerateMock<ISystem>();
            systemMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var brugere = fixture.CreateMany<IBruger>(7).ToList();
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepositoryMock.Stub(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemMock.Nummer)))
                .Return(brugere)
                .Repeat.Any();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var brugerliste = commandHandler.BrugerlisteGetBySystemAndInitialer(systemMock, brugere.ElementAt(1).Initialer);
            Assert.That(brugerliste, Is.Not.Null);
            Assert.That(brugerliste.Count(), Is.EqualTo(1));

            kalenderRepositoryMock.AssertWasCalled(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemMock.Nummer)));
        }

        /// <summary>
        /// Tester, at BrugerlisteGetBySystemAndInitialer kaster en IntranetRepositoryException, hvis der ikke findes brugere med de givne initialer.
        /// </summary>
        [Test]
        public void TestAtBrugerlisteGetBySystemAndInitialerKasterIntranetRepositoryExceptionHvisInitialerIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Customize<IBruger>(e => e.FromFactory(() =>
            {
                var brugerMock = MockRepository.GenerateMock<IBruger>();
                brugerMock.Stub(m => m.Initialer)
                    .Return(fixture.Create<string>())
                    .Repeat.Any();
                return brugerMock;
            }));
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var systemMock = MockRepository.GenerateMock<ISystem>();
            systemMock.Stub(m => m.Nummer)
                .Return(fixture.Create<int>())
                .Repeat.Any();

            var brugere = fixture.CreateMany<IBruger>(7).ToList();
            var kalenderRepositoryMock = MockRepository.GenerateMock<IKalenderRepository>();
            kalenderRepositoryMock.Stub(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemMock.Nummer)))
                .Return(brugere)
                .Repeat.Any();

            var commandHandler = new MyKalenderCommandHandler(kalenderRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var initialer = fixture.Create<string>();
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.BrugerlisteGetBySystemAndInitialer(systemMock, initialer));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.NoCalendarUserWithThoseInitials, initialer)));
            Assert.That(exception.InnerException, Is.Null);

            kalenderRepositoryMock.AssertWasCalled(m => m.BrugerGetAllBySystem(Arg<int>.Is.Equal(systemMock.Nummer)));
        }
    }
}
