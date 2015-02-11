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
        /// Tests that SystemManagement returns the claim type for system management.
        /// </summary>
        [Test]
        public void TestThatSystemManagementReturnClaimType()
        {
            Assert.That(FoodWasteClaimTypes.SystemManagement, Is.Not.Null);
            Assert.That(FoodWasteClaimTypes.SystemManagement, Is.Not.Empty);
            Assert.That(FoodWasteClaimTypes.SystemManagement, Is.EqualTo("http://osdevgrp.local/foodwaste/security/systemmanagement"));
        }

        /// <summary>
        /// Tests that ValidatedUser returns the claim type for validated user.
        /// </summary>
        [Test]
        public void TestThatValidatedUserReturnClaimType()
        {
            Assert.That(FoodWasteClaimTypes.ValidatedUser, Is.Not.Null);
            Assert.That(FoodWasteClaimTypes.ValidatedUser, Is.Not.Empty);
            Assert.That(FoodWasteClaimTypes.ValidatedUser, Is.EqualTo("http://osdevgrp.local/foodwaste/security/user"));
        }
    }
}
