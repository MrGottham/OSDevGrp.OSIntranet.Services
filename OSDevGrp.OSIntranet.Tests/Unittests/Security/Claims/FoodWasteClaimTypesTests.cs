using NUnit.Framework;
using OSDevGrp.OSIntranet.Security.Claims;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security.Claims
{
    /// <summary>
    /// Tests the claim types for the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodWasteClaimTypesTests
    {
        /// <summary>
        /// Tests that the getter of SystemManagement returns the claim type for system management.
        /// </summary>
        [Test]
        public void TestThatSystemManagementGetterReturnClaimType()
        {
            var claimType = FoodWasteClaimTypes.SystemManagement;
            Assert.That(claimType, Is.Not.Null);
            Assert.That(claimType, Is.Not.Empty);
            Assert.That(claimType, Is.EqualTo("urn://osdevgrp/foodwaste/security/systemmanagement"));
        }

        /// <summary>
        /// Tests that the getter of ValidatedUser returns the claim type for validated user.
        /// </summary>
        [Test]
        public void TestThatValidatedUserGetterReturnClaimType()
        {
            var claimType = FoodWasteClaimTypes.ValidatedUser;
            Assert.That(claimType, Is.Not.Null);
            Assert.That(claimType, Is.Not.Empty);
            Assert.That(claimType, Is.EqualTo("urn://osdevgrp/foodwaste/security/user"));
        }
    }
}
