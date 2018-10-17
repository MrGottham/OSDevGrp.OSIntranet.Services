using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
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
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given static text used by the food waste domain.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStaticTextProxy()
        {
            IStaticTextProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Type, Is.EqualTo(default(StaticTextType)));
            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Empty);
            Assert.That(sut.SubjectTranslationIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(sut.SubjectTranslation, Is.Null);
            Assert.That(sut.SubjectTranslations, Is.Not.Null);
            Assert.That(sut.SubjectTranslations, Is.Empty);
            Assert.That(sut.BodyTranslationIdentifier, Is.Null);
            Assert.That(sut.BodyTranslationIdentifier.HasValue, Is.False);
            Assert.That(sut.SubjectTranslation, Is.Null);
            Assert.That(sut.SubjectTranslations, Is.Not.Null);
            Assert.That(sut.SubjectTranslations, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the static text has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenStaticTextProxyHasNoIdentifier()
        {
            IStaticTextProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            sut.Identifier = null;

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the static text.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForStaticTextProxy()
        {
            Guid identifier = Guid.NewGuid();

            IStaticTextProxy sut = CreateSut(identifier);
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
            IStaticTextProxy sut = CreateSut();
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
            IStaticTextProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataMapsDataIntoProxy(bool withBodyTranslation)
        {
            foreach (StaticTextType staticTextTypeToTest in Enum.GetValues(typeof(StaticTextType)).Cast<StaticTextType>())
            {
                IStaticTextProxy sut = CreateSut();
                Assert.That(sut, Is.Not.Null);

                Guid staticTextIdentifier = Guid.NewGuid();
                Guid subjectTranslationIdentifier = Guid.NewGuid();
                Guid? bodyTranslationIdentifier = withBodyTranslation ? Guid.NewGuid() : (Guid?) null;
                MySqlDataReader dataReader = CreateMySqlDataReader(staticTextIdentifier, staticTextTypeToTest, subjectTranslationIdentifier, bodyTranslationIdentifier);

                IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

                sut.MapData(dataReader, dataProvider);

                Assert.That(sut.Identifier, Is.Not.Null);
                Assert.That(sut.Identifier, Is.EqualTo(staticTextIdentifier));
                Assert.That(sut.Type, Is.EqualTo(staticTextTypeToTest));
                Assert.That(sut.SubjectTranslationIdentifier, Is.EqualTo(subjectTranslationIdentifier));
                if (withBodyTranslation)
                {
                    Assert.That(sut.BodyTranslationIdentifier, Is.Not.Null);
                    Assert.That(sut.BodyTranslationIdentifier, Is.EqualTo(bodyTranslationIdentifier));
                }
                else
                {
                    Assert.That(sut.BodyTranslationIdentifier, Is.Null);
                }

                dataReader.AssertWasCalled(m => m.GetString("StaticTextIdentifier"), opt => opt.Repeat.Once());
                dataReader.AssertWasCalled(m => m.GetInt16("StaticTextType"), opt => opt.Repeat.Once());
                dataReader.AssertWasCalled(m => m.GetString("SubjectTranslationIdentifier"), opt => opt.Repeat.Once());
                dataReader.AssertWasCalled(m => m.GetOrdinal("BodyTranslationIdentifier"), opt => opt.Repeat.Once());
                dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(3)), opt => opt.Repeat.Once());
                if (withBodyTranslation)
                {
                    dataReader.AssertWasCalled(m => m.GetString("BodyTranslationIdentifier"), opt => opt.Repeat.Once());
                }
                else
                {
                    dataReader.AssertWasNotCalled(m => m.GetString("BodyTranslationIdentifier"));
                }

                dataProvider.AssertWasNotCalled(m => m.Clone());
            }
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IStaticTextProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations maps translations of the subject to the static text into the proxy.
        /// </summary>
        [Test]
        public void TestThatMapRelationsMapsSubjectTranslationsIntoProxy()
        {
            Guid subjectTranslationIdentifier = Guid.NewGuid();
            Guid? bodyTranslationIdentifier = _random.Next(100) > 50 ? Guid.NewGuid() : (Guid?) null;

            IStaticTextProxy sut = CreateSut(Guid.NewGuid(), _fixture.Create<StaticTextType>(), subjectTranslationIdentifier, bodyTranslationIdentifier);
            Assert.That(sut, Is.Not.Null);

            List<TranslationProxy> translationProxyCollection = new List<TranslationProxy>(BuildTranslationProxyCollection(subjectTranslationIdentifier));
            if (bodyTranslationIdentifier.HasValue)
            {
                translationProxyCollection.AddRange(BuildTranslationProxyCollection(bodyTranslationIdentifier.Value));
            }
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));
            Assert.That(sut.SubjectTranslation, Is.Null);
            Assert.That(sut.SubjectTranslations, Is.Not.Null);
            Assert.That(sut.SubjectTranslations, Is.Not.Empty);
            foreach (TranslationProxy translationProxy in translationProxyCollection.Where(m => m.TranslationOfIdentifier == subjectTranslationIdentifier))
            {
                Assert.That(sut.SubjectTranslations.Contains(translationProxy), Is.True);
            }

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1 + Convert.ToInt32(bodyTranslationIdentifier.HasValue)));
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Times(1 + Convert.ToInt32(bodyTranslationIdentifier.HasValue)));

            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull))
                .First()
                .ElementAt(0);
            new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", subjectTranslationIdentifier)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that MapRelations maps translations of the body to the static text into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapRelationsMapsBodyTranslationsIntoProxy(bool withBodyTranslation)
        {
            Guid subjectTranslationIdentifier = Guid.NewGuid();
            Guid? bodyTranslationIdentifier = withBodyTranslation ? Guid.NewGuid() : (Guid?) null;

            IStaticTextProxy sut = CreateSut(Guid.NewGuid(), _fixture.Create<StaticTextType>(), subjectTranslationIdentifier, bodyTranslationIdentifier);
            Assert.That(sut, Is.Not.Null);

            List<TranslationProxy> translationProxyCollection = new List<TranslationProxy>(BuildTranslationProxyCollection(subjectTranslationIdentifier));
            if (bodyTranslationIdentifier.HasValue)
            {
                translationProxyCollection.AddRange(BuildTranslationProxyCollection(bodyTranslationIdentifier.Value));
            }
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(translationProxyCollection);

            sut.MapRelations(dataProvider);

            Assert.That(sut.Translation, Is.Null);
            Assert.That(sut.Translations, Is.Not.Null);
            Assert.That(sut.Translations, Is.Not.Empty);
            Assert.That(sut.Translations, Is.EqualTo(translationProxyCollection));
            Assert.That(sut.BodyTranslation, Is.Null);
            Assert.That(sut.BodyTranslations, Is.Not.Null);
            if (bodyTranslationIdentifier.HasValue)
            {
                Assert.That(sut.BodyTranslations, Is.Not.Empty);
                foreach (TranslationProxy translationProxy in translationProxyCollection.Where(m => m.TranslationOfIdentifier == bodyTranslationIdentifier.Value))
                {
                    Assert.That(sut.BodyTranslations.Contains(translationProxy), Is.True);
                }
            }
            else
            {
                Assert.That(sut.BodyTranslations, Is.Empty);
            }

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1 + Convert.ToInt32(bodyTranslationIdentifier.HasValue)));
            dataProvider.AssertWasCalled(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull), opt => opt.Repeat.Times(1 + Convert.ToInt32(bodyTranslationIdentifier.HasValue)));

            if (bodyTranslationIdentifier.HasValue == false)
            {
                return;
            }
            MySqlCommand cmd = (MySqlCommand) dataProvider.GetArgumentsForCallsMadeOn(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.NotNull))
                .Last()
                .ElementAt(0);
            new DbCommandTestBuilder("SELECT t.TranslationIdentifier AS TranslationIdentifier,t.OfIdentifier AS OfIdentifier,ti.TranslationInfoIdentifier AS InfoIdentifier,ti.CultureName AS CultureName,t.Value AS Value FROM Translations AS t INNER JOIN TranslationInfos AS ti ON ti.TranslationInfoIdentifier=t.InfoIdentifier WHERE t.OfIdentifier=@ofIdentifier ORDER BY ti.CultureName")
                .AddCharDataParameter("@ofIdentifier", bodyTranslationIdentifier.Value)
                .Build()
                .Run(cmd);
        }

        /// <summary>
        /// Tests that SaveRelations throws an NotSupportedException when the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsNotSupportedExceptionWhenDataProviderIsNull()
        {
            IStaticTextProxy sut = CreateSut();
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
            IStaticTextProxy sut = CreateSut();
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
            IStaticTextProxy sut = CreateSut();
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
            IStaticTextProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertNotSupportedExceptionIsValid(result);
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given static text.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IStaticTextProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier FROM StaticTexts WHERE StaticTextIdentifier=@staticTextIdentifier")
                .AddCharDataParameter("@staticTextIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert a given static text.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool withBodyTranslation)
        {
            foreach (StaticTextType staticTextTypeToTest in Enum.GetValues(typeof(StaticTextType)).Cast<StaticTextType>())
            {
                Guid identifier = Guid.NewGuid();
                Guid subjectTranslationIdentifier = Guid.NewGuid();
                Guid? bodyTranslationIdentifier = withBodyTranslation ? Guid.NewGuid() : (Guid?) null;
                IStaticTextProxy sut = CreateSut(identifier, staticTextTypeToTest, subjectTranslationIdentifier, bodyTranslationIdentifier);

                new DbCommandTestBuilder("INSERT INTO StaticTexts (StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier) VALUES(@staticTextIdentifier,@staticTextType,@subjectTranslationIdentifier,@bodyTranslationIdentifier)")
                    .AddCharDataParameter("@staticTextIdentifier", identifier)
                    .AddTinyIntDataParameter("@staticTextType", (int) staticTextTypeToTest, 4)
                    .AddCharDataParameter("@subjectTranslationIdentifier", subjectTranslationIdentifier)
                    .AddCharDataParameter("@bodyTranslationIdentifier", bodyTranslationIdentifier, true)
                    .Build()
                    .Run(sut.CreateInsertCommand());
            }
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update a given static text.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool withBodyTranslation)
        {
            foreach (StaticTextType staticTextTypeToTest in Enum.GetValues(typeof(StaticTextType)).Cast<StaticTextType>())
            {
                Guid identifier = Guid.NewGuid();
                Guid subjectTranslationIdentifier = Guid.NewGuid();
                Guid? bodyTranslationIdentifier = withBodyTranslation ? Guid.NewGuid() : (Guid?)null;
                IStaticTextProxy sut = CreateSut(identifier, staticTextTypeToTest, subjectTranslationIdentifier, bodyTranslationIdentifier);

                new DbCommandTestBuilder("UPDATE StaticTexts SET StaticTextType=@staticTextType,SubjectTranslationIdentifier=@subjectTranslationIdentifier,BodyTranslationIdentifier=@bodyTranslationIdentifier WHERE StaticTextIdentifier=@staticTextIdentifier")
                    .AddCharDataParameter("@staticTextIdentifier", identifier)
                    .AddTinyIntDataParameter("@staticTextType", (int)staticTextTypeToTest, 4)
                    .AddCharDataParameter("@subjectTranslationIdentifier", subjectTranslationIdentifier)
                    .AddCharDataParameter("@bodyTranslationIdentifier", bodyTranslationIdentifier, true)
                    .Build()
                    .Run(sut.CreateUpdateCommand());
            }
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete a given static text.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();
            IStaticTextProxy sut = CreateSut(identifier);

            new DbCommandTestBuilder("DELETE FROM StaticTexts WHERE StaticTextIdentifier=@staticTextIdentifier")
                .AddCharDataParameter("@staticTextIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given static text used by the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy to a given static text used by the food waste domain.</returns>
        private IStaticTextProxy CreateSut()
        {
            return new StaticTextProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given static text used by the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy to a given static text used by the food waste domain.</returns>
        private IStaticTextProxy CreateSut(Guid identifier)
        {
            return new StaticTextProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy to a given static text used by the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy to a given static text used by the food waste domain.</returns>
        private IStaticTextProxy CreateSut(Guid identifier, StaticTextType staticTextType, Guid subjectTranslationIdentifier, Guid? bodyTranslationIdentifier)
        {
            return new StaticTextProxy(staticTextType, subjectTranslationIdentifier, bodyTranslationIdentifier)
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? staticTextIdentifier = null, StaticTextType? staticTextType = null, Guid? subjectTranslationIdentifier = null, Guid? bodyTranslationIdentifier = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString("StaticTextIdentifier"))
                .Return(staticTextIdentifier.HasValue ? staticTextIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt16("StaticTextType"))
                .Return(staticTextType.HasValue ? (short) staticTextType : (short) _fixture.Create<StaticTextType>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString("SubjectTranslationIdentifier"))
                .Return(subjectTranslationIdentifier.HasValue ? subjectTranslationIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal("BodyTranslationIdentifier"))
                .Return(3)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(3)))
                .Return(bodyTranslationIdentifier.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("BodyTranslationIdentifier")))
                .Return(bodyTranslationIdentifier.HasValue ? bodyTranslationIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IEnumerable<TranslationProxy> translationProxyCollection = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Clone())
                .Return(foodWasteDataProvider)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<TranslationProxy>(Arg<MySqlCommand>.Is.Anything))
                .WhenCalled(e =>
                {
                    if (translationProxyCollection == null)
                    {
                        e.ReturnValue = BuildTranslationProxyCollection(Guid.NewGuid());
                        return;
                    }

                    Guid translationOfIdentifier = new Guid((string) ((MySqlCommand) e.Arguments.ElementAt(0)).Parameters["@ofIdentifier"].Value);
                    e.ReturnValue = translationProxyCollection .Where(m => m.TranslationOfIdentifier == translationOfIdentifier).ToList();
                })
                .Return(null)
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Build a collection of translation data proxies.
        /// </summary>
        /// <returns>Collection of translation data proxies.</returns>
        private IEnumerable<TranslationProxy> BuildTranslationProxyCollection(Guid translationOfIdentifier)
        {
            _fixture.Customize<TranslationProxy>(composerTransformation => composerTransformation.FromFactory(() => new TranslationProxy(translationOfIdentifier, _fixture.Create<TranslationInfoProxy>(), _fixture.Create<string>())));

            return _fixture.CreateMany<TranslationProxy>(_random.Next(5, 10)).ToList();
        }
    }
}
