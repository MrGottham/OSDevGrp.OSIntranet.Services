using System;
using System.Globalization;
using System.Threading;
using AutoFixture;
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

        private Fixture _fixture;
        private Random _random;
        private IDomainObjectValidations _domainObjectValidationsMock;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
            _domainObjectValidationsMock = MockRepository.GenerateMock<IDomainObjectValidations>();
        }

        /// <summary>
        /// Tests that the constructor initialize a storage without a description.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageWithoutDescription()
        {
            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            int sortOrder = GetLegalSortOrder();
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetLegalTemperature(storageType.TemperatureRange);
            DateTime creationTime = DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120));

            IStorage sut = CreateSut(householdMock, sortOrder, storageType, temperature, creationTime);
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
            IHousehold householdMock = DomainObjectMockBuilder.BuildHouseholdMock();
            int sortOrder = GetLegalSortOrder();
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetLegalTemperature(storageType.TemperatureRange);
            DateTime creationTime = DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120));
            string description = _fixture.Create<string>();

            IStorage sut = CreateSut(householdMock, sortOrder, storageType, temperature, creationTime, description);
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
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new Storage(null, GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the storage type for the storage is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenStorageTypeIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), null, _fixture.Create<int>(), _fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storageType");
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the common validations used by domain objects in the food waste domain is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenDomainObjectValidationsIsNull()
        {
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MyStorage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, _fixture.Create<string>(), GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>(), null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "domainObjectValidations");
        }

        /// <summary>
        /// Tests that the constructor calls InRange on the common validations used by domain objects in the food waste domain with the sort order for the storage.
        /// </summary>
        [Test]
        public void TestThatConstructorCallsInRangeOnDomainObjectValidationsWithSortOrder()
        {
            int sortOrder = GetLegalSortOrder();

            MyStorage sut = CreateMySut(sortOrder);
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
            int sortOrder = GetIllegalSortOrder();
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            // ReSharper disable ObjectCreationAsStatement
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), sortOrder, storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, sortOrder, "sortOrder");
        }

        /// <summary>
        /// Tests that the constructor calls InRange on the common validations used by domain objects in the food waste domain with the temperature for the storage.
        /// </summary>
        [Test]
        public void TestThatConstructorCallsInRangeOnDomainObjectValidationsWithTemperature()
        {
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetLegalTemperature(storageType.TemperatureRange);

            MyStorage sut = CreateMySut(storageType: storageType, temperature: temperature);
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
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            int temperature = GetIllegalTemperature(storageType.TemperatureRange);

            // ReSharper disable ObjectCreationAsStatement
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => new Storage(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, temperature, _fixture.Create<DateTime>()));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertIntranetSystemExceptionIsValid(result, ExceptionMessage.IllegalValue, temperature, "temperature");
        }

        /// <summary>
        /// Tests that the setter of Household sets a new value.
        /// </summary>
        [Test]
        public void TestThatHouseholdSetterSetsNewValue()
        {
            MyStorage sut = CreateMySut();
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
            MyStorage sut = CreateMySut();
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
            MyStorage sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.SortOrder + _fixture.Create<int>();
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
            MyStorage sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.SortOrder + _fixture.Create<int>();
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
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            int newValue = GetIllegalSortOrder();
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
            MyStorage sut = CreateMySut();
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
            MyStorage sut = CreateMySut();
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
            MyStorage sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

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
        /// Tests that the setter of Description sets the value to null.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterSetsValueToNull()
        {
            MyStorage sut = CreateMySut();
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
            MyStorage sut = CreateMySut();
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.Temperature + _fixture.Create<int>();
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
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            MyStorage sut = CreateMySut(storageType: storageType);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.Temperature + _fixture.Create<int>();
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
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            int newValue = GetIllegalTemperature(storageType.TemperatureRange);
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
            MyStorage sut = CreateMySut(creationTime: DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)));
            Assert.That(sut, Is.Not.Null);

            DateTime newValue = sut.CreationTime.AddMinutes(_random.Next(1, 60));
            Assert.That(newValue, Is.Not.EqualTo(sut.CreationTime));

            sut.CreationTime = newValue;
            Assert.That(sut.CreationTime, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that Translate throws an <see cref="ArgumentNullException"/> when the translation culture is null.
        /// </summary>
        [Test]
        public void TestThatTranslateThrowsArgumentNullExceptionWhenTranslationCultureIsNull()
        {
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Translate(null, _fixture.Create<bool>(), _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "translationCulture");
        }

        /// <summary>
        /// Tests that Translate calls Translate on the <see cref="IHousehold"/> when translate household is true.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnHouseholdWhenTranslateHouseholdIsTrue()
        {
            IHousehold household = DomainObjectMockBuilder.BuildHouseholdMock();
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(household, GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            CultureInfo translationCulture = Thread.CurrentThread.CurrentUICulture;

            sut.Translate(translationCulture, true, _fixture.Create<bool>());

            household.AssertWasCalled(m => m.Translate(
                    Arg<CultureInfo>.Is.Equal(translationCulture),
                    Arg<bool>.Is.Equal(true),
                    Arg<bool>.Is.Equal(false)),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translate does not call Translate on the <see cref="IHousehold"/> when translate household is false.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnHouseholdWhenTranslateHouseholdIsFalse()
        {
            IHousehold household = DomainObjectMockBuilder.BuildHouseholdMock();
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(household, GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            CultureInfo translationCulture = Thread.CurrentThread.CurrentUICulture;

            sut.Translate(translationCulture, false, _fixture.Create<bool>());

            household.AssertWasNotCalled(m => m.Translate(
                Arg<CultureInfo>.Is.Anything,
                Arg<bool>.Is.Anything,
                Arg<bool>.Is.Anything));
        }

        /// <summary>
        /// Tests that Translate calls Translate on the <see cref="IStorageType"/> when translate storage type is true.
        /// </summary>
        [Test]
        public void TestThatTranslateCallsTranslateOnStorageTypeWhenTranslateStorageTypeIsTrue()
        {
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            CultureInfo translationCulture = Thread.CurrentThread.CurrentUICulture;

            sut.Translate(translationCulture, _fixture.Create<bool>());

            storageType.AssertWasCalled(m => m.Translate(Arg<CultureInfo>.Is.Equal(translationCulture)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that Translate does not call Translate on the <see cref="IStorageType"/> when translate storage type is false.
        /// </summary>
        [Test]
        public void TestThatTranslateDoesNotCallTranslateOnStorageTypeWhenTranslateStorageTypeIsFalse()
        {
            IStorageType storageType = DomainObjectMockBuilder.BuildStorageTypeMock();

            IStorage sut = CreateSut(DomainObjectMockBuilder.BuildHouseholdMock(), GetLegalSortOrder(), storageType, GetLegalTemperature(storageType.TemperatureRange), _fixture.Create<DateTime>());
            Assert.That(sut, Is.Not.Null);

            CultureInfo translationCulture = Thread.CurrentThread.CurrentUICulture;

            sut.Translate(translationCulture, _fixture.Create<bool>(), false);

            storageType.AssertWasNotCalled(m => m.Translate(Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Creates an instance of the <see cref="Storage"/> which can be used for unit testing.
        /// </summary>
        /// <param name="household">The <see cref="IHousehold"/> for the <see cref="Storage"/>.</param>
        /// <param name="sortOrder">The sort order for the <see cref="Storage"/>.</param>
        /// <param name="storageType">The <see cref="IStorageType"/> for the <see cref="Storage"/>.</param>
        /// <param name="temperature">The temperature for the <see cref="Storage"/>.</param>
        /// <param name="creationTime">The creation date and time for when the <see cref="Storage"/> was created.</param>
        /// <param name="description">The description for the <see cref="Storage"/>.</param>
        /// <returns>Instance of the <see cref="Storage"/> which can be used for unit testing.</returns>
        private IStorage CreateSut(IHousehold household, int sortOrder, IStorageType storageType, int temperature, DateTime creationTime, string description = null)
        {
            return new Storage(household, sortOrder, storageType, temperature, creationTime, description);
        }

        /// <summary>
        /// Creates an instance of the private class for testing the storage.
        /// </summary>
        /// <param name="sortOrder">Sets the sort order for the storage.</param>
        /// <param name="storageType">Sets the storage type for the storage.</param>
        /// <param name="temperature">Sets the temperature for the storage.</param>
        /// <param name="creationTime">Sets the creation date and time for when the storage was created.</param>
        /// <returns>Instance of the private class for testing the storage</returns>
        private MyStorage CreateMySut(int? sortOrder = null, IStorageType storageType = null, int? temperature = null, DateTime? creationTime = null)
        {
            _domainObjectValidationsMock.Stub(m => m.InRange(Arg<int>.Is.Anything, Arg<IRange<int>>.Is.Anything))
                .Return(true)
                .Repeat.Any();

            if (storageType == null)
            {
                storageType = DomainObjectMockBuilder.BuildStorageTypeMock();
            }

            return new MyStorage(DomainObjectMockBuilder.BuildHouseholdMock(), sortOrder ?? GetLegalSortOrder(), storageType, _fixture.Create<string>(), temperature ?? GetLegalTemperature(storageType.TemperatureRange), creationTime ?? _fixture.Create<DateTime>(), _domainObjectValidationsMock);
        }

        /// <summary>
        /// Gets a legal sort order for a storage.
        /// </summary>
        /// <returns>Legal sort order for a storage.</returns>
        private int GetLegalSortOrder()
        {
            return _random.Next(1, 100);
        }

        /// <summary>
        /// Gets an illegal sort order for a storage.
        /// </summary>
        /// <returns>Illegal sort order for a storage.</returns>
        private int GetIllegalSortOrder()
        {
            return GetLegalSortOrder() + 100;
        }

        /// <summary>
        /// Gets a legal temperature for a storage.
        /// </summary>
        /// <param name="temperatureRange">Temperature range.</param>
        /// <returns>Legal temperature for a storage.</returns>
        private int GetLegalTemperature(IRange<int> temperatureRange)
        {
            ArgumentNullGuard.NotNull(temperatureRange, nameof(temperatureRange));

            return _random.Next(temperatureRange.StartValue, temperatureRange.EndValue);
        }

        /// <summary>
        /// Gets an illegal temperature for a storage.
        /// </summary>
        /// <param name="temperatureRange">Temperature range.</param>
        /// <returns>Illegal temperature for a storage.</returns>
        private int GetIllegalTemperature(IRange<int> temperatureRange)
        {
            ArgumentNullGuard.NotNull(temperatureRange, nameof(temperatureRange));

            return GetLegalTemperature(temperatureRange) + 100;
        }
    }
}
