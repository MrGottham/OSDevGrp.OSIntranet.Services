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
        NoRegistrationForDelegate
    }
}
