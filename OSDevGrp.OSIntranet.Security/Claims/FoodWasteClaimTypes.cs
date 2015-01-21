namespace OSDevGrp.OSIntranet.Security.Claims
{
    /// <summary>
    /// Claim types for the food waste domain.
    /// </summary>
    public static class FoodWasteClaimTypes
    {
        /// <summary>
        /// Claim type for system management.
        /// </summary>
        public static string SystemManagement
        {
            get { return "urn://osdevgrp/foodwaste/security/systemmanagement"; }
        }

        /// <summary>
        /// Claim type for a validated user.
        /// </summary>
        public static string ValidatedUser
        {
            get { return "urn://osdevgrp/foodwaste/security/user"; }
        }
    }
}
