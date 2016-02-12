using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for an internal or external stakeholder.
    /// </summary>
    public interface IStakeholder : IIdentifiable
    {
        /// <summary>
        /// Type of the internal or external stakeholder.
        /// </summary>
        StakeholderType StakeholderType { get; }

        /// <summary>
        /// Mail address for the internal or external stakeholder.
        /// </summary>
        string MailAddress { get; }
    }
}
