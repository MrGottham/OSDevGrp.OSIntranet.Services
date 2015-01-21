using System;

namespace OSDevGrp.OSIntranet.Security.Attributes
{
    /// <summary>
    /// Attribute which indicate that a given claim type is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredClaimTypeAttribute : Attribute
    {
        #region Private variables

        private readonly string _requiredClaimType;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an attribute which indicate that a given claim type is required.
        /// </summary>
        /// <param name="requiredClaimType">Claim type which are required.</param>
        public RequiredClaimTypeAttribute(string requiredClaimType)
        {
            if (string.IsNullOrEmpty(requiredClaimType))
            {
                throw new ArgumentNullException("requiredClaimType");
            }
            _requiredClaimType = requiredClaimType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the claim type which are required.
        /// </summary>
        public virtual string RequiredClaimType
        {
            get { return _requiredClaimType; }
        }

        #endregion
    }
}
