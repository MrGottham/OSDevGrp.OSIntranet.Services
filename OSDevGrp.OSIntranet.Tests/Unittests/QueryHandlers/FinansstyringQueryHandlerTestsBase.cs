using System;
using System.Collections.Generic;
using System.Linq;
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
            var bogføringslinje = new Bogføringslinje(1, new DateTime(2010, 1, 1), null, "Saldo", 3000M, 0M);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(2, new DateTime(2010, 10, 31), null, "Kvickly", 0M, 250M);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            budgetkontoSupermarkeder.TilføjBogføringslinje(bogføringslinje);
            bogføringslinje = new Bogføringslinje(3, new DateTime(2010, 11, 07), null,
                                                  "Indbetaling fra Sygesikring Danmark", 300M, 0M);
            kontoDankort.TilføjBogføringslinje(bogføringslinje);
            budgetkonotØvrigeIndtægter.TilføjBogføringslinje(bogføringslinje);
            // Dan mockup af repository.
            var repository = MockRepository.GenerateMock<IFinansstyringRepository>();
            repository.Expect(m => m.RegnskabslisteGet()).Return(regnskaber);
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(1))).Return(regnskaber.Single(m => m.Nummer == 1));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(2))).Return(regnskaber.Single(m => m.Nummer == 2));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Equal(3))).Return(regnskaber.Single(m => m.Nummer == 3));
            repository.Expect(m => m.RegnskabGet(Arg<int>.Is.Anything)).Throw(
                new IntranetRepositoryException("Regnskab ikke fundet."));
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
