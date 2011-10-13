using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProviders
{
    /// <summary>
    /// Tester basis data provider.
    /// </summary>
    [TestFixture]
    public class DataProviderBaseBaseTests 
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
                _fixture = fixture;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Henter og returnerer data fra data provideren.
            /// </summary>
            /// <typeparam name="TDataProxy">Typen for data proxy til data provideren.</typeparam>
            /// <param name="query">Foresprøgelse efter data.</param>
            /// <returns>Collection indeholdende data.</returns>
            public override IEnumerable<TDataProxy> GetCollection<TDataProxy>(string query)
            {
                var dataProxies = _fixture.CreateMany<TDataProxy>(3).ToList();
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
                var dataProxy = _fixture.CreateAnonymous<TDataProxy>();
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

            /// <summary>
            /// Danner ny instans af data provideren.
            /// </summary>
            /// <returns>Ny instans af data provideren.</returns>
            public override object Clone()
            {
                return new MyDataProvider(_fixture);
            }

            #endregion
        }

        /// <summary>
        /// Tester, at konstruktøren initierer en basis data provider.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererDataProviderBase()
        {
            var fixture = new Fixture();
            fixture.Inject(new MyDataProxy());
            fixture.Inject(new MyDataProvider(fixture));

            var dataProvider = fixture.CreateAnonymous<MyDataProvider>();
            Assert.That(dataProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at GetCollection henter data fra basis data provider.
        /// </summary>
        [Test]
        public void TestAtGetCollectionHenterData()
        {
            var fixture = new Fixture();
            fixture.Inject(new MyDataProxy());
            fixture.Inject(new MyDataProvider(fixture));

            var dataProvider = fixture.CreateAnonymous<MyDataProvider>();
            Assert.That(dataProvider, Is.Not.Null);

            var result = dataProvider.GetCollection<MyDataProxy>(fixture.CreateAnonymous<string>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Tester, at Get henter data fra basis data provider.
        /// </summary>
        [Test]
        public void TestAtGetHenterData()
        {
            var fixture = new Fixture();
            fixture.Inject(new MyDataProxy());
            fixture.Inject(new MyDataProvider(fixture));

            var dataProvider = fixture.CreateAnonymous<MyDataProvider>();
            Assert.That(dataProvider, Is.Not.Null);

            var result = dataProvider.Get(new MyDataProxy());
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Add tilføjer data til data provideren.
        /// </summary>
        [Test]
        public void TestAtAddTilføjerData()
        {
            var fixture = new Fixture();
            fixture.Inject(new MyDataProxy());
            fixture.Inject(new MyDataProvider(fixture));

            var dataProvider = fixture.CreateAnonymous<MyDataProvider>();
            Assert.That(dataProvider, Is.Not.Null);

            var result = dataProvider.Add(fixture.CreateAnonymous<MyDataProxy>());
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Save gemmer data i data provideren.
        /// </summary>
        [Test]
        public void TestAtSaveGemmerData()
        {
            var fixture = new Fixture();
            fixture.Inject(new MyDataProxy());
            fixture.Inject(new MyDataProvider(fixture));

            var dataProvider = fixture.CreateAnonymous<MyDataProvider>();
            Assert.That(dataProvider, Is.Not.Null);

            var result = dataProvider.Save(fixture.CreateAnonymous<MyDataProxy>());
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at Delete sletter data fra data provideren.
        /// </summary>
        [Test]
        public void TestAtDeleteSletterData()
        {
            var fixture = new Fixture();
            fixture.Inject(new MyDataProxy());
            fixture.Inject(new MyDataProvider(fixture));

            var dataProvider = fixture.CreateAnonymous<MyDataProvider>();
            Assert.That(dataProvider, Is.Not.Null);

            dataProvider.Delete(fixture.CreateAnonymous<MyDataProxy>());
        }
    }
}
