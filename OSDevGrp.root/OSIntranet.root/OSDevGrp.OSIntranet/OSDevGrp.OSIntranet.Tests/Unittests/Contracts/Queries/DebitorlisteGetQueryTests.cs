using System;
using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter en debitorliste.
    /// </summary>
    [TestFixture]
    public class DebitorlisteGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var statusDato = new DateTime(2010, 10, 31);
            var query = new DebitorlisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = statusDato
                            };
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(query.StatusDato, Is.EqualTo(statusDato));
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var statusDato = new DateTime(2010, 10, 31);
            var query = new DebitorlisteGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = statusDato
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
