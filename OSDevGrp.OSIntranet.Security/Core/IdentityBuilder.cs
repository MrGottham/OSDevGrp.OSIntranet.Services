using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security;
using System.Security.Principal;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Security.Configuration;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Functionality which can build an identity.
    /// </summary>
    public class IdentityBuilder : IIdentityBuilder
    {
        #region Methods

        /// <summary>
        /// Build an identity from a security token.
        /// </summary>
        /// <param name="securityToken">Security token from which to build the identity.</param>
        /// <param name="identityProperties">Dictonary containing properties for the identity.</param>
        /// <returns>Identity build from the security token.</returns>
        public virtual IIdentity Build(SecurityToken securityToken, IDictionary<string, string> identityProperties = null)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException("securityToken");
            }
            if (securityToken.GetType() == typeof (UserNameSecurityToken))
            {
                return Build((UserNameSecurityToken) securityToken, identityProperties == null ? null : identityProperties.Select(m => new KeyValuePair<string, string>(m.Key, m.Value)));
            }
            throw new SecurityException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, securityToken, "securityToken"));
        }

        /// <summary>
        /// Build an identity from a username security token.
        /// </summary>
        /// <param name="userNameSecurityToken">Username security token from which to build the identity.</param>
        /// <param name="identityProperties">Dictonary containing properties for the identity.</param>
        /// <returns>Identity build from the username security token.</returns>
        private static IIdentity Build(UserNameSecurityToken userNameSecurityToken, IEnumerable<KeyValuePair<string, string>> identityProperties)
        {
            if (userNameSecurityToken == null)
            {
                throw new ArgumentNullException("userNameSecurityToken");
            }
            var issuerTokenName = ConfigurationProvider.Instance.IssuerTokenName.Address;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userNameSecurityToken.UserName, ClaimValueTypes.String, issuerTokenName)
            };
            if (identityProperties == null)
            {
                return Build(userNameSecurityToken, claims);
            }
            foreach (var identityProperty in identityProperties.Where(m => claims.Any(c => string.Compare(c.ClaimType, m.Key, StringComparison.Ordinal) == 0) == false))
            {
                claims.Add(new Claim(identityProperty.Key, identityProperty.Value, ClaimValueTypes.String, issuerTokenName));
            }
            return Build(userNameSecurityToken, claims);
        }

        /// <summary>
        /// Build an identity with claims from a username security token.
        /// </summary>
        /// <param name="userNameSecurityToken">Username security token from which to build the identity.</param>
        /// <param name="claims">Claims for the identity.</param>
        /// <returns>Identity with claims build from the username security token.</returns>
        private static IIdentity Build(UserNameSecurityToken userNameSecurityToken, IEnumerable<Claim> claims)
        {
            if (userNameSecurityToken == null)
            {
                throw new ArgumentNullException("userNameSecurityToken");
            }
            if (claims == null)
            {
                throw new ArgumentNullException("claims");
            }
            return new ClaimsIdentity(claims, AuthenticationTypes.Password, userNameSecurityToken)
            {
                Label = userNameSecurityToken.UserName,
            };
        }

        #endregion
    }
}
