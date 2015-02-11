using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Security token authenticator which can authenticate users who has a mail address as username.
    /// </summary>
    public class UserNameAsMailAddressSecurityTokenAuthenticator : CustomUserNameSecurityTokenAuthenticator
    {
        #region Private variables

        private readonly IIdentityBuilder _identityBuilder;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a security token authenticator which can authenticate users who has a mail address as username.
        /// </summary>
        public UserNameAsMailAddressSecurityTokenAuthenticator()
            : this(new UserNameAsMailAddressValidator(), new IdentityBuilder())
        {
        }

        /// <summary>
        /// Creates a security token authenticator which can authenticate users who has a mail address as username.
        /// </summary>
        /// <param name="validator">Validator which can validate username and password.</param>
        /// <param name="identityBuilder">Functionality which can build an identity.</param>
        public UserNameAsMailAddressSecurityTokenAuthenticator(UserNamePasswordValidator validator, IIdentityBuilder identityBuilder) 
            : base(validator)
        {
            if (identityBuilder == null)
            {
                throw new ArgumentNullException("identityBuilder");
            }
            _identityBuilder = identityBuilder;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Authenticates the specified security token and returns the set of authorization policies for the security token.
        /// </summary>
        /// <param name="token">The security token which should be authenticated.</param>
        /// <returns>Set of authorization policies for the authenticated security token.</returns>
        protected override ReadOnlyCollection<IAuthorizationPolicy> ValidateTokenCore(SecurityToken token)
        {
            try
            {
                var authorizationPolicies = base.ValidateTokenCore(token);
                if (authorizationPolicies == null)
                {
                    throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.SecurityTokenCouldNotBeValidated));
                }
                var identityProperties = new Dictionary<string, string>
                {
                    {ClaimTypes.Email, ((UserNameSecurityToken) token).UserName}
                };
                return new List<IAuthorizationPolicy>(authorizationPolicies)
                {
                    new UserNameAsMailAddressAuthorizationPolicy(_identityBuilder.Build(token, identityProperties))
                }.AsReadOnly();
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
