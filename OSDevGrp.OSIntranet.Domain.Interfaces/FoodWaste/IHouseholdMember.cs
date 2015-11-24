using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a household member.
    /// </summary>
    public interface IHouseholdMember : IIdentifiable
    {
        /// <summary>
        /// Mail address for the household member.
        /// </summary>
        string MailAddress { get; }

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
        /// Date and time for when the household member was created.
        /// </summary>
        DateTime CreationTime { get; }
    }
}
