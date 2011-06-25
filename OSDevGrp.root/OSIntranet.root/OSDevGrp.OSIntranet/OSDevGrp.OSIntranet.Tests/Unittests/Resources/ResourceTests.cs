using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
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
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.RepositoryError);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.RepositoryError,
                                                            MethodBase.GetCurrentMethod().Name,
                                                            fixture.CreateAnonymous<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UnhandledSwitchValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUnhandledSwitchValueHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue,
                                                            fixture.CreateAnonymous<int>(),
                                                            fixture.CreateAnonymous<string>(),
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
            var fixture = new Fixture();
            fixture.Inject(typeof(Konto));

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                            fixture.CreateAnonymous<Type>(),
                                                            fixture.CreateAnonymous<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for IllegalValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForIllegalValueHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IllegalValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, null,
                                                            fixture.CreateAnonymous<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CantAutoMapType hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCantAutoMapTypeHentes()
        {
            var fixture = new Fixture();
            fixture.Inject(typeof(Konto));

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType,
                                                            fixture.CreateAnonymous<Type>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ErrorInCommandHandlerWithReturnValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForErrorInCommandHandlerWithReturnValueHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue,
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>(),
                                                            fixture.CreateAnonymous<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineDateToOld hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineDateToOldHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld,
                                                            fixture.CreateAnonymous<int>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineDateIsForwardInTime hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineDateIsForwardInTimeHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateIsForwardInTime);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineAccountNumberMissing hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineAccountNumberMissingHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineAccountNumberMissing);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineTextMissing hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineTextMissingHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineTextMissing);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineValueBelowZero hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineValueBelowZeroHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineValueMissing hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineValueMissingHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueMissing);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for AccountIsOverdrawn hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForAccountIsOverdrawnHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.AccountIsOverdrawn);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BudgetAccountIsOverdrawn hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBudgetAccountIsOverdrawnHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountIsOverdrawn);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NoRegistrationForDelegate hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNoRegistrationForDelegateHentes()
        {
            var fixture = new Fixture();
            fixture.Inject(typeof(Func<int, Kontogruppe>));

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate,
                                                            fixture.CreateAnonymous<Type>());
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
