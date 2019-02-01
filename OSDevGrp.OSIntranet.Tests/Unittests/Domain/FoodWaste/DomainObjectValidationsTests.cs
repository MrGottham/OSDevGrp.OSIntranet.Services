using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the common validations used by domain objects in the food waste domain.
    /// </summary>
    [TestFixture]
    public class DomainObjectValidationsTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

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
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatIsMailAddressThrowsArgumentNullExceptionWhenMailAddressIsNullEmptyOrWhiteSpace(string validMailAddress)
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
                int result = sut.GetHouseholdLimit(membershipToTest);
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
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.InRange(_fixture.Create<int>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "range");
        }

        /// <summary>
        /// Tests that CanAddStorage throws an ArgumentNullException when the storage to validate is null.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.CanAddStorage(null, new List<IStorage>(0)));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that CanAddStorage throws an ArgumentNullException when the existing storage collection is null.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageThrowsArgumentNullExceptionWhenExistingStorageCollectionIsNull()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.CanAddStorage(BuildStorageMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "existingStorageCollection");
        }

        /// <summary>
        /// Tests that CanAddStorage calls StorageType on the storage to validate.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageCallsStorageTypeOnStorageToValidate()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> existingStorageCollection = new List<IStorage>
            {
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForRefrigerator)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForFreezer)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket))
            };

            sut.CanAddStorage(storageMock, existingStorageCollection);

            storageMock.AssertWasCalled(m => m.StorageType, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that CanAddStorage returns false when the storage to validate does not have a storage type.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageReturnsFalseWhenStorageDoesNotHaveStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock(false);
            IEnumerable<IStorage> existingStorageCollection = new List<IStorage>
            {
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForRefrigerator)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForFreezer)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket))
            };

            bool result = sut.CanAddStorage(storageMock, existingStorageCollection);

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that CanAddStorage calls Creatable on the storage type when the storage to validate does have a storage type.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageCallsCreatableOnStorageTypeWhenStorageDoesHaveStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorageType storageTypeMock = BuildStorageTypeMock(StorageType.IdentifierForRefrigerator);
            IStorage storageMock = BuildStorageMock(storageType: storageTypeMock);
            IEnumerable<IStorage> existingStorageCollection = new List<IStorage>
            {
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForRefrigerator)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForFreezer)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket))
            };

            sut.CanAddStorage(storageMock, existingStorageCollection);

            storageTypeMock.AssertWasCalled(m => m.Creatable, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that CanAddStorage returns true when the storage to validate does have a creatable storage type.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageReturnsTrueWhenStorageDoesHaveCreatableStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorageType storageTypeMock = BuildStorageTypeMock(StorageType.IdentifierForRefrigerator, true);
            IStorage storageMock = BuildStorageMock(storageType: storageTypeMock);
            IEnumerable<IStorage> existingStorageCollection = new List<IStorage>
            {
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForRefrigerator)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForFreezer)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket))
            };

            bool result = sut.CanAddStorage(storageMock, existingStorageCollection);

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that CanAddStorage returns true when the storage to validate does have a non creatable storage type and the existing storage collection does not contain a storage of the storage type.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageReturnsTrueWhenStorageDoesHaveNonCreatableStorageTypeAndExistingStorageCollectionDoesNotContainStorageOfStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorageType storageTypeMock = BuildStorageTypeMock(StorageType.IdentifierForRefrigerator, false);
            IStorage storageMock = BuildStorageMock(storageType: storageTypeMock);
            IEnumerable<IStorage> existingStorageCollection = new List<IStorage>
            {
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForFreezer)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket))
            };

            bool result = sut.CanAddStorage(storageMock, existingStorageCollection);

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that CanAddStorage returns false when the storage to validate does have a non creatable storage type and the existing storage collection does contain a storage of the storage type.
        /// </summary>
        [Test]
        public void TestThatCanAddStorageReturnsFalseWhenStorageDoesHaveNonCreatableStorageTypeAndExistingStorageCollectionDoesContainStorageOfStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorageType storageTypeMock = BuildStorageTypeMock(StorageType.IdentifierForRefrigerator, false);
            IStorage storageMock = BuildStorageMock(storageType: storageTypeMock);
            IEnumerable<IStorage> existingStorageCollection = new List<IStorage>
            {
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForRefrigerator)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForFreezer)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets)),
                BuildStorageMock(storageType: BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket))
            };

            bool result = sut.CanAddStorage(storageMock, existingStorageCollection);

            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that CanRemoveStorage throws an ArgumentNullException when the storage to validate is null.
        /// </summary>
        [Test]
        public void TestThatCanRemoveStorageThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.CanRemoveStorage(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that CanRemoveStorage calls StorageType on the storage to validate.
        /// </summary>
        [Test]
        public void TestThatCanRemoveStorageCallsStorageTypeOnStorageToValidate()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();

            sut.CanRemoveStorage(storageMock);

            storageMock.AssertWasCalled(m => m.StorageType, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that CanRemoveStorage returns true when the storage to validate does not have a storage type.
        /// </summary>
        [Test]
        public void TestThatCanRemoveStorageReturnsTrueWhenStorageDoesNotHaveStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock(false);

            bool result = sut.CanRemoveStorage(storageMock);

            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Tests that CanRemoveStorage calls Deletable on the storage type when the storage to validate does have a storage type.
        /// </summary>
        [Test]
        public void TestThatCanRemoveStorageCallsDeletableOnStorageTypeWhenStorageDoesHaveStorageType()
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorageType storageTypeMock = BuildStorageTypeMock(StorageType.IdentifierForRefrigerator);
            IStorage storageMock = BuildStorageMock(storageType: storageTypeMock);

            sut.CanRemoveStorage(storageMock);

            storageTypeMock.AssertWasCalled(m => m.Deletable, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that CanRemoveStorage returns whether the storage can be deleted when the storage to validate does have a storage type.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCanRemoveStorageReturnsDeletableFromStorageTypeWhenStorageDoesHaveCreatableStorageType(bool deletable)
        {
            IDomainObjectValidations sut = new DomainObjectValidations();
            Assert.That(sut, Is.Not.Null);

            IStorageType storageTypeMock = BuildStorageTypeMock(StorageType.IdentifierForRefrigerator, deletable: deletable);
            IStorage storageMock = BuildStorageMock(storageType: storageTypeMock);

            bool result = sut.CanRemoveStorage(storageMock);

            Assert.That(result, Is.EqualTo(deletable));
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

        /// <summary>
        /// Builds a mockup for a storage which can be used for unit testing.
        /// </summary>
        /// <returns>Mockup for a storage which can be used for unit testing.</returns>
        private IStorage BuildStorageMock(bool hasStorageType = true, IStorageType storageType = null)
        {
            IStorage storageMock = MockRepository.GenerateMock<IStorage>();
            storageMock.Stub(m => m.StorageType)
                .Return(hasStorageType ? storageType ?? BuildStorageTypeMock(StorageType.IdentifierForRefrigerator) : null)
                .Repeat.Any();
            return storageMock;
        }

        /// <summary>
        /// Builds a mockup for a storage type which can be used for unit testing.
        /// </summary>
        /// <returns>Mockup for a storage type which can be used for unit testing.</returns>
        private IStorageType BuildStorageTypeMock(Guid storageTypeIdentifier, bool? creatable = null, bool? deletable = null)
        {
            IStorageType storageTypeMock = MockRepository.GenerateMock<IStorageType>();
            storageTypeMock.Stub(m => m.Identifier)
                .Return(storageTypeIdentifier)
                .Repeat.Any();
            storageTypeMock.Stub(m => m.Creatable)
                .Return(creatable ?? _fixture.Create<bool>())
                .Repeat.Any();
            storageTypeMock.Stub(m => m.Deletable)
                .Return(deletable ?? _fixture.Create<bool>())
                .Repeat.Any();
            return storageTypeMock;
        }
    }
}
