namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a configuration repository to the food waste domain.
    /// </summary>
    public interface IConfigurationRepository
    {
        /// <summary>
        /// Gets the address for the SMTP server.
        /// </summary>
        string SmtpServer { get; }
    }
}
