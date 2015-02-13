using System;
using System.Data;
using System.Globalization;
using System.Threading;
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
    /// Tests the data proxy for translation information which are used for translation.
    /// </summary>
    [TestFixture]
    public class TranslationInfoProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy for translation information which are used for translation.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeTranslationInfoProxy()
        {
            var translationInfoProxy = new TranslationInfoProxy();
            Assert.That(translationInfoProxy, Is.Not.Null);
            Assert.That(translationInfoProxy.Identifier, Is.Null);
            Assert.That(translationInfoProxy.Identifier.HasValue, Is.False);
            Assert.That(translationInfoProxy.CultureName, Is.Null);
            Assert.That(translationInfoProxy.CultureInfo, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the translation information has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenTranslationInformationHasNoIdentifier()
        {
            var translationInfoProxy = new TranslationInfoProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = translationInfoProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationInfoProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the translation information.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForTranslationInfoProxy()
        {
            var translationInfoProxy = new TranslationInfoProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = translationInfoProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(translationInfoProxy.Identifier.ToString().ToUpper()));
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given translation information is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenTranslationInformationIsNull()
        {
            var translationInfoProxy = new TranslationInfoProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = translationInfoProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("translationInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given translation information has no value..
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnTranslationInfoHasNoValue()
        {
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var translationInfoProxy = new TranslationInfoProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = translationInfoProxy.GetSqlQueryForId(translationInfoMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, translationInfoMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given translation information.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var translationInfoProxy = new TranslationInfoProxy();

            var sqlQueryForId = translationInfoProxy.GetSqlQueryForId(translationInfoMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT TranslationInfoIdentifier,CultureName FROM TranslationInfos WHERE TranslationInfoIdentifier='{0}'", translationInfoMock.Identifier.ToString().ToUpper())));
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert this translation information.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert()
        {
            var translationInfoProxy = new TranslationInfoProxy(Thread.CurrentThread.CurrentUICulture.Name)
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = translationInfoProxy.GetSqlCommandForInsert();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO TranslationInfos (TranslationInfoIdentifier,CultureName) VALUES('{0}','{1}')", translationInfoProxy.UniqueId, translationInfoProxy.CultureName)));
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to update this translation information..
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate()
        {
            var translationInfoProxy = new TranslationInfoProxy(Thread.CurrentThread.CurrentUICulture.Name)
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = translationInfoProxy.GetSqlCommandForUpdate();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE TranslationInfos SET CultureName='{1}' WHERE TranslationInfoIdentifier='{0}'", translationInfoProxy.UniqueId, translationInfoProxy.CultureName)));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete this translation information..
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var translationInfoProxy = new TranslationInfoProxy
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = translationInfoProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM TranslationInfos WHERE TranslationInfoIdentifier='{0}'", translationInfoProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var fixture = new Fixture();
            fixture.Customize<IDataProviderBase>(e => e.FromFactory(() => MockRepository.GenerateMock<IDataProviderBase>()));

            var translationInfoProxy = new TranslationInfoProxy();
            Assert.That(translationInfoProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationInfoProxy.MapData(null, fixture.Create<IDataProviderBase>()));
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

            var translationInfoProxy = new TranslationInfoProxy();
            Assert.That(translationInfoProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => translationInfoProxy.MapData(fixture.Create<MySqlDataReader>(), null));
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

            var translationInfoProxy = new TranslationInfoProxy();
            Assert.That(translationInfoProxy, Is.Not.Null);

            var exception = Assert.Throws<IntranetRepositoryException>(() => translationInfoProxy.MapData(dataReader, fixture.Create<IDataProviderBase>()));
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
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("TranslationInfoIdentifier")))
                .Return("1D120312-3694-4ADA-9272-A44E791460E6")
                .Repeat.Any();
            dataReader.Stub(m => m.GetString(Arg<string>.Is.Equal("CultureName")))
                .Return(CultureInfo.CurrentCulture.Name)
                .Repeat.Any();

            var translationInfoProxy = new TranslationInfoProxy();
            Assert.That(translationInfoProxy, Is.Not.Null);
            Assert.That(translationInfoProxy.Identifier, Is.Null);
            Assert.That(translationInfoProxy.Identifier.HasValue, Is.False);
            Assert.That(translationInfoProxy.CultureName, Is.Null);
            Assert.That(translationInfoProxy.CultureInfo, Is.Null);

            translationInfoProxy.MapData(dataReader, fixture.Create<IDataProviderBase>());
            Assert.That(translationInfoProxy.Identifier, Is.Not.Null);
            Assert.That(translationInfoProxy.Identifier.HasValue, Is.True);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(translationInfoProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReader.GetString("TranslationInfoIdentifier")));
            // ReSharper restore PossibleInvalidOperationException
            Assert.That(translationInfoProxy.CultureName, Is.Not.Null);
            Assert.That(translationInfoProxy.CultureName, Is.Not.Empty);
            Assert.That(translationInfoProxy.CultureName, Is.EqualTo(dataReader.GetString("CultureName")));
            Assert.That(translationInfoProxy.CultureInfo, Is.Not.Null);
            Assert.That(translationInfoProxy.CultureInfo.Name, Is.Not.Null);
            Assert.That(translationInfoProxy.CultureInfo.Name, Is.Not.Empty);
            Assert.That(translationInfoProxy.CultureInfo.Name, Is.EqualTo(dataReader.GetString("CultureName")));
        }
    }
}
