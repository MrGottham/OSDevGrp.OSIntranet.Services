using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Repository used for communication with the household members in the food waste domain.
    /// </summary>
    public class CommunicationRepository : ICommunicationRepository
    {
        #region Private variables

        private readonly IConfigurationRepository _configurationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a repository used for communication with the household members in the food waste domain.
        /// </summary>
        /// <param name="configurationRepository">Implementation of the configuration repository to the food waste domain.</param>
        public CommunicationRepository(IConfigurationRepository configurationRepository)
        {
            if (configurationRepository == null)
            {
                throw new ArgumentNullException("configurationRepository");
            }
            _configurationRepository = configurationRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Send a mail.
        /// </summary>
        /// <param name="toMailAddress">Mail address for the receiver.</param>
        /// <param name="subject">Subject for the mail.</param>
        /// <param name="body">Body for the mail.</param>
        public void SendMail(string toMailAddress, string subject, string body)
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
            try
            {
                var mailMessage = new MailMessage(_configurationRepository.FromMailAddress, toMailAddress, subject, body)
                {
                    IsBodyHtml = true,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };

                using (var smtpClient = new SmtpClient(_configurationRepository.SmtpServer, _configurationRepository.SmtpPort))
                {
                    if (_configurationRepository.UseSmtpAuthentication)
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_configurationRepository.SmtpUserName, _configurationRepository.SmtpPassword);
                    }
                    smtpClient.EnableSsl = _configurationRepository.UseSmtpSecureConnection;
                    smtpClient.Send(mailMessage);
                }
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
            }
        }

        #endregion
    }
}
