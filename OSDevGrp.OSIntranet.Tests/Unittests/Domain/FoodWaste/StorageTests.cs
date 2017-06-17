using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the storage.
    /// </summary>
    [TestFixture]
    public class StorageTests
    {
        /// <summary>
        /// Private class for testing the storage.
        /// </summary>
        private class MyStorage : Storage
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class for testing the storage.
            /// </summary>
            /// <param name="household">Household where the storage are placed.</param>
            /// <param name="sortOrder">Sort order for the storage.</param>
            /// <param name="domainObjectValidations">Implementation of the common validations used by domain objects in the food waste domain.</param>
            public MyStorage(IHousehold household, int sortOrder, IDomainObjectValidations domainObjectValidations)
                : base(household, sortOrder, domainObjectValidations)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the household where the storage are placed.
            /// </summary>
            public new IHousehold Household
            {
                get => base.Household;
                set => base.Household = value;
            }

            #endregion
        }

        #region Private variables

        private IDomainObjectValidations _domainObjectValidationsMock;

        #endregion

        /// <summary>
        /// Tests that the constructor initialize a storage.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorage()
        {
            Fixture fixture = new Fixture();

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            int sortOrder = GetLegalSortOrder(fixture);

            IStorage sut = new Storage(householdMock, sortOrder);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Household, Is.Not.Null);
            Assert.That(sut.Household, Is.EqualTo(householdMock));
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the household where the are placed is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new Storage(null, GetLegalSortOrder(fixture)));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyStorage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture), null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "domainObjectValidations");
        }

        /// <summary>
        /// Tests that the constructor calls InRange on the common validations used by domain objects in the food waste domain with the sort order for the storage.
        /// </summary>
        [Test]
        public void TestThatConstructorCallsInRangeOnDomainObjectValidationsWithSortOrder()
        {
            Fixture fixture = new Fixture();

            int sortOrder = GetLegalSortOrder(fixture);

            MyStorage sut = CreateMySut(fixture, sortOrder);
            Assert.That(sut, Is.Not.Null);

            _domainObjectValidationsMock.AssertWasCalled(m => m.InRange(
                    Arg<int>.Is.Equal(sortOrder),
                    Arg<IRange<int>>.Matches(src => src != null && src.StartValue == 1 && src.EndValue == 100)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that the constructor throws an IntranetSystemException when the sort order for the storage is not in the allowed range.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsIntranetSystemExceptionWhenSortOrderIsNotInRange()
        {
            Fixture fixture = new Fixture();

            int sortOrder = GetIllegalSortOrder(fixture);

            // ReSharper disable ObjectCreationAsStatement
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), sortOrder));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, sortOrder, "sortOrder");
        }

        /// <summary>
        /// Tests that the setter of Household sets a new value.
        /// </summary>
        [Test]
        public void TestThatHouseholdSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            IHousehold newValue = DomainObjectMockBuilder.BuildHouseholdMock();
            Assert.That(newValue, Is.Not.EqualTo(sut.Household));

            sut.Household = newValue;
            Assert.That(sut.Household, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of Household throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatHouseholdSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Household = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter of SortOrder sets a new value.
        /// </summary>
        [Test]
        public void TestThatSortOrderSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.SortOrder + fixture.Create<int>();
            Assert.That(newValue, Is.Not.EqualTo(sut.SortOrder));

            sut.SortOrder = newValue;
            Assert.That(sut.SortOrder, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of SortOrder calls InRange on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatSortOrderSetterCallsInRangeOnDomainObjectValidations()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.SortOrder + fixture.Create<int>();
            Assert.That(newValue, Is.Not.EqualTo(sut.SortOrder));

            sut.SortOrder = newValue;

            _domainObjectValidationsMock.AssertWasCalled(m => m.InRange(
                    Arg<int>.Is.Equal(newValue),
                    Arg<IRange<int>>.Matches(src => src != null && src.StartValue == 1 && src.EndValue == 100)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that the setter of SortOrder throws an IntranetSystemException when the sort order for the storage is not in the allowed range.
        /// </summary>
        [Test]
        public void TestThatSortOrderSetterThrowsIntranetSystemExceptionWhenSortOrderIsNotInRange()
        {
            Fixture fixture = new Fixture();

            IStorage sut = new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture));
            Assert.That(sut, Is.Not.Null);

            int newValue = GetIllegalSortOrder(fixture);
            Assert.That(newValue, Is.Not.EqualTo(sut.SortOrder));

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.SortOrder = newValue);

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, newValue, "value");
        }

        /// <summary>
        /// Creates an instance of the private class for testing the storage.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <param name="sortOrder">Sets the sort order for the storage.</param>
        /// <returns>Instance of the private class for testing the storage</returns>
        private MyStorage CreateMySut(Fixture fixture, int? sortOrder = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            _domainObjectValidationsMock.Stub(m => m.InRange(Arg<int>.Is.Anything, Arg<IRange<int>>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            return new MyStorage(DomainObjectMockBuilder.BuildHouseholdMock(), sortOrder ?? GetLegalSortOrder(fixture), _domainObjectValidationsMock);
        }

        /// <summary>
        /// Gets a legal sort order for a storage.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <returns>Legal sort order for a storage.</returns>
        private int GetLegalSortOrder(Fixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());
            return random.Next(1, 100);
        }

        /// <summary>
        /// Gets an illegal sort order for a storage.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <returns>Illegal sort order for a storage.</returns>
        private int GetIllegalSortOrder(Fixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            Random random = new Random(fixture.Create<int>());
            return random.Next(1, 100) + 100;
        }
    }
}
