using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Microsoft.IdentityModel.Claims;
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
        private FlowTestExecutor(string mailAddress)
        {
            if (mailAddress == null)
            {
                throw new ArgumentNullException("mailAddress");
            }

            _currentPrincipal = Thread.CurrentPrincipal;

            var claimPrincipal = new ClaimsPrincipal(_currentPrincipal);
            claimPrincipal.Identities.ElementAt(0).Claims.Add(new Claim(ClaimTypes.Email, mailAddress));
            claimPrincipal.Identities.ElementAt(0).Claims.Add(new Claim(FoodWasteClaimTypes.ValidatedUser, mailAddress));
            claimPrincipal.Identities.ElementAt(0).Claims.Add(new Claim(FoodWasteClaimTypes.HouseHoldManagement, mailAddress));

            Thread.CurrentPrincipal = claimPrincipal;
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
                var claimPrincipal = Thread.CurrentPrincipal as IClaimsPrincipal;
                return claimPrincipal == null ? string.Empty : claimPrincipal.Identities.ElementAt(0).Claims.First(claim => String.Compare(claim.ClaimType, ClaimTypes.Email, StringComparison.Ordinal) == 0).Value;
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
