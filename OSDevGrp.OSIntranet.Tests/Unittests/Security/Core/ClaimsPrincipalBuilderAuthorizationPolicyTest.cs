using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Core
{
    /// <summary>
    /// Authorization policy which can build and set a claims principal.
    /// </summary>
    public class ClaimsPrincipalBuilderAuthorizationPolicy : IAuthorizationPolicy
    {
        #region Properties
        #endregion

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            throw new System.NotImplementedException();
        }

        public ClaimSet Issuer
        {
            get { throw new NotImplementedException(); }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
    }
}
