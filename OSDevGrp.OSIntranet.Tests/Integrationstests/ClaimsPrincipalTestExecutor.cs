using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using OSDevGrp.OSIntranet.Security.Claims;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests
{
    /// <summary>
    /// Internal class which can setup, execute and dispose the environment with claims.
    /// </summary>
    internal class ClaimsPrincipalTestExecutor : IDisposable
    {
        #region Private variables

        private readonly IPrincipal _currentPrincipal;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the internal class which can setup, execute and dispose the environment with claims.
        /// </summary>
        public ClaimsPrincipalTestExecutor()
            : this(string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower()))
        {
        }

        /// <summary>
        /// Creates an instance of the internal class which can setup, execute and dispose the environment with claims.
        /// </summary>
        /// <param name="mailAddress">Mail address used in the claims principal.</param>
        public ClaimsPrincipalTestExecutor(string mailAddress)
        {
            if (mailAddress == null)
            {
                throw new ArgumentNullException("mailAddress");
            }

            _currentPrincipal = Thread.CurrentPrincipal;

            var claimsIdentity = new ClaimsIdentity(WindowsIdentity.GetCurrent());
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, mailAddress));
            claimsIdentity.AddClaim(new Claim(FoodWasteClaimTypes.ValidatedUser, mailAddress));
            claimsIdentity.AddClaim(new Claim(FoodWasteClaimTypes.HouseHoldManagement, mailAddress));

            Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the mail address which are used in the claims principal.
        /// </summary>
        public string MailAddress
        {
            get
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return string.Empty;
                }
                var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);
                return emailClaim == null ? string.Empty : emailClaim.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose the executor.
        /// </summary>
        public void Dispose()
        {
            if (_currentPrincipal == null)
            {
                return;
            }
            Thread.CurrentPrincipal = _currentPrincipal;
        }

        #endregion
    }
}
