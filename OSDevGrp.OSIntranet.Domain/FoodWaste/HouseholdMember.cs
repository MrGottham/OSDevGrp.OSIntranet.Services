using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Household member.
    /// </summary>
    public class HouseholdMember : IdentifiableBase, IHouseholdMember
    {
        #region Private variables

        private string _mailAddress;
        private string _activationCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        public HouseholdMember(string mailAddress)
        {
            if (string.IsNullOrEmpty(mailAddress))
            {
                throw new ArgumentNullException("mailAddress");
            }
            if (IsMailAddress(mailAddress) == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, mailAddress, "mailAddress"));
            }
            _mailAddress = mailAddress;
            _activationCode = GenerateActivationCode();
        }

        /// <summary>
        /// Creates a household member.
        /// </summary>
        protected HouseholdMember()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mail address for the household member.
        /// </summary>
        public virtual string MailAddress
        {
            get
            {
                return _mailAddress;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (IsMailAddress(value) == false)
                {
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "value"));
                }
                _mailAddress = value;
            }
        }

        /// <summary>
        /// Activation code for the household member.
        /// </summary>
        public virtual string ActivationCode
        {
            get
            {
                return _activationCode;
            }
            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _activationCode = value;
            }
        }

        /// <summary>
        /// Date and time for when the household member was activated.
        /// </summary>
        public virtual DateTime? ActivationTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Indicates whether the household member is activated.
        /// </summary>
        public virtual bool IsActivated
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Date and time for when the household member was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Validates whether a value is a mail address.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if the value is a mail address otherwise false.</returns>
        private static bool IsMailAddress(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            var regularExpression = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.Compiled);
            return regularExpression.IsMatch(value);
        }

        /// <summary>
        /// Generates an activation code.
        /// </summary>
        /// <returns>Activation code.</returns>
        private static string GenerateActivationCode()
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("D")));

                var activationCodeBuilder = new StringBuilder();
                for (var i = 0; i < hash.Length; i++)
                {
                    activationCodeBuilder.Append(hash[i].ToString("X2"));
                }
                return activationCodeBuilder.ToString();
            }
        }

        #endregion
    }
}
