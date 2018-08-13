using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using AutoFixture;
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
            public MyHousehold(string name, string description = null)
                : base(name, description)
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

            #endregion
        }

        /// <summary>
        /// Tests that the constructor without the description initialize a household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutDescriptionInitializeHousehold()
        {
            Fixture fixture = new Fixture();

            string name = fixture.Create<string>();
            
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
        }

        /// <summary>
        /// Tests that the constructor with the description initialize a household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithDescriptionInitializeHousehold()
        {
            Fixture fixture = new Fixture();

            string name = fixture.Create<string>();
            string description = fixture.Create<string>();

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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);

            string newValue = fixture.Create<string>();
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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Description, Is.Null);

            string newValue = fixture.Create<string>();
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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture, true);
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
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            MyHousehold sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            DateTime newValue = sut.CreationTime.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
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
            Fixture fixture = new Fixture();

            MyHousehold sut = CreateMySut(fixture);
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
            Fixture fixture = new Fixture();

            MyHousehold sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            IEnumerable<IHouseholdMember> householdMemberMockCollection = new List<IHouseholdMember>
            {
                MockRepository.GenerateMock<IHouseholdMember>(),
                MockRepository.GenerateMock<IHouseholdMember>(),
                MockRepository.GenerateMock<IHouseholdMember>()
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
        /// Tests that HouseholdMemberAdd throws an ArgumentNullException when the household member who should be member of the household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

            sut.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.Households);
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd calls HouseholdAdd on the household member who should be member of the household when the household member is not member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCallsHouseholdAddOnHouseholdMemberWhenHouseholdMemberIsNotMemberOfHousehold()
        {
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

            sut.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.HouseholdAdd(Arg<IHousehold>.Is.Equal(sut)));
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd does not call HouseholdAdd on the household member who should be member of the household when the household member is member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddDoesNotCallHouseholdAddOnHouseholdMemberWhenHouseholdMemberIsMemberOfHousehold()
        {
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);

            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold> {sut})
                .Repeat.Any();

            sut.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasNotCalled(m => m.HouseholdAdd(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that HouseholdMemberRemove throws an ArgumentNullException when the household member which should be removed as a member of this household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberRemoveThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHouseholdMember householdMember1Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember1Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember2Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember2Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember3Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember3Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHouseholdMember householdMember1Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember1Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember2Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember2Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember3Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember3Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHouseholdMember householdMember1Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember1Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember2Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember2Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember3Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember3Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            var householdMember1Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember1Mock.Stub(m => m.Households)
                .Return(new List<IHousehold> {sut})
                .Repeat.Any();

            var householdMember2Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember2Mock.Stub(m => m.Households)
                .Return(new List<IHousehold> {sut})
                .Repeat.Any();

            var householdMember3Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember3Mock.Stub(m => m.Households)
                .Return(new List<IHousehold> {sut})
                .Repeat.Any();

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
            Fixture fixture = new Fixture();

            IHouseholdMember householdMember1Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember1Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember2Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember2Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember3Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember3Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHousehold sut = CreateSut(fixture);
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
            Fixture fixture = new Fixture();

            IHouseholdMember householdMember1Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember1Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember2Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember2Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHouseholdMember householdMember3Mock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMember3Mock.Stub(m => m.Households)
                .Return(new List<Household>(0))
                .Repeat.Any();

            IHousehold sut = CreateSut(fixture);
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
        /// Tests that Translate throws an ArgumentNullException when the culture information which are used for translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslateThrowsArgumentNullExceptionWhenTranslationCultureIsNull()
        {
            Fixture fixture = new Fixture();

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Translate(null, fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "translationCulture");
        }

        /// <summary>
        /// Tests that Translate calls Translate for each household member who has a membership on this household when they should be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachHouseholdMemberWhenTranslateHouseholdMembersIsTrue()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            int numberOfHouseholdMembers = random.Next(1, 5);
            while (sut.HouseholdMembers.Count() < numberOfHouseholdMembers)
            {
                IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Households)
                    .Return(new List<IHousehold>(0))
                    .Repeat.Any();
                sut.HouseholdMemberAdd(householdMemberMock);
            }
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(numberOfHouseholdMembers));

            CultureInfo translationCulture = CultureInfo.CurrentCulture;

            sut.Translate(translationCulture, true);

            foreach (var householdMemberMock in sut.HouseholdMembers)
            {
                householdMemberMock.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture), Arg<bool>.Is.Equal(false), Arg<bool>.Is.Equal(true)));
            }
        }

        /// <summary>
        /// Tests that Translate does not call Translate on any household member who has a membership on this household when they should not be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnAnyHouseholdMemberWhenTranslateHouseholdMembersIsFalse()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IHousehold sut = CreateSut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            int numberOfHouseholdMembers = random.Next(1, 5);
            while (sut.HouseholdMembers.Count() < numberOfHouseholdMembers)
            {
                IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Households)
                    .Return(new List<IHousehold>(0))
                    .Repeat.Any();
                sut.HouseholdMemberAdd(householdMemberMock);
            }
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(numberOfHouseholdMembers));

            sut.Translate(CultureInfo.CurrentCulture, false);

            foreach (var householdMemberMock in sut.HouseholdMembers)
            {
                householdMemberMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything, Arg<bool>.Is.Anything, Arg<bool>.Is.Anything));
            }
        }

        /// <summary>
        /// Creates an instance of a household which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of a household which can be used for unit testing.</returns>
        private IHousehold CreateSut(Fixture fixture, bool hasDescription = false)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }
            return new Household(fixture.Create<string>(), hasDescription ? fixture.Create<string>() : null);
        }

        /// <summary>
        /// Creates an instance of the private class for a household which can be used for unit testing.
        /// </summary>
        /// <returns>Instance of the private class for a household which can be used for unit testing.</returns>
        private MyHousehold CreateMySut(Fixture fixture, bool hasDescription = false)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }
            return new MyHousehold(fixture.Create<string>(), hasDescription ? fixture.Create<string>() : null);
        }
    }
}
