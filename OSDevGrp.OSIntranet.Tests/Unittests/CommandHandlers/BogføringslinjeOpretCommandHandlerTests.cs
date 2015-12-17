using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.CommandHandlers
{
    /// <summary>
    /// Tester CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandHandlerTests
    {
        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationsrepository er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisKonfigurationRepositoryErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var exception = Assert.Throws<ArgumentNullException>(() => new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, null, objectMapperMock, exceptionBuilderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("konfigurationRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute tilføjer en bogføringslinje uden adressekonto.
        /// </summary>
        [Test]
        public void TestAtExecuteTilføjerBogføringslinjeUdenAdressekonto()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject<KontoBaseView>(new KontoView());
            fixture.Inject(fixture.CreateMany<BogføringsadvarselResponse>(3));
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Anything, Arg<string>.Is.Anything, Arg<Konto>.Is.NotNull, Arg<string>.Is.NotNull, Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.GreaterThan(0M), Arg<decimal>.Is.GreaterThan(0M), Arg<AdresseBase>.Is.Null))
                .WhenCalled(m =>
                {
                    var bogføringslinje = new Bogføringslinje(fixture.Create<int>(), (DateTime) m.Arguments[0], (string) m.Arguments[1], (string) m.Arguments[3], (decimal) m.Arguments[5], (decimal) m.Arguments[6]);
                    if (m.Arguments[2] as Konto != null)
                    {
                        ((Konto) m.Arguments[2]).TilføjBogføringslinje(bogføringslinje);
                    }
                    if (m.Arguments[4] as Budgetkonto != null)
                    {
                        ((Budgetkonto) m.Arguments[4]).TilføjBogføringslinje(bogføringslinje);
                    }
                    m.ReturnValue = bogføringslinje;
                })
                .Return(null)
                .Repeat.Any();

            var adressekonti = fixture.CreateMany<Person>(25).ToList();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adressekonti)
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            objectMapperMock.Expect(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull))
                .Return(fixture.Create<BogføringslinjeOpretResponse>())
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>()
            };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);
 
            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Equal(command.Dato), Arg<string>.Is.Equal(command.Bilag), Arg<Konto>.Is.NotNull, Arg<string>.Is.Equal(command.Tekst), Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.Equal(command.Debit), Arg<decimal>.Is.Equal(command.Kredit), Arg<AdresseBase>.Is.Null));
            objectMapperMock.AssertWasCalled(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Execute tilføjer en bogføringslinje med adressekonto.
        /// </summary>
        [Test]
        public void TestAtExecuteTilføjerBogføringslinjeMedAdressekonto()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject<KontoBaseView>(new KontoView());
            fixture.Inject(fixture.CreateMany<BogføringsadvarselResponse>(3));
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();
            finansstyringRepositoryMock.Expect(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Anything, Arg<string>.Is.Anything, Arg<Konto>.Is.NotNull, Arg<string>.Is.NotNull, Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.GreaterThan(0M), Arg<decimal>.Is.GreaterThan(0M), Arg<AdresseBase>.Is.NotNull))
                .WhenCalled(m =>
                {
                    var bogføringslinje = new Bogføringslinje(fixture.Create<int>(), (DateTime) m.Arguments[0], (string) m.Arguments[1], (string) m.Arguments[3], (decimal) m.Arguments[5], (decimal) m.Arguments[6]);
                    if (m.Arguments[2] as Konto != null)
                    {
                        ((Konto) m.Arguments[2]).TilføjBogføringslinje(bogføringslinje);
                    }
                    if (m.Arguments[4] as Budgetkonto != null)
                    {
                        ((Budgetkonto) m.Arguments[4]).TilføjBogføringslinje(bogføringslinje);
                    }
                    if (m.Arguments[7] as AdresseBase != null)
                    {
                        ((AdresseBase) m.Arguments[7]).TilføjBogføringslinje(bogføringslinje);
                    }
                    m.ReturnValue = bogføringslinje;
                })
                .Return(null)
                .Repeat.Any();

            var adressekonti = fixture.CreateMany<Person>(25).ToList();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adressekonti)
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            objectMapperMock.Expect(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull))
                .Return(fixture.Create<BogføringslinjeOpretResponse>())
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = adressekonti.ElementAt(1).Nummer
            };
            var result = commandHandler.Execute(command);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Advarsler, Is.Not.Null);

            finansstyringRepositoryMock.AssertWasCalled(m => m.BogføringslinjeAdd(Arg<DateTime>.Is.Equal(command.Dato), Arg<string>.Is.Equal(command.Bilag), Arg<Konto>.Is.NotNull, Arg<string>.Is.Equal(command.Tekst), Arg<Budgetkonto>.Is.NotNull, Arg<decimal>.Is.Equal(command.Debit), Arg<decimal>.Is.Equal(command.Kredit), Arg<AdresseBase>.Is.NotNull));
            objectMapperMock.AssertWasCalled(m => m.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(Arg<IBogføringsresultat>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Execute kaster en ArgumentNullException, hvis kommandoen er null.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterArgumentNullExceptionHvisCommandErNull()
        {
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => commandHandler.Execute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("command"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisRegnskabIkkeFindes()
        {
            var fixture = new Fixture();
            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var intranetRepositoryException = fixture.Create<IntranetRepositoryException>();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Throw(intranetRepositoryException)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = fixture.Create<int>(),
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = fixture.Create<string>(),
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };

            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(intranetRepositoryException));
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis bogføringsdato er for gammel.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisBogføringsdatoErForGammel()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now.AddDays(-31));
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = fixture.Create<string>(),
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
        
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld, 30)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis bogføringsdato er fremme i tiden.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisBogføringsdatoErFremmeITiden()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now.AddDays(1));
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = fixture.Create<string>(),
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateIsForwardInTime)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis kontonummer ikke er angivet.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisKontonummerIkkeErAngivet()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = null,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineAccountNumberMissing)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis konto ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisKontoIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = fixture.Create<string>(),
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Konto).Name, command.Kontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis tekst ikke er angivet.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisTekstIkkeErAngivet()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = null,
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineTextMissing)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis budgetkonto ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisBudgetkontoIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = fixture.Create<string>(),
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto).Name, command.Budgetkontonummer)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis debitbeløb er under 0.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisDebitBeløbErUnderNul()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                Debit = fixture.Create<decimal>()*-1,
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis kreditbeløb er under 0.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisKreditBeløbErUnderNul()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>()*-1,
                Adressekonto = fixture.Create<int>()
            };
         
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetBusinessException, hvis debitbeløb og kreditbeløb er 0.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetBusinessExceptionHvisDebitBeløbOgKreditBeløbErNul()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject(0M);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(fixture.CreateMany<Person>(25))
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetBusinessException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueMissing)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tester, at Execute kaster en IntranetRepositoryException, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtExecuteKasterIntranetRepositoryExceptionHvisAdresseIkkeFindes()
        {
            var fixture = new Fixture();
            fixture.Inject(DateTime.Now);
            fixture.Inject(250);
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();

            var regnskab = fixture.Create<Regnskab>();
            foreach (var konto in fixture.CreateMany<Konto>(25))
            {
                regnskab.TilføjKonto(konto);
            }
            foreach (var budgetkonto in fixture.CreateMany<Budgetkonto>(25))
            {
                regnskab.TilføjKonto(budgetkonto);
            }
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            finansstyringRepositoryMock.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, Brevhoved>>.Is.NotNull, Arg<Func<int, AdresseBase>>.Is.NotNull))
                .Return(regnskab)
                .Repeat.Any();

            var adressekonti = fixture.CreateMany<Person>(25).ToList();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            adresseRepositoryMock.Expect(m => m.AdresseGetAll())
                .Return(adressekonti)
                .Repeat.Any();

            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            fællesRepositoryMock.Expect(m => m.BrevhovedGetAll())
                .Return(fixture.CreateMany<Brevhoved>(3))
                .Repeat.Any();

            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            konfigurationRepositoryMock.Expect(m => m.DageForBogføringsperiode)
                .Return(30)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var command = new BogføringslinjeOpretCommand
            {
                Regnskabsnummer = regnskab.Nummer,
                Dato = fixture.Create<DateTime>(),
                Bilag = fixture.Create<string>(),
                Kontonummer = regnskab.Konti.OfType<Konto>().ElementAt(1).Kontonummer,
                Tekst = fixture.Create<string>(),
                Budgetkontonummer = regnskab.Konti.OfType<Budgetkonto>().ElementAt(1).Kontonummer,
                Debit = fixture.Create<decimal>(),
                Kredit = fixture.Create<decimal>(),
                Adressekonto = adressekonti.Max(m => m.Nummer) + fixture.Create<int>()
            };
            
            var exception = Assert.Throws<IntranetRepositoryException>(() => commandHandler.Execute(command));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (AdresseBase).Name, command.Adressekonto)));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        /// <summary>
        /// Tester, at HandleException kalder Build på builderen, der kan bygge exceptions.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKalderBuildPåExceptionBuilder()
        {
            var fixture = new Fixture();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();
            
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.NotNull))
                .WhenCalled(e =>
                {
                    var methodBase = (MethodBase) e.Arguments.ElementAt(1);
                    Assert.That(methodBase, Is.Not.Null);
                    Assert.That(methodBase.ReflectedType.Name, Is.EqualTo(typeof (BogføringslinjeOpretCommandHandler).Name));
                })
                .Return(fixture.Create<Exception>())
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = fixture.Create<Exception>();
            Assert.Throws<Exception>(() => commandHandler.HandleException(fixture.Create<BogføringslinjeOpretCommand>(), exception));

            exceptionBuilderMock.AssertWasCalled(m => m.Build(Arg<Exception>.Is.Equal(exception), Arg<MethodBase>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at HandleException kaster exception fra builderen, der kan bygge exceptions.
        /// </summary>
        [Test]
        public void TestAtHandleExceptionKasterExceptionFraExceptionBuilder()
        {
            var fixture = new Fixture();
            var finansstyringRepositoryMock = MockRepository.GenerateMock<IFinansstyringRepository>();
            var adresseRepositoryMock = MockRepository.GenerateMock<IAdresseRepository>();
            var fællesRepositoryMock = MockRepository.GenerateMock<IFællesRepository>();
            var konfigurationRepositoryMock = MockRepository.GenerateMock<IKonfigurationRepository>();
            var objectMapperMock = MockRepository.GenerateMock<IObjectMapper>();

            var exceptionToThrow = fixture.Create<Exception>();
            var exceptionBuilderMock = MockRepository.GenerateMock<IExceptionBuilder>();
            exceptionBuilderMock.Stub(m => m.Build(Arg<Exception>.Is.Anything, Arg<MethodBase>.Is.Anything))
                .Return(exceptionToThrow)
                .Repeat.Any();

            var commandHandler = new BogføringslinjeOpretCommandHandler(finansstyringRepositoryMock, adresseRepositoryMock, fællesRepositoryMock, konfigurationRepositoryMock, objectMapperMock, exceptionBuilderMock);
            Assert.That(commandHandler, Is.Not.Null);

            var exception = Assert.Throws<Exception>(() => commandHandler.HandleException(fixture.Create<BogføringslinjeOpretCommand>(), fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.EqualTo(exceptionToThrow));
        }
    }
}
