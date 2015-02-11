﻿using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Authorization policy which can build and set a claims principal.
    /// </summary>
    public class ClaimsPrincipalBuilderAuthorizationPolicy : IAuthorizationPolicy
    {
        #region Private variables
        #endregion

        #region Properties

        /// <summary>
        /// Gets a string that identifies this authorization component.
        /// </summary>
        public virtual string Id
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a claim set that represents the issuer of the authorization policy.
        /// </summary>
        public virtual ClaimSet Issuer
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Evaluates whether a user meets the requirements for this authorization policy.
        /// </summary>
        /// <param name="evaluationContext">Evaluation context.</param>
        /// <param name="state">State.</param>
        /// <returns>False if the method for this authorization policy must be called if additional claims are added by other authorization policies to evaluationContext otherwise, true to state no additional evaluation is required by this authorization policy.</returns>
        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}