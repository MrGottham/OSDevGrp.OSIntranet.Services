using System;
using System.Globalization;
using System.Threading;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy for translation information which are used for translation.
    /// </summary>
    [TestFixture]
    public class TranslationInfoProxyTests
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
        /// Tests that the constructor initialize a data proxy for translation information which are used for translation.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslationInfoProxy()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.CultureName, Is.Null);
            Assert.That(sut.CultureInfo, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the translation information has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenTranslationInfoHasNoIdentifier()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.Identifier = null;

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the translation information.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForTranslationInfoProxy()
        {
            Guid identifier = Guid.NewGuid();

            ITranslationInfoProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
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
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData and MapRelations maps data into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapDataMapsDataIntoProxy()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid translationInfoIdentifier = Guid.NewGuid();
            string cultureName = CultureInfo.CurrentCulture.Name;
            MySqlDataReader dataReader = CreateMySqlDataReader(translationInfoIdentifier, cultureName);

            sut.MapData(dataReader, CreateFoodWasteDataProvider());

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(translationInfoIdentifier));
            Assert.That(sut.CultureName, Is.Not.Null);
            Assert.That(sut.CultureName, Is.Not.Empty);
            Assert.That(sut.CultureName, Is.EqualTo(cultureName));
            Assert.That(sut.CultureInfo, Is.Not.Null);
            Assert.That(sut.CultureInfo.Name, Is.Not.Null);
            Assert.That(sut.CultureInfo.Name, Is.Not.Empty);
            Assert.That(sut.CultureInfo.Name, Is.EqualTo(cultureName));

            dataReader.AssertWasCalled(m => m.GetString("TranslationInfoIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString("CultureName"), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(null));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given translation information.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            ITranslationInfoProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos WHERE TranslationInfoIdentifier=@translationInfoIdentifier")
                .AddCharDataParameter("@translationInfoIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this translation information.
        /// </summary>
        [Test]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert()
        {
            Guid identifier = Guid.NewGuid();
            string cultureName = Thread.CurrentThread.CurrentUICulture.Name;

            ITranslationInfoProxy sut = CreateSut(identifier, cultureName);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO TranslationInfos (TranslationInfoIdentifier,CultureName) VALUES(@translationInfoIdentifier,@cultureName)")
                .AddCharDataParameter("@translationInfoIdentifier", identifier)
                .AddCharDataParameter("@cultureName", cultureName, 5)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this translation information.
        /// </summary>
        [Test]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate()
        {
            Guid identifier = Guid.NewGuid();
            string cultureName = Thread.CurrentThread.CurrentUICulture.Name;

            ITranslationInfoProxy sut = CreateSut(identifier, cultureName);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE TranslationInfos SET CultureName=@cultureName WHERE TranslationInfoIdentifier=@translationInfoIdentifier")
                .AddCharDataParameter("@translationInfoIdentifier", identifier)
                .AddCharDataParameter("@cultureName", cultureName, 5)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this translation information.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            ITranslationInfoProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM TranslationInfos WHERE TranslationInfoIdentifier=@translationInfoIdentifier")
                .AddCharDataParameter("@translationInfoIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateFoodWasteDataProvider(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), null, _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the column name collection is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfColumnNameCollectionIsNull()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Creates creates a new translation information data proxy from the data reader.
        /// </summary>
        [Test]
        public void TestThatCreateCreatesTranslationInfoProxy()
        {
            ITranslationInfoProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid translationInfoIdentifier = Guid.NewGuid();
            string cultureName = CultureInfo.CurrentCulture.Name;
            MySqlDataReader dataReader = CreateMySqlDataReader(translationInfoIdentifier, cultureName);

            ITranslationInfoProxy result = sut.Create(dataReader, CreateFoodWasteDataProvider(), "TranslationInfoIdentifier", "CultureName");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(translationInfoIdentifier));
            Assert.That(result.CultureName, Is.Not.Null);
            Assert.That(result.CultureName, Is.Not.Empty);
            Assert.That(result.CultureName, Is.EqualTo(cultureName));
            Assert.That(result.CultureInfo, Is.Not.Null);
            Assert.That(result.CultureInfo.Name, Is.Not.Null);
            Assert.That(result.CultureInfo.Name, Is.Not.Empty);
            Assert.That(result.CultureInfo.Name, Is.EqualTo(cultureName));

            dataReader.AssertWasCalled(m => m.GetString("TranslationInfoIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString("CultureName"), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Creates an instance of the data proxy to translation information which are used for translation.
        /// </summary>
        /// <returns>Instance of the data proxy to translation information which are used for translation.</returns>
        private ITranslationInfoProxy CreateSut()
        {
            return new TranslationInfoProxy();
        }

        /// <summary>
        /// Creates an instance of the data proxy to translation information which are used for translation.
        /// </summary>
        /// <returns>Instance of the data proxy to translation information which are used for translation.</returns>
        private ITranslationInfoProxy CreateSut(Guid identifier)
        {
            return new TranslationInfoProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to translation information which are used for translation.
        /// </summary>
        /// <returns>Instance of the data proxy to translation information which are used for translation.</returns>
        private ITranslationInfoProxy CreateSut(Guid identifier, string cultureName)
        {
            return new TranslationInfoProxy(cultureName)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? translationInfoIdentifier = null, string cultureName = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("TranslationInfoIdentifier")))
                .Return(translationInfoIdentifier.HasValue ? translationInfoIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("CultureName")))
                .Return(cultureName ?? CultureInfo.CurrentCulture.Name)
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider()
        {
            return MockRepository.GenerateMock<IFoodWasteDataProvider>();
        }
    }
}
