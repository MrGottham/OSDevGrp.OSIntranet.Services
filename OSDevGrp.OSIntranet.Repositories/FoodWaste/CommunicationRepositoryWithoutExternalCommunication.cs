using System;
using System.Diagnostics;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Repository without external communication used for communication with internal and external stakeholders in the food waste domain.
    /// </summary>
    public class CommunicationRepositoryWithoutExternalCommunication : ICommunicationRepository
    {
        #region Methods

        /// <summary>
        /// Send a mail.
        /// </summary>
        /// <param name="toMailAddress">Mail address for the receiver.</param>
        /// <param name="subject">Subject for the mail.</param>
        /// <param name="body">Body for the mail.</param>
        public virtual void SendMail(string toMailAddress, string subject, string body)
        {
            if (string.IsNullOrEmpty(toMailAddress))
            {
                throw new ArgumentNullException("toMailAddress");
            }
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException("subject");
            }
            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException("body");
            }
            Debug.WriteLine(string.Format("Mail to '{0}' has been send: {1}", toMailAddress, subject));
        }

        #endregion
    }
}
