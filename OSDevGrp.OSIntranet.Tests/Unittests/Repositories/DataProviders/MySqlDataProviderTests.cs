using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProviders
{
    /// <summary>
    /// Tester data provider, som benytter MySql.
    /// </summary>
    [TestFixture]
    public class MySqlDataProviderTests
    {
        #region Private constants

        private const string MySqlDataProviderConnectionStringSettingsName = "OSDevGrp.OSIntranet.Repositories.DataProviders.MySqlDataProvider";

        #endregion

        /// <summary>
        /// Egen data proxy til test af data provider, som benytter MySql.
        /// </summary>
        private sealed class MyDataProxy : IMySqlDataProxy
        {
            #region Private variables

            private readonly Fixture _fixture;

            #endregion

            #region Constructors

            /// <summary>
            /// Creates an instance of a data proxy which can be used for unit testing a MySql data provider.
            /// </summary>
            public MyDataProxy()
            {
                _fixture = new Fixture();
            }

            #endregion

            #region Properties

            /// <summary>
            /// Systemnummer.
            /// </summary>
            public int SystemNo
            {
                get;
                set;
            }

            /// <summary>
            /// Titel.
            /// </summary>
            public string Title
            {
                get;
                private set;
            }

            /// <summary>
            /// Angivelse af, at MapData er blevet kaldt.
            /// </summary>
            public bool MapDataIsCalled
            {
                get; 
                private set;
            }

            /// <summary>
            /// Angivelse af, at MapRelations er blevet kaldt.
            /// </summary>
            public bool MapRelationsIsCalled
            {
                get; 
                private set;
            }

            /// <summary>
            /// Angivelse af, at SaveRelations er blevet kaldt.
            /// </summary>
            public bool SaveRelationsIsCalled
            {
                get; 
                private set; 
            }

            /// <summary>
            /// Angivelse af den værdi for indsættelse eller opdatering, som SaveRelations er blevet kaldt med.
            /// </summary>
            public bool IsInserting
            {
                get; 
                private set;
            }

            /// <summary>
            /// Angivelse af, at DeleteRelations er blevet kaldt.
            /// </summary>
            public bool DeleteRelationsIsCalled
            {
                get; 
                private set; 
            }

            #endregion

            #region IDataProxyBase Members

            /// <summary>
            /// Mapper data fra en data reader.
            /// </summary>
            /// <param name="dataReader">Data reader for data provideren.</param>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
            {
                SystemNo = dataReader.GetInt32("SystemNo");
                Title = dataReader.GetString("Title");

                MapDataIsCalled = true;
            }

            /// <summary>
            /// Mapper releationer.
            /// </summary>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
            {
                Assert.That(dataProvider, Is.Not.Null);

                MapRelationsIsCalled = true;
            }

            /// <summary>
            /// Gemmer relationer.
            /// </summary>
            /// <param name="dataProvider">Dataprovider.</param>
            /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
            public void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
            {
                Assert.That(dataProvider, Is.Not.Null);

                SaveRelationsIsCalled = true;
                IsInserting = isInserting;
            }

            /// <summary>
            /// Sletter relationer.
            /// </summary>
            /// <param name="dataProvider">Dataprovider.</param>
            public void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
            {
                Assert.That(dataProvider, Is.Not.Null);

                DeleteRelationsIsCalled = true;
            }

            /// <summary>
            /// Creates the SQL statement for getting this data proxy.
            /// </summary>
            /// <returns>SQL statement for getting this data proxy.</returns>
            public MySqlCommand CreateGetCommand()
            {
                MySqlCommand command = new MySqlCommand("SELECT SystemNo,Title FROM Systems WHERE SystemNo=@systemNo")
                {
                    CommandType = CommandType.Text
                };

                MySqlParameter systemNoParameter = command.Parameters.AddWithValue("@systemNo", SystemNo);
                systemNoParameter.MySqlDbType = MySqlDbType.Int16;
                systemNoParameter.IsNullable = false;

                return command;
            }

            /// <summary>
            /// Creates the SQL statement for inserting this data proxy.
            /// </summary>
            /// <returns>SQL statement for inserting this data proxy.</returns>
            public MySqlCommand CreateInsertCommand()
            {
                DateTime now = DateTime.Now;
                MySqlCommand command = new MySqlCommand("INSERT INTO Calapps (SystemNo,CalId,Date,FromTime,ToTime,Subject) VALUES(1,77777,@date,@fromTime,@toTime,@subject)")
                {
                    CommandType = CommandType.Text
                };

                MySqlParameter dateParameter = command.Parameters.AddWithValue("@date", now.Date);
                dateParameter.MySqlDbType = MySqlDbType.Date;
                dateParameter.IsNullable = false;

                MySqlParameter fromTimeParameter = command.Parameters.AddWithValue("@fromTime", now.TimeOfDay);
                fromTimeParameter.MySqlDbType = MySqlDbType.Time;
                fromTimeParameter.IsNullable = false;

                MySqlParameter toTime = command.Parameters.AddWithValue("@toTime", now.AddMinutes(15).TimeOfDay);
                toTime.MySqlDbType = MySqlDbType.Time;
                toTime.IsNullable = false;

                MySqlParameter subjectParameter = command.Parameters.AddWithValue("@subject", _fixture.Create<string>());
                subjectParameter.MySqlDbType = MySqlDbType.VarChar;
                subjectParameter.Size = 255;
                subjectParameter.IsNullable = true;

                return command;
            }

            /// <summary>
            /// Creates the SQL statement for updating this data proxy.
            /// </summary>
            /// <returns>SQL statement for updating this data proxy.</returns>
            public MySqlCommand CreateUpdateCommand()
            {
                MySqlCommand command = new MySqlCommand("UPDATE Calapps SET Subject=@subject WHERE SystemNo=1 AND CalId=77777")
                {
                    CommandType = CommandType.Text
                };

                MySqlParameter subjectParameter = command.Parameters.AddWithValue("@subject", _fixture.Create<string>());
                subjectParameter.MySqlDbType = MySqlDbType.VarChar;
                subjectParameter.Size = 255;
                subjectParameter.IsNullable = true;

                return command;
            }

            /// <summary>
            /// Creates the SQL statement for deleting this data proxy.
            /// </summary>
            /// <returns>SQL statement for deleting this data proxy.</returns>
            public MySqlCommand CreateDeleteCommand()
            {
                return new MySqlCommand("DELETE FROM Calapps WHERE SystemNo=1 AND CalId=77777")
                {
                    CommandType = CommandType.Text
                };
            }

            #endregion

            #region IMySqlDataProxy Members

            /// <summary>
            /// Returnerer den unikke identifikation for data proxy.
            /// </summary>
            public string UniqueId => SystemNo.ToString(CultureInfo.InvariantCulture);

            #endregion
        }

        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each unit test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en data provider, som benytter MySql.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMySqlDataProvider()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis konfigurationen til connection streng er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisConnectionStringSettingsErNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new MySqlDataProvider(null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "connectionStringSettings");
        }

        /// <summary>
        /// Tester, at Clone initierer ny data provider, som benytter MySql.
        /// </summary>
        [Test]
        public void TestAtCloneInitiererNyMySqlDataProvider()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                using (IMySqlDataProvider clonedMySqlDataProvider = sut.Clone() as IMySqlDataProvider)
                {
                    Assert.That(clonedMySqlDataProvider, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Tester, at GetCollection kaster en ArgumentNullException, hvis query command er null, tom eller white space.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterArgumenutNullExceptionHvisQueryCommandErNull()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.GetCollection<MyDataProxy>(null));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "queryCommand");
            }
        }

        /// <summary>
        /// Tester, at GetCollection, henter data proxies fra MySql.
        /// </summary>
        [Test]
        public void TestAtGetCollectionHenterDataProxies()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MySqlCommand queryCommand = new MySqlCommand("SELECT SystemNo,Title FROM Systems ORDER BY SystemNo")
                {
                    CommandType = CommandType.Text
                };
                IEnumerable<MyDataProxy> result = sut.GetCollection<MyDataProxy>(queryCommand);
                // ReSharper disable PossibleMultipleEnumeration
                Assert.That(result, Is.Not.Null);
                // ReSharper restore PossibleMultipleEnumeration

                // ReSharper disable PossibleMultipleEnumeration
                List<MyDataProxy> proxyCollection = result.ToList();
                // ReSharper restore PossibleMultipleEnumeration
                Assert.That(proxyCollection, Is.Not.Null);
                Assert.That(proxyCollection.Count, Is.GreaterThan(0));

                proxyCollection.ForEach(proxy =>
                {
                    Assert.That(proxy.MapDataIsCalled, Is.True);
                    Assert.That(proxy.MapRelationsIsCalled, Is.True);
                });
            }
        }

        /// <summary>
        /// Tester, at GetCollection kaster en MySqlException, hvis query ikke kan udføres.
        /// </summary>
        [Test]
        public void TestAtGetCollectionKasterMySqlExceptionHvisQueryIkkeKanUdføres()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MySqlCommand queryCommand = new MySqlCommand("SELECT SystemNo,Title FROM XYZSystems WHERE SystemNo=@systemNo ORDER BY SystemNo")
                {
                    CommandType = CommandType.Text
                };
                MySqlParameter systemNoParameter = queryCommand.Parameters.AddWithValue("@systemNo", _random.Next(90, 99));
                systemNoParameter.MySqlDbType = MySqlDbType.Int16;
                systemNoParameter.IsNullable = false;

                Assert.Throws<MySqlException>(() => sut.GetCollection<MyDataProxy>(queryCommand));
            }
        }

        /// <summary>
        /// Tester, at Get henter data proxy fra MySql.
        /// </summary>
        [Test]
        public void TestAtGetHenterDataProxy()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MyDataProxy result = sut.Get(new MyDataProxy {SystemNo = 1});
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SystemNo, Is.EqualTo(1));
                Assert.That(result.Title, Is.Not.Null);
                Assert.That(result.Title.Length, Is.GreaterThan(0));

                Assert.That(result.MapDataIsCalled, Is.True);
                Assert.That(result.MapRelationsIsCalled, Is.True);
            }
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis den data proxy, der foresprøges efter, er null.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Get<MyDataProxy>(null));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProxy");
            }
        }

        /// <summary>
        /// Tester, at Get henter data proxy fra MySql.
        /// </summary>
        [Test]
        public void TestAtGetKasterIntranetRepositoryExceptionHvisIdIkkeFindes()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                int systemNo = _random.Next(1, 9) * -1;
                MyDataProxy queryForDataProxy = new MyDataProxy {SystemNo = systemNo};
                IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.Get(queryForDataProxy));

                TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.CantFindObjectById, queryForDataProxy.GetType().Name, systemNo);
            }
        }

        /// <summary>
        /// Tester, at Add tilfjøer data proxy i MySql.
        /// </summary>
        [Test]
        public void TestAtAddTilføjerDataProxy()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                int systemNo = _random.Next(90, 99);
                MyDataProxy mySqlDataProxy = _fixture.Build<MyDataProxy>()
                    .With(m => m.SystemNo, systemNo)
                    .Create();
                Assert.That(mySqlDataProxy, Is.Not.Null);
                Assert.That(mySqlDataProxy.SaveRelationsIsCalled, Is.False);

                MyDataProxy result = sut.Add(mySqlDataProxy);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SaveRelationsIsCalled, Is.True);
                Assert.That(result.IsInserting, Is.True);

                sut.Delete(result);
            }
        }

        /// <summary>
        /// Tester, at Add kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtAddKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Add<MyDataProxy>(null));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProxy");
            }
        }

        /// <summary>
        /// Tester, at Add kaster en MySqlException, hvis data proxy allerede findes.
        /// </summary>
        [Test]
        public void TestAtAddKasterMySqlExceptionHvisDataProxyFindes()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                int systemNo = _random.Next(90, 99);
                MyDataProxy mySqlDataProxy = _fixture.Build<MyDataProxy>()
                    .With(m => m.SystemNo, systemNo)
                    .Create();
                Assert.That(mySqlDataProxy, Is.Not.Null);

                MyDataProxy result = sut.Add(mySqlDataProxy);
                Assert.That(result, Is.Not.Null);

                Assert.Throws<MySqlException>(() => sut.Add(mySqlDataProxy));

                sut.Delete(result);
            }
        }

        /// <summary>
        /// Tester, at Save gemmer data proxy i MySql.
        /// </summary>
        [Test]
        public void TestAtSaveGemmerDataProxy()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                int systemNo = _random.Next(90, 99);
                MyDataProxy mySqlDataProxy = _fixture.Build<MyDataProxy>()
                    .With(m => m.SystemNo, systemNo)
                    .Create();
                Assert.That(mySqlDataProxy, Is.Not.Null);
                Assert.That(mySqlDataProxy.SaveRelationsIsCalled, Is.False);

                MyDataProxy result = sut.Add(mySqlDataProxy);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SaveRelationsIsCalled, Is.True);
                Assert.That(result.IsInserting, Is.True);

                result = sut.Save(mySqlDataProxy);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SaveRelationsIsCalled, Is.True);
                Assert.That(result.IsInserting, Is.False);

                sut.Delete(result);
            }
        }

        /// <summary>
        /// Tester, at Save kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtSaveKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Save<MyDataProxy>(null));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProxy");
            }
        }

        /// <summary>
        /// Tester, at Delete sletter data proxy fra MySql.
        /// </summary>
        [Test]
        public void TestAtDeleteSletterDataProxy()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                int systemNo = _random.Next(90, 99);
                MyDataProxy mySqlDataProxy = _fixture.Build<MyDataProxy>()
                    .With(m => m.SystemNo, systemNo)
                    .Create();
                Assert.That(mySqlDataProxy, Is.Not.Null);
                Assert.That(mySqlDataProxy.DeleteRelationsIsCalled, Is.False);

                MyDataProxy result = sut.Add(mySqlDataProxy);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.DeleteRelationsIsCalled, Is.False);

                sut.Delete(result);

                Assert.That(result.DeleteRelationsIsCalled, Is.True);
            }
        }

        /// <summary>
        /// Tester, at Delete kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtDeleteKasterArgumenutNullExceptionHvisDataProxyErNull()
        {
            using (IMySqlDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Delete<MyDataProxy>(null));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProxy");
            }
        }

        /// <summary>
        /// Creates an instance of the MySql data provider for unit testing.
        /// </summary>
        /// <returns>Instance of the MySql data provider for unit testing.</returns>
        private IMySqlDataProvider CreateSut()
        {
            return new MySqlDataProvider(ConfigurationManager.ConnectionStrings[MySqlDataProviderConnectionStringSettingsName]);
        }
    }
}
