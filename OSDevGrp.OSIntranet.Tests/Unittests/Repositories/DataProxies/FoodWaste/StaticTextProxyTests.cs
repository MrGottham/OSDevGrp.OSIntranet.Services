using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given static text used by the food waste domain.
    /// </summary>
    [TestFixture]
    public class StaticTextProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given static text used by the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextProxy()
        {
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);
            Assert.That(staticTextProxy.Identifier, Is.Null);
            Assert.That(staticTextProxy.Identifier.HasValue, Is.False);
            Assert.That(staticTextProxy.Type, Is.EqualTo(default(StaticTextType)));
            Assert.That(staticTextProxy.Translation, Is.Null);
            Assert.That(staticTextProxy.Translations, Is.Not.Null);
            Assert.That(staticTextProxy.Translations, Is.Empty);
            Assert.That(staticTextProxy.SubjectTranslationIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Empty);
            Assert.That(staticTextProxy.BodyTranslationIdentifier, Is.Null);
            Assert.That(staticTextProxy.BodyTranslationIdentifier.HasValue, Is.False);
            Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
            Assert.That(staticTextProxy.SubjectTranslations, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the static text has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStaticTextProxyHasNoIdentifier()
        {
            var staticTextProxy = new StaticTextProxy
            {
                Identifier = null
            };

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var uniqueId = staticTextProxy.UniqueId; });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, staticTextProxy.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the static text.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStaticTextProxy()
        {
            var staticTextProxy = new StaticTextProxy
            {
                Identifier = Guid.NewGuid()
            };

            var uniqueId = staticTextProxy.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(uniqueId, Is.EqualTo(staticTextProxy.Identifier.Value.ToString("D").ToUpper()));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given static text is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenStaticTextIsNull()
        {
            var staticTextProxy = new StaticTextProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<ArgumentNullException>(() => { var sqlQueryForId = staticTextProxy.GetSqlQueryForId(null); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("staticText"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given static text has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnStaticTextHasNoValue()
        {
            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Expect(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            var staticTextProxy = new StaticTextProxy();

            // ReSharper disable UnusedVariable
            var exception = Assert.Throws<IntranetRepositoryException>(() => { var sqlQueryForId = staticTextProxy.GetSqlQueryForId(staticTextMock); });
            // ReSharper restore UnusedVariable
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, staticTextMock.Identifier, "Identifier")));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given static text.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();

            var staticTextProxy = new StaticTextProxy();

            var sqlQueryForId = staticTextProxy.GetSqlQueryForId(staticTextMock);
            Assert.That(sqlQueryForId, Is.Not.Null);
            Assert.That(sqlQueryForId, Is.Not.Empty);
            // ReSharper disable PossibleInvalidOperationException
            Assert.That(sqlQueryForId, Is.EqualTo(string.Format("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextIdentifier='{0}'", staticTextMock.Identifier.Value.ToString("D").ToUpper())));
            // ReSharper restore PossibleInvalidOperationException
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a given static text.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(bool withBodyTranslation)
        {
            foreach (var staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                var staticTextProxy = new StaticTextProxy(staticTextTypeToTest, Guid.NewGuid(), withBodyTranslation ? (Guid?) Guid.NewGuid() : null)
                {
                    Identifier = Guid.NewGuid()
                };

                var subjectTranslationIdentifierSqlValue = staticTextProxy.SubjectTranslationIdentifier.ToString("D").ToUpper();
                var bodyTranslationIdentifierSqlValue = staticTextProxy.BodyTranslationIdentifier.HasValue ? string.Format("'{0}'", staticTextProxy.BodyTranslationIdentifier.Value.ToString("D").ToUpper()) : "NULL";
                var sqlCommand = staticTextProxy.GetSqlCommandForInsert();
                Assert.That(sqlCommand, Is.Not.Null);
                Assert.That(sqlCommand, Is.Not.Empty);
                Assert.That(sqlCommand, Is.EqualTo(string.Format("INSERT INTO StaticTexts (StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier) VALUES('{0}',{1},'{2}',{3})", staticTextProxy.UniqueId, (int) staticTextTypeToTest, subjectTranslationIdentifierSqlValue, bodyTranslationIdentifierSqlValue)));
            }
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a given static text.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(bool withBodyTranslation)
        {
            foreach (var staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                var staticTextProxy = new StaticTextProxy(staticTextTypeToTest, Guid.NewGuid(), withBodyTranslation ? (Guid?) Guid.NewGuid() : null)
                {
                    Identifier = Guid.NewGuid()
                };

                var subjectTranslationIdentifierSqlValue = staticTextProxy.SubjectTranslationIdentifier.ToString("D").ToUpper();
                var bodyTranslationIdentifierSqlValue = staticTextProxy.BodyTranslationIdentifier.HasValue ? string.Format("'{0}'", staticTextProxy.BodyTranslationIdentifier.Value.ToString("D").ToUpper()) : "NULL";
                var sqlCommand = staticTextProxy.GetSqlCommandForUpdate();
                Assert.That(sqlCommand, Is.Not.Null);
                Assert.That(sqlCommand, Is.Not.Empty);
                Assert.That(sqlCommand, Is.EqualTo(string.Format("UPDATE StaticTexts SET StaticTextType={1},SubjectTranslationIdentifier='{2}',BodyTranslationIdentifier={3} WHERE StaticTextIdentifier='{0}'", staticTextProxy.UniqueId, (int) staticTextTypeToTest, subjectTranslationIdentifierSqlValue, bodyTranslationIdentifierSqlValue)));
            }
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete a given static text.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            var fixture = new Fixture();

            var staticTextProxy = new StaticTextProxy(fixture.Create<StaticTextType>(), Guid.NewGuid())
            {
                Identifier = Guid.NewGuid()
            };

            var sqlCommand = staticTextProxy.GetSqlCommandForDelete();
            Assert.That(sqlCommand, Is.Not.Null);
            Assert.That(sqlCommand, Is.Not.Empty);
            Assert.That(sqlCommand, Is.EqualTo(string.Format("DELETE FROM StaticTexts WHERE StaticTextIdentifier='{0}'", staticTextProxy.UniqueId)));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextProxy.MapData(null, MockRepository.GenerateMock<IMySqlDataProvider>()));
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
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextProxy.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that MapData and MapRelations maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataAndMapRelationsMapsDataIntoProxy(bool withBodyTranslation)
        {
            var fixture = new Fixture();
            foreach (var staticTextTypeToTest in Enum.GetValues(typeof (StaticTextType)).Cast<StaticTextType>())
            {
                var subjectTranslation1Proxy = new TranslationProxy(new Guid("2F5E2130-2176-483C-80EF-DFC871186A6D"), MockRepository.GenerateMock<ITranslationInfo>(), fixture.Create<string>());
                var subjectTranslation2Proxy = new TranslationProxy(new Guid("2F5E2130-2176-483C-80EF-DFC871186A6D"), MockRepository.GenerateMock<ITranslationInfo>(), fixture.Create<string>());
                var bodyTranslation1Proxy = new TranslationProxy(new Guid("DB8E4201-5E79-4075-9020-5C72E989F399"), MockRepository.GenerateMock<ITranslationInfo>(), fixture.Create<string>());
                var bodyTranslation2Proxy = new TranslationProxy(new Guid("DB8E4201-5E79-4075-9020-5C72E989F399"), MockRepository.GenerateMock<ITranslationInfo>(), fixture.Create<string>());

                var dataProviderBaseMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
                dataProviderBaseMock.Stub(m => m.Clone())
                    .Return(dataProviderBaseMock)
                    .Repeat.Any();
                dataProviderBaseMock.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull))
                    .WhenCalled(e =>
                    {
                        var sqlQuery = ((MySqlCommand) e.Arguments.ElementAt(0)).CommandText;
                        Assert.That(sqlQuery, Is.Not.Null);
                        Assert.That(sqlQuery, Is.Not.Empty);
                        if (sqlQuery.Contains("2F5E2130-2176-483C-80EF-DFC871186A6D"))
                        {
                            e.ReturnValue = new List<TranslationProxy>
                            {
                                subjectTranslation1Proxy,
                                subjectTranslation2Proxy
                            };
                            return;
                        }
                        if (sqlQuery.Contains("DB8E4201-5E79-4075-9020-5C72E989F399"))
                        {
                            e.ReturnValue = new List<TranslationProxy>
                            {
                                bodyTranslation1Proxy,
                                bodyTranslation2Proxy
                            };
                            return;
                        }
                        e.ReturnValue = new List<TranslationProxy>(0);
                    })
                    .Return(null)
                    .Repeat.Any();

                var dataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
                dataReaderStub.Stub(m => m.GetString("StaticTextIdentifier"))
                    .Return("96F878E8-09BA-4836-A22C-5A3B8B1A6AB5")
                    .Repeat.Any();
                dataReaderStub.Stub(m => m.GetInt16("StaticTextType"))
                    .Return((short) staticTextTypeToTest)
                    .Repeat.Any();
                dataReaderStub.Stub(m => m.GetString("SubjectTranslationIdentifier"))
                    .Return("2F5E2130-2176-483C-80EF-DFC871186A6D")
                    .Repeat.Any();
                dataReaderStub.Stub(m => m.GetOrdinal("BodyTranslationIdentifier"))
                    .Return(3)
                    .Repeat.Any();
                dataReaderStub.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(3)))
                    .Return(!withBodyTranslation)
                    .Repeat.Any();
                dataReaderStub.Stub(m => m.GetString(Arg<int>.Is.Equal(3)))
                    .Return("DB8E4201-5E79-4075-9020-5C72E989F399")
                    .Repeat.Any();

                var staticTextProxy = new StaticTextProxy();
                Assert.That(staticTextProxy, Is.Not.Null);
                Assert.That(staticTextProxy.Identifier, Is.Null);
                Assert.That(staticTextProxy.Identifier.HasValue, Is.False);
                Assert.That(staticTextProxy.Type, Is.EqualTo(default(StaticTextType)));
                Assert.That(staticTextProxy.Translation, Is.Null);
                Assert.That(staticTextProxy.Translations, Is.Not.Null);
                Assert.That(staticTextProxy.Translations, Is.Empty);
                Assert.That(staticTextProxy.SubjectTranslationIdentifier, Is.EqualTo(default(Guid)));
                Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
                Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
                Assert.That(staticTextProxy.SubjectTranslations, Is.Empty);
                Assert.That(staticTextProxy.BodyTranslationIdentifier, Is.Null);
                Assert.That(staticTextProxy.BodyTranslationIdentifier.HasValue, Is.False);
                Assert.That(staticTextProxy.BodyTranslation, Is.Null);
                Assert.That(staticTextProxy.BodyTranslations, Is.Not.Null);
                Assert.That(staticTextProxy.BodyTranslations, Is.Empty);

                staticTextProxy.MapData(dataReaderStub, dataProviderBaseMock);
                staticTextProxy.MapRelations(dataProviderBaseMock);
                Assert.That(staticTextProxy.Identifier, Is.Not.Null);
                Assert.That(staticTextProxy.Identifier.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(staticTextProxy.Identifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReaderStub.GetString("StaticTextIdentifier")));
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(staticTextProxy.Type, Is.EqualTo(staticTextTypeToTest));
                Assert.That(staticTextProxy.Translation, Is.Null);
                Assert.That(staticTextProxy.Translations, Is.Not.Null);
                Assert.That(staticTextProxy.Translations, Is.Not.Empty);
                Assert.That(staticTextProxy.Translations.Contains(subjectTranslation1Proxy), Is.True);
                Assert.That(staticTextProxy.Translations.Contains(subjectTranslation2Proxy), Is.True);
                Assert.That(staticTextProxy.Translations.Contains(bodyTranslation1Proxy), Is.EqualTo(withBodyTranslation));
                Assert.That(staticTextProxy.Translations.Contains(bodyTranslation2Proxy), Is.EqualTo(withBodyTranslation));
                Assert.That(staticTextProxy.SubjectTranslationIdentifier.ToString("D").ToUpper(), Is.EqualTo(dataReaderStub.GetString("SubjectTranslationIdentifier")));
                Assert.That(staticTextProxy.SubjectTranslation, Is.Null);
                Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Null);
                Assert.That(staticTextProxy.SubjectTranslations, Is.Not.Empty);
                Assert.That(staticTextProxy.SubjectTranslations.Contains(subjectTranslation1Proxy), Is.True);
                Assert.That(staticTextProxy.SubjectTranslations.Contains(subjectTranslation2Proxy), Is.True);
                Assert.That(staticTextProxy.SubjectTranslations.Contains(bodyTranslation1Proxy), Is.False);
                Assert.That(staticTextProxy.SubjectTranslations.Contains(bodyTranslation2Proxy), Is.False);
                if (withBodyTranslation)
                {
                    Assert.That(staticTextProxy.BodyTranslationIdentifier, Is.Not.Null);
                    Assert.That(staticTextProxy.BodyTranslationIdentifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(staticTextProxy.BodyTranslationIdentifier.Value.ToString("D").ToUpper(), Is.EqualTo(dataReaderStub.GetString(3)));
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(staticTextProxy.BodyTranslation, Is.Null);
                    Assert.That(staticTextProxy.BodyTranslations, Is.Not.Null);
                    Assert.That(staticTextProxy.BodyTranslations, Is.Not.Empty);
                    Assert.That(staticTextProxy.BodyTranslations.Contains(subjectTranslation1Proxy), Is.False);
                    Assert.That(staticTextProxy.BodyTranslations.Contains(subjectTranslation2Proxy), Is.False);
                    Assert.That(staticTextProxy.BodyTranslations.Contains(bodyTranslation1Proxy), Is.True);
                    Assert.That(staticTextProxy.BodyTranslations.Contains(bodyTranslation2Proxy), Is.True);
                }
                else
                {
                    Assert.That(staticTextProxy.BodyTranslationIdentifier, Is.Null);
                    Assert.That(staticTextProxy.BodyTranslationIdentifier.HasValue, Is.False);
                    Assert.That(staticTextProxy.BodyTranslation, Is.Null);
                    Assert.That(staticTextProxy.BodyTranslations, Is.Not.Null);
                    Assert.That(staticTextProxy.BodyTranslations, Is.Empty);
                }


                dataProviderBaseMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1 + Convert.ToInt32(withBodyTranslation)));
                dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", dataReaderStub.GetString("SubjectTranslationIdentifier")))));
                if (withBodyTranslation)
                {
                    dataProviderBaseMock.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", dataReaderStub.GetString(3)))));
                }
                else
                {
                    dataProviderBaseMock.AssertWasNotCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == string.Format("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t, TranslationInfos AS ti WHERE t.OfIdentifier='{0}' AND ti.TranslationInfoIdentifier=t.InfoIdentifier ORDER BY CultureName", dataReaderStub.GetString(3)))));
                }
            }
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => staticTextProxy.MapRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("dataProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            var fixture = new Fixture();

            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<NotSupportedException>(() => staticTextProxy.SaveRelations(null, fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            var fixture = new Fixture();

            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<NotSupportedException>(() => staticTextProxy.SaveRelations(MockRepository.GenerateMock<IMySqlDataProvider>(), fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that DeleteRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<NotSupportedException>(() => staticTextProxy.DeleteRelations(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is not null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsNotSupportedExceptionWhenDataProviderIsNotNull()
        {
            var staticTextProxy = new StaticTextProxy();
            Assert.That(staticTextProxy, Is.Not.Null);

            var exception = Assert.Throws<NotSupportedException>(() => staticTextProxy.DeleteRelations(MockRepository.GenerateMock<IMySqlDataProvider>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
