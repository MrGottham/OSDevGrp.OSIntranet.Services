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
                                                            fixture.Create<string>());
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
                                                            fixture.Create<int>(),
                                                            fixture.Create<string>(),
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
                                                            fixture.Create<Type>(),
                                                            fixture.Create<string>());
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
                                                            fixture.Create<string>());
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
                                                            fixture.Create<Type>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ErrorInCommandHandlerWithoutReturnValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForErrorInCommandHandlerWithoutReturnValueHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue,
                                                            fixture.Create<string>(),
                                                            fixture.Create<string>());
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
                                                            fixture.Create<string>(),
                                                            fixture.Create<string>(),
                                                            fixture.Create<string>());
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
                                                            fixture.Create<int>());
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
                                                            fixture.Create<Type>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UserAppointmentAlreadyExists hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUserAppointmentAlreadyExistsHentes()
        {
            var fixture = new Fixture();
            fixture.Inject(typeof(Func<int, Kontogruppe>));

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentAlreadyExists);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UserAppointmentDontExists hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUserAppointmentDontExistsHentes()
        {
            var fixture = new Fixture();
            fixture.Inject(typeof(Func<int, Kontogruppe>));

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentDontExists);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NoCalendarUserWithThoseInitials hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNoCalendarUserWithThoseInitialsHentes()
        {
            var fixture = new Fixture();
            fixture.Inject(typeof(Func<int, Kontogruppe>));

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoCalendarUserWithThoseInitials);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoCalendarUserWithThoseInitials, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CertificateWasNotFound hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCertificateWasNotFoundHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CertificateWasNotFound);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CertificateWasNotFound, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for InvalidRelyingPartyAddress hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForInvalidRelyingPartyAddressHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for AppliesToMustBeSuppliedInRequestSecurityToken hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForAppliesToMustBeSuppliedInRequestSecurityTokenHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for AppliesToMustHaveX509CertificateEndpointIdentity hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForAppliesToMustHaveX509CertificateEndpointIdentityHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustHaveX509CertificateEndpointIdentity);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NotAuthorizedToUseService hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNotAuthorizedToUseServiceHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NoClaimsWasFound hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNoClaimsWasFoundHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for MissingClaimTypeForIdentity hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForMissingClaimTypeForIdentityHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for SecurityTokenCouldNotBeValidated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForSecurityTokenCouldNotBeValidatedHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UserNameAndPasswordCouldNotBeValidated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUserNameAndPasswordCouldNotBeValidatedHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UserNameAndPasswordCouldNotBeValidated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ValueMustBeGivenForProperty hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForValueMustBeGivenForPropertyHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, fixture.Create<string>());
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
