using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Resources;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Resources
{
    /// <summary>
    /// Tester klassen Resource.
    /// </summary>
    [TestFixture]
    public class ResourceTests
    {
        /// <summary>
        /// Tester, at ExceptionMessage for RepositoryError hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForRepositoryErrorHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.RepositoryError,
                                                                MethodBase.GetCurrentMethod().Name, "Test");
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UnhandledSwitchValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUnhandledSwitchValueHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, "1", "Test",
                                                                MethodBase.GetCurrentMethod().Name);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CantFindObjectById hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCantFindObjectByIdHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Konto), "XYZ");
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ResourceException kastes, hvis ExceptionMessage ikke findes.
        /// </summary>
        [Test]
        public void TestAtResourceExceptionKastesHvisExceptionMessageIkkeFindes()
        {
            Assert.Throws<ResourceException>(() => Resource.GetExceptionMessage((ExceptionMessage) 100));
            Assert.Throws<ResourceException>(() => Resource.GetExceptionMessage((ExceptionMessage) 100, 1, 2, 3));
        }
    }
}
