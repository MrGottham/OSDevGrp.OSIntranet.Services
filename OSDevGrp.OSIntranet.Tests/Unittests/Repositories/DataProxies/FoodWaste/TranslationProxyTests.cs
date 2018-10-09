using System;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
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
    /// Tests the data proxy to a given translation for a domain object.
    /// </summary>
    [TestFixture]
    public class TranslationProxyTests
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
        /// Tests that the constructor initialize a data proxy to a given translation for a domain object.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslation()
        {
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.TranslationOfIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(sut.TranslationInfo, Is.Null);
            Assert.That(sut.Value, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the translation has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenTranslationHasNoIdentifier()
        {
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.Identifier = null;

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the translation.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForTranslationProxy()
        {
            Guid identifier = Guid.NewGuid();

            ITranslationProxy sut = CreateSut(identifier);
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
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateMySqlDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            ITranslationProxy sut = CreateSut();
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
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid identifier = Guid.NewGuid();
            Guid translationOfIdentifier = Guid.NewGuid();
            string value = _fixture.Create<string>();
            MySqlDataReader dataReader = CreateMySqlDataReader(identifier, translationOfIdentifier, value);

            ITranslationInfoProxy translationInfoProxy = BuildTranslationInfoProxy(Guid.NewGuid());
            IMySqlDataProvider dataProvider = CreateMySqlDataProvider(translationInfoProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(identifier));
            Assert.That(sut.TranslationOfIdentifier, Is.EqualTo(translationOfIdentifier));
            Assert.That(sut.TranslationInfo, Is.Not.Null);
            Assert.That(sut.TranslationInfo, Is.EqualTo(translationInfoProxy));
            Assert.That(sut.Value, Is.Not.Null);
            Assert.That(sut.Value, Is.Not.Empty);
            Assert.That(sut.Value, Is.EqualTo(value));

            dataReader.AssertWasCalled(m => m.GetString("TranslationIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString("OfIdentifier"), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString("Value"), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<ITranslationInfoProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 2 &&
                                               e[0] == "InfoIdentifier" &&
                                               e[1] == "CultureName")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            ITranslationProxy sut = CreateSut();
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
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the translation is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateMySqlDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the translation is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            ITranslationProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateMySqlDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given translation.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            ITranslationProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.TranslationIdentifier=@translationIdentifier")
                .AddCharDataParameter("@translationIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this translation.
        /// </summary>
        [Test]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert()
        {
            Guid identifier = Guid.NewGuid();
            Guid translationOfIdentifier = Guid.NewGuid();
            Guid translationInfoIdentifier = Guid.NewGuid();
            ITranslationInfoProxy translationInfoProxy = BuildTranslationInfoProxy(translationInfoIdentifier);
            string value = _fixture.Create<string>();

            ITranslationProxy sut = CreateSut(identifier, translationOfIdentifier, translationInfoProxy, value);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES(@translationIdentifier,@ofIdentifier,@translationInfoIdentifier,@value)")
                .AddCharDataParameter("@translationIdentifier", identifier)
                .AddCharDataParameter("@ofIdentifier", translationOfIdentifier)
                .AddCharDataParameter("@translationInfoIdentifier", translationInfoIdentifier)
                .AddVarCharDataParameter("@value", value, 4096)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this translation.
        /// </summary>
        [Test]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate()
        {
            Guid identifier = Guid.NewGuid();
            Guid translationOfIdentifier = Guid.NewGuid();
            Guid translationInfoIdentifier = Guid.NewGuid();
            ITranslationInfoProxy translationInfoProxy = BuildTranslationInfoProxy(translationInfoIdentifier);
            string value = _fixture.Create<string>();

            ITranslationProxy sut = CreateSut(identifier, translationOfIdentifier, translationInfoProxy, value);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE Translations SET OfIdentifier=@ofIdentifier,InfoIdentifier=@translationInfoIdentifier,Value=@value WHERE TranslationIdentifier=@translationIdentifier")
                .AddCharDataParameter("@translationIdentifier", identifier)
                .AddCharDataParameter("@ofIdentifier", translationOfIdentifier)
                .AddCharDataParameter("@translationInfoIdentifier", translationInfoIdentifier)
                .AddVarCharDataParameter("@value", value, 4096)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this translation.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            ITranslationProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM Translations WHERE TranslationIdentifier=@translationIdentifier")
                .AddCharDataParameter("@translationIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given translation for a domain object.
        /// </summary>
        /// <returns>Instance of the data proxy to a given translation for a domain object.</returns>
        private ITranslationProxy CreateSut()
        {
            return new TranslationProxy();
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given translation for a domain object.
        /// </summary>
        /// <returns>Instance of the data proxy to a given translation for a domain object.</returns>
        private ITranslationProxy CreateSut(Guid identifier)
        {
            return new TranslationProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given translation for a domain object.
        /// </summary>
        /// <returns>Instance of the data proxy to a given translation for a domain object.</returns>
        private ITranslationProxy CreateSut(Guid identifier, Guid translationOfIdentifier, ITranslationInfo translationInfo, string value)
        {
            ArgumentNullGuard.NotNull(translationInfo, nameof(translationInfo))
                .NotNullOrWhiteSpace(value, nameof(value));

            return new TranslationProxy(translationOfIdentifier, translationInfo, value)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? translationIdentifier = null, Guid? translationOfIdentifier = null, string value = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("TranslationIdentifier")))
                .Return(translationIdentifier.HasValue ? translationIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("OfIdentifier")))
                .Return(translationOfIdentifier.HasValue ? translationOfIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("Value")))
                .Return(value ?? _fixture.Create<string>())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which uses MySQL.
        /// </summary>
        /// <returns>Mockup for the data provider which uses MySQL.</returns>
        private IMySqlDataProvider CreateMySqlDataProvider(ITranslationInfoProxy translationInfoProxy = null)
        {
            IMySqlDataProvider mySqlDataProviderMock = MockRepository.GenerateMock<IMySqlDataProvider>();
            mySqlDataProviderMock.Stub(m => m.Create(Arg<ITranslationInfoProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(translationInfoProxy ?? BuildTranslationInfoProxy(Guid.NewGuid()))
                .Repeat.Any();
            return mySqlDataProviderMock;
        }

        /// <summary>
        /// Builds a mockup for a translation information.
        /// </summary>
        /// <returns>Mockup for a translation information.</returns>
        private ITranslationInfoProxy BuildTranslationInfoProxy(Guid identifier)
        {
            ITranslationInfoProxy translationInfoMock = MockRepository.GenerateMock<ITranslationInfoProxy>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            return translationInfoMock;
        }
    }
}
