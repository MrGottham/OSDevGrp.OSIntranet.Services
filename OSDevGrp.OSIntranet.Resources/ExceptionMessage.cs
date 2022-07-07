namespace OSDevGrp.OSIntranet.Resources
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
        ErrorInCommandHandlerWithoutReturnValue,
        ErrorInCommandHandlerWithReturnValue,
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
        OperationNotAllowedOnStorage
    }
}