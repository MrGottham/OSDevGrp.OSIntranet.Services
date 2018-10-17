using System;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given foreign key.
    /// </summary>
    [TestFixture]
    public class ForeignKeyProxyTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given foreign key.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeForeignKeyProxy()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.DataProvider, Is.Null);
            Assert.That(sut.ForeignKeyForIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(sut.ForeignKeyForTypes, Is.Null);
            Assert.That(sut.ForeignKeyValue, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the foreign key has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenForeignKeyProxyHasNoIdentifier()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.Identifier = null;

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the foreign key.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForForeignKeyProxy()
        {
            Guid identifier = Guid.NewGuid();

            IForeignKeyProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateFoodWasteDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapDataMapsDataIntoProxy()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foreignKeyIdentifier = Guid.NewGuid();
            Guid foreignKeyForIdentifier = Guid.NewGuid();
            Type foreignKeyForType = typeof(DataProvider);
            string foreignKeyValue = _fixture.Create<string>();
            MySqlDataReader dataReader = CreateMySqlDataReader(foreignKeyIdentifier, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue);

            IDataProviderProxy dataProviderProxy = BuildDataProviderProxy();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(dataProviderProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(foreignKeyIdentifier));
            Assert.That(sut.DataProvider, Is.Not.Null);
            Assert.That(sut.DataProvider, Is.EqualTo(dataProviderProxy));
            Assert.That(sut.ForeignKeyForIdentifier, Is.EqualTo(foreignKeyForIdentifier));
            Assert.That(sut.ForeignKeyForTypes, Is.Not.Null);
            Assert.That(sut.ForeignKeyForTypes, Is.Not.Empty);
            Assert.That(sut.ForeignKeyForTypes.Count(), Is.EqualTo(4));
            Assert.That(sut.ForeignKeyForTypes.Contains(typeof(IDomainObject)), Is.True);
            Assert.That(sut.ForeignKeyForTypes.Contains(typeof(IIdentifiable)), Is.True);
            Assert.That(sut.ForeignKeyForTypes.Contains(typeof(ITranslatable)), Is.True);
            Assert.That(sut.ForeignKeyForTypes.Contains(typeof(IDataProvider)), Is.True);
            Assert.That(sut.ForeignKeyValue, Is.Not.Null);
            Assert.That(sut.ForeignKeyValue, Is.Not.Empty);
            Assert.That(sut.ForeignKeyValue, Is.EqualTo(foreignKeyValue));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyForIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyForTypes")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ForeignKeyValue")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IDataProviderProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 4 &&
                                               e[0] == "DataProviderIdentifier" &&
                                               e[1] == "DataProviderName" &&
                                               e[2] == "HandlesPayments" &&
                                               e[3] == "DataSourceStatementIdentifier")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the foreign key is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the foreign key is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IForeignKeyProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given foreign key.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IForeignKeyProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT fk.ForeignKeyIdentifier,fk.DataProviderIdentifier,dp.Name AS DataProviderName,dp.HandlesPayments,dp.DataSourceStatementIdentifier,fk.ForeignKeyForIdentifier,fk.ForeignKeyForTypes,fk.ForeignKeyValue FROM ForeignKeys AS fk INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=fk.DataProviderIdentifier WHERE fk.ForeignKeyIdentifier=@foreignKeyIdentifier")
                .AddCharDataParameter("@foreignKeyIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this foreign key.
        /// </summary>
        [Test]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert()
        {
            Guid identifier = Guid.NewGuid();
            Guid dataProviderIdentifier = Guid.NewGuid();
            IDataProviderProxy dataProviderProxy = BuildDataProviderProxy(dataProviderIdentifier);
            Guid foreignKeyForIdentifier = Guid.NewGuid();
            Type foreignKeyForType = typeof(DataProvider);
            string foreignKeyValue = _fixture.Create<string>();

            IForeignKeyProxy sut = CreateSut(identifier, dataProviderProxy, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO ForeignKeys (ForeignKeyIdentifier,DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes,ForeignKeyValue) VALUES(@foreignKeyIdentifier,@dataProviderIdentifier,@foreignKeyForIdentifier,@foreignKeyForTypes,@foreignKeyValue)")
                .AddCharDataParameter("@foreignKeyIdentifier", identifier)
                .AddCharDataParameter("@dataProviderIdentifier", dataProviderIdentifier)
                .AddCharDataParameter("@foreignKeyForIdentifier", foreignKeyForIdentifier)
                .AddVarCharDataParameter("@foreignKeyForTypes", string.Join(";", foreignKeyForType.GetInterfaces().Select(m => m.Name)), 128)
                .AddVarCharDataParameter("@foreignKeyValue", foreignKeyValue, 128)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this foreign key.
        /// </summary>
        [Test]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate()
        {
            Guid identifier = Guid.NewGuid();
            Guid dataProviderIdentifier = Guid.NewGuid();
            IDataProviderProxy dataProviderProxy = BuildDataProviderProxy(dataProviderIdentifier);
            Guid foreignKeyForIdentifier = Guid.NewGuid();
            Type foreignKeyForType = typeof(DataProvider);
            string foreignKeyValue = _fixture.Create<string>();

            IForeignKeyProxy sut = CreateSut(identifier, dataProviderProxy, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE ForeignKeys SET DataProviderIdentifier=@dataProviderIdentifier,ForeignKeyForIdentifier=@foreignKeyForIdentifier,ForeignKeyForTypes=@foreignKeyForTypes,ForeignKeyValue=@foreignKeyValue WHERE ForeignKeyIdentifier=@foreignKeyIdentifier")
                .AddCharDataParameter("@foreignKeyIdentifier", identifier)
                .AddCharDataParameter("@dataProviderIdentifier", dataProviderIdentifier)
                .AddCharDataParameter("@foreignKeyForIdentifier", foreignKeyForIdentifier)
                .AddVarCharDataParameter("@foreignKeyForTypes", string.Join(";", foreignKeyForType.GetInterfaces().Select(m => m.Name)), 128)
                .AddVarCharDataParameter("@foreignKeyValue", foreignKeyValue, 128)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this foreign key.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IForeignKeyProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM ForeignKeys WHERE ForeignKeyIdentifier=@foreignKeyIdentifier")
                .AddCharDataParameter("@foreignKeyIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy to a given foreign key to a domain object in the food waste domain.</returns>
        private IForeignKeyProxy CreateSut()
        {
            return new ForeignKeyProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy to a given foreign key to a domain object in the food waste domain.</returns>
        private IForeignKeyProxy CreateSut(Guid identifier)
        {
            return new ForeignKeyProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy to a given foreign key to a domain object in the food waste domain.</returns>
        private IForeignKeyProxy CreateSut(Guid identifier, IDataProviderProxy dataProviderProxy, Guid foreignKeyForIdentifier, Type foreignKeyForType, string foreignKeyValue)
        {
            ArgumentNullGuard.NotNull(dataProviderProxy, nameof(dataProviderProxy))
                .NotNull(foreignKeyForType, nameof(foreignKeyForType))
                .NotNullOrWhiteSpace(foreignKeyValue, nameof(foreignKeyValue));

            return new ForeignKeyProxy(dataProviderProxy, foreignKeyForIdentifier, foreignKeyForType, foreignKeyValue)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? foreignKeyIdentifier = null, Guid? foreignKeyForIdentifier = null, Type foreignKeyForType = null, string foreignKeyValue = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString("ForeignKeyIdentifier"))
                .Return(foreignKeyIdentifier.HasValue ? foreignKeyIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString("ForeignKeyForIdentifier"))
                .Return(foreignKeyForIdentifier.HasValue ? foreignKeyForIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString("ForeignKeyForTypes"))
                .Return(string.Join(";", (foreignKeyForType ?? typeof(DataProvider)).GetInterfaces().Select(m => m.Name)))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString("ForeignKeyValue"))
                .Return(string.IsNullOrWhiteSpace(foreignKeyValue) == false ? foreignKeyValue : _fixture.Create<string>())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IDataProviderProxy dataProviderProxy = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IDataProviderProxy>.Is.Anything, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(dataProviderProxy ?? BuildDataProviderProxy())
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Builds a mockup for a data proxy for a given data provider.
        /// </summary>
        /// <param name="dataProviderIdentifier">Identifier for the data provider.</param>
        /// <returns>Mockup for a data proxy for a given data provider.</returns>
        private IDataProviderProxy BuildDataProviderProxy(Guid? dataProviderIdentifier = null)
        {
            IDataProviderProxy dataProviderProxyMock= MockRepository.GenerateMock<IDataProviderProxy>();
            dataProviderProxyMock.Stub(m => m.Identifier)
                .Return(dataProviderIdentifier ?? Guid.NewGuid())
                .Repeat.Any();
            return dataProviderProxyMock;
        }
    }
}
