using System;
using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter en konto.
    /// </summary>
    [TestFixture]
    public class KontoGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var statusDato = new DateTime(2010, 10, 31);
            var query = new KontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = statusDato,
                                Kontonummer = "DANKORT"
                            };
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(query.StatusDato, Is.EqualTo(statusDato));
            Assert.That(query.Kontonummer, Is.Not.Null);
            Assert.That(query.Kontonummer, Is.EqualTo("DANKORT"));
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var statusDato = new DateTime(2010, 10, 31);
            var query = new KontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = statusDato,
                                Kontonummer = "DANKORT"
                            };
            Assert.That(query, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(query.GetType());
                serializer.WriteObject(memoryStream, query);
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
