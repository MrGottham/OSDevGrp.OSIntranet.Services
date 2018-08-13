using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
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

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, fixture.Create<Type>().Name, fixture.Create<string>());
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

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue, fixture.Create<string>(), fixture.Create<string>());
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

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
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
        /// Tester, at ExceptionMessage for IdentifierUnknownToSystem hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForIdentifierUnknownToSystemHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, fixture.Create<string>());
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
        /// Tester, at ExceptionMessage for ValueForPropertyContainsIllegalChars hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForValueForPropertyContainsIllegalCharsHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdLimitHasBeenReached hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdLimitHasBeenReachedHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdLimitHasBeenReached);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberNotCreated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberNotCreatedHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberNotActivated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberNotActivatedHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotActivated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberHasNotAcceptedPrivacyPolicy hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberHasNotAcceptedPrivacyPolicyHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotAcceptedPrivacyPolicy);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberHasNotRequiredMembership hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberHasNotRequiredMembershipHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for WrongActivationCodeForHouseholdMember hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForWrongActivationCodeForHouseholdMemberHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.WrongActivationCodeForHouseholdMember);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for GenericTypeHasInvalidType hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForGenericTypeHasInvalidTypeHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType, fixture.Create<string>(), typeof (object).Name);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ValueForPropertyIsInvalid hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForValueForPropertyIsInvalidHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyIsInvalid);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyIsInvalid, fixture.Create<string>(), fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for MembershipCannotDowngrade hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForMembershipCannotDowngradeHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.MembershipCannotDowngrade);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for DateTimeValueForPropertyIsNotInPast hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForDateTimeValueForPropertyIsNotInPastHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInPast);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInPast, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for DateTimeValueForPropertyIsNotInFuture hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForDateTimeValueForPropertyIsNotInFutureHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInFuture);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInFuture, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for DataProviderDoesNotHandlesPayments hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForDataProviderDoesNotHandlesPaymentsHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DataProviderDoesNotHandlesPayments);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DataProviderDoesNotHandlesPayments, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for LengthForPropertyIsInvalid hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForLengthForPropertyIsInvalidHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, fixture.Create<string>(), fixture.Create<int>(), fixture.Create<int>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CannotModifyHouseholdMembershipForYourself hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCannotModifyHouseholdMembershipForYourselffHentes()
        {
            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CannotModifyHouseholdMembershipForYourself);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberAlreadyExistsOnHousehold hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberAlreadyExistsOnHouseholdHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberAlreadyExistsOnHousehold);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberAlreadyExistsOnHousehold, fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberDoesNotExistOnHousehold hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberDoesNotExistOnHouseholdHentes()
        {
            var fixture = new Fixture();

            var exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberDoesNotExistOnHousehold);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberDoesNotExistOnHousehold, fixture.Create<string>());
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
