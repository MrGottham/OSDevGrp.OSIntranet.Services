using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers.Core
{
    /// <summary>
    /// Tester basisklasse for en CommandHandler til regnskaber.
    /// </summary>
    [TestFixture]
    public class RegnskabCommandHandlerBaseTests
    {
        /// <summary>
        /// Egen klasse til test af basisklasse for en CommandHandler til regnskaber. 
        /// </summary>
        private class MyCommandHandler : RegnskabCommandHandlerBase
        {
            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af basisklasse for en CommandHandler til regnskaber. 
            /// </summary>
            /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
            /// <param name="adresseRepository">Implementering af repository til adresser.</param>
            /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
            /// <param name="objectMapper">Implementering af objectmapper.</param>
            /// <param name="exceptionBuilder">Implementering af builderen, der kan bygge exceptions.</param>
            public MyCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
                : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper, exceptionBuilder)
            {
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer RegnskabQueryHandlerBase.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererRegnskabQueryHandlerBase()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.Not.Null);
            Assert.That(commandHandler.Repository, Is.EqualTo(finansstyringRepositoryMock));
            Assert.That(commandHandler.ObjectMapper, Is.Not.Null);
            Assert.That(commandHandler.ObjectMapper, Is.EqualTo(objectMapperMock));
            Assert.That(commandHandler.ExceptionBuilder, Is.Not.Null);
            Assert.That(commandHandler.ExceptionBuilder, Is.EqualTo(exceptionBuilderMock));
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til finansstyring er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFinansstyringRepositoryErNull()
        {
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(null, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("finansstyringRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til adresser er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisAdresseRepositoryErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepositoryMock, null, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("adresseRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis repository til fælles elementer i domænet er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisFællesRepositoryErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, null, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("fællesRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis objektmapperen er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisObjectMapperErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, null, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("objectMapper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis builder, der kan bygge exceptions, er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisExceptionBuilderErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();

            var exception = Assert.Throws<ArgumentNullException>(() => new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionBuilder"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at RegnskabGetByNummer henter et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtRegnskabGetByNummerHenterRegnskab()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var regnskab = commandHandler.RegnskabGetByNummer(regnskaber.ElementAt(1).Nummer);
            Assert.That(regnskab, Is.Not.Null);
            Assert.That(regnskab.Nummer, Is.EqualTo(regnskaber.ElementAt(1).Nummer));
            Assert.That(regnskab.Navn, Is.Not.Null);
            Assert.That(regnskab.Navn, Is.EqualTo(regnskaber.ElementAt(1).Navn));

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull));
            adresseRepositoryMock.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepositoryMock.AssertWasCalled(m => m.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at KontoGetAllByRegnskab henter alle konti i et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKontoGetAllByRegnskabHenterKontiFraRegnskab()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.KontoGetAllByRegnskab(regnskaber.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at KontoGetByRegnskabAndKontonummer kaster en ArgumentNullException, hvis kontonummeret er null.
        /// </summary>
        [Test]
        public void TestAtKontoGetByRegnskabAndKontonummerKasterEnArgumentNullExceptionHvisKontonummerErNull()
        {
            var fixture = new Fixture();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.KontoGetByRegnskabAndKontonummer(fixture.Create<int>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at KontoGetByRegnskabAndKontonummer henter en given konto fra et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKontoGetByRegnskabAndKontonummerHenterKontoFraRegnskab()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var konto = commandHandler.KontoGetByRegnskabAndKontonummer(konti.ElementAt(1).Regnskab.Nummer, konti.ElementAt(1).Kontonummer);
            Assert.That(konto, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.Not.Null);
            Assert.That(konto.Kontonummer, Is.EqualTo(konti.ElementAt(1).Kontonummer));
            Assert.That(konto.Kontonavn, Is.Not.Null);
            Assert.That(konto.Kontonavn, Is.EqualTo(konti.ElementAt(1).Kontonavn));
        }

        /// <summary>
        /// Tester, at KontoGetByRegnskabAndKontonummer kaster en IntranetRepositoryException, hvis kontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtKontoGetByRegnskabAndKontonummerKasterEnIntranetRepositoryExceptionHvisKontoIkkeFindes()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var kontonummer = fixture.Create<string>();
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.KontoGetByRegnskabAndKontonummer(regnskaber.ElementAt(1).Nummer, kontonummer));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Konto).Name, kontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at BudgetkontoGetAllByRegnskab henter alle budgetkonti i et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetAllByRegnskabBudgetkontiFraRegnskab()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var result = commandHandler.BudgetkontoGetAllByRegnskab(regnskaber.ElementAt(1).Nummer);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at BudgetkontoGetByRegnskabAndKontonummer kaster en ArgumentNullException, hvis kontonummeret er null.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetByRegnskabAndKontonummerKasterEnArgumentNullExceptionHvisKontonummerErNull()
        {
            var fixture = new Fixture();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.BudgetkontoGetByRegnskabAndKontonummer(fixture.Create<int>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("kontonummer"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at BudgetkontoGetByRegnskabAndKontonummer henter en given budgetkonto fra et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetByRegnskabAndKontonummerHenterBudgetkontoFraRegnskab()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            
            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var budgetkonto = commandHandler.BudgetkontoGetByRegnskabAndKontonummer(budgetkonti.ElementAt(1).Regnskab.Nummer, budgetkonti.ElementAt(1).Kontonummer);
            Assert.That(budgetkonto, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
            Assert.That(budgetkonto.Kontonummer, Is.EqualTo(budgetkonti.ElementAt(1).Kontonummer));
            Assert.That(budgetkonto.Kontonavn, Is.Not.Null);
            Assert.That(budgetkonto.Kontonavn, Is.EqualTo(budgetkonti.ElementAt(1).Kontonavn));
        }

        /// <summary>
        /// Tester, at BudgetkontoGetByRegnskabAndKontonummer kaster en IntranetRepositoryException, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoGetByRegnskabAndKontonummerKasterEnIntranetRepositoryExceptionHvisBudgetkontoIkkeFindes()
        {
            var fixture = new Fixture();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            fixture.Inject(regnskaber.ElementAt(1));
            var konti = fixture.CreateMany<Konto>(3).ToList();
            var budgetkonti = fixture.CreateMany<Budgetkonto>(3).ToList();
            konti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            budgetkonti.ForEach(regnskaber.ElementAt(1).TilføjKonto);
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(3))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var budgetkontonummer = fixture.Create<string>();
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.BudgetkontoGetByRegnskabAndKontonummer(regnskaber.ElementAt(1).Nummer, budgetkontonummer));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto).Name, budgetkontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at AdressekontoGetAllByRegnskab henter alle adressekonti fra et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetAllByRegnskabHenterAdressekontiFraRegnskab()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var adresser = fixture.CreateMany<Person>(3).ToList();
            adresser.ElementAt(0).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            adresser.ElementAt(1).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            adresser.ElementAt(2).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var adressekonti = commandHandler.AdressekontoGetAllByRegnskab(regnskaber.ElementAt(1).Nummer);
            Assert.That(adressekonti, Is.Not.Null);
            Assert.That(adressekonti.Count(), Is.EqualTo(3));

            finansstyringRepositoryMock.AssertWasCalled(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull));
            adresseRepositoryMock.AssertWasCalled(m => m.AdresseGetAll());
            fællesRepositoryMock.AssertWasCalled(m => m.BrevhovedGetAll());
        }

        /// <summary>
        /// Tester, at AdressekontoGetByRegnskabAndNummer henter en given adressekonto fra et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetByRegnskabAndNummerHenterAdressekontoFraRegnskab()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var adresser = fixture.CreateMany<Person>(3).ToList();
            adresser.ElementAt(0).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            adresser.ElementAt(1).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            adresser.ElementAt(2).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var adressekonto = commandHandler.AdressekontoGetByRegnskabAndNummer(regnskaber.ElementAt(1).Nummer, adresser.ElementAt(1).Nummer);
            Assert.That(adressekonto, Is.Not.Null);
            Assert.That(adressekonto.Nummer, Is.EqualTo(adresser.ElementAt(1).Nummer));
            Assert.That(adressekonto.Navn, Is.Not.Null);
            Assert.That(adressekonto.Navn, Is.EqualTo(adresser.ElementAt(1).Navn));
            Assert.That(adressekonto.Bogføringslinjer, Is.Not.Null);
            Assert.That(adressekonto.Bogføringslinjer.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at AdressekontoGetByRegnskabAndNummer kaster en IntranetRepositoryException, hvis adressekontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtAdressekontoGetByRegnskabAndNummerHenterKasterIntranetRepositoryExceptionHvisAdressekontoIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(new DateTime(2010, 12, 31));
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskaber = fixture.CreateMany<Regnskab>(3).ToList();
            var adresser = fixture.CreateMany<Person>(3).ToList();
            adresser.ElementAt(0).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            adresser.ElementAt(1).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<decimal>(), 0M));
            adresser.ElementAt(2).TilføjBogføringslinje(new Bogføringslinje(fixture.Create<int>(), fixture.Create<DateTime>(), fixture.Create<string>(), fixture.Create<string>(), 0M, fixture.Create<decimal>()));

            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(regnskaber.ElementAt(1).Nummer), Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskaber.ElementAt(1))
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adresser)
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new MyCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var adressekonto = fixture.Create<int>();
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.AdressekontoGetByRegnskabAndNummer(regnskaber.ElementAt(1).Nummer, adressekonto));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (AdresseBase).Name, adressekonto)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
