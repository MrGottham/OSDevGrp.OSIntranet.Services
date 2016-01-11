namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a household.
    /// </summary>
    public interface IHousehold : IIdentifiable
    {
        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        string Description { get; set; }
    }
}
