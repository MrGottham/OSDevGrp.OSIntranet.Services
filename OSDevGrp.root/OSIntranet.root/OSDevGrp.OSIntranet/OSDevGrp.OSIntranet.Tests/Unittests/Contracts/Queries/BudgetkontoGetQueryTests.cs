using System;
using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter en budgetkonto.
    /// </summary>
    [TestFixture]
    public class BudgetkontoGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var statusDato = new DateTime(2010, 10, 31);
            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = statusDato,
                                Kontonummer = "1000"
                            };
            Assert.That(query, Is.Not.Null);
            Assert.That(query.Regnskabsnummer, Is.EqualTo(1));
            Assert.That(query.StatusDato, Is.EqualTo(statusDato));
            Assert.That(query.Kontonummer, Is.Not.Null);
            Assert.That(query.Kontonummer, Is.EqualTo("1000"));
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var statusDato = new DateTime(2010, 10, 31);
            var query = new BudgetkontoGetQuery
                            {
                                Regnskabsnummer = 1,
                                StatusDato = statusDato,
                                Kontonummer = "1000"
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
