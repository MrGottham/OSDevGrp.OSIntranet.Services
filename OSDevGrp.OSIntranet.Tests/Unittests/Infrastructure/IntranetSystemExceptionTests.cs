using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester klassen IntranetSystemException.
    /// </summary>
    [TestFixture]
    public class IntranetSystemExceptionTests
    {
        /// <summary>
        /// Tester, at IntranetSystemException kan instantieres.
        /// </summary>
        [Test]
        public void TestAtIntranetSystemExceptionKanInstantieres()
        {
            var exception = new IntranetSystemException("Test");
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Null);

            exception = new IntranetSystemException("Test", new Exception());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.GetType(), Is.EqualTo(typeof(Exception)));
        }
    }
}
