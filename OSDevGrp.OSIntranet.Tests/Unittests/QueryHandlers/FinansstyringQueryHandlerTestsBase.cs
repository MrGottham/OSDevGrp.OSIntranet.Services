using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Basisklasse til test af QueryHandlers for finansstyring.
    /// </summary>
    public abstract class FinansstyringQueryHandlerTestsBase
    {
        /// <summary>
        /// Danner og returnerer repository for adressekartotek.
        /// </summary>
        /// <returns>Repository for adressekartotek, der kan benyttes til test.</returns>
        protected static IAdresseRepository GetAdresseRepository()
        {
            // Dan testdata for adressegrupper.
            var adresseGruppePersoner = new Adressegruppe(1, "Personer", 0);
            var adresseGruppeFirmaer = new Adressegruppe(2, "Firmaer", 0);
            // Dan testdata for betalingsbetingelser.
            var betalingsbetingelser = new List<Betalingsbetingelse>
                                           {
                                               new Betalingsbetingelse(1, "Kontant"),
                                               new Betalingsbetingelse(1, "Netto + 8 dage")
                                           };
            // Dan testdata for adresser.
            var adresser = new List<AdresseBase>
                               {
                                   new Person(1, "Ole Sørensen", adresseGruppePersoner),
                                   new Person(2, "Bente Susanne Rasmussen", adresseGruppePersoner),
                                   new Firma(3, "OS Development Group", adresseGruppeFirmaer)
                               };

            // Dan mockup af repoistory.
            var repository = MockRepository.GenerateMock<IAdresseRepository>();
            repository.Expect(m => m.AdresseGetAll()).Return(adresser);
            repository.Expect(m => m.BetalingsbetingelseGetAll()).Return(betalingsbetingelser);
            return repository;
        }

        /// <summary>
        /// Danner og returnerer et repository for finansstyring.
        /// </summary>
        /// <returns>Repository for finansstyring, der kan benyttes til test.</returns>
        protected static IFinansstyringRepository GetFinansstyringRepository()
        {
            // Dan testdata for regnskaber.
            var regnskaber = new List<Regnskab>
                                 {
                                     new Regnskab(1, "Privatregnskab, Ole Sørensen"),
                                     new Regnskab(2, "Privatregnskab, Patrick Emil Sørensen"),
                                     new Regnskab(3, "Privatregnskab, Mathias Johannes Sørensen")
                                 };
            // Dan testdata for konti.
            var kontogruppeBankkonti = new Kontogruppe(1, "Bankkonti", KontogruppeType.Aktiver);
            var kontogrupper = new List<Kontogruppe>
                                   {
                                       kontogruppeBankkonti
                                   };
            var regnskab = regnskaber.Single(m => m.Nummer == 1);
            var kontoDankort = new Konto(regnskab, "DANKORT", "Dankort", kontogruppeBankkonti);
            var kontoOpsparing = new Konto(regnskab, "OPSPARING", "Opsparing", kontogruppeBankkonti);
            regnskab.TilføjKonto(kontoDankort);
            regnskab.TilføjKonto(kontoOpsparing);
            // Dan testdata for kreditter.
            kontoDankort.TilføjKreditoplysninger(new Kreditoplysninger(2010, 10, 5000M));
            // Dan testdata for budgetkontogrupper.
            var budgetkontogruppeIndtægter = new Budgetkontogruppe(1, "Indtægter");
            var budgetkontogruppeUdgifter = new Budgetkontogruppe(2, "Udgifter");
            var budgetkontogrupper = new List<Budgetkontogruppe>
                                         {
                                             budgetkontogruppeIndtægter,
                                             budgetkontogruppeUdgifter
                                         };
            var budgetkontoLønninger = new Budgetkonto(regnskab, "1000", "Lønninger", budgetkontogruppeIndtægter);
            var budgetkonotØvrigeIndtægter = new Budgetkonto(regnskab, "1010", "Øvrige indtægter",
                                                             budgetkontogruppeIndtægter);
            var budgetkontoSupermarkeder = new Budgetkonto(regnskab, "2000", "Supermarkeder",
                                                           budgetkontogruppeUdgifter);
            var budgetkontoØvrigeUdgifter = new Budgetkonto(regnskab, "2010", "Øvrige udgifter",
                                                            budgetkontogruppeUdgifter);
            for (var i = 0; i < 12; i++)
            {
                budgetkontoLønninger.TilføjBudgetoplysninger(new Budgetoplysninger(2010, i + 1, 15000M, 0M));
                budgetkontoSupermarkeder.TilføjBudgetoplysninger(new Budgetoplysninger(2010, i + 1, 0M, 3500M));
            }
            regnskab.TilføjKonto(budgetkontoLønninger);
            regnskab.TilføjKonto(budgetkonotØvrigeIndtægter);
            regnskab.TilføjKonto(budgetkontoSupermarkeder);
            regnskab.TilføjKonto(budgetkontoØvrigeUdgifter);
            // Dan testdata for bogføringslinjer.
            var bogføringslinje = new Bogføringslinje(1, new DateTime(2010, 1, 1), null, "Saldo", 7500M, 0M);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(2, new DateTime(2010, 10, 31), null, "Kvickly", 0M, 250M);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            budgetkontoSupermarkeder.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(3, new DateTime(2010, 11, 7), null,
                                                  "Indbetaling fra Sygesikring Danmark", 300M, 0M);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            budgetkonotØvrigeIndtægter.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(4, new DateTime(2011, 3, 2), null, "Udlån", 0M, 1000M);
            var tempAdresse = new Person(1, "Temporary", new Adressegruppe(1, "Temporary", 0));
            tempAdresse.TilføjBogføringslinje(bogføringslinje);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(5, new DateTime(2011, 3, 4), null, "Indbetaling", 1000M, 0M);
            tempAdresse = new Person(2, "Temporary", new Adressegruppe(1, "Temporary", 0));
            tempAdresse.TilføjBogføringslinje(bogføringslinje);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(6, new DateTime(2011, 3, 5), null, "Udlån", 0M, 5000M);
            tempAdresse = new Person(-1, "Temporary", new Adressegruppe(1, "Temporary", 0));
            tempAdresse.TilføjBogføringslinje(bogføringslinje);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            // Dan mockup af repository.
            var repository = MockRepository.GenerateMock<IFinansstyringRepository>();
            repository.Expect(m => m.RegnskabslisteGet(null)).Return(regnskaber);
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(1))).Return(regnskaber.Single(m => m.Nummer == 1));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(2))).Return(regnskaber.Single(m => m.Nummer == 2));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(3))).Return(regnskaber.Single(m => m.Nummer == 3));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything))
                .Throw(new IntranetRepositoryException("Regnskab ikke fundet."));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(1), Arg<Func<int, AdresseBase>>.Is.Anything))
                .WhenCalled(x =>
                                {
                                    var callback = (Func<int, AdresseBase>) x.Arguments[1];
                                    regnskab = regnskaber.Single(m => m.Nummer == 1);
                                    foreach (var linje in regnskab.Konti.OfType<Konto>().SelectMany(m => m.Bogføringslinjer))
                                    {
                                        if (linje.Adresse == null)
                                        {
                                            continue;
                                        }
                                        try
                                        {
                                            var adresse = callback(linje.Adresse.Nummer);
                                            adresse.TilføjBogføringslinje(linje);
                                        }
                                        catch (IntranetRepositoryException)
                                        {
                                            // Nothing to do.
                                        }
                                    }
                                })
                .Return(regnskaber.Single(m => m.Nummer == 1));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything, Arg<Func<int, AdresseBase>>.Is.Anything))
                .Throw(new IntranetRepositoryException("Regnskab ikke fundet."));
            repository.Expect(m => m.KontogruppeGetAll()).Return(kontogrupper);
            repository.Expect(m => m.BudgetkontogruppeGetAll()).Return(budgetkontogrupper);
            return repository;
        }

        /// <summary>
        /// Danner og returnerer en objectmapper.
        /// </summary>
        /// <returns>ObjectMapper.</returns>
        protected static IObjectMapper GetObjectMapper()
        {
            return new ObjectMapper();
        }
    }
}
