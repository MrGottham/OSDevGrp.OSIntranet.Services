using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given storage.
    /// </summary>
    [TestFixture]
    public class StorageProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given storage.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageProxy()
        {
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Household, Is.Null);
            Assert.That(sut.SortOrder, Is.EqualTo(default(int)));
            Assert.That(sut.StorageType, Is.Null);
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.Temperature, Is.EqualTo(default(int)));
            Assert.That(sut.CreationTime, Is.EqualTo(default(DateTime)));
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the storage.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStorageProxy()
        {
            Guid identifier = Guid.NewGuid();

            IStorageProxy sut = CreateSut(identifier);
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
        /// Tests that getter of UniqueId throws an IntranetRepositoryException when the storage has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStorageHasNoIdentifier()
        {
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.UniqueId.ToUpper());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given storage is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenStorageIsNull()
        {
            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetSqlQueryForId(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "storage");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given storage has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnStorageHasNoValue()
        {
            IStorage storageMock = MockRepository.GenerateMock<IStorage>();
            storageMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.GetSqlQueryForId(storageMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, storageMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given storage.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            Guid identifier = Guid.NewGuid();

            IStorage storageMock = MockRepository.GenerateMock<IStorage>();
            storageMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();

            IStorageProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string result = sut.GetSqlQueryForId(storageMock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"SELECT StorageIdentifier,HouseholdIdentifier,SortOrder,StorageTypeIdentifier,Descr,Temperature,CreationTime FROM Storages WHERE StorageIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage which should be used for unit testing.</returns>
        private static IStorageProxy CreateSut(Guid? storageIdentifier = null)
        {
            return new StorageProxy
            {
                Identifier = storageIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given storage which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given storage which should be used for unit testing.</returns>
        private static IStorageProxy CreateSut(Guid storageIdentifier, IHousehold household, int sortOrder, IStorageType storageType, int temperature, DateTime creationTime, string description = null)
        {
            return new StorageProxy(household, sortOrder, storageType, temperature, creationTime, description)
            {
                Identifier = storageIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of a MySQL data reader which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a MySQL data reader which should be used for unit testing.</returns>
        private static MySqlDataReader CreateMySqlDataReaderStub(Guid storageIdentifier, Guid householdIdentifier, int sortOrder, Guid storageTypeIdentifier, string description, int temperature, IRange<int> temperatureRange, DateTime creationTime)
        {
            if (temperatureRange == null)
            {
                throw new ArgumentNullException(nameof(temperatureRange));
            }

            MySqlDataReader mySqlDataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("StorageIdentifier")))
                .Return(storageIdentifier.ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return(householdIdentifier.ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("SortOrder")))
                .Return((short) sortOrder)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("StorageTypeIdentifier")))
                .Return(storageTypeIdentifier.ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")))
                .Return(4)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<int>.Is.Equal(4)))
                .Return(description)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetInt16(Arg<string>.Is.Equal("Temperature")))
                .Return((short) temperature)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(creationTime)
                .Repeat.Any();
            return mySqlDataReaderStub;
        }

        /// <summary>
        /// Creates an instance of a data provider which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a data provider which should be used for unit testing</returns>
        private static IDataProviderBase CreateDataProviderMock(Fixture fixture, StorageTypeProxy storageTypeProxy = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            IDataProviderBase dataProviderMock = MockRepository.GenerateMock<IDataProviderBase>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Get(Arg<StorageTypeProxy>.Is.Anything))
                .Return(storageTypeProxy ?? fixture.Create<StorageTypeProxy>())
                .Repeat.Any();
            return dataProviderMock;
        }
    }
}
