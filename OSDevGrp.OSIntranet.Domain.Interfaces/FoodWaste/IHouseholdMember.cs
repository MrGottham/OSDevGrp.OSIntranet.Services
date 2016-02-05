﻿using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a household member.
    /// </summary>
    public interface IHouseholdMember : IStakeholder
    {
        /// <summary>
        /// Membership.
        /// </summary>
        Membership Membership { get; }

        /// <summary>
        /// Date and time for when the membership expires.
        /// </summary>
        DateTime? MembershipExpireTime { get; }

        /// <summary>
        /// Activation code for the household member.
        /// </summary>
        string ActivationCode { get; }

        /// <summary>
        /// Date and time for when the household member was activated.
        /// </summary>
        DateTime? ActivationTime { get; set; }

        /// <summary>
        /// Indicates whether the household member is activated.
        /// </summary>
        bool IsActivated { get; }

        /// <summary>
        /// Date and time for when the household member has accepted our privacy policy.
        /// </summary>
        DateTime? PrivacyPolicyAcceptedTime { get; set; }

        /// <summary>
        /// Indicates whether the household member has accepted our privacy policy.
        /// </summary>
        bool IsPrivacyPolictyAccepted { get; }

        /// <summary>
        /// Date and time for when the household member was created.
        /// </summary>
        DateTime CreationTime { get; }

        /// <summary>
        /// Households on which the household member has a membership.
        /// </summary>
        IEnumerable<IHousehold> Households { get; }

        /// <summary>
        /// Payments made by the household member.
        /// </summary>
        IEnumerable<IPayment> Payments { get; }

        /// <summary>
        /// Validates whether the household members current membership matches the required membership.
        /// </summary>
        /// <param name="requiredMembership">Required membership.</param>
        /// <returns>True when the household members current membership matches the required membership otherwise false.</returns>
        bool HasRequiredMembership(Membership requiredMembership);

        /// <summary>
        /// Applies a new membership to the household member.
        /// </summary>
        /// <param name="membership">Membership which should be applied to the household member.</param>
        void MembershipApply(Membership membership);

        /// <summary>
        /// Adds a household to the household member
        /// </summary>
        /// <param name="household">Household on which the household member has a membership.</param>
        void HouseholdAdd(IHousehold household);

        /// <summary>
        /// Adds a payment made by the household member.
        /// </summary>
        /// <param name="payment">Payment made by the household member.</param>
        void PaymentAdd(IPayment payment);
    }
}
