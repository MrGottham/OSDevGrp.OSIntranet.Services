using System;
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
    }
}
