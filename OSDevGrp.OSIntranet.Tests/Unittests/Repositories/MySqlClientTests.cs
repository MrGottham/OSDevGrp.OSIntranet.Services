using OSDevGrp.OSIntranet.Repositories;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories
{
    /// <summary>
    /// Tester implementering af en MySql klient.
    /// </summary>
    [TestFixture]
    public class MySqlClientTests
    {
        /// <summary>
        /// Tester, at konstruktøren initierer en MySql klient.
        /// </summary>
        [Test]
        public void TestAtConstructorInitiererMySqlClient()
        {
            using (var mySqlClient = new MySqlClient())
            {
                mySqlClient.Dispose();
            }
        }
    }
}
