using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the common validations used by domain objects in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DomainObjectValidationsTests
    {
        /// <summary>
        /// Tests that the constructor initialize common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDomainObjectValidations()
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress throws ArgumentNullException when the value for the mail address is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatIsMailAddressThrowsArgumentNullExceptionWhenMailAddressIsNullOrEmpty(string validMailAddress)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => domainObjectValidations.IsMailAddress(validMailAddress));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress returns true for valid mail addresses.
        /// </summary>
        [Test]
        [TestCase("mrgottham@gmail.com")]
        [TestCase("test@osdevgrp.dk")]
        [TestCase("ole.sorensen@visma.com")]
        public void TestThatIsMailAddressReturnsTrueForValidMailAddress(string validMailAddress)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.IsMailAddress(validMailAddress);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that IsMailAddress returns false for invalid mail addresses.
        /// </summary>
        [Test]
        [TestCase("XYZ")]
        [TestCase("XYZ.XXX")]
        [TestCase("XYZ.XXX.COM")]
        public void TestThatIsMailAddressReturnsFalseForInvalidMailAddress(string validMailAddress)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.IsMailAddress(validMailAddress);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that GetHouseholdLimit returns a limit of households for each membership.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdLimitReturnsHouseholdLimitForEachMembership()
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            foreach (var membershipToTest in Enum.GetValues(typeof (Membership)).Cast<Membership>())
            {
                var result = domainObjectValidations.GetHouseholdLimit(membershipToTest);
                Assert.That(result, Is.GreaterThan(0));
            }
        }

        /// <summary>
        /// Tests that GetHouseholdLimit returns the limit of households when a membership is given.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, 1)]
        [TestCase(Membership.Deluxe, 2)]
        [TestCase(Membership.Premium, 999)]
        public void TestThatGetHouseholdLimitReturnsActualHouseholdLimit(Membership membership, int householdLimit)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.GetHouseholdLimit(membership);
            Assert.That(result, Is.EqualTo(householdLimit));
        }

        /// <summary>
        /// Tests that HasReachedHouseholdLimit validates whether a household limit has been reached.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, 0, false)]
        [TestCase(Membership.Basic, 1, true)]
        [TestCase(Membership.Basic, 2, true)]
        [TestCase(Membership.Basic, 3, true)]
        [TestCase(Membership.Deluxe, 0, false)]
        [TestCase(Membership.Deluxe, 1, false)]
        [TestCase(Membership.Deluxe, 2, true)]
        [TestCase(Membership.Deluxe, 3, true)]
        [TestCase(Membership.Premium, 0, false)]
        [TestCase(Membership.Premium, 1, false)]
        [TestCase(Membership.Premium, 2, false)]
        [TestCase(Membership.Premium, 3, false)]
        [TestCase(Membership.Premium, 997, false)]
        [TestCase(Membership.Premium, 998, false)]
        [TestCase(Membership.Premium, 999, true)]
        [TestCase(Membership.Premium, 1000, true)]
        public void TestThatHasReachedHouseholdLimitValidatesWhetherHouseholdLimitHasBeenReached(Membership membership, int numberOfHouseholds, bool expectedResult)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.HasReachedHouseholdLimit(membership, numberOfHouseholds);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that HasRequiredMembership validates whether a given membership matches a required membership.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, Membership.Basic, true)]
        [TestCase(Membership.Basic, Membership.Deluxe, false)]
        [TestCase(Membership.Basic, Membership.Premium, false)]
        [TestCase(Membership.Deluxe, Membership.Basic, true)]
        [TestCase(Membership.Deluxe, Membership.Deluxe, true)]
        [TestCase(Membership.Deluxe, Membership.Premium, false)]
        [TestCase(Membership.Premium, Membership.Basic, true)]
        [TestCase(Membership.Premium, Membership.Deluxe, true)]
        [TestCase(Membership.Premium, Membership.Premium, true)]
        public void TestThatHasRequiredMembershipValidatedsWhetherMembershipMatchesRequiredMembership(Membership membership, Membership requiredMembership, bool expectedResult)
        {
            var domainObjectValidations = new DomainObjectValidations();
            Assert.That(domainObjectValidations, Is.Not.Null);

            var result = domainObjectValidations.HasRequiredMembership(membership, requiredMembership);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that Create initialize common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatCreateInitializeDomainObjectValidations()
        {
            var domainObjectValidations = DomainObjectValidations.Create();
            Assert.That(domainObjectValidations, Is.Not.Null);
            Assert.That(domainObjectValidations, Is.TypeOf<DomainObjectValidations>());
        }
    }
}
