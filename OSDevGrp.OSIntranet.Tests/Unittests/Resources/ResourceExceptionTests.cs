using System;
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
    }
}
