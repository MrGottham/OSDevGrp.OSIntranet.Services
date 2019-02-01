using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoFixture;
using Castle.Core.Internal;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the household.
    /// </summary>
    [TestFixture]
    public class HouseholdTests
    {
        /// <summary>
        /// Private class for testing the household.
        /// </summary>
        private class MyHousehold : Household
        {
            #region Constructor

            /// <summary>
            /// Creates a instance of the private class for testing the household.
            /// </summary>
            /// <param name="name">Name for the household.</param>
            /// <param name="description">Description for the household.</param>
            /// <param name="domainObjectValidations">Implementation for common validations used by domain objects in the food waste domain.</param>
            public MyHousehold(string name, string description = null, IDomainObjectValidations domainObjectValidations = null)
                : base(name, description, domainObjectValidations)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Date and time for when the household was created.
            /// </summary>
            public new DateTime CreationTime
            {
                get => base.CreationTime;
                set => base.CreationTime = value;
            }

            /// <summary>
            /// Household members who is member of this household.
            /// </summary>
            public new IEnumerable<IHouseholdMember> HouseholdMembers
            {
                get => base.HouseholdMembers;
                set => base.HouseholdMembers = value;
            }

            /// <summary>
            /// Storages in this household.
            /// </summary>
            public new IEnumerable<IStorage> Storages
            {
                get => base.Storages;
                set => base.Storages = value;
            }

            #endregion
        }

        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor without the description initialize a household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutDescriptionInitializeHousehold()
        {
            string name = _fixture.Create<string>();
            
            IHousehold sut = new Household(name);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);
            Assert.That(sut.Name, Is.EqualTo(name));
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor with the description initialize a household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithDescriptionInitializeHousehold()
        {
            string name = _fixture.Create<string>();
            string description = _fixture.Create<string>();

            IHousehold sut = new Household(name, description);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);
            Assert.That(sut.Name, Is.EqualTo(name));
            Assert.That(sut.Description, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Empty);
            Assert.That(sut.Description, Is.EqualTo(description));
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the name is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenNameIsInvalid(string invalidValue)
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new Household(invalidValue));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "name");
        }

        /// <summary>
        /// Tests that the setter for Name throws an ArgumentNullException when the value is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatNameSetterThrowsArgumentNullExceptionWhenValueIsInvalid(string invalidValue)
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Name = invalidValue);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for Name updates the name.
        /// </summary>
        [Test]
        public void TestThatNameSetterUpdatesValueOfName()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);

            string newValue = _fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);
            Assert.That(newValue, Is.Not.EqualTo(sut.Name));

            sut.Name = newValue;
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);
            Assert.That(sut.Name, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Description updates the description when value is not null.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterUpdatesValueOfDescriptionWhenValueIsNotNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Description, Is.Null);

            string newValue = _fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);
            Assert.That(newValue, Is.Not.EqualTo(sut.Description));

            sut.Description = newValue;
            Assert.That(sut.Description, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Empty);
            Assert.That(sut.Description, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Description updates the description when value is null.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterUpdatesValueOfDescriptionValueIsNull()
        {
            IHousehold sut = CreateSut(true);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Empty);

            sut.Description = null;
            Assert.That(sut.Description, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for CreationTime updates the date and time for when the household was created.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterUpdatesValueOfCreationTime()
        {
            MyHousehold sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            DateTime newValue = sut.CreationTime.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(120, 240));
            Assert.That(newValue, Is.Not.EqualTo(sut.CreationTime));

            sut.CreationTime = newValue;
            Assert.That(sut.CreationTime, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for HouseholdMembers throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            MyHousehold sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMembers = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for HouseholdMembers updates household members who is member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersSetterUpdatesValueOfHouseholdMembers()
        {
            MyHousehold sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            IEnumerable<IHouseholdMember> householdMemberMockCollection = new List<IHouseholdMember>
            {
                BuildHouseholdMemberMock(),
                BuildHouseholdMemberMock(),
                BuildHouseholdMemberMock()
            };
            Assert.That(householdMemberMockCollection, Is.Not.Null);
            Assert.That(householdMemberMockCollection, Is.Not.Empty);
            Assert.That(householdMemberMockCollection, Is.Not.EqualTo(sut.HouseholdMembers));

            sut.HouseholdMembers = householdMemberMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers, Is.EqualTo(householdMemberMockCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that the setter for Storages throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatStoragesSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            MyHousehold sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Storages = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter for Storages updates storages in the household.
        /// </summary>
        [Test]
        public void TestThatStoragesSetterUpdatesValueOfStorages()
        {
            MyHousehold sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);

            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                BuildStorageMock(),
                BuildStorageMock(),
                BuildStorageMock()
            };
            Assert.That(storageMockCollection, Is.Not.Null);
            Assert.That(storageMockCollection, Is.Not.Empty);
            Assert.That(storageMockCollection, Is.Not.EqualTo(sut.Storages));

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages, Is.EqualTo(storageMockCollection));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd throws an ArgumentNullException when the household member who should be member of the household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd adds the household member who should be member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddAddsHouseholdMember()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.HouseholdMemberAdd(householdMemberMock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(1));
            Assert.That(sut.HouseholdMembers.Contains(householdMemberMock), Is.True);
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd calls Households on the household member who should be member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCallsHouseholdsOnHouseholdMember()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.Households);
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd calls HouseholdAdd on the household member who should be member of the household when the household member is not member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCallsHouseholdAddOnHouseholdMemberWhenHouseholdMemberIsNotMemberOfHousehold()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.HouseholdAdd(Arg<IHousehold>.Is.Equal(sut)));
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd does not call HouseholdAdd on the household member who should be member of the household when the household member is member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddDoesNotCallHouseholdAddOnHouseholdMemberWhenHouseholdMemberIsMemberOfHousehold()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock(_fixture.Create<bool>(), _fixture.Create<bool>(), sut);

            sut.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasNotCalled(m => m.HouseholdAdd(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove throws an ArgumentNullException when the household member which should be removed as a member of this household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.HouseholdMemberRemove(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove returns null when the household member which should be removed as a member of this household does not exists on the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveReturnsNullWhenHouseholdMemberDoesNotExistOnHousehold()
        {
            IHouseholdMember householdMember1Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember2Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember3Mock = BuildHouseholdMemberMock();

            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMember1Mock);
            sut.HouseholdMemberAdd(householdMember2Mock);
            sut.HouseholdMemberAdd(householdMember3Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(3));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);

            IHouseholdMember result = sut.HouseholdMemberRemove(MockRepository.GenerateMock<IHouseholdMember>());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove removes the household member which should be removed as a member of this household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveRemovesHouseholdMember()
        {
            IHouseholdMember householdMember1Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember2Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember3Mock = BuildHouseholdMemberMock();

            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMember1Mock);
            sut.HouseholdMemberAdd(householdMember2Mock);
            sut.HouseholdMemberAdd(householdMember3Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(3));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);

            sut.HouseholdMemberRemove(householdMember2Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(2));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.False);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove calls Households on the household member which should be removed as a member of this household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveCallsHouseholdsOnHouseholdMember()
        {
            IHouseholdMember householdMember1Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember2Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember3Mock = BuildHouseholdMemberMock();

            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMember1Mock);
            sut.HouseholdMemberAdd(householdMember2Mock);
            sut.HouseholdMemberAdd(householdMember3Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(3));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);

            sut.HouseholdMemberRemove(householdMember2Mock);

            householdMember2Mock.AssertWasCalled(m => m.Households, opt => opt.Repeat.Times(2));  // One time when added and one time when removed.
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove calls HouseholdRemove on the household member which should be removed as a member of this household when the household has the household member as a member.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveCallsHouseholdRemoveOnHouseholdMemberWhenHouseholdExistsOnHouseholdMember()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            IHouseholdMember householdMember1Mock = BuildHouseholdMemberMock(_fixture.Create<bool>(), _fixture.Create<bool>(), sut);
            IHouseholdMember householdMember2Mock = BuildHouseholdMemberMock(_fixture.Create<bool>(), _fixture.Create<bool>(), sut);
            IHouseholdMember householdMember3Mock = BuildHouseholdMemberMock(_fixture.Create<bool>(), _fixture.Create<bool>(), sut);

            sut.HouseholdMemberAdd(householdMember1Mock);
            sut.HouseholdMemberAdd(householdMember2Mock);
            sut.HouseholdMemberAdd(householdMember3Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(3));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);

            sut.HouseholdMemberRemove(householdMember2Mock);

            householdMember2Mock.AssertWasCalled(m => m.HouseholdRemove(Arg<IHousehold>.Is.Equal(sut)));
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove does not call HouseholdRemove on the household member which should be removed as a member of this household when the household does not have the household member as a member.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveDoesNotCallHouseholdRemoveOnHouseholdMemberWhenHouseholdDoesNotExistOnHouseholdMember()
        {
            IHouseholdMember householdMember1Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember2Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember3Mock = BuildHouseholdMemberMock();

            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMember1Mock);
            sut.HouseholdMemberAdd(householdMember2Mock);
            sut.HouseholdMemberAdd(householdMember3Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(3));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);

            sut.HouseholdMemberRemove(householdMember2Mock);

            householdMember2Mock.AssertWasNotCalled(m => m.HouseholdRemove(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove returns null the household member which has been removed as a member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveReturnsRemovedHouseholdMember()
        {
            IHouseholdMember householdMember1Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember2Mock = BuildHouseholdMemberMock();
            IHouseholdMember householdMember3Mock = BuildHouseholdMemberMock();

            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMember1Mock);
            sut.HouseholdMemberAdd(householdMember2Mock);
            sut.HouseholdMemberAdd(householdMember3Mock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(3));
            Assert.That(sut.HouseholdMembers.Contains(householdMember1Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember2Mock), Is.True);
            Assert.That(sut.HouseholdMembers.Contains(householdMember3Mock), Is.True);

            IHouseholdMember result = sut.HouseholdMemberRemove(householdMember2Mock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(householdMember2Mock));
        }

        /// <summary>
        /// Tests that StorageAdd throws an ArgumentNullException when the storage to add is null.
        /// </summary>
        [Test]
        public void TestThatStorageAddThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageAdd(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that StorageAdd calls CanAddStorage on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatStorageAddCallsCanAddStorageOnDomainObjectValidations()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            IHousehold sut = CreateSut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();

            sut.StorageAdd(storageMock);

            domainObjectValidationsMock.AssertWasCalled(m => m.CanAddStorage(Arg<IStorage>.Is.Equal(storageMock), Arg<IEnumerable<IStorage>>.Is.Equal(sut.Storages)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StorageAdd throws an IntranetSystemException when the storage cannot be added to the household.
        /// </summary>
        [Test]
        public void TestThatStorageAddThrowsIntranetSystemExceptionWhenStorageCannotBeAdded()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock(false);

            IHousehold sut = CreateSut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.StorageAdd(storageMock));

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.OperationNotAllowedOnStorage, "StorageAdd");
        }

        /// <summary>
        /// Tests that StorageAdd adds the storage when the storage can be added to the household.
        /// </summary>
        [Test]
        public void TestThatStorageAddAddsStorageWhenStorageCanBeAdded()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            IHousehold sut = CreateSut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);

            IStorage storageMock = BuildStorageMock();

            sut.StorageAdd(storageMock);

            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
        }

        /// <summary>
        /// Tests that StorageAdd called with a household member throws an ArgumentNullException when the storage to add is null.
        /// </summary>
        [Test]
        public void TestThatStorageAddCalledWithHouseholdMemberThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageAdd(null, BuildHouseholdMemberMock()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that StorageAdd called with a household member throws an ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatStorageAddCalledWithHouseholdMemberThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageAdd(BuildStorageMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that StorageAdd called with a household member calls CanCreateStorage on the household member.
        /// </summary>
        [Test]
        public void TestThatStorageAddCalledWithHouseholdMemberCallsCanCreateStorageOnHouseholdMember()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            IHousehold sut = CreateSut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.StorageAdd(storageMock, householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.CanCreateStorage, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StorageAdd called with a household member throws an IntranetBusinessException when the household member cannot create storages.
        /// </summary>
        [Test]
        public void TestThatStorageAddCalledWithHouseholdMemberThrowsIntranetBusinessExceptionWhenHouseholdMemberCannotCreateStorage()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            IHousehold sut = CreateSut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock(false);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.StorageAdd(storageMock, householdMemberMock));

            TestHelper.AssertIntranetBusinessExceptionIsValid(result, ExceptionMessage.HouseholdMemberHasNotRequiredMembership);
        }

        /// <summary>
        /// Tests that StorageAdd called with a household member adds the storage when the household member can create storages.
        /// </summary>
        [Test]
        public void TestThatStorageAddCalledWithHouseholdMemberAddsStorageWhenHouseholdMemberCanCreateStorage()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            IHousehold sut = CreateSut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);

            IStorage storageMock = BuildStorageMock();
            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.StorageAdd(storageMock, householdMemberMock);

            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
        }

        /// <summary>
        /// Tests that StorageRemove throws an ArgumentNullException when the storage to remove is null.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageRemove(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that StorageRemove does not call CanRemoveStorage on the common validations used by domain objects in the food waste domain when the storage to remove is not part of the household storage collection.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveDoesNotCallCanRemoveStorageOnDomainObjectValidationsNullWhenStorageNotInStoragesOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                BuildStorageMock(),
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.False);
            // ReSharper restore PossibleMultipleEnumeration

            sut.StorageRemove(storageMock);

            domainObjectValidationsMock.AssertWasNotCalled(m => m.CanRemoveStorage(Arg<IStorage>.Is.Anything));
        }

        /// <summary>
        /// Tests that StorageRemove returns null when the storage to remove is not part of the household storage collection.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveReturnsNullWhenStorageNotInStoragesOnHousehold()
        {
            MyHousehold sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                BuildStorageMock(),
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.False);
            // ReSharper restore PossibleMultipleEnumeration

            IStorage result = sut.StorageRemove(storageMock);

            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that StorageRemove calls CanRemoveStorage on the common validations used by domain objects in the food waste domain when the storage to remove is part of the household storage collection.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCallsCanRemoveStorageOnDomainObjectValidationsNullWhenStorageInStoragesOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            sut.StorageRemove(storageMock);

            domainObjectValidationsMock.AssertWasCalled(m => m.CanRemoveStorage(Arg<IStorage>.Is.Equal(storageMock)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StorageRemove throws an IntranetSystemException when the storage to remove cannot be removed and is part of the household storage collection.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveThrowsIntranetSystemExceptionWhenStorageCannotBeRemovedAndStorageInStoragesOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock(canRemoveStorage: false);

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.StorageRemove(storageMock));

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.OperationNotAllowedOnStorage, "StorageRemove");
        }

        /// <summary>
        /// Tests that StorageRemove removes the storage when the storage to remove can be removed and is part of the household storage collection.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveRemovesStorageWhenStorageCanBeRemovedAndStorageInStoragesOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            sut.StorageRemove(storageMock);

            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.False);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that StorageRemove returns the removed storage when the storage to remove can be removed and is part of the household storage collection.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveReturnsRemovedStorageWhenStorageCanBeRemovedAndStorageInStoragesOnHousehold()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            IStorage result = sut.StorageRemove(storageMock);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(storageMock));
        }

        /// <summary>
        /// Tests that StorageRemove called with a household member throws an ArgumentNullException when the storage to remove is null.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCalledWithHouseholdMemberThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageRemove(null, BuildHouseholdMemberMock()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that StorageRemove called with a household member throws an ArgumentNullException when the household member is null.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCalledWithHouseholdMemberThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageRemove(BuildStorageMock(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "householdMember");
        }

        /// <summary>
        /// Tests that StorageRemove called with a household member calls CanDeleteStorage on the household member.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCalledWithHouseholdMemberCallsCanDeleteStorageOnHouseholdMember()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.StorageRemove(storageMock, householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.CanDeleteStorage, opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that StorageRemove called with a household member throws an IntranetBusinessException when the household member cannot delete storages.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCalledWithHouseholdMemberThrowsIntranetBusinessExceptionWhenHouseholdMemberCannotDeleteStorage()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock(canDeleteStorage: false);

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.StorageRemove(storageMock, householdMemberMock));

            TestHelper.AssertIntranetBusinessExceptionIsValid(result, ExceptionMessage.HouseholdMemberHasNotRequiredMembership);
        }

        /// <summary>
        /// Tests that StorageRemove called with a household member removes the storage when the household member can delete storages.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCalledWithHouseholdMemberRemovesStorageWhenHouseholdMemberCanDeleteStorage()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            sut.StorageRemove(storageMock, householdMemberMock);

            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.False);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Tests that StorageRemove called with a household member returns the removed storage when the household member can delete storages.
        /// </summary>
        [Test]
        public void TestThatStorageRemoveCalledWithHouseholdMemberReturnsRemovedStorageWhenHouseholdMemberCanDeleteStorage()
        {
            IDomainObjectValidations domainObjectValidationsMock = BuildDomainObjectValidationsMock();

            MyHousehold sut = CreateMySut(domainObjectValidations: domainObjectValidationsMock);
            Assert.That(sut, Is.Not.Null);

            IStorage storageMock = BuildStorageMock();
            IEnumerable<IStorage> storageMockCollection = new List<IStorage>
            {
                storageMock,
                BuildStorageMock(),
                BuildStorageMock()
            };

            sut.Storages = storageMockCollection;
            // ReSharper disable PossibleMultipleEnumeration
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Contains(storageMock), Is.True);
            // ReSharper restore PossibleMultipleEnumeration

            IHouseholdMember householdMemberMock = BuildHouseholdMemberMock();

            IStorage result = sut.StorageRemove(storageMock, householdMemberMock);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(storageMock));
        }

        /// <summary>
        /// Tests that Translate throws an ArgumentNullException when the culture information which are used for translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslateThrowsArgumentNullExceptionWhenTranslationCultureIsNull()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Translate(null, _fixture.Create<bool>(), _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "translationCulture");
        }

        /// <summary>
        /// Tests that Translate calls Translate for each household member who has a membership on this household when they should be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachHouseholdMemberWhenTranslateHouseholdMembersIsTrue()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            int numberOfHouseholdMembers = _random.Next(1, 5);
            while (sut.HouseholdMembers.Count() < numberOfHouseholdMembers)
            {
                sut.HouseholdMemberAdd(BuildHouseholdMemberMock());
            }
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(numberOfHouseholdMembers));

            CultureInfo translationCulture = CultureInfo.CurrentCulture;

            sut.Translate(translationCulture, true, _fixture.Create<bool>());

            foreach (IHouseholdMember householdMemberMock in sut.HouseholdMembers)
            {
                householdMemberMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture), Arg<bool>.Is.Equal(false), Arg<bool>.Is.Equal(true)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that Translate does not call Translate on any household member who has a membership on this household when they should not be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnAnyHouseholdMemberWhenTranslateHouseholdMembersIsFalse()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            int numberOfHouseholdMembers = _random.Next(1, 5);
            while (sut.HouseholdMembers.Count() < numberOfHouseholdMembers)
            {
                sut.HouseholdMemberAdd(BuildHouseholdMemberMock());
            }
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(numberOfHouseholdMembers));

            sut.Translate(CultureInfo.CurrentCulture, false, _fixture.Create<bool>());

            foreach (IHouseholdMember householdMemberMock in sut.HouseholdMembers)
            {
                householdMemberMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything, Arg<bool>.Is.Anything, Arg<bool>.Is.Anything));
            }
        }

        /// <summary>
        /// Tests that Translate calls Translate for each storage on this household when they should be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachStorageWhenTranslateStoragesIsTrue()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);

            IList<IStorage> storageMockCollection = DomainObjectMockBuilder.BuildStorageMockCollection().ToList();
            storageMockCollection.ForEach(storageMock => sut.StorageAdd(storageMock));

            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Count(), Is.EqualTo(storageMockCollection.Count));

            CultureInfo translationCulture = CultureInfo.CurrentCulture;

            sut.Translate(translationCulture, _fixture.Create<bool>());

            foreach (IStorage storageMock in sut.Storages)
            {
                storageMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture), Arg<bool>.Is.Equal(false), Arg<bool>.Is.Equal(true)), opt => opt.Repeat.Once());
            }
        }

        /// <summary>
        /// Tests that Translate does not call Translate on any storage on this household when they should not be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnAnyStorageWhenTranslateStoragesIsFalse()
        {
            IHousehold sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Empty);

            IList<IStorage> storageMockCollection = DomainObjectMockBuilder.BuildStorageMockCollection().ToList();
            storageMockCollection.ForEach(storageMock => sut.StorageAdd(storageMock));

            Assert.That(sut.Storages, Is.Not.Null);
            Assert.That(sut.Storages, Is.Not.Empty);
            Assert.That(sut.Storages.Count(), Is.EqualTo(storageMockCollection.Count));

            CultureInfo translationCulture = CultureInfo.CurrentCulture;

            sut.Translate(translationCulture, _fixture.Create<bool>(), false);

            foreach (IStorage storageMock in sut.Storages)
            {
                storageMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything, Arg<bool>.Is.Anything, Arg<bool>.Is.Anything));
            }
        }

        /// <summary>
        /// Creates an instance of a household which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of a household which can be used for unit testing.</returns>
        private IHousehold CreateSut(bool hasDescription = false, IDomainObjectValidations domainObjectValidations = null)
        {
            return new Household(_fixture.Create<string>(), hasDescription ? _fixture.Create<string>() : null, domainObjectValidations);
        }

        /// <summary>
        /// Creates an instance of the private class for a household which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the private class for a household which can be used for unit testing.</returns>
        private MyHousehold CreateMySut(bool hasDescription = false, IDomainObjectValidations domainObjectValidations = null)
        {
            return new MyHousehold(_fixture.Create<string>(), hasDescription ? _fixture.Create<string>() : null, domainObjectValidations);
        }

        /// <summary>
        /// Build a mockup for the <see cref="IDomainObjectValidations"/> which can be used for unit testing.
        /// </summary>
        /// <returns>Mockup for the <see cref="IDomainObjectValidations"/> which can be used for unit testing.</returns>
        private IDomainObjectValidations BuildDomainObjectValidationsMock(bool canAddStorage = true, bool canRemoveStorage = true)
        {
            IDomainObjectValidations domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            domainObjectValidationsMock.Stub(m => m.CanAddStorage(Arg<IStorage>.Is.Anything, Arg<IEnumerable<IStorage>>.Is.Anything))
                .Return(canAddStorage)
                .Repeat.Any();
            domainObjectValidationsMock.Stub(m => m.CanRemoveStorage(Arg<IStorage>.Is.Anything))
                .Return(canRemoveStorage)
                .Repeat.Any();
            return domainObjectValidationsMock;
        }

        /// <summary>
        /// Builds a mockup for a household member.
        /// </summary>
        /// <returns>Mockup for a household member.</returns>
        private IHouseholdMember BuildHouseholdMemberMock(bool canCreateStorage = true, bool canDeleteStorage = true)
        {
            return BuildHouseholdMemberMock(canCreateStorage, canDeleteStorage, new IHousehold[0]);
        }

        /// <summary>
        /// Builds a mockup for a household member.
        /// </summary>
        /// <returns>Mockup for a household member.</returns>
        private IHouseholdMember BuildHouseholdMemberMock(bool canCreateStorage, bool canDeleteStorage, params IHousehold[] householdCollection)
        {
            ArgumentNullGuard.NotNull(householdCollection, nameof(householdCollection));

            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.CanCreateStorage)
                .Return(canCreateStorage)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.CanDeleteStorage)
                .Return(canDeleteStorage)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.Households)
                .Return(householdCollection)
                .Repeat.Any();
            return householdMemberMock;
        }

        /// <summary>
        /// Builds a mockup for a storage.
        /// </summary>
        /// <returns>Mockup for a storage.</returns>
        private IStorage BuildStorageMock()
        {
            return MockRepository.GenerateMock<IStorage>();
        }
    }
}
