using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Tests the storage type.
    /// </summary>
    [TestFixture]
    public class StorageTypeTests
    {
        /// <summary>
        /// Private class for testing the storage type domain object.
        /// </summary>
        private class MyStorageType : StorageType
        {
            #region Constructor

            /// <summary>
            /// Creates an instance of the private class which can be used to test the storage type domain object.
            /// </summary>
            /// <param name="sortOrder">Order for sortering storage types.</param>
            /// <param name="temperature">Default temperature for the storage type.</param>
            /// <param name="temperatureRange">Temperature range for the storage type.</param>
            /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
            /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
            /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
            public MyStorageType(int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable) 
                : base(sortOrder, temperature, temperatureRange, creatable, editable, deletable)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the order for sortering storage types.
            /// </summary>
            public new int SortOrder
            {
                get => base.SortOrder;
                set => base.SortOrder = value;
            }

            /// <summary>
            /// Gets or sets the default temperature for the storage type.
            /// </summary>
            public new int Temperature
            {
                get => base.Temperature;
                set => base.Temperature = value;
            }

            /// <summary>
            /// Gets or sets the temperature range for the storage type.
            /// </summary>
            public new IRange<int> TemperatureRange
            {
                get => base.TemperatureRange;
                set => base.TemperatureRange = value;
            }

            /// <summary>
            /// Gets or sets whether household members can create storages of this type.
            /// </summary>
            public new bool Creatable
            {
                get => base.Creatable;
                set => base.Creatable = value;
            }

            /// <summary>
            /// Gets or sets whether household members can edit storages of this type.
            /// </summary>
            public new bool Editable
            {
                get => base.Editable;
                set => base.Editable = value;
            }

            /// <summary>
            /// Gets whether household members can delete storages of this type.
            /// </summary>
            public new bool Deletable
            {
                get => base.Deletable;
                set => base.Deletable = value;
            }

            #endregion
        }

        /// <summary>
        /// Tests that the constructor initialize a storage type.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageType()
        {
            Fixture fixture = new Fixture();

            int sortOrder = fixture.Create<int>();
            int temperature = fixture.Create<int>();
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            bool creatable = fixture.Create<bool>();
            bool editable = fixture.Create<bool>();
            bool deletable = fixture.Create<bool>();

            IStorageType sut = CreateSut(sortOrder, temperature, temperatureRange, creatable, editable, deletable);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.SortOrder, Is.EqualTo(sortOrder));
            Assert.That(sut.Temperature, Is.EqualTo(temperature));
            Assert.That(sut.TemperatureRange, Is.Not.Null);
            Assert.That(sut.TemperatureRange, Is.EqualTo(temperatureRange));
            Assert.That(sut.Creatable, Is.EqualTo(creatable));
            Assert.That(sut.Editable, Is.EqualTo(editable));
            Assert.That(sut.Deletable, Is.EqualTo(deletable));
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the temperature range for the storage type is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenTemperatureRangeIsNull()
        {
            Fixture fixture = new Fixture();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => CreateSut(fixture.Create<int>(), fixture.Create<int>(), null, fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "temperatureRange");
        }

        /// <summary>
        /// Tests that the setter of SortOrder sets a new value.
        /// </summary>
        [Test]
        public void TestThatSortOrderSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.SortOrder + fixture.Create<int>();
            Assert.That(sut.SortOrder, Is.Not.EqualTo(newValue));

            sut.SortOrder = newValue;
            Assert.That(sut.SortOrder, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of Temperature sets a new value.
        /// </summary>
        [Test]
        public void TestThatTemperatureSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            int newValue = sut.Temperature + fixture.Create<int>();
            Assert.That(sut.Temperature, Is.Not.EqualTo(newValue));

            sut.Temperature = newValue;
            Assert.That(sut.Temperature, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of TemperatureRange sets a new value.
        /// </summary>
        [Test]
        public void TestThatTemperatureRangeSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            IRange<int> newValue = DomainObjectMockBuilder.BuildIntRange();
            Assert.That(sut.TemperatureRange, Is.Not.EqualTo(newValue));

            sut.TemperatureRange = newValue;
            Assert.That(sut.TemperatureRange, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of TemperatureRange throws an ArgumentNullException when the value is null.
        /// </summary>
        [Test]
        public void TestThatTemperatureRangeSetterThrowsArgumentNullExceptionWhenNewValueIsNull()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.TemperatureRange = null);

            TestHelper.AssertArgumentNullExceptionIsValid(result, "value");
        }

        /// <summary>
        /// Tests that the setter of Creatable sets a new value.
        /// </summary>
        [Test]
        public void TestThatCreatableSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            bool newValue = !sut.Creatable;
            Assert.That(sut.Creatable, Is.Not.EqualTo(newValue));

            sut.Creatable = newValue;
            Assert.That(sut.Creatable, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of Editable sets a new value.
        /// </summary>
        [Test]
        public void TestThatEditableSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            bool newValue = !sut.Editable;
            Assert.That(sut.Editable, Is.Not.EqualTo(newValue));

            sut.Editable = newValue;
            Assert.That(sut.Editable, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter of Deletable sets a new value.
        /// </summary>
        [Test]
        public void TestThatDeletableSetterSetsNewValue()
        {
            Fixture fixture = new Fixture();

            MyStorageType sut = CreateMySut(fixture);
            Assert.That(sut, Is.Not.Null);

            bool newValue = !sut.Deletable;
            Assert.That(sut.Deletable, Is.Not.EqualTo(newValue));

            sut.Deletable = newValue;
            Assert.That(sut.Deletable, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Test that the getter of IdentifierForRefrigerator gets the unique identifier for the refrigerator storage type.
        /// </summary>
        [Test]
        public void TestThatIdentifierForRefrigeratorGetterGetsUniqueIdentifier()
        {
            Guid uniqueIdentifier = StorageType.IdentifierForRefrigerator;
            Assert.That(uniqueIdentifier.ToString("D").ToUpper(), Is.EqualTo("3CEA8A7D-01A4-40BF-AB96-F70354015352"));
        }

        /// <summary>
        /// Test that the getter of IdentifierForFreezer gets the unique identifier for the freezer storage type.
        /// </summary>
        [Test]
        public void TestThatIdentifierForFreezerGetterGetsUniqueIdentifier()
        {
            Guid uniqueIdentifier = StorageType.IdentifierForFreezer;
            Assert.That(uniqueIdentifier.ToString("D").ToUpper(), Is.EqualTo("959A0D7D-A034-405C-8F6E-EF49ED5E7553"));
        }

        /// <summary>
        /// Test that the getter of IdentifierForKitchenCabinets gets the unique identifier for the kitchen cabinets storage type.
        /// </summary>
        [Test]
        public void TestThatIdentifierForKitchenCabinetsGetterGetsUniqueIdentifier()
        {
            Guid uniqueIdentifier = StorageType.IdentifierForKitchenCabinets;
            Assert.That(uniqueIdentifier.ToString("D").ToUpper(), Is.EqualTo("0F78276B-87D1-4660-8708-A119C5DAA3A9"));
        }

        /// <summary>
        /// Test that the getter of IdentifierForShoppingBasket gets the unique identifier for the shopping basket storage type.
        /// </summary>
        [Test]
        public void TestThatIdentifierForShoppingBasketGetterGetsUniqueIdentifier()
        {
            Guid uniqueIdentifier = StorageType.IdentifierForShoppingBasket;
            Assert.That(uniqueIdentifier.ToString("D").ToUpper(), Is.EqualTo("B5A0B40D-1709-48D9-83F2-E87D54ED80F5"));
        }

        /// <summary>
        /// Creates an instance of the storage type which can be used for unit testning.
        /// </summary>
        /// <param name="sortOrder">Order for sortering storage types.</param>
        /// <param name="temperature">Default temperature for the storage type.</param>
        /// <param name="temperatureRange">Temperature range for the storage type.</param>
        /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
        /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
        /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
        /// <returns>Instance of the storage type which can be used for unit testning.</returns>
        private static IStorageType CreateSut(int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
        {
            return new StorageType(sortOrder, temperature, temperatureRange, creatable, editable, deletable);
        }

        /// <summary>
        /// Creates an instance of the private class which can be used to test the storage type domain object.
        /// </summary>
        /// <param name="fixture">Fixture which can provide random values.</param>
        /// <returns>instance of the private class which can be used to test the storage type domain object.</returns>
        private static MyStorageType CreateMySut(Fixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new MyStorageType(fixture.Create<int>(), fixture.Create<int>(), DomainObjectMockBuilder.BuildIntRange(), fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>());
        }
    }
}
