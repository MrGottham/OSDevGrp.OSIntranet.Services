using System;
using System.Data;
using System.Globalization;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given translation for a domain object.
    /// </summary>
    [TestFixture]
    public class TranslationProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given translation for a domain object.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslation()
        {
            var translationProxy = new TranslationProxy();
            Assert.That(translationProxy, Is.Not.Null);
            Assert.That(translationProxy.Identifier, Is.Null);
            Assert.That(translationProxy.Identifier.HasValue, Is.False);
            Assert.That(translationProxy.TranslationOfIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(translationProxy.TranslationInfo, Is.Null);
            Assert.That(translationProxy.Value, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the translation has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenTranslationHasNoIdentifier()
        {
            var translationProxy = new TranslationProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = translationProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the translation.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForTranslationProxy()
        {
            var translationProxy = new TranslationProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = translationProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(translationProxy.Identifier.ToString().ToUpper()));
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given translation is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenTranslationIsNull()
        {
            var translationProxy = new TranslationProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = translationProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translation"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given translation has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnTranslationHasNoValue()
        {
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var translationProxy = new TranslationProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = translationProxy.GetSqlQueryForId(translationMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given translation.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var translationProxy = new TranslationProxy();

            var sqlQueryForId = translationProxy.GetSqlQueryForId(translationMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.TranslationIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier", translationMock.Identifier.ToString().ToUpper())));
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this translation.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            var fixture = new Fixture();

            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var translationProxy = new TranslationProxy(Guid.NewGuid(), translationInfoMock, fixture.Create<string>())
            {
                Identifier = Guid.NewGuid(),
            };

            var sqlCommand = translationProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('{0}','{1}','{2}','{3}')", translationProxy.UniqueId, translationProxy.TranslationOfIdentifier.ToString().ToUpper(), translationInfoMock.Identifier.Value.ToString().ToUpper(), translationProxy.Value)));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update this translation.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            var fixture = new Fixture();

            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var translationProxy = new TranslationProxy(Guid.NewGuid(), translationInfoMock, fixture.Create<string>())
            {
                Identifier = Guid.NewGuid(),
            };

            var sqlCommand = translationProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE Translations SET OfIdentifier='{1}',InfoIdentifier='{2}',Value='{3}' WHERE TranslationIdentifier='{0}'", translationProxy.UniqueId, translationProxy.TranslationOfIdentifier.ToString().ToUpper(), translationInfoMock.Identifier.Value.ToString().ToUpper(), translationProxy.Value)));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this translation.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var fixture = new Fixture();

            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();

            var translationProxy = new TranslationProxy(Guid.NewGuid(), translationInfoMock, fixture.Create<string>())
            {
                Identifier = Guid.NewGuid(),
            };

            var sqlCommand = translationProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM Translations WHERE TranslationIdentifier='{0}'", translationProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var translationProxy = new TranslationProxy();
            Assert.That(translationProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationProxy.MapData(null, fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataReader"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<MySqlDataReader>(e => e.FromFactory(() => MockRepository.GenerateStub<MySqlDataReader>()));

            var translationProxy = new TranslationProxy();
            Assert.That(translationProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationProxy.MapData(fixture.Create<MySqlDataReader>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData throws an IntranetRepositoryException if the data reader is not type of MySqlDataReader.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsIntranetRepositoryExceptionIfDataReaderIsNotTypeOfMySqlDataReader()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var dataReader = MockRepository.GenerateMock<IDataReader>();

            var translationProxy = new TranslationProxy();
            Assert.That(translationProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => translationProxy.MapData(dataReader, fixture.Create<IDataProviderBase>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name)));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapDataMapsDataIntoProxy()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var dataReader = MockRepository.GenerateStub<MySqlDataReader>();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("TranslationIdentifier")))
                .Return("65FDC52C-78CD-497A-B8B6-EBC124077060")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("OfIdentifier")))
                .Return("DA4182EB-FE50-4700-93ED-4527B46EBA85")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("InfoIdentifier")))
                .Return("56465CCB-3C7E-48CC-B8F8-E80046395D0C")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("CultureName")))
                .Return(CultureInfo.CurrentCulture.Name)
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("Value")))
                .Return(fixture.Create<string>())
                .Repeat.Any();

            var translationProxy = new TranslationProxy();
            Assert.That(translationProxy, Is.Not.Null);
            Assert.That(translationProxy.Identifier, Is.Null);
            Assert.That(translationProxy.Identifier.HasValue, Is.False);
            Assert.That(translationProxy.TranslationOfIdentifier, Is.EqualTo(Guid.Empty));
            Assert.That(translationProxy.TranslationInfo, Is.Null);
            Assert.That(translationProxy.Value, Is.Null);

            translationProxy.MapData(dataReader, fixture.Create<IDataProviderBase>());
            Assert.That(translationProxy.Identifier, Is.Not.Null);
            Assert.That(translationProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(translationProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("TranslationIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translationProxy.TranslationOfIdentifier.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("OfIdentifier")));
            Assert.That(translationProxy.TranslationInfo, Is.Not.Null);
            Assert.That(translationProxy.TranslationInfo.Identifier, Is.Not.Null);
            Assert.That(translationProxy.TranslationInfo.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(translationProxy.TranslationInfo.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("InfoIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translationProxy.TranslationInfo.CultureName, Is.Not.Null);
            Assert.That(translationProxy.TranslationInfo.CultureName, Is.Not.Empty);
            Assert.That(translationProxy.TranslationInfo.CultureName, Is.EqualTo(dataReader.GetString("CultureName")));
            Assert.That(translationProxy.TranslationInfo.CultureInfo, Is.Not.Null);
            Assert.That(translationProxy.Value, Is.Not.Null);
            Assert.That(translationProxy.Value, Is.Not.Empty);
            Assert.That(translationProxy.Value, Is.EqualTo(dataReader.GetString("Value")));
        }
    }
}
