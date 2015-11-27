using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories.Interface.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.FoodWaste
{
    /// <summary>
    /// Configuration repository to the food waste domain.
    /// </summary>
    public class ConfigurationRepository : KonfigurationRepositoryBase, IConfigurationRepository
    {
        #region Privaee constants

        private const string Iv = "5mhI+y042nU=";
        private const string Key = "EifruIRUF3HxmrR061aUc4ozOrqebJ8b";

        #endregion

        #region Private variables

        private readonly string _smtpServer;
        private readonly string _smtpUserName;
        private readonly string _smtpPassword;
        private readonly string _fromMailAddress;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a configuration repository to the food waste domain.
        /// </summary>
        /// <param name="nameValueCollection">Name value collection containing configurations.</param>
        public ConfigurationRepository(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException("nameValueCollection");
            }
            try
            {
                _smtpServer = base.GetStringFromApplicationSettings(nameValueCollection, "SmtpServer");
                _smtpUserName = base.GetStringFromApplicationSettings(nameValueCollection, "SmtpUserName");
                _smtpPassword = base.GetStringFromApplicationSettings(nameValueCollection, "SmtpPassword");
                _fromMailAddress = base.GetStringFromApplicationSettings(nameValueCollection, "FromMailAddress");
            }
            catch (CommonRepositoryException ex)
            {
                throw new IntranetRepositoryException(ex.Message, ex);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the address for the SMTP server.
        /// </summary>
        public virtual string SmtpServer
        {
            get { return _smtpServer; }
        }

        /// <summary>
        /// Gets the user name for the SMTP server.
        /// </summary>
        public virtual string SmtpUserName
        {
            get { return _smtpUserName; }
        }

        /// <summary>
        /// Gets the password for the SMTP server.
        /// </summary>
        public virtual string SmtpPassword
        {
            get { return DecryptValue(_smtpPassword); }
        }

        /// <summary>
        /// Gets the mail address from which to send the mails.
        /// </summary>
        public virtual string FromMailAddress
        {
            get { return _fromMailAddress; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Encrypts a value.
        /// </summary>
        /// <param name="value">Value to encrypt.</param>
        /// <returns>Encrypted value.</returns>
        public static string EncryptValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            using (var tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                using (var encryptor = tripleDesCryptoServiceProvider.CreateEncryptor(Convert.FromBase64String(Key), Convert.FromBase64String(Iv)))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(value);
                                streamWriter.Flush();
                            }
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a value.
        /// </summary>
        /// <param name="value">Value to descrypt.</param>
        /// <returns>Decrypted value.</returns>
        public static string DecryptValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            using (var tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                using (var encryptor = tripleDesCryptoServiceProvider.CreateDecryptor(Convert.FromBase64String(Key), Convert.FromBase64String(Iv)))
                {
                    using (var memoryStream = new MemoryStream(Convert.FromBase64String(value)))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read))
                        {
                            using (var streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
