using System;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Infrastructure
{
    /// <summary>
    /// Tester klassen IntranetRepositoryException.
    /// </summary>
    [TestFixture]
    public class IntranetRepositoryExceptionTests
    {
        /// <summary>
        /// Tester, at IntranetRepositoryException kan instantieres.
        /// </summary>
        [Test]
        public void TestAtIntranetRepositoryExceptionKanInstantieres()
        {
            var exception = new IntranetRepositoryException("Test");
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Null);

            exception = new IntranetRepositoryException("Test", new Exception());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Test"));
            Assert.That(exception.InnerException, Is.Not.Null);
            Assert.That(exception.InnerException.GetType(), Is.EqualTo(typeof(Exception)));
        }
    }
}
