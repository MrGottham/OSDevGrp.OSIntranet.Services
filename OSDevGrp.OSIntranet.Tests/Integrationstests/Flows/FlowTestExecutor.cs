using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using OSDevGrp.OSIntranet.Security.Claims;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Flows
{
    /// <summary>
    /// Internal class which can setup, execute and dispose the environment a flow test.
    /// </summary>
    internal class FlowTestExecutor : IDisposable
    {
        #region Private variables

        private readonly IPrincipal _currentPrincipal;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the internal class which can setup, execute and dispose the environment a flow test.
        /// </summary>
        public FlowTestExecutor()
            : this(string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower()))
        {
        }

        /// <summary>
        /// Creates an instance of the internal class which can setup, execute and dispose the environment a flow test.
        /// </summary>
        /// <param name="mailAddress">Mail address used in the flow test.</param>
        public FlowTestExecutor(string mailAddress)
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
        /// Gets the mail address which are used in the flow test.
        /// </summary>
        public string MailAddress
        {
            get
            {
                var claimsPrincipal = new ClaimsPrincipal(Thread.CurrentPrincipal);
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
