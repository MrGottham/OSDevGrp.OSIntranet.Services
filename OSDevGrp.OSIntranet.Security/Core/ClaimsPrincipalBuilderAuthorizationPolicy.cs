using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Authorization policy which can build and set a claims principal.
    /// </summary>
    public class ClaimsPrincipalBuilderAuthorizationPolicy : IAuthorizationPolicy
    {
        #region Private variables

        private readonly Guid _id = Guid.NewGuid();

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
            if (evaluationContext == null)
            {
                throw new ArgumentNullException("evaluationContext");
            }
            try
            {
                object principalObject;
                if (evaluationContext.Properties.TryGetValue("Principal", out principalObject) == false)
                {
                    principalObject = new ClaimsPrincipal();
                    evaluationContext.Properties.Add("Principal", principalObject);
                }

                var claimsPrincipal = principalObject as IClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return false;
                }

                if (evaluationContext.ClaimSets == null)
                {
                    evaluationContext.Properties["Principal"] = claimsPrincipal;
                    return true;
                }
                evaluationContext.Properties["Principal"] = claimsPrincipal;
                return true;
            }
            catch (Exception ex)
            {
                throw new FaultException(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, ex.Message));
            }
        }

        #endregion
    }
}
