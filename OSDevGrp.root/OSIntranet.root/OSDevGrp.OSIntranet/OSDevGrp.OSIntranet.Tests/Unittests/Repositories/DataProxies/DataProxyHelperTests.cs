﻿using System;
using System.Reflection;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies
{
    /// <summary>
    /// Tester hjælpeklasse til en data proxy.
    /// </summary>
    [TestFixture]
    public class DataProxyHelperTests
    {
        /// <summary>
        /// Egen klasse til test af hjælpeklasse til en data proxy.
        /// </summary>
        private class MyDataProxy : IDataProxyBase
        {
            #region Private variables

            private readonly int _nummer;

            #endregion

            #region Constructor

            /// <summary>
            /// Danner egen klasse til test af hjælpeklasse til en data proxy.
            /// </summary>
            public MyDataProxy()
            {
                _nummer = 0;
                MapData(null, null);
            }

            #endregion

            #region Properties

            /// <summary>
            /// Nummer.
            /// </summary>
            public int Nummer
            {
                get
                {
                    return _nummer;
                }
            }

            #endregion

            #region IDataProxyBase Members

            /// <summary>
            /// Mapper data fra data reader til data proxy.
            /// </summary>
            /// <param name="dataReader">Data reader.</param>
            /// <param name="dataProvider">Data proxy.</param>
            public void MapData(object dataReader, IDataProviderBase dataProvider)
            {
            }

            #endregion
        }

        /// <summary>
        /// Egen nedarvet klasse til test af hjælpeklasse til en data proxy.
        /// </summary>
        private class MyInheritDataProxy : MyDataProxy
        {
        }

        /// <summary>
        /// Tester, at SetFieldValue kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueKasterArgumentNullExceptionHvisDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject("_nummer");

            Assert.Throws<ArgumentNullException>(
                () =>
                DataProxyHelper.SetFieldValue(null, fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<int>()));
        }

        /// <summary>
        /// Tester, at SetFieldValue kaster en ArgumentNullException, hvis navnet på variablen er null.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueKasterArgumentNullExceptionHvisFieldNameErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<string>(null);

            var dataProxy = new MyDataProxy();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () => dataProxy.SetFieldValue(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<int>()));
        }

        /// <summary>
        /// Tester, at SetFieldValue kaster en ArgumentNullException, hvis navnet på variablen er tomt.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueKasterArgumentNullExceptionHvisFieldNameErEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(string.Empty);

            var dataProxy = new MyDataProxy();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () => dataProxy.SetFieldValue(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<int>()));
        }

        /// <summary>
        /// Tester, at SetFieldValue kaster en ArgumentNullException, hvis værdien er null.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueKasterArgumentNullExceptionHvisValueErNull()
        {
            var fixture = new Fixture();
            fixture.Inject("_nummer");
            fixture.Inject<object>(null);

            var dataProxy = new MyDataProxy();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () => dataProxy.SetFieldValue(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<object>()));
        }

        /// <summary>
        /// Tester, at SetFieldValue sætter værdi på variablen.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueSætterFieldValue()
        {
            var fixture = new Fixture();
            fixture.Inject("_nummer");

            var dataProxy = new MyDataProxy();
            Assert.That(dataProxy, Is.Not.Null);
            Assert.That(dataProxy.Nummer, Is.EqualTo(0));

            dataProxy.SetFieldValue(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<int>());
            Assert.That(dataProxy.Nummer, Is.Not.EqualTo(0));
        }

        /// <summary>
        /// Tester, at SetFieldValue sætter værdi på variablen i basisklassen.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueSætterFieldValuePåBase()
        {
            var fixture = new Fixture();
            fixture.Inject("_nummer");

            var dataProxy = new MyInheritDataProxy();
            Assert.That(dataProxy, Is.Not.Null);
            Assert.That(dataProxy.Nummer, Is.EqualTo(0));

            dataProxy.SetFieldValue(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<int>());
            Assert.That(dataProxy.Nummer, Is.Not.EqualTo(0));
        }

        /// <summary>
        /// Tester, at SetFieldValue kaster en IntranetSystemException, hvis der ikke findes en variablen med det givne navn.
        /// </summary>
        [Test]
        public void TestAtSetFieldValueKasterIntranetSystemExceptionHvisVariableIkkeFindes()
        {
            var fixture = new Fixture();

            var dataProxy = new MyDataProxy();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<IntranetSystemException>(
                () => dataProxy.SetFieldValue(fixture.CreateAnonymous<string>(), fixture.CreateAnonymous<int>()));
        }

        /// <summary>
        /// Tester, at GetNullableSqlString kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtGetNullableSqlStringKasterArgumentNullExceptionHvisDataProxyErNull()
        {
            var fixture = new Fixture();

            Assert.Throws<ArgumentNullException>(
                () => DataProxyHelper.GetNullableSqlString(null, fixture.CreateAnonymous<string>()));
        }

        /// <summary>
        /// Tester, at GetNullableSqlString returnerer SQL streng for værdi.
        /// </summary>
        [Test]
        public void TestAtGetNullableSqlStringReturnererSqlStringForValue()
        {
            var fixture = new Fixture();
            var value = fixture.CreateAnonymous<string>();

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            var sqlString = dataProxy.GetNullableSqlString(value);
            Assert.That(sqlString, Is.Not.Null);
            Assert.That(sqlString, Is.EqualTo(string.Format("'{0}'", value)));
        }

        /// <summary>
        /// Tester, at GetNullableSqlString returnerer SQL streng for null.
        /// </summary>
        [Test]
        public void TestAtGetNullableSqlStringReturnererSqlStringForNull()
        {
            var fixture = new Fixture();

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            var sqlString = dataProxy.GetNullableSqlString(null);
            Assert.That(sqlString, Is.Not.Null);
            Assert.That(sqlString, Is.EqualTo("NULL"));
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis data proxy er null.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumentNullExceptionHvisDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            Assert.Throws<ArgumentNullException>(
                () =>
                DataProxyHelper.Get(null, fixture.CreateAnonymous<IDataProviderBase>(),
                                    fixture.CreateAnonymous<MyDataProxy>(), MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis data provider, som skal hente data proxy, er null.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumentNullExceptionHvisDataProviderErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IDataProviderBase>(null);

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(), fixture.CreateAnonymous<MyDataProxy>(),
                              MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis data proxy der, skal søges efter, er null.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumentNullExceptionHvisQueryForDataProxyErNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            fixture.Inject<MyDataProxy>(null);
            Assert.Throws<ArgumentNullException>(
                () =>
                dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(), fixture.CreateAnonymous<MyDataProxy>(),
                              MethodBase.GetCurrentMethod().Name));
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis navn på metoden, der kaldes fra er null.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumentNullExceptionHvisCallerNameNull()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(), fixture.CreateAnonymous<MyDataProxy>(), null));
        }

        /// <summary>
        /// Tester, at Get kaster en ArgumentNullException, hvis navn på metoden, der kaldes fra er tom.
        /// </summary>
        [Test]
        public void TestAtGetKasterArgumentNullExceptionHvisCallerNameEmpty()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IDataProviderBase>());

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<ArgumentNullException>(
                () =>
                dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(), fixture.CreateAnonymous<MyDataProxy>(),
                              string.Empty));
        }

        /// <summary>
        /// Tester, at Get henter data proxy gennem data provideren.
        /// </summary>
        [Test]
        public void TestAtGetHenterDataProxyGennemDataProvider()
        {
            var fixture = new Fixture();

            var dataProvider = MockRepository.GenerateMock<IDataProviderBase>();
            dataProvider.Expect(m => m.Get(Arg<MyDataProxy>.Is.NotNull))
                .Return(fixture.CreateAnonymous<MyDataProxy>());
            fixture.Inject(dataProvider);

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            var loadDataProxy = dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(),
                                              fixture.CreateAnonymous<MyDataProxy>(), MethodBase.GetCurrentMethod().Name);
            Assert.That(loadDataProxy, Is.Not.Null);

            dataProvider.AssertWasCalled(m => m.Get(Arg<MyDataProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Get kaster en IntranetRepositoryException ved IntranetRepositoryException.
        /// </summary>
        [Test]
        public void TestAtGetKasterIntranetRepositoryExceptionVedIntranetRepositoryException()
        {
            var fixture = new Fixture();

            var dataProvider = MockRepository.GenerateMock<IDataProviderBase>();
            dataProvider.Expect(m => m.Get(Arg<MyDataProxy>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<IntranetRepositoryException>());
            fixture.Inject(dataProvider);

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(), fixture.CreateAnonymous<MyDataProxy>(),
                              MethodBase.GetCurrentMethod().Name));

            dataProvider.AssertWasCalled(m => m.Get(Arg<MyDataProxy>.Is.NotNull));
        }

        /// <summary>
        /// Tester, at Get kaster en IntranetRepositoryException ved Exception.
        /// </summary>
        [Test]
        public void TestAtGetKasterIntranetRepositoryExceptionVedException()
        {
            var fixture = new Fixture();

            var dataProvider = MockRepository.GenerateMock<IDataProviderBase>();
            dataProvider.Expect(m => m.Get(Arg<MyDataProxy>.Is.NotNull))
                .Throw(fixture.CreateAnonymous<Exception>());
            fixture.Inject(dataProvider);

            var dataProxy = fixture.CreateAnonymous<MyDataProxy>();
            Assert.That(dataProxy, Is.Not.Null);

            Assert.Throws<IntranetRepositoryException>(
                () =>
                dataProxy.Get(fixture.CreateAnonymous<IDataProviderBase>(), fixture.CreateAnonymous<MyDataProxy>(),
                              MethodBase.GetCurrentMethod().Name));

            dataProvider.AssertWasCalled(m => m.Get(Arg<MyDataProxy>.Is.NotNull));
        }
    }
}