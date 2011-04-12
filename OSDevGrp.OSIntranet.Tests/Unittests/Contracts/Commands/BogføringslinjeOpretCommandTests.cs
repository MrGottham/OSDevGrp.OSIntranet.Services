using System;
using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Commands;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Commands
{
    /// <summary>
    /// Tester datakontrakt til oprettelse af bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretCommandTests
    {
        /// <summary>
        /// Tester, at Command kan initieres.
        /// </summary>
        [Test]
        public void TestAtCommandKanInitieres()
        {
            var bogføringsdato = new DateTime(2011, 3, 15);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = bogføringsdato,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 10000M,
                                  Kredit = 5000M,
                                  Adressekonto = 1
                              };
            Assert.That(command, Is.Not.Null);
            Assert.That(command.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(command.Dato, Is.EqualTo(bogføringsdato));
            Assert.That(command.Bilag, Is.Not.Null);
            Assert.That(command.Bilag, Is.EqualTo("XYZ"));
            Assert.That(command.Kontonummer, Is.Not.Null);
            Assert.That(command.Kontonummer, Is.EqualTo("DANKORT"));
            Assert.That(command.Tekst, Is.Not.Null);
            Assert.That(command.Tekst, Is.EqualTo("Test"));
            Assert.That(command.Budgetkontonummer, Is.Not.Null);
            Assert.That(command.Budgetkontonummer, Is.EqualTo("1000"));
            Assert.That(command.Debit, Is.EqualTo(10000M));
            Assert.That(command.Kredit, Is.EqualTo(5000M));
            Assert.That(command.Adressekonto, Is.EqualTo(1));
        }

        /// <summary>
        /// Tester, at Command kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtCommandKanSerialiseres()
        {
            var bogføringsdato = new DateTime(2011, 3, 15);
            var command = new BogføringslinjeOpretCommand
                              {
                                  Regnskabsnummer = 1,
                                  Dato = bogføringsdato,
                                  Bilag = "XYZ",
                                  Kontonummer = "DANKORT",
                                  Tekst = "Test",
                                  Budgetkontonummer = "1000",
                                  Debit = 10000M,
                                  Kredit = 5000M,
                                  Adressekonto = 1
                              };
            Assert.That(command, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(command.GetType());
                serializer.WriteObject(memoryStream, command);
                memoryStream.Flush();
                Assert.That(memoryStream.Length, Is.GreaterThan(0));
            }
            finally
            {
                memoryStream.Close();
            }
        }
    }
}
