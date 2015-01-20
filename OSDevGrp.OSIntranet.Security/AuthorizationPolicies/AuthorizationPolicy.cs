using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace OSDevGrp.OSIntranet.Security.AuthorizationPolicies
{
    /// <summary>
    /// Authorization policy which can be used by WCF services.
    /// </summary>
    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        #region Private variables

        private readonly IAuthorizationPolicyHandler _authorizationPolicyHandler;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an authorization policy which can be used by WCF services.
        /// </summary>
        public AuthorizationPolicy() 
            : this(new AuthorizationPolicyHandler())
        {
        }

        /// <summary>
        /// Creates an authorization policy which can be used by WCF services.
        /// </summary>
        /// <param name="authorizationPolicyHandler">Functionality which can handle the authorization policy.</param>
        public AuthorizationPolicy(IAuthorizationPolicyHandler authorizationPolicyHandler)
        {
            if (authorizationPolicyHandler == null)
            {
                throw new ArgumentNullException("authorizationPolicyHandler");
            }
            _authorizationPolicyHandler = authorizationPolicyHandler;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a string that identifies this authorization component.
        /// </summary>
        public virtual string Id
        {
            get { return "{C4579216-DD42-4755-BB96-968EF1E54F5C}"; }
        }

        /// <summary>
        /// Gets a claim set that represents the issuer of the authorization policy.
        /// </summary>
        public virtual ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether a user meets the requirements for this authorization policy.
        /// </summary>
        /// <param name="evaluationContext">Evaluation context.</param>
        /// <param name="state">State.</param>
        /// <returns>Whether a user meets the requirements for this authorization policy.</returns>
        public virtual bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            if (evaluationContext == null)
            {
                throw new ArgumentNullException("evaluationContext");
            }
            if (state == null)
            {
                throw new ArgumentNullException("state");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
