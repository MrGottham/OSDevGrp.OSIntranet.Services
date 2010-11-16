using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Services.Implementations;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Services
{
    /// <summary>
    /// Test af basisklasse for en service til OS Intranet.
    /// </summary>
    [TestFixture]
    public class IntranetServiceBaseTests
    {
        /// <summary>
        /// Privat klasse til test af den abstrakte basisklasse for en service til OS Intranet.
        /// </summary>
        private class TestSerivce : IntranetServiceBase
        {
            /// <summary>
            /// Returnerer en fault for en fejl i repositories.
            /// </summary>
            /// <param name="exception">Exception fra repositories.</param>
            /// <returns>Fault.</returns>
            public FaultException<IntranetRepositoryFault> GetIntranetRepositoryFault(IntranetRepositoryException exception)
            {
                return CreateIntranetRepositoryFault(exception);
            }

            /// <summary>
            /// Returnerer en fault for en fejl i forretningslogik.
            /// </summary>
            /// <param name="exception">Exception for fejl i forretningslogik</param>
            /// <returns>Fault.</returns>
            public FaultException<IntranetBusinessFault> GetIntranetBusinessFault(IntranetBusinessException exception)
            {
                return CreateIntranetBusinessFault(exception);
            }

            /// <summary>
            /// Returnerer en fault for en systemfejl.
            /// </summary>
            /// <param name="exception">Exception for systemfejl.</param>
            /// <returns>Fault.</returns>
            public FaultException<IntranetSystemFault> GetIntranetSystemFault(IntranetSystemException exception)
            {
                return CreateIntranetSystemFault(exception);
            }

            /// <summary>
            /// Returnerer en fault for en unhandled exception.
            /// </summary>
            /// <param name="exception">Unhandled exception.</param>
            /// <returns>Fault.</returns>
            public FaultException<IntranetSystemFault> GetIntranetSystemFault(Exception exception)
            {
                return CreateIntranetSystemFault(exception);
            }
        }

        /// <summary>
        /// Tester, at CreateIntranetRepositoryFault danner en IntranetRepositoryFault.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetRepositoryFaultDannerIntranetRepositoryFault()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);

            var exception = new IntranetRepositoryException("Test", new NotSupportedException());
            var fault = service.GetIntranetRepositoryFault(exception);
            Assert.That(fault, Is.Not.Null);
            Assert.That(fault, Is.TypeOf(typeof(FaultException<IntranetRepositoryFault>)));
        }

        /// <summary>
        /// Tester, at CreateIntranetRepositoryFault kaster en ArgumentNullException, hvis IntranetRepositoryException er null.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetRepositoryFaultKasterArgumentNullExceptionHvisIntranetRepositoryExceptionErNull()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => service.GetIntranetRepositoryFault(null));
        }

        /// <summary>
        /// Tester, at CreateIntranetBusinessFault danner en IntranetBusinessFault.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetBusinessFaultDannerIntranetBusinessFault()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);

            var exception = new IntranetBusinessException("Test", new NotSupportedException());
            var fault = service.GetIntranetBusinessFault(exception);
            Assert.That(fault, Is.Not.Null);
            Assert.That(fault, Is.TypeOf(typeof(FaultException<IntranetBusinessFault>)));
        }

        /// <summary>
        /// Tester, at CreateIntranetBusinessFault kaster en ArgumentNullException, hvis IntranetBusinessException er null.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetBusinessFaultKasterArgumentNullExceptionHvisIntranetBusinessExceptionErNull()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);
            Assert.Throws<ArgumentNullException>(() => service.GetIntranetBusinessFault(null));
        }

        /// <summary>
        /// Tester, at CreateIntranetSystemFault danner en IntranetSystemFault.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetSystemFaultDannerIntranetSystemFault()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);

            var exception = new IntranetSystemException("Test", new NotSupportedException());
            var fault = service.GetIntranetSystemFault(exception);
            Assert.That(fault, Is.Not.Null);
            Assert.That(fault, Is.TypeOf(typeof(FaultException<IntranetSystemFault>)));
        }

        /// <summary>
        /// Tester, at CreateIntranetSystemFault kaster en ArgumentNullException, hvis IntranetSystemException er null.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetSystemFaultKasterArgumentNullExceptionHvisIntranetSystemExceptionErNull()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);
            IntranetSystemException intranetSystemException = null;
            Assert.Throws<ArgumentNullException>(() => service.GetIntranetSystemFault(intranetSystemException));
        }

        /// <summary>
        /// Tester, at CreateIntranetSystemFault danner en IntranetSystemFault for en unhandled exception.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetSystemFaultDannerIntranetSystemFaultForUnhandledException()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);

            var exception = new Exception("Test", new NotSupportedException());
            var fault = service.GetIntranetSystemFault(exception);
            Assert.That(fault, Is.Not.Null);
            Assert.That(fault, Is.TypeOf(typeof(FaultException<IntranetSystemFault>)));
        }

        /// <summary>
        /// Tester, at CreateIntranetSystemFault kaster en ArgumentNullException, hvis unhandled Exception er null.
        /// </summary>
        [Test]
        public void TestAtCreateIntranetSystemFaultKasterArgumentNullExceptionHvisUnhandledExceptionErNull()
        {
            var service = new TestSerivce();
            Assert.That(service, Is.Not.Null);
            Exception exception = null;
            Assert.Throws<ArgumentNullException>(() => service.GetIntranetSystemFault(exception));
        }
    }
}
