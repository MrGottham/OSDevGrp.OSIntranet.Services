using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using OSDevGrp.OSIntranet.Resources;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Resources
{
    /// <summary>
    /// Tester klassen ResourceException.
    /// </summary>
    [TestFixture]
    public class ResourceExceptionTests
    {
        /// <summary>
        /// Tester, at ResourceException kan instantieres.
        /// </summary>
        [Test]
        public void TestAtResourceExceptionKanInstantieres()
        {
            var exception = new ResourceException("Test");
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Null);

            exception = new ResourceException("Test", new Exception());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.GetType(), Is.EqualTo(typeof(Exception)));
        }

        /// <summary>
        /// Tester, at ResourceException kan serialiseres og deserialiseres.
        /// </summary>
        [Test]
        public void TestAtResourceExceptionKanSerialiseresOgDeserialiseres()
        {
            var exception = new ResourceException("Test");
            Assert.That(exception, Is.Not.Null);
            var memoryStream = new MemoryStream();
            try
            {
                var serializer = new SoapFormatter();
                serializer.Serialize(memoryStream, exception);
                Assert.That(memoryStream.Length, Is.GreaterThan(0));

                memoryStream.Seek(0, SeekOrigin.Begin);
                Assert.That(memoryStream.Position, Is.EqualTo(0));

                var deserializedException = (ResourceException)serializer.Deserialize(memoryStream);
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
