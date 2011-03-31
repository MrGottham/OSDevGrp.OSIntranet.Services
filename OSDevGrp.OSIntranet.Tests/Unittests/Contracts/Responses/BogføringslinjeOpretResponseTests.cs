using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Contracts.Responses;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Responses
{
    /// <summary>
    /// Tester datakontrakt til svar fra oprettelse af bogføringslinje.
    /// </summary>
    [TestFixture]
    public class BogføringslinjeOpretResponseTests
    {
        /// <summary>
        /// Tester, at Response kan initieres.
        /// </summary>
        [Test]
        public void TestAtResponseKanInitieres()
        {
            var response = new BogføringslinjeOpretResponse
                               {
                                   Advarsler = new List<string>
                                                   {
                                                       "Test 1",
                                                       "Test 2",
                                                       "Test 3"
                                                   }
                               };
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Advarsler, Is.Not.Null);
            Assert.That(response.Advarsler.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Response kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtResponseKanSerialiseres()
        {
            var response = new BogføringslinjeOpretResponse
                               {
                                   Advarsler = new List<string>
                                                   {
                                                       "Test 1",
                                                       "Test 2",
                                                       "Test 3"
                                                   }
                               };
            Assert.That(response, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(response.GetType());
                serializer.WriteObject(memoryStream, response);
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
