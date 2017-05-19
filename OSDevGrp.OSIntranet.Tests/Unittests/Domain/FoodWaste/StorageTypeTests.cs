using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using Ploeh.AutoFixture;

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
            /// <param name="temperature">Defualt temperature for the storage type.</param>
            /// <param name="temperatureRange">Temperature range for the storage type.</param>
            /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
            /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
            /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
            public MyStorageType(int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable) 
                : base(temperature, temperatureRange, creatable, editable, deletable)
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the defualt temperature for the storage type.
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

            int temperature = fixture.Create<int>();
            IRange<int> temperatureRange = DomainObjectMockBuilder.BuildIntRange();
            bool creatable = fixture.Create<bool>();
            bool editable = fixture.Create<bool>();
            bool deletable = fixture.Create<bool>();

            IStorageType sut = CreateSut(temperature, temperatureRange, creatable, editable, deletable);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
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

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => CreateSut(fixture.Create<int>(), null, fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "temperatureRange");
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
        /// Creates an instance of the storage type which can be used for unit testning.
        /// </summary>
        /// <param name="temperature">Defualt temperature for the storage type.</param>
        /// <param name="temperatureRange">Temperature range for the storage type.</param>
        /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
        /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
        /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
        /// <returns>Instance of the storage type which can be used for unit testning.</returns>
        private static IStorageType CreateSut(int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
        {
            return new StorageType(temperature, temperatureRange, creatable, editable, deletable);
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

            return new MyStorageType(fixture.Create<int>(), DomainObjectMockBuilder.BuildIntRange(), fixture.Create<bool>(), fixture.Create<bool>(), fixture.Create<bool>());
        }
    }
}
