using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Authorization policy to use when authorizing users who has a mail address as username.
    /// </summary>
    public class UserNameAsMailAddressAuthorizationPolicy : IAuthorizationPolicy, IIdentityProvider
    {
        #region Private variables

        private readonly Guid _id = Guid.NewGuid();
        private readonly IIdentity _primaryIdentity;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an authorization policy to use when authorizing users who has a mail address as username.
        /// </summary>
        /// <param name="primaryIdentity">The primary identity.</param>
        public UserNameAsMailAddressAuthorizationPolicy(IIdentity primaryIdentity)
        {
            if (primaryIdentity == null)
            {
                throw new ArgumentNullException("primaryIdentity");
            }
            _primaryIdentity = primaryIdentity;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a string that identifies this authorization component.
        /// </summary>
        public virtual string Id
        {
            get { return _id.ToString(); }
        }

        /// <summary>
        /// Gets a claim set that represents the issuer of the authorization policy.
        /// </summary>
        public virtual ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        /// <summary>
        /// Gets the primary identity.
        /// </summary>
        public virtual IIdentity PrimaryIdentity
        {
            get { return _primaryIdentity; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether a user meets the requirements for this authorization policy.
        /// </summary>
        /// <param name="evaluationContext">Evaluation context.</param>
        /// <param name="state">State.</param>
        /// <returns>False if the method for this authorization policy must be called if additional claims are added by other authorization policies to evaluationContext otherwise, true to state no additional evaluation is required by this authorization policy.</returns>
        public virtual bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            if (evaluationContext == null)
            {
                throw new ArgumentNullException("evaluationContext");
            }
            throw new NotSupportedException();
        }

        #endregion
    }
}
