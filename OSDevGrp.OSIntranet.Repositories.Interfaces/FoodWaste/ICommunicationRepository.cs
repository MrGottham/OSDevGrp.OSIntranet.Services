namespace OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a repository used for communication with internal and external stakeholders in the food waste domain.
    /// </summary>
    public interface ICommunicationRepository
    {
        /// <summary>
        /// Send a mail.
        /// </summary>
        /// <param name="toMailAddress">Mail address for the receiver.</param>
        /// <param name="subject">Subject for the mail.</param>
        /// <param name="body">Body for the mail.</param>
        void SendMail(string toMailAddress, string subject, string body);
    }
}
