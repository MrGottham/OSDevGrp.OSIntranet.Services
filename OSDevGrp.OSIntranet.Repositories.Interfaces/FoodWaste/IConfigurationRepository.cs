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

        /// <summary>
        /// Gets the user name for the SMTP server.
        /// </summary>
        string SmtpUserName { get; }

        /// <summary>
        /// Gets the password for the SMTP server.
        /// </summary>
        string SmtpPassword { get; }

        /// <summary>
        /// Gets the mail address from which to send the mails.
        /// </summary>
        string FromMailAddress { get; }
    }
}
