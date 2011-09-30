using System;
using System.Data;
using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester klasse til behandling af data i en record fra MySql.
    /// </summary>
    public class MySqlDataRecordTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer klasse til behandling af data i en record fra MySql.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMySqlDataRecord()
        {
            var fixture = new Fixture();
            fixture.Inject(MockRepository.GenerateMock<IDataRecord>());

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);
        }

        /// <summary>
        /// Tester, at konstruktøren kaster en ArgumentNullException, hvis data record fra MySql er null.
        /// </summary>
        [Test]
        public void TestAtConstructorKasterArgumentNullExceptionHvisDataRecordErNull()
        {
            var fixture = new Fixture();
            fixture.Inject<IDataRecord>(null);

            Assert.Throws<ArgumentNullException>(() => new MySqlDataRecord(fixture.CreateAnonymous<IDataRecord>()));
        }

        /// <summary>
        /// Tester, at GetInt16 henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetInt16HenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetInt16(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<short>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            Assert.That(mySqlDataRecord.GetInt16(fixture.CreateAnonymous<int>()), Is.GreaterThan(0));

            dataRecord.AssertWasCalled(m => m.GetInt16(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetInt32 henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetInt32HenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetInt32(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<int>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            Assert.That(mySqlDataRecord.GetInt32(fixture.CreateAnonymous<int>()), Is.GreaterThan(0));

            dataRecord.AssertWasCalled(m => m.GetInt32(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetInt64 henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetInt64HenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetInt64(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<long>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            Assert.That(mySqlDataRecord.GetInt64(fixture.CreateAnonymous<int>()), Is.GreaterThan(0));

            dataRecord.AssertWasCalled(m => m.GetInt64(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetDecimal henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetDecimalHenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetDecimal(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<decimal>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            Assert.That(mySqlDataRecord.GetDecimal(fixture.CreateAnonymous<int>()), Is.GreaterThan(0M));

            dataRecord.AssertWasCalled(m => m.GetDecimal(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetBoolean henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetBooleanHenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetBoolean(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(true);
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            Assert.That(mySqlDataRecord.GetBoolean(fixture.CreateAnonymous<int>()), Is.True);

            dataRecord.AssertWasCalled(m => m.GetBoolean(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetString henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetStringHenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetString(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<string>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            Assert.That(mySqlDataRecord.GetString(fixture.CreateAnonymous<int>()), Is.Not.Null);

            dataRecord.AssertWasCalled(m => m.GetString(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetDateTime henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetDateTimeHenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetDateTime(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<DateTime>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            mySqlDataRecord.GetDateTime(fixture.CreateAnonymous<int>());

            dataRecord.AssertWasCalled(m => m.GetDateTime(Arg<int>.Is.GreaterThanOrEqual(0)));
        }

        /// <summary>
        /// Tester, at GetTimeSpan henter en værdi.
        /// </summary>
        [Test]
        public void TestAtGetTimeSpanHenterVærdi()
        {
            var fixture = new Fixture();
            var dataRecord = MockRepository.GenerateMock<IDataRecord>();
            dataRecord.Expect(m => m.GetDateTime(Arg<int>.Is.GreaterThanOrEqual(0)))
                .Return(fixture.CreateAnonymous<DateTime>());
            fixture.Inject(dataRecord);

            var mySqlDataRecord = fixture.CreateAnonymous<MySqlDataRecord>();
            Assert.That(mySqlDataRecord, Is.Not.Null);

            mySqlDataRecord.GetTimeSpan(fixture.CreateAnonymous<int>());

            dataRecord.AssertWasCalled(m => m.GetDateTime(Arg<int>.Is.GreaterThanOrEqual(0)));
        }
    }
}
