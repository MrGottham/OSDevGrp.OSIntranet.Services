using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester klassen IntranetBusinessException.
    /// </summary>
    [TestFixture]
    public class IntranetBusinessExceptionTests
    {
        /// <summary>
        /// Tester, at IntranetBusinessException kan instantieres.
        /// </summary>
        [Test]
        public void TestAtIntranetBusinessExceptionKanInstantieres()
        {
            var exception = new IntranetBusinessException("Test");
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Null);

            exception = new IntranetBusinessException("Test", new Exception());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.GetType(), Is.EqualTo(typeof(Exception)));
        }

        /// <summary>
        /// Tester, at IntranetBusinessException kan serialiseres og deserialiseres.
        /// </summary>
        [Test]
        public void TestAtIntranetBusinessExceptionKanSerialiseresOgDeserialiseres()
        {
            var exception = new IntranetBusinessException("Test");
            Assert.That(exception, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new SoapFormatter();
                serializer.Serialize(memoryStream, exception);
                Assert.That(memoryStream.Length, Is.GreaterThan(0));

                memoryStream.Seek(0, SeekOrigin.Begin);
                Assert.That(memoryStream.Position, Is.EqualTo(0));

                var deserializedException = (IntranetBusinessException) serializer.Deserialize(memoryStream);
                Assert.That(deserializedException, Is.Not.Null);
                Assert.That(deserializedException.Message, Is.Not.Null);
                Assert.That(deserializedException.Message, Is.EqualTo(exception.Message));
            }
            finally
            {
                memoryStream.Close();
            }
        }
    }
}
