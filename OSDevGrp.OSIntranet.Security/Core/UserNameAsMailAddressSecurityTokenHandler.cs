using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;
using UserNameSecurityTokenHandler = Microsoft.IdentityModel.Tokens.UserNameSecurityTokenHandler;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Security token handler for authentication of users who has a mail address as username.
    /// </summary>
    public class UserNameAsMailAddressSecurityTokenHandler : UserNameSecurityTokenHandler
    {
        #region Private variables

        private readonly SecurityTokenAuthenticator _securityTokenAuthenticator;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a security token handler for authentication of users who has a mail address as username.
        /// </summary>
        public UserNameAsMailAddressSecurityTokenHandler() 
            : this(new UserNameAsMailAddressSecurityTokenAuthenticator())
        {
        }

        /// <summary>
        /// Creates a security token handler for authentication of users who has a mail address as username.
        /// </summary>
        /// <param name="securityTokenAuthenticator">The security token authenticator to use in this security token handler.</param>
        public UserNameAsMailAddressSecurityTokenHandler(SecurityTokenAuthenticator securityTokenAuthenticator)
        {
            if (securityTokenAuthenticator == null)
            {
                throw new ArgumentNullException("securityTokenAuthenticator");
            }
            _securityTokenAuthenticator = securityTokenAuthenticator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this handler supports validation of tokens handled by this instance.
        /// </summary>
        public override bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates a security token.
        /// </summary>
        /// <param name="token">Security token to validate.</param>
        /// <returns>Collection of claims identities.</returns>
        public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            try
            {
                if (_securityTokenAuthenticator.CanValidateToken(token) == false)
                {
                    throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated));
                }
                var claimsIdentities = _securityTokenAuthenticator.ValidateToken(token)
                    .OfType<IIdentityProvider>()
                    .Where(identityProvider => identityProvider.PrimaryIdentity != null && identityProvider.PrimaryIdentity.GetType() == typeof (ClaimsIdentity))
                    .Select(identityProvider => (ClaimsIdentity) identityProvider.PrimaryIdentity)
                    .ToList();
                return new ClaimsIdentityCollection(claimsIdentities);
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated), ex);
            }
        }

        #endregion
    }
}
