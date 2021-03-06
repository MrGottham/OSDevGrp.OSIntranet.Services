﻿namespace OSDevGrp.OSIntranet.Resources
{
    /// <summary>
    /// Exception messages.
    /// </summary>
    public enum ExceptionMessage
    {
        RepositoryError,
        UnhandledSwitchValue,
        CantFindObjectById,
        IllegalValue,
        CantAutoMapType,
        ErrorInCommandHandlerWithoutReturnValue,
        ErrorInCommandHandlerWithReturnValue,
        BalanceLineDateToOld,
        BalanceLineDateIsForwardInTime,
        BalanceLineAccountNumberMissing,
        BalanceLineTextMissing,
        BalanceLineValueBelowZero,
        BalanceLineValueMissing,
        AccountIsOverdrawn,
        BudgetAccountIsOverdrawn,
        NoRegistrationForDelegate,
        UserAppointmentAlreadyExists,
        UserAppointmentDontExists,
        NoCalendarUserWithThoseInitials,
        CertificateWasNotFound,
        InvalidRelyingPartyAddress,
        AppliesToMustBeSuppliedInRequestSecurityToken,
        AppliesToMustHaveX509CertificateEndpointIdentity,
        NotAuthorizedToUseService,
        NoClaimsWasFound,
        MissingClaimTypeForIdentity,
        SecurityTokenCouldNotBeValidated,
        UserNameAndPasswordCouldNotBeValidated,
        IdentifierUnknownToSystem,
        ValueMustBeGivenForProperty,
        ValueForPropertyContainsIllegalChars,
        HouseholdLimitHasBeenReached,
        HouseholdMemberNotCreated,
        HouseholdMemberNotActivated,
        HouseholdMemberHasNotAcceptedPrivacyPolicy,
        HouseholdMemberHasNotRequiredMembership,
        WrongActivationCodeForHouseholdMember,
        GenericTypeHasInvalidType,
        ValueForPropertyIsInvalid,
        MembershipCannotDowngrade,
        DateTimeValueForPropertyIsNotInPast,
        DateTimeValueForPropertyIsNotInFuture,
        DataProviderDoesNotHandlesPayments,
        LengthForPropertyIsInvalid,
        CannotModifyHouseholdMembershipForYourself,
        HouseholdMemberAlreadyExistsOnHousehold,
        HouseholdMemberDoesNotExistOnHousehold,
        OperationNotAllowedOnStorage,
    }
}
