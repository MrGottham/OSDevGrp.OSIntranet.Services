using System.IO;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Contracts.Faults;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Contracts.Faults
{
    /// <summary>
    /// Tester fault for systemfejl i OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetSystemFaultTests
    {
        /// <summary>
        /// Tester, at Fault kan initieres.
        /// </summary>
        [Test]
        public void TestAtFaultKanInitieres()
        {
            var fault = new IntranetSystemFault
                            {
                                Message = "Test",
                                ExceptionMessages = "Test -> Test"
                            };
            Assert.That(fault, Is.Not.Null);
            Assert.That(fault.Message, Is.Not.Null);
            Assert.That(fault.Message, Is.EqualTo("Test"));
            Assert.That(fault.ExceptionMessages, Is.Not.Null);
            Assert.That(fault.ExceptionMessages, Is.EqualTo("Test -> Test"));
        }

        /// <summary>
        /// Tester, at Fault kan serialiseres.
        /// </summary>
        [Test]
        public void TestAtFaultKanSerialiseres()
        {
            var fault = new IntranetSystemFault
                            {
                                Message = "Test",
                                ExceptionMessages = "Test -> Test"
                            };
            Assert.That(fault, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new DataContractSerializer(fault.GetType());
                serializer.WriteObject(memoryStream, fault);
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
