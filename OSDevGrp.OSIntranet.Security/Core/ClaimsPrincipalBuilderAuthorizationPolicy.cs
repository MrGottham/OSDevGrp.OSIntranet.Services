using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;
using OSDevGrp.OSIntranet.Resources;
using Claim = Microsoft.IdentityModel.Claims.Claim;
using ClaimTypes = Microsoft.IdentityModel.Claims.ClaimTypes;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Authorization policy which can build and set a claims principal.
    /// </summary>
    public class ClaimsPrincipalBuilderAuthorizationPolicy : IAuthorizationPolicy
    {
        #region Private variables

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAuthorizationHandler _authorizationHandler;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an authorization policy which can build and set a claims principal.
        /// </summary>
        public ClaimsPrincipalBuilderAuthorizationPolicy()
            : this(new AuthorizationHandler())
        {
        }

        /// <summary>
        /// Creates an authorization policy which can build and set a claims principal.
        /// </summary>
        /// <param name="authorizationHandler">Functionality which can handle authorization.</param>
        public ClaimsPrincipalBuilderAuthorizationPolicy(IAuthorizationHandler authorizationHandler)
        {
            if (authorizationHandler == null)
            {
                throw new ArgumentNullException("authorizationHandler");
            }
            _authorizationHandler = authorizationHandler;
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
                    claimsPrincipal.Identities.AddRange(CreateClaimsIdentity(null));
                    evaluationContext.Properties["Principal"] = claimsPrincipal;
                    return true;
                }
                claimsPrincipal.Identities.AddRange(CreateClaimsIdentity(evaluationContext.ClaimSets));
                evaluationContext.Properties["Principal"] = claimsPrincipal;
                return true;
            }
            catch (Exception ex)
            {
                throw new FaultException(Resource.GetExceptionMessage(ExceptionMessage.NotAuthorizedToUseService, ex.Message));
            }
        }

        /// <summary>
        /// Creates a collection of claims identities based on the given claim sets.
        /// </summary>
        /// <param name="claimSets">Claim sets which should be used to create claims identities.</param>
        /// <returns>Collection of claims identities based on the given claims sets.</returns>
        private IEnumerable<IClaimsIdentity> CreateClaimsIdentity(IEnumerable<ClaimSet> claimSets)
        {
            if (claimSets == null)
            {
                return new List<ClaimsIdentity> {new ClaimsIdentity()};
            }
            var trustedClaimSets = _authorizationHandler.GetTrustedClaimSets(claimSets);
            return trustedClaimSets
                .Select(claimSet =>
                {
                    var issuer = GetIssuer(claimSet.Issuer);
                    var claims = claimSet
                        .Where(claim => string.Compare(claim.Right, Rights.PossessProperty, StringComparison.Ordinal) == 0)
                        .Select(claim =>
                        {
                            if (string.Compare(claim.ClaimType, ClaimTypes.Sid, StringComparison.Ordinal) == 0 && claim.Resource is SecurityIdentifier)
                            {
                                if (string.Compare(claim.Right, Rights.Identity, StringComparison.Ordinal) == 0)
                                {
                                    return new Claim(ClaimTypes.PrimarySid, ((SecurityIdentifier) claim.Resource).Value, ClaimValueTypes.String, issuer, issuer);
                                }
                                return new Claim(ClaimTypes.GroupSid, ((SecurityIdentifier) claim.Resource).Value, ClaimValueTypes.String, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.Email, StringComparison.Ordinal) == 0 && claim.Resource is MailAddress)
                            {
                                return new Claim(claim.ClaimType, ((MailAddress) claim.Resource).Address, ClaimValueTypes.String, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.Thumbprint, StringComparison.Ordinal) == 0 && claim.Resource is byte[])
                            {
                                return new Claim(claim.ClaimType, Convert.ToBase64String((byte[]) claim.Resource), ClaimValueTypes.Base64Binary, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.Hash, StringComparison.Ordinal) == 0 && claim.Resource is byte[])
                            {
                                return new Claim(claim.ClaimType, Convert.ToBase64String((byte[]) claim.Resource), ClaimValueTypes.Base64Binary, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.NameIdentifier, StringComparison.Ordinal) == 0 && claim.Resource is SamlNameIdentifierClaimResource)
                            {
                                var newClaim = new Claim(claim.ClaimType, ((SamlNameIdentifierClaimResource) claim.Resource).Name, ClaimValueTypes.String, issuer, issuer);
                                if (((SamlNameIdentifierClaimResource) claim.Resource).Format != null)
                                {
                                    newClaim.Properties[ClaimProperties.SamlNameIdentifierFormat] = ((SamlNameIdentifierClaimResource) claim.Resource).Format;
                                }
                                if (((SamlNameIdentifierClaimResource) claim.Resource).NameQualifier != null)
                                {
                                    newClaim.Properties[ClaimProperties.SamlNameIdentifierNameQualifier] = ((SamlNameIdentifierClaimResource) claim.Resource).NameQualifier;
                                }
                                return newClaim;
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.X500DistinguishedName, StringComparison.Ordinal) == 0 && claim.Resource is X500DistinguishedName)
                            {
                                return new Claim(claim.ClaimType, ((X500DistinguishedName) claim.Resource).Name, ClaimValueTypes.X500Name, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.Uri, StringComparison.Ordinal) == 0 && claim.Resource is Uri)
                            {
                                return new Claim(claim.ClaimType, ((Uri) claim.Resource).AbsoluteUri, ClaimValueTypes.String, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.Rsa, StringComparison.Ordinal) == 0 && claim.Resource is RSA)
                            {
                                return new Claim(claim.ClaimType, ((RSA) claim.Resource).ToXmlString(false), ClaimValueTypes.RsaKeyValue, issuer, issuer);
                            }
                            if (string.Compare(claim.ClaimType, ClaimTypes.DenyOnlySid, StringComparison.Ordinal) == 0 && claim.Resource is SecurityIdentifier)
                            {
                                return new Claim(claim.ClaimType, ((SecurityIdentifier) claim.Resource).Value, ClaimValueTypes.String, issuer, issuer);
                            }
                            if (claim.Resource as string != null)
                            {
                                return new Claim(claim.ClaimType, (string) claim.Resource, ClaimValueTypes.String, issuer, issuer);
                            }
                            return new Claim(claim.ClaimType, claim.Resource == null ? "{null}" : claim.Resource.ToString(), ClaimValueTypes.String, issuer, issuer);
                        })
                        .ToArray();
                    return new ClaimsIdentity(claims);
                })
                .ToArray();
        }

        /// <summary>
        /// Gets the name of the issuer.
        /// </summary>
        /// <param name="issuerClaimSet">Claim set for the issuer.</param>
        /// <returns>Name of the issuer.</returns>
        private static string GetIssuer(ClaimSet issuerClaimSet)
        {
            if (issuerClaimSet == null)
            {
                throw new ArgumentNullException("issuerClaimSet");
            }
            var claimTypes = issuerClaimSet.FindClaims(ClaimTypes.Name, Rights.PossessProperty);
            if (claimTypes == null)
            {
                return ClaimsIdentity.DefaultIssuer;
            }
            var iterator = claimTypes.GetEnumerator();
            if (iterator.MoveNext())
            {
                return (string) iterator.Current.Resource;
            }
            return ClaimsIdentity.DefaultIssuer;
        }

        #endregion
    }
}
