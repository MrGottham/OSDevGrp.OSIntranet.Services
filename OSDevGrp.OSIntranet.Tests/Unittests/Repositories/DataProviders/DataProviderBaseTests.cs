﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProviders
{
    /// <summary>
    /// Tester basis data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderBaseTests 
    {
        /// <summary>
        /// Egen klasse for en data proxy til test af basis data provider.
        /// </summary>
        private class MyDataProxy : IDataProxyBase<IDataReader, IDbCommand>, IDataProxyCreatorBase<MyDataProxy, IDataReader, IDbCommand>
        {
            #region Properties

            /// <summary>
            /// Gets whether the Create method has been called.
            /// </summary>
            public bool CreateHasBeenCalled { get; private set; }
            
            #endregion
            
            #region IDataProxyBase<IDataReader, IDbCommand> Members

            /// <summary>
            /// Mapper data fra en data reader.
            /// </summary>
            /// <param name="dataReader">Data reader for data provideren.</param>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapData(IDataReader dataReader, IDataProviderBase<IDataReader, IDbCommand> dataProvider)
            {
                Assert.That(dataReader, Is.Not.Null);
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Mapper relationer.
            /// </summary>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapRelations(IDataProviderBase<IDataReader, IDbCommand> dataProvider)
            {
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Gemmer relationer.
            /// </summary>
            /// <param name="dataProvider">Dataprovider.</param>
            /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
            public void SaveRelations(IDataProviderBase<IDataReader, IDbCommand> dataProvider, bool isInserting)
            {
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Sletter relationer.
            /// </summary>
            /// <param name="dataProvider">Dataprovider.</param>
            public void DeleteRelations(IDataProviderBase<IDataReader, IDbCommand> dataProvider)
            {
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Creates the SQL statement for getting this data proxy.
            /// </summary>
            /// <returns>SQL statement for getting this data proxy.</returns>
            public IDbCommand CreateGetCommand()
            {
                return MockRepository.GenerateMock<IDbCommand>();
            }

            /// <summary>
            /// Creates the SQL statement for inserting this data proxy.
            /// </summary>
            /// <returns>SQL statement for inserting this data proxy.</returns>
            public IDbCommand CreateInsertCommand()
            {
                return MockRepository.GenerateMock<IDbCommand>();
            }

            /// <summary>
            /// Creates the SQL statement for updating this data proxy.
            /// </summary>
            /// <returns>SQL statement for updating this data proxy.</returns>
            public IDbCommand CreateUpdateCommand()
            {
                return MockRepository.GenerateMock<IDbCommand>();
            }

            /// <summary>
            /// Creates the SQL statement for deleting this data proxy.
            /// </summary>
            /// <returns>SQL statement for deleting this data proxy.</returns>
            public IDbCommand CreateDeleteCommand()
            {
                return MockRepository.GenerateMock<IDbCommand>();
            }

            #endregion

            #region IDataProxyCreatorBase<MyDataProxy, IDataReader, IDbCommand> Members

            /// <summary>
            /// Creates an instance of the data proxy with values from the data reader.
            /// </summary>
            /// <param name="dataReader">Data reader from which column values should be read.</param>
            /// <param name="dataProvider">Data provider which supports the data reader.</param>
            /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
            /// <returns>Instance of the data proxy with values from the data reader.</returns>
            public MyDataProxy Create(IDataReader dataReader, IDataProviderBase<IDataReader, IDbCommand> dataProvider, params string[] columnNameCollection)
            {
                Assert.That(dataReader, Is.Not.Null);
                Assert.That(dataProvider, Is.Not.Null);
                Assert.That(columnNameCollection, Is.Not.Null);

                CreateHasBeenCalled = true;

                return new MyDataProxy();
            }

            #endregion
        }

        /// <summary>
        /// Egen klasse til test af basis data provider.
        /// </summary>
        private class MyDataProvider : DataProviderBase<IDataReader, IDbCommand>
        {
            #region Private variables

            private readonly Fixture _fixture;

            #endregion

            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af basis data provider.
            /// </summary>
            /// <param name="fixture">AutoFixture.</param>
            public MyDataProvider(Fixture fixture)
            {
                ArgumentNullGuard.NotNull(fixture, nameof(fixture));

                _fixture = fixture;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Frigørelse af allokerede ressourcer i data provideren.
            /// </summary>
            public override void Dispose()
            {
            }

            /// <summary>
            /// Danner ny instans af data provideren.
            /// </summary>
            /// <returns>Ny instans af data provideren.</returns>
            public override object Clone()
            {
                return new MyDataProvider(_fixture);
            }

            /// <summary>
            /// Henter og returnerer data fra data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="queryCommand">Database command for the SQL query statement.</param>
            /// <returns>Collection indeholdende data.</returns>
            public override IEnumerable<TDataProxy> GetCollection<TDataProxy>(IDbCommand queryCommand)
            {
                IEnumerable<TDataProxy> dataProxies = _fixture.CreateMany<TDataProxy>(3).ToList();
                foreach (var dataProxy in dataProxies)
                {
                    dataProxy.MapData(MockRepository.GenerateMock<IDataReader>(), this);
                }
                return dataProxies;
            }

            /// <summary>
            /// Henter data for en given data proxy i data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="dataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning.</param>
            /// <returns>Data proxy.</returns>
            public override TDataProxy Get<TDataProxy>(TDataProxy dataProxy)
            {
                TDataProxy result = _fixture.Create<TDataProxy>();
                result.MapData(MockRepository.GenerateMock<IDataReader>(), this);
                return result;
            }

            /// <summary>
            /// Tilføjer data til data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="dataProxy">Data proxy med data, som skal tilføjes data provideren.</param>
            /// <returns>Data proxy med tilføjede data.</returns>
            public override TDataProxy Add<TDataProxy>(TDataProxy dataProxy)
            {
                return dataProxy;
            }

            /// <summary>
            /// Gemmer data i data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="dataProxy">Data proxy med data, som skal gemmes i data provideren.</param>
            /// <returns>Data proxy med gemte data.</returns>
            public override TDataProxy Save<TDataProxy>(TDataProxy dataProxy)
            {
                return dataProxy;
            }

            /// <summary>
            /// Sletter data fra data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="dataProxy">Data proxy med data, som skal slettes fra data provideren.</param>
            public override void Delete<TDataProxy>(TDataProxy dataProxy)
            {
            }

            #endregion
        }

        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each unit test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en basis data provider.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererDataProviderBase()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tester, at Clone initierer ny basis data provider.
        /// </summary>
        [Test]
        public void TestAtCloneInitiererNyDataProviderBase()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                using (IDataProviderBase<IDataReader, IDbCommand> clonedDataProvider = sut.Clone() as IDataProviderBase<IDataReader, IDbCommand>)
                {
                    Assert.That(clonedDataProvider, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Tester, at GetCollection henter data fra basis data provider.
        /// </summary>
        [Test]
        public void TestAtGetCollectionHenterData()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                IEnumerable<MyDataProxy> result = sut.GetCollection<MyDataProxy>(MockRepository.GenerateMock<IDbCommand>());
                // ReSharper disable PossibleMultipleEnumeration
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count(), Is.EqualTo(3));
                // ReSharper restore PossibleMultipleEnumeration
            }
        }

        /// <summary>
        /// Tester, at Get henter data fra basis data provider.
        /// </summary>
        [Test]
        public void TestAtGetHenterData()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MyDataProxy result = sut.Get(_fixture.Create<MyDataProxy>());
                Assert.That(result, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tester, at Add tilføjer data til data provideren.
        /// </summary>
        [Test]
        public void TestAtAddTilføjerData()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MyDataProxy result = sut.Add(_fixture.Create<MyDataProxy>());
                Assert.That(result, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tester, at Save gemmer data i data provideren.
        /// </summary>
        [Test]
        public void TestAtSaveGemmerData()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MyDataProxy result = sut.Save(_fixture.Create<MyDataProxy>());
                Assert.That(result, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tester, at Delete sletter data fra data provideren.
        /// </summary>
        [Test]
        public void TestAtDeleteSletterData()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                sut.Delete(_fixture.Create<MyDataProxy>());
            }
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException when the data proxy creator is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenDataProxyCreatorIsNull()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create<MyDataProxy>(null, MockRepository.GenerateMock<IDataReader>(), "XYZ"));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProxyCreator");
            }
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException when the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenDataReaderIsNull()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(new MyDataProxy(), null, "XYZ"));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
            }
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException when the collection of column names is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionWhenColumnNameCollectionIsNull()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(new MyDataProxy(), MockRepository.GenerateMock<IDataReader>(), null));

                TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
            }
        }

        /// <summary>
        /// Tests that Create calls Create on the data proxy creator.
        /// </summary>
        [Test]
        public void TestThatCreateCallsCreateOnDataProxyCreator()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MyDataProxy dataProxyCreator = new MyDataProxy();
                Assert.That(dataProxyCreator, Is.Not.Null);
                Assert.That(dataProxyCreator.CreateHasBeenCalled, Is.False);

                sut.Create(dataProxyCreator, MockRepository.GenerateMock<IDataReader>(), "XYZ");

                Assert.That(dataProxyCreator.CreateHasBeenCalled, Is.True);
            }
        }

        /// <summary>
        /// Tests that Create creates a data proxy.
        /// </summary>
        [Test]
        public void TestThatCreateCreatesDataProxy()
        {
            using (IDataProviderBase<IDataReader, IDbCommand> sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                MyDataProxy result = sut.Create(new MyDataProxy(), MockRepository.GenerateMock<IDataReader>(), "XYZ");
                Assert.That(result, Is.Not.Null);
            }
        }

        /// <summary>
        /// Creates an instance of the data provider for unit testning.
        /// </summary>
        /// <returns>Instance of the data provider for unit testning.</returns>
        private IDataProviderBase<IDataReader, IDbCommand> CreateSut()
        {
            return new MyDataProvider(_fixture);
        }
    }
}
