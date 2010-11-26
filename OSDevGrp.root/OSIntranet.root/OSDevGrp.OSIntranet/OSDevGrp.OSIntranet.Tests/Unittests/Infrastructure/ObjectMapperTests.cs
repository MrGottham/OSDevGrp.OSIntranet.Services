using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester ObjectMapper.
    /// </summary>
    [TestFixture]
    public class ObjectMapperTests
    {
        /// <summary>
        /// Tester, at ObjectMapper kan initieres.
        /// </summary>
        [Test]
        public void TestAtObjectMapperKanInitieres()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at et regnskab kan mappes til et regnskabslisteview.
        /// </summary>
        [Test]
        public void TestAtRegnskabKanMappesTilRegnskabslisteView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var regnskabslisteView = objectMapper.Map<Regnskab, RegnskabslisteView>(regnskab);
            Assert.That(regnskabslisteView, Is.Not.Null);
            Assert.That(regnskabslisteView.Nummer, Is.EqualTo(1));
            Assert.That(regnskabslisteView.Navn, Is.Not.Null);
            Assert.That(regnskabslisteView.Navn, Is.EqualTo("Privatregnskab, Ole Sørensen"));
        }

        /// <summary>
        /// Tester, at en konto kan mappes til et kontoplanview.
        /// </summary>
        [Test]
        public void TestAtKontoKanMappesTilKontoplanView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var kontogruppe = new Kontogruppe(1, "Bankkonti", KontogruppeType.Aktiver);
            var konto = new Konto(regnskab, "DANKORT", "Dankort", kontogruppe);
            konto.SætBeskrivelse("Dankort/lønkonto");
            konto.SætNote("Kredit på kr. 5.000,00");
            konto.TilføjKreditoplysninger(new Kreditoplysninger(2010, 10, 5000M));
            konto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Saldo", 2500M, 0M));
            konto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 1), null, "Kvickly", 0M, 250M));
            konto.Calculate(new DateTime(2010, 10, 31));

            var kontoplanView = objectMapper.Map<Konto, KontoplanView>(konto);
            Assert.That(kontoplanView, Is.Not.Null);
            Assert.That(kontoplanView.Regnskab, Is.Not.Null);
            Assert.That(kontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(kontoplanView.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(kontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(kontoplanView.Kontonavn, Is.EqualTo("Dankort"));
            Assert.That(kontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(kontoplanView.Beskrivelse, Is.EqualTo("Dankort/lønkonto"));
            Assert.That(kontoplanView.Notat, Is.Not.Null);
            Assert.That(kontoplanView.Notat, Is.EqualTo("Kredit på kr. 5.000,00"));
            Assert.That(kontoplanView.Kontogruppe, Is.Not.Null);
            Assert.That(kontoplanView.Kredit, Is.EqualTo(5000M));
            Assert.That(kontoplanView.Saldo, Is.EqualTo(2250M));
            Assert.That(kontoplanView.Disponibel, Is.EqualTo(7250M));
        }

        /// <summary>
        /// Tester, at en budgetkonto kan mappes til et budgetkontoplanview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoKanMappesTilBudgetkontoplanView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkonto = new Budgetkonto(regnskab, "1000", "Indtægter", budgetkontogruppe);
            budgetkonto.SætBeskrivelse("Salg m.m.");
            budgetkonto.SætNote("Indtægter vedrørende salg m.m.");
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(2010, 10, 15000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Indbetaling", 10000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 5), null, "Indbetaling", 4000M, 0M));
            budgetkonto.Calculate(new DateTime(2010, 10, 31));

            var bugetkontoplanView = objectMapper.Map<Budgetkonto, BudgetkontoplanView>(budgetkonto);
            Assert.That(bugetkontoplanView, Is.Not.Null);
            Assert.That(bugetkontoplanView.Regnskab, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonummer, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonummer, Is.EqualTo("1000"));
            Assert.That(bugetkontoplanView.Kontonavn, Is.Not.Null);
            Assert.That(bugetkontoplanView.Kontonavn, Is.EqualTo("Indtægter"));
            Assert.That(bugetkontoplanView.Beskrivelse, Is.Not.Null);
            Assert.That(bugetkontoplanView.Beskrivelse, Is.EqualTo("Salg m.m."));
            Assert.That(bugetkontoplanView.Notat, Is.Not.Null);
            Assert.That(bugetkontoplanView.Notat, Is.EqualTo("Indtægter vedrørende salg m.m."));
            Assert.That(bugetkontoplanView.Budgetkontogruppe, Is.Not.Null);
            Assert.That(bugetkontoplanView.Budget, Is.EqualTo(15000M));
            Assert.That(bugetkontoplanView.Bogført, Is.EqualTo(14000M));
            Assert.That(bugetkontoplanView.Disponibel, Is.EqualTo(1000M));
            Assert.That(bugetkontoplanView.Budgetoplysninger, Is.Not.Null);
            Assert.That(bugetkontoplanView.Budgetoplysninger.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at budgetoplysninger kan mappes til et budgetoplysningsview.
        /// </summary>
        [Test]
        public void TestAtBudgetoplysningerKanMappesTilBudgetoplysningerView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var regnskab = new Regnskab(1, "Privatregnskab, Ole Sørensen");
            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkonto = new Budgetkonto(regnskab, "1000", "Indtægter", budgetkontogruppe);
            budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(2010, 10, 15000M, 0M));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(1, new DateTime(2010, 10, 1), null, "Indbetaling", 5000M, 0));
            budgetkonto.TilføjBogføringslinje(new Bogføringslinje(2, new DateTime(2010, 10, 5), null, "Indbetaling", 9000M, 0));
            budgetkonto.Calculate(new DateTime(2010, 10, 31));

            var budgetoplysningerView =
                objectMapper.Map<Budgetoplysninger, BudgetoplysningerView>(
                    budgetkonto.Budgetoplysninger.Single(m => m.År == 2010 && m.Måned == 10));
            Assert.That(budgetoplysningerView, Is.Not.Null);
            Assert.That(budgetoplysningerView.År, Is.EqualTo(2010));
            Assert.That(budgetoplysningerView.Måned, Is.EqualTo(10));
            Assert.That(budgetoplysningerView.Budget, Is.EqualTo(15000M));
            Assert.That(budgetoplysningerView.Bogført, Is.EqualTo(14000M));
            Assert.That(budgetoplysningerView.Disponibel, Is.EqualTo(1000M));
        }

        /// <summary>
        /// Tester, at en kontogruppe kan mappes til et kontogruppeview.
        /// </summary>
        [Test]
        public void TestAtKontogruppeKanMappesTilKontogruppeView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var kontogruppeAktiver = new Kontogruppe(1, "Bankkonti", KontogruppeType.Aktiver);
            var kontogruppeAktiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppeAktiver);
            Assert.That(kontogruppeAktiverView, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Nummer, Is.EqualTo(1));
            Assert.That(kontogruppeAktiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppeAktiverView.Navn, Is.EqualTo("Bankkonti"));
            Assert.That(kontogruppeAktiverView.ErAktiver, Is.True);
            Assert.That(kontogruppeAktiverView.ErPassiver, Is.False);

            var kontogruppePassiver = new Kontogruppe(2, "Kreditorer", KontogruppeType.Passiver);
            var kontogruppePassiverView = objectMapper.Map<Kontogruppe, KontogruppeView>(kontogruppePassiver);
            Assert.That(kontogruppePassiverView, Is.Not.Null);
            Assert.That(kontogruppePassiverView.Nummer, Is.EqualTo(2));
            Assert.That(kontogruppePassiverView.Navn, Is.Not.Null);
            Assert.That(kontogruppePassiverView.Navn, Is.EqualTo("Kreditorer"));
            Assert.That(kontogruppePassiverView.ErAktiver, Is.False);
            Assert.That(kontogruppePassiverView.ErPassiver, Is.True);
        }

        /// <summary>
        /// Tester, at en budgetkontogruppe kan mappes til et bugdetkontogruppeview.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeKanMappesTilBudgetkontogruppeView()
        {
            var objectMapper = new ObjectMapper();
            Assert.That(objectMapper, Is.Not.Null);

            var budgetkontogruppe = new Budgetkontogruppe(1, "Indtægter");
            var budgetkontogruppeView = objectMapper.Map<Budgetkontogruppe, BudgetkontogruppeView>(budgetkontogruppe);
            Assert.That(budgetkontogruppeView, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Nummer, Is.EqualTo(1));
            Assert.That(budgetkontogruppeView.Navn, Is.Not.Null);
            Assert.That(budgetkontogruppeView.Navn, Is.EqualTo("Indtægter"));
        }
    }
}
