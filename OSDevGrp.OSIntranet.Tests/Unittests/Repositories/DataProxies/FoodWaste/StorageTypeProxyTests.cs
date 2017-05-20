using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given storage type.
    /// </summary>
    [TestFixture]
    public class StorageTypeProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given storage type.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageTypeProxy()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.SortOrder, Is.EqualTo(default(int)));
            Assert.That(sut.Temperature, Is.EqualTo(default(int)));
            Assert.That(sut.TemperatureRange, Is.Null);
            Assert.That(sut.Creatable, Is.EqualTo(default(bool)));
            Assert.That(sut.Editable, Is.EqualTo(default(bool)));
            Assert.That(sut.Deletable, Is.EqualTo(default(bool)));
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the storage type.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStorageTypeProxy()
        {
            Guid identifier = Guid.NewGuid();

            IStorageTypeProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.Value, Is.EqualTo(identifier));

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that getter of UniqueId throws an IntranetRepositoryException when the storage type has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStorageTypeHasNoIdentifier()
        {
            IStorageTypeProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.UniqueId.ToUpper());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage type which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage type which should be used for unit testing.</returns>
        private static IStorageTypeProxy CreateSut(Guid? storageTypeIdentifier = null)
        {
            return new StorageTypeProxy
            {
                Identifier = storageTypeIdentifier
            };
        }
    }
}
