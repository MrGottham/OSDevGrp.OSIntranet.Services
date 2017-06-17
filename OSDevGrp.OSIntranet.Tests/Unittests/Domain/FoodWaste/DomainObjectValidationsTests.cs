using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using Ploeh.AutoFixture;

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
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsMailAddress throws ArgumentNullException when the value for the mail address is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatIsMailAddressThrowsArgumentNullExceptionWhenMailAddressIsNullOrEmpty(string validMailAddress)
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.IsMailAddress(validMailAddress));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
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
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            bool result = sut.IsMailAddress(validMailAddress);
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
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            bool result = sut.IsMailAddress(validMailAddress);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that GetHouseholdLimit returns a limit of households for each membership.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdLimitReturnsHouseholdLimitForEachMembership()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            foreach (Membership membershipToTest in Enum.GetValues(typeof(Membership)).Cast<Membership>())
            {
                var result = sut.GetHouseholdLimit(membershipToTest);
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
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            int result = sut.GetHouseholdLimit(membership);
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
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            bool result = sut.HasReachedHouseholdLimit(membership, numberOfHouseholds);
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
        public void TestThatHasRequiredMembershipValidatesWhetherMembershipMatchesRequiredMembership(Membership membership, Membership requiredMembership, bool expectedResult)
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            bool result = sut.HasRequiredMembership(membership, requiredMembership);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that CanUpgradeMembership validates whether a given membership can be upgraded to another membership.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, Membership.Basic, true)]
        [TestCase(Membership.Basic, Membership.Deluxe, true)]
        [TestCase(Membership.Basic, Membership.Premium, true)]
        [TestCase(Membership.Deluxe, Membership.Basic, false)]
        [TestCase(Membership.Deluxe, Membership.Deluxe, true)]
        [TestCase(Membership.Deluxe, Membership.Premium, true)]
        [TestCase(Membership.Premium, Membership.Basic, false)]
        [TestCase(Membership.Premium, Membership.Deluxe, false)]
        [TestCase(Membership.Premium, Membership.Premium, true)]
        public void TestThatCanUpgradeMembershipValidatesWhetherMembershipCanBeUpgraded(Membership currentMembership, Membership upgradeToMembership, bool expectedResult)
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            bool result = sut.CanUpgradeMembership(currentMembership, upgradeToMembership);
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that InRange validates whether the value is in a given range.
        /// </summary>
        [Test]
        [TestCase(-129, -128, 127, false)]
        [TestCase(-128, -128, 127, true)]
        [TestCase(-127, -128, 127, true)]
        [TestCase(-1, -128, 127, true)]
        [TestCase(0, -128, 127, true)]
        [TestCase(1, -128, 127, true)]
        [TestCase(126, -128, 127, true)]
        [TestCase(127, -128, 127, true)]
        [TestCase(128, -128, 127, false)]
        public void TestThatInRangeValidatesWhetherValueIsInRange(int value, int startValue, int endValue, bool expectedResult)
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            bool result = sut.InRange(value, new Range<int>(startValue, endValue));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Tests that InRange throws an ArgumentNullException when the given range is null.
        /// </summary>
        [Test]
        public void TestThatInRangeThrowsArgumentNullExceptionWhenRangeIsNull()
        {
            Fixture fixture = new Fixture();

            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.InRange(fixture.Create<int>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "range");
        }

        /// <summary>
        /// Tests that Create initialize common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatCreateInitializeDomainObjectValidations()
        {
            IDomainObjectValidations domainObjectValidations = DomainObjectValidations.Create();
            Assert.That(domainObjectValidations, Is.Not.Null);
            Assert.That(domainObjectValidations, Is.TypeOf<DomainObjectValidations>());
        }
    }
}
