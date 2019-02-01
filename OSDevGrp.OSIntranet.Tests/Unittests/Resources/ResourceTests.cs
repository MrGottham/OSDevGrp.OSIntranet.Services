using System;
using System.Reflection;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Resources
{
    /// <summary>
    /// Tester klassen Resource.
    /// </summary>
    [TestFixture]
    public class ResourceTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tester, at ExceptionMessage for RepositoryError hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForRepositoryErrorHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.RepositoryError);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UnhandledSwitchValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUnhandledSwitchValueHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, _fixture.Create<int>(), _fixture.Create<string>(), MethodBase.GetCurrentMethod().Name);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CantFindObjectById hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCantFindObjectByIdHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Konto).Name, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for IllegalValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForIllegalValueHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IllegalValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, null, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CantAutoMapType hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCantAutoMapTypeHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, typeof(Konto));
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ErrorInCommandHandlerWithoutReturnValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForErrorInCommandHandlerWithoutReturnValueHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue, _fixture.Create<string>(), _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ErrorInCommandHandlerWithReturnValue hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForErrorInCommandHandlerWithReturnValueHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineDateToOld hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineDateToOldHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld, _fixture.Create<int>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineDateIsForwardInTime hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineDateIsForwardInTimeHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateIsForwardInTime);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineAccountNumberMissing hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineAccountNumberMissingHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineAccountNumberMissing);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineTextMissing hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineTextMissingHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineTextMissing);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineValueBelowZero hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineValueBelowZeroHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BalanceLineValueMissing hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBalanceLineValueMissingHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueMissing);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for AccountIsOverdrawn hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForAccountIsOverdrawnHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.AccountIsOverdrawn);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for BudgetAccountIsOverdrawn hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForBudgetAccountIsOverdrawnHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountIsOverdrawn);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NoRegistrationForDelegate hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNoRegistrationForDelegateHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, typeof(Func<int, Kontogruppe>));
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UserAppointmentAlreadyExists hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUserAppointmentAlreadyExistsHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentAlreadyExists);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UserAppointmentDontExists hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUserAppointmentDontExistsHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UserAppointmentDontExists);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NoCalendarUserWithThoseInitials hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNoCalendarUserWithThoseInitialsHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoCalendarUserWithThoseInitials);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoCalendarUserWithThoseInitials, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CertificateWasNotFound hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCertificateWasNotFoundHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CertificateWasNotFound);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CertificateWasNotFound, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for InvalidRelyingPartyAddress hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForInvalidRelyingPartyAddressHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.InvalidRelyingPartyAddress, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for AppliesToMustBeSuppliedInRequestSecurityToken hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForAppliesToMustBeSuppliedInRequestSecurityTokenHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustBeSuppliedInRequestSecurityToken);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for AppliesToMustHaveX509CertificateEndpointIdentity hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForAppliesToMustHaveX509CertificateEndpointIdentityHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.AppliesToMustHaveX509CertificateEndpointIdentity);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NotAuthorizedToUseService hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNotAuthorizedToUseServiceHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for NoClaimsWasFound hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForNoClaimsWasFoundHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.NoClaimsWasFound);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for MissingClaimTypeForIdentity hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForMissingClaimTypeForIdentityHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.MissingClaimTypeForIdentity);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for SecurityTokenCouldNotBeValidated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForSecurityTokenCouldNotBeValidatedHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for UserNameAndPasswordCouldNotBeValidated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForUserNameAndPasswordCouldNotBeValidatedHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.UserNameAndPasswordCouldNotBeValidated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for IdentifierUnknownToSystem hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForIdentifierUnknownToSystemHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ValueMustBeGivenForProperty hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForValueMustBeGivenForPropertyHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ValueForPropertyContainsIllegalChars hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForValueForPropertyContainsIllegalCharsHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdLimitHasBeenReached hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdLimitHasBeenReachedHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdLimitHasBeenReached);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberNotCreated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberNotCreatedHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberNotActivated hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberNotActivatedHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotActivated);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberHasNotAcceptedPrivacyPolicy hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberHasNotAcceptedPrivacyPolicyHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotAcceptedPrivacyPolicy);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberHasNotRequiredMembership hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberHasNotRequiredMembershipHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for WrongActivationCodeForHouseholdMember hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForWrongActivationCodeForHouseholdMemberHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.WrongActivationCodeForHouseholdMember);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for GenericTypeHasInvalidType hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForGenericTypeHasInvalidTypeHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.GenericTypeHasInvalidType, _fixture.Create<string>(), typeof (object).Name);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for ValueForPropertyIsInvalid hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForValueForPropertyIsInvalidHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyIsInvalid);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyIsInvalid, _fixture.Create<string>(), _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for MembershipCannotDowngrade hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForMembershipCannotDowngradeHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.MembershipCannotDowngrade);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for DateTimeValueForPropertyIsNotInPast hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForDateTimeValueForPropertyIsNotInPastHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInPast);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInPast, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for DateTimeValueForPropertyIsNotInFuture hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForDateTimeValueForPropertyIsNotInFutureHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInFuture);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInFuture, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for DataProviderDoesNotHandlesPayments hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForDataProviderDoesNotHandlesPaymentsHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DataProviderDoesNotHandlesPayments);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.DataProviderDoesNotHandlesPayments, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for LengthForPropertyIsInvalid hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForLengthForPropertyIsInvalidHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, _fixture.Create<string>(), _fixture.Create<int>(), _fixture.Create<int>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for CannotModifyHouseholdMembershipForYourself hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForCannotModifyHouseholdMembershipForYourselffHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.CannotModifyHouseholdMembershipForYourself);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberAlreadyExistsOnHousehold hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberAlreadyExistsOnHouseholdHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberAlreadyExistsOnHousehold);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberAlreadyExistsOnHousehold, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for HouseholdMemberDoesNotExistOnHousehold hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForHouseholdMemberDoesNotExistOnHouseholdHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberDoesNotExistOnHousehold);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberDoesNotExistOnHousehold, _fixture.Create<string>());
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tester, at ExceptionMessage for OperationNotAllowedOnStorage hentes.
        /// </summary>
        [Test]
        public void TestAtExceptionMessageForOperationNotAllowedOnStorageHentes()
        {
            string exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.OperationNotAllowedOnStorage);
            Assert.That(exceptionMessage, Is.Not.Null);
            Assert.That(exceptionMessage.Length, Is.GreaterThan(0));

            exceptionMessage = Resource.GetExceptionMessage(ExceptionMessage.OperationNotAllowedOnStorage, _fixture.Create<string>());
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
