using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Queries;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Queries
{
    /// <summary>
    /// Tester datakontrakt til forespørgelse efter kontogrupper.
    /// </summary>
    [TestFixture]
    public class BudgetkontogrupperGetQueryTests
    {
        /// <summary>
        /// Tester, at Query kan initieres.
        /// </summary>
        [Test]
        public void TestAtQueryKanInitieres()
        {
            var query = new BudgetkontogrupperGetQuery();
            Assert.That(query, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Query kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtQueryKanSerialiseres()
        {
            var query = new BudgetkontogrupperGetQuery();
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
