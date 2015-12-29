namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for an internal or external stakeholder.
    /// </summary>
    public interface IStakeholder : IIdentifiable
    {
        /// <summary>
        /// Mail address for the internal or external stakeholder.
        /// </summary>
        string MailAddress { get; }
    }
}
