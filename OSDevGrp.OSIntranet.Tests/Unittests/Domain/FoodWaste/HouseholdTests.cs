using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Ploeh.AutoFixture;
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
                get { return base.CreationTime; }
                set { base.CreationTime = value; }
            }

            /// <summary>
            /// Household members who is member of this household.
            /// </summary>
            public new IEnumerable<IHouseholdMember> HouseholdMembers
            {
                get { return base.HouseholdMembers; }
                set { base.HouseholdMembers = value; }
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor without the description initialize a household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutDescriptionInitializeHousehold()
        {
            var fixture = new Fixture();
            var name = fixture.Create<string>();
            
            var household = new Household(name);
            Assert.That(household, Is.Not.Null);
            Assert.That(household.Identifier, Is.Null);
            Assert.That(household.Identifier.HasValue, Is.False);
            Assert.That(household.Name, Is.Not.Null);
            Assert.That(household.Name, Is.Not.Empty);
            Assert.That(household.Name, Is.EqualTo(name));
            Assert.That(household.Description, Is.Null);
            Assert.That(household.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor with the description initialize a household.
        /// </summary>
        [Test]
        public void TestThatConstructorWithDescriptionInitializeHousehold()
        {
            var fixture = new Fixture();
            var name = fixture.Create<string>();
            var description = fixture.Create<string>();

            var household = new Household(name, description);
            Assert.That(household, Is.Not.Null);
            Assert.That(household.Identifier, Is.Null);
            Assert.That(household.Identifier.HasValue, Is.False);
            Assert.That(household.Name, Is.Not.Null);
            Assert.That(household.Name, Is.Not.Empty);
            Assert.That(household.Name, Is.EqualTo(name));
            Assert.That(household.Description, Is.Not.Null);
            Assert.That(household.Description, Is.Not.Empty);
            Assert.That(household.Description, Is.EqualTo(description));
            Assert.That(household.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Empty);
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
            var exception = Assert.Throws<ArgumentNullException>(() => new Household(invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("name"));
            Assert.That(exception.InnerException, Is.Null);
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
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => household.Name = invalidValue);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Name updates the name.
        /// </summary>
        [Test]
        public void TestThatNameSetterUpdatesValueOfName()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.Name, Is.Not.Null);
            Assert.That(household.Name, Is.Not.Empty);

            var newValue = fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);
            Assert.That(newValue, Is.Not.EqualTo(household.Name));

            household.Name = newValue;
            Assert.That(household.Name, Is.Not.Null);
            Assert.That(household.Name, Is.Not.Empty);
            Assert.That(household.Name, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Description updates the description when value is not null.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterUpdatesValueOfDescriptionWhenValueIsNotNull()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.Description, Is.Null);

            var newValue = fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);
            Assert.That(newValue, Is.Not.EqualTo(household.Description));

            household.Description = newValue;
            Assert.That(household.Description, Is.Not.Null);
            Assert.That(household.Description, Is.Not.Empty);
            Assert.That(household.Description, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Description updates the description when value is null.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterUpdatesValueOfDescriptionValueIsNull()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>(), fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.Description, Is.Not.Null);
            Assert.That(household.Description, Is.Not.Empty);

            household.Description = null;
            Assert.That(household.Description, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for CreationTime updates the date and time for when the household was created.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterUpdatesValueOfCreationTime()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var household = new MyHousehold(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.CreationTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            var newValue = household.CreationTime.AddDays(random.Next(1, 7)*-1).AddMinutes(random.Next(120, 240));
            Assert.That(newValue, Is.Not.EqualTo(household.CreationTime));

            household.CreationTime = newValue;
            Assert.That(household.CreationTime, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for HouseholdMembers throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            var fixture = new Fixture();

            var household = new MyHousehold(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => household.HouseholdMembers = null);
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("value"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for HouseholdMembers updates household members who is member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersSetterUpdatesValueOfHouseholdMembers()
        {
            var fixture = new Fixture();

            var household = new MyHousehold(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Empty);

            var householdMemberMockCollection = new List<IHouseholdMember>
            {
                MockRepository.GenerateMock<IHouseholdMember>(),
                MockRepository.GenerateMock<IHouseholdMember>(),
                MockRepository.GenerateMock<IHouseholdMember>()
            };
            Assert.That(householdMemberMockCollection, Is.Not.Null);
            Assert.That(householdMemberMockCollection, Is.Not.Empty);
            Assert.That(householdMemberMockCollection, Is.Not.EqualTo(household.HouseholdMembers));

            household.HouseholdMembers = householdMemberMockCollection;
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Empty);
            Assert.That(household.HouseholdMembers, Is.EqualTo(householdMemberMockCollection));
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd throws an ArgumentNullException when the household member who should be member of the household is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddThrowsArgumentNullExceptionWhenHouseholdMemberIsNull()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => household.HouseholdMemberAdd(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMember"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd adds the household member who should be member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddAddsHouseholdMember()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Empty);

            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

            household.HouseholdMemberAdd(householdMemberMock);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Empty);
            Assert.That(household.HouseholdMembers.Count(), Is.EqualTo(1));
            Assert.That(household.HouseholdMembers.Contains(householdMemberMock), Is.True);
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd calls Households on the household member who should be member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCallsHouseholdsOnHouseholdMember()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

            household.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.Households);
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd calls HouseholdAdd on the household member who should be member of the household when the household member is not member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddCallsHouseholdAddOnHouseholdMemberWhenHouseholdMemberIsNotMemberOfHousehold()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

            household.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasCalled(m => m.HouseholdAdd(Arg<IHousehold>.Is.Equal(household)));
        }

        /// <summary>
        /// Tests that HouseholdMemberAdd does not call HouseholdAdd on the household member who should be member of the household when the household member is member of the household.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAddDoesNotCallHouseholdAddOnHouseholdMemberWhenHouseholdMemberIsMemberOfHousehold()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold> {household})
                .Repeat.Any();

            household.HouseholdMemberAdd(householdMemberMock);

            householdMemberMock.AssertWasNotCalled(m => m.HouseholdAdd(Arg<IHousehold>.Is.Anything));
        }

        /// <summary>
        /// Tests that Translate throws an ArgumentNullException when the culture information which are used for translation is null.
        /// </summary>
        [Test]
        public void TestThatTranslateThrowsArgumentNullExceptionWhenTranslationCultureIsNull()
        {
            var fixture = new Fixture();

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => household.Translate(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationCulture"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Translate calls Translate for each household member who has a membership on this household when they should be translated.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnEachHouseholdMemberWhenTranslateHouseholdMembersIsTrue()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Empty);

            var numberOfHouseholdMembers = random.Next(1, 5);
            while (household.HouseholdMembers.Count() < numberOfHouseholdMembers)
            {
                var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Households)
                    .Return(new List<IHousehold>(0))
                    .Repeat.Any();
                household.HouseholdMemberAdd(householdMemberMock);
            }
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Empty);
            Assert.That(household.HouseholdMembers.Count(), Is.EqualTo(numberOfHouseholdMembers));

            var translationCulture = CultureInfo.CurrentCulture;

            household.Translate(translationCulture, true);

            foreach (var householdMemberMock in household.HouseholdMembers)
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
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());

            var household = new Household(fixture.Create<string>());
            Assert.That(household, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Empty);

            var numberOfHouseholdMembers = random.Next(1, 5);
            while (household.HouseholdMembers.Count() < numberOfHouseholdMembers)
            {
                var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Households)
                    .Return(new List<IHousehold>(0))
                    .Repeat.Any();
                household.HouseholdMemberAdd(householdMemberMock);
            }
            Assert.That(household.HouseholdMembers, Is.Not.Null);
            Assert.That(household.HouseholdMembers, Is.Not.Empty);
            Assert.That(household.HouseholdMembers.Count(), Is.EqualTo(numberOfHouseholdMembers));

            household.Translate(CultureInfo.CurrentCulture, false);

            foreach (var householdMemberMock in household.HouseholdMembers)
            {
                householdMemberMock.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything, Arg<bool>.Is.Anything, Arg<bool>.Is.Anything));
            }
        }
    }
}
