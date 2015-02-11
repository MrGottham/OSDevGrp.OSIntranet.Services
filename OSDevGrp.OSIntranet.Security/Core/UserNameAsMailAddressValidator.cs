using System;
using System.IdentityModel.Selectors;
using System.Security;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Validator which can validate whether an username is a mail address.
    /// </summary>
    public class UserNameAsMailAddressValidator : UserNamePasswordValidator
    {
        #region Private variables

        private readonly ICommonValidation _commonValidation;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a validator which can validate whether an username is a mail address.
        /// </summary>
        public UserNameAsMailAddressValidator()
            : this(new CommonValidation())
        {
        }

        /// <summary>
        /// Creates a validator which can validate whether an username is a mail address.
        /// </summary>
        /// <param name="commonValidation">Implementation of the common validations.</param>
        public UserNameAsMailAddressValidator(ICommonValidation commonValidation)
        {
            if (commonValidation == null)
            {
                throw new ArgumentNullException("commonValidation");
            }
            _commonValidation = commonValidation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the specified username and password.
        /// </summary>
        /// <param name="userName">The username to validate.</param>
        /// <param name="password">The password to validate.</param>
        public override void Validate(string userName, string password)
        {
            try
            {
                if (_commonValidation.IsMailAddress(userName))
                {
                    return;
                }
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.UserNameAndPasswordCouldNotBeValidated));
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.UserNameAndPasswordCouldNotBeValidated), ex);
            }
        }

        #endregion
    }
}
