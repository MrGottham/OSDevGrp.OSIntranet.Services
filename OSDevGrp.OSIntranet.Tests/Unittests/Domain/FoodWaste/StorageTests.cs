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
            /// <param name="storageType">Storage type for the storage.</param>
            /// <param name="description">Description for the storage.</param>
            /// <param name="temperature">Temperature for the storage.</param>
            /// <param name="creationTime">Creation date and time for when the storage was created.</param>
            /// <param name="domainObjectValidations">Implementation of the common validations used by domain objects in the food waste domain.</param>
            public MyStorage(IHousehold household, int sortOrder, IStorageType storageType, string description, int temperature, DateTime creationTime, IDomainObjectValidations domainObjectValidations)
                : base(household, sortOrder, storageType, description, temperature, creationTime, domainObjectValidations)
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

            /// <summary>
            /// Gets or sets the storage type for the storage.
            /// </summary>
            public new IStorageType StorageType
            {
                get => base.StorageType;
                set => base.StorageType = value;
            }

            /// <summary>
            /// Gets or sets the creation date and time for when the storage was created.
            /// </summary>
            public new DateTime CreationTime
            {
                get => base.CreationTime;
                set => base.CreationTime = value;
            }

            #endregion
        }

        #region Private variables

        private IDomainObjectValidations _domainObjectValidationsMock;

        #endregion

        /// <summary>
        /// Tests that the constructor initialize a storage without a description.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageWithoutDescription()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            int sortOrder = GetLegalSortOrder(fixture);
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetLegalTemperature(fixture, storageType.TemperatureRange);
            DateTime creationTime = DateTime.Now.AddDays(random.Next(1, 7) * -1).AddMinutes(random.Next(-120, 120));

            IStorage sut = new Storage(householdMock, sortOrder, storageType, temperature, creationTime);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Household, Is.Not.Null);
            Assert.That(sut.Household, Is.EqualTo(householdMock));
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(sut.StorageType, Is.Not.Null);
            Assert.That(sut.StorageType, Is.EqualTo(storageType));
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.Temperature, Is.EqualTo(temperature));
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime));
        }

        /// <summary>
        /// Tests that the constructor initialize a storage with a description.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageWithDescription()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            int sortOrder = GetLegalSortOrder(fixture);
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetLegalTemperature(fixture, storageType.TemperatureRange);
            DateTime creationTime = DateTime.Now.AddDays(random.Next(1, 7) * -1).AddMinutes(random.Next(-120, 120));
            string description = fixture.Create<string>();

            IStorage sut = new Storage(householdMock, sortOrder, storageType, temperature, creationTime, description);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Household, Is.Not.Null);
            Assert.That(sut.Household, Is.EqualTo(householdMock));
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(sut.StorageType, Is.Not.Null);
            Assert.That(sut.StorageType, Is.EqualTo(storageType));
            Assert.That(sut.Description, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Empty);
            Assert.That(sut.Description, Is.EqualTo(description));
            Assert.That(sut.Temperature, Is.EqualTo(temperature));
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the household where the are placed is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            Fixture fixture = new Fixture();

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new Storage(null, GetLegalSortOrder(fixture), storageType, GetLegalTemperature(fixture, storageType.TemperatureRange), fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the storage type for the storage is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenStorageTypeIsNull()
        {
            Fixture fixture = new Fixture();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture), null, fixture.Create<int>(), fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storageType");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            Fixture fixture = new Fixture();

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyStorage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture), storageType, fixture.Create<string>(), GetLegalTemperature(fixture, storageType.TemperatureRange), fixture.Create<DateTime>(), null));
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
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            // ReSharper disable ObjectCreationAsStatement
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), sortOrder, storageType, GetLegalTemperature(fixture, storageType.TemperatureRange), fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, sortOrder, "sortOrder");
        }

        /// <summary>
        /// Tests that the constructor calls InRange on the common validations used by domain objects in the food waste domain with the temperature for the storage.
        /// </summary>
        [Test]
        public void TestThatConstructorCallsInRangeOnDomainObjectValidationsWithTemperature()
        {
            Fixture fixture = new Fixture();

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetLegalTemperature(fixture, storageType.TemperatureRange);

            MyStorage sut = CreateMySut(fixture, storageType: storageType, temperature: temperature);
            Assert.That(sut, Is.Not.Null);

            _domainObjectValidationsMock.AssertWasCalled(m => m.InRange(
                    Arg<int>.Is.Equal(temperature),
                    Arg<IRange<int>>.Matches(src => src != null && src.StartValue == storageType.TemperatureRange.StartValue && src.EndValue == storageType.TemperatureRange.EndValue)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that the constructor throws an IntranetSystemException when the temperature for the storage is not in the allowed range.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsIntranetSystemExceptionWhenTemperatureIsNotInRange()
        {
            Fixture fixture = new Fixture();

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetIllegalTemperature(fixture, storageType.TemperatureRange);

            // ReSharper disable ObjectCreationAsStatement
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture), storageType, temperature, fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, temperature, "temperature");
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

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture), storageType, GetLegalTemperature(fixture, storageType.TemperatureRange), fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            int newValue = GetIllegalSortOrder(fixture);
            Assert.That(newValue, Is.Not.EqualTo(sut.SortOrder));

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.SortOrder = newValue);

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, newValue, "value");
        }

        /// <summary>
        /// Tests that the setter of StorageType sets a new value.
        /// </summary>
        [Test]
        public void TestThatStorageTypeSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            IStorageType newValue = DomainObjectMockBuilder.BuildStorageTypeMock();
            Assert.That(newValue, Is.Not.EqualTo(sut.StorageType));

            sut.StorageType = newValue;
            Assert.That(sut.StorageType, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of StorageType throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatStorageTypeSetterThrowsArgumentNullExceptionWhenValueIsNull()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StorageType = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter of Description sets a new value.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

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
        /// Tests that the setter of Description sets the value to null.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterSetsValueToNull()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Null);
            Assert.That(sut.Description, Is.Not.Empty);

            sut.Description = null;
            Assert.That(sut.Description, Is.Null);
        }

        /// <summary>
        /// Tests that the setter of Temperature sets a new value.
        /// </summary>
        [Test]
        public void TestThatTemperatureSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorage sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.Temperature + fixture.Create<int>();
            Assert.That(newValue, Is.Not.EqualTo(sut.Temperature));

            sut.Temperature = newValue;
            Assert.That(sut.Temperature, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of Temperature calls InRange on the common validations used by domain objects in the food waste domain.
        /// </summary>
        [Test]
        public void TestThatTemperatureSetterCallsInRangeOnDomainObjectValidations()
        {
            Fixture fixture = new Fixture();

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            MyStorage sut = CreateMySut(fixture, storageType: storageType);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.Temperature + fixture.Create<int>();
            Assert.That(newValue, Is.Not.EqualTo(sut.Temperature));

            sut.Temperature = newValue;

            _domainObjectValidationsMock.AssertWasCalled(m => m.InRange(
                    Arg<int>.Is.Equal(newValue),
                    Arg<IRange<int>>.Matches(src => src != null && src.StartValue == storageType.TemperatureRange.StartValue && src.EndValue == storageType.TemperatureRange.EndValue)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that the setter of Temperature throws an IntranetSystemException when the temperature for the storage is not in the allowed range.
        /// </summary>
        [Test]
        public void TestThatTemperatureSetterThrowsIntranetSystemExceptionWhenTemperatureIsNotInRange()
        {
            Fixture fixture = new Fixture();

            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(fixture), storageType, GetLegalTemperature(fixture, storageType.TemperatureRange), fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            int newValue = GetIllegalTemperature(fixture, storageType.TemperatureRange);
            Assert.That(newValue, Is.Not.EqualTo(sut.Temperature));

            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.Temperature = newValue);

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, newValue, "value");
        }

        /// <summary>
        /// Tests that the setter of CreationTime sets a new value.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            MyStorage sut = CreateMySut(fixture, creationTime: DateTime.Now.AddDays(random.Next(1, 7) * -1).AddMinutes(random.Next(-120, 120)));
            Assert.That(sut, Is.Not.Null);

            DateTime newValue = sut.CreationTime.AddMinutes(random.Next(1, 60));
            Assert.That(newValue, Is.Not.EqualTo(sut.CreationTime));

            sut.CreationTime = newValue;
            Assert.That(sut.CreationTime, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Creates an instance of the private class for testing the storage.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <param name="sortOrder">Sets the sort order for the storage.</param>
        /// <param name="storageType">Sets the storage type for the storage.</param>
        /// <param name="temperature">Sets the temperature for the storage.</param>
        /// <param name="creationTime">Sets the creation date and time for when the storage was created.</param>
        /// <returns>Instance of the private class for testing the storage</returns>
        private MyStorage CreateMySut(Fixture fixture, int? sortOrder = null, IStorageType storageType = null, int? temperature = null, DateTime? creationTime = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            _domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
            _domainObjectValidationsMock.Stub(m => m.InRange(Arg<int>.Is.Anything, Arg<IRange<int>>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            if (storageType == null)
            {
                storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            }

            return new MyStorage(DomainObjectMockBuilder.BuildHouseholdMock(), sortOrder ?? GetLegalSortOrder(fixture), storageType, fixture.Create<string>(), temperature ?? GetLegalTemperature(fixture, storageType.TemperatureRange), creationTime ?? fixture.Create<DateTime>(), _domainObjectValidationsMock);
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

            return GetLegalSortOrder(fixture) + 100;
        }

        /// <summary>
        /// Gets a legal temperature for a storage.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <param name="temperatureRange">Temperature range.</param>
        /// <returns>Legal temperature for a storage.</returns>
        private int GetLegalTemperature(Fixture fixture, IRange<int> temperatureRange)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }
            if (temperatureRange == null)
            {
                throw new ArgumentNullException(nameof(temperatureRange));
            }

            Random random = new Random(fixture.Create<int>());
            return random.Next(temperatureRange.StartValue, temperatureRange.EndValue);
        }

        /// <summary>
        /// Gets an illegal temperature for a storage.
        /// </summary>
        /// <param name="fixture">Auto fixture.</param>
        /// <param name="temperatureRange">Temperature range.</param>
        /// <returns>Illegal temperature for a storage.</returns>
        private int GetIllegalTemperature(Fixture fixture, IRange<int> temperatureRange)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }
            if (temperatureRange == null)
            {
                throw new ArgumentNullException(nameof(temperatureRange));
            }

            return GetLegalTemperature(fixture, temperatureRange) + 100;
        }
    }
}
