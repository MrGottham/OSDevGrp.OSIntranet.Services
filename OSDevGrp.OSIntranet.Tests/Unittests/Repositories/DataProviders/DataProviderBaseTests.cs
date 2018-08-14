using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;

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
        private class MyDataProxy : IDataProxyBase
        {
            #region IDataProxyBase Members

            /// <summary>
            /// Mapper data fra en data reader.
            /// </summary>
            /// <param name="dataReader">Data reader for data provideren.</param>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapData(object dataReader, IDataProviderBase dataProvider)
            {
                Assert.That(dataReader, Is.Not.Null);
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Mapper relationer.
            /// </summary>
            /// <param name="dataProvider">Data provider, hvorfra data mappes.</param>
            public void MapRelations(IDataProviderBase dataProvider)
            {
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Gemmer relationer.
            /// </summary>
            /// <param name="dataProvider">Dataprovider.</param>
            /// <param name="isInserting">Angivelse af, om der indsættes eller opdateres.</param>
            public void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
            {
                Assert.That(dataProvider, Is.Not.Null);
            }

            /// <summary>
            /// Sletter relationer.
            /// </summary>
            /// <param name="dataProvider">Dataprovider.</param>
            public void DeleteRelations(IDataProviderBase dataProvider)
            {
                Assert.That(dataProvider, Is.Not.Null);
            }

            #endregion
        }

        /// <summary>
        /// Egen klasse til test af basis data provider.
        /// </summary>
        private class MyDataProvider : DataProviderBase
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
            /// <param name="query">Foresprøgelse efter data.</param>
            /// <returns>Collection indeholdende data.</returns>
            public override IEnumerable<TDataProxy> GetCollection<TDataProxy>(string query)
            {
                IEnumerable<TDataProxy> dataProxies = _fixture.CreateMany<TDataProxy>(3).ToList();
                foreach (var dataProxy in dataProxies)
                {
                    dataProxy.MapData(_fixture, this);
                }
                return dataProxies;
            }

            /// <summary>
            /// Henter data for en given data proxy i data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="queryForDataProxy">Data proxy, som indeholder nødvendige værdier til fremsøgning.</param>
            /// <returns>Data proxy.</returns>
            public override TDataProxy Get<TDataProxy>(TDataProxy queryForDataProxy)
            {
                TDataProxy dataProxy = _fixture.Create<TDataProxy>();
                dataProxy.MapData(_fixture, this);
                return dataProxy;
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
            using (IDataProviderBase sut = CreateSut())
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
            using (IDataProviderBase sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                using (IDataProviderBase clonedDataProvider = sut.Clone() as IDataProviderBase)
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
            using (IDataProviderBase sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                IEnumerable<MyDataProxy> result = sut.GetCollection<MyDataProxy>(_fixture.Create<string>());
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
            using (IDataProviderBase sut = CreateSut())
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
            using (IDataProviderBase sut = CreateSut())
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
            using (IDataProviderBase sut = CreateSut())
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
            using (IDataProviderBase sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                sut.Delete(_fixture.Create<MyDataProxy>());
            }
        }

        /// <summary>
        /// Creates an instance of the data provider for unit testning.
        /// </summary>
        /// <returns>Instance of the data provider for unit testning.</returns>
        private IDataProviderBase CreateSut()
        {
            return new MyDataProvider(_fixture);
        }
    }
}
