using System;
using System.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.DataProviders;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProviders
{
    /// <summary>
    /// Tests the data provider which can access data in the food waste repository.
    /// </summary>
    [TestFixture]
    public class FoodWasteDataProviderTests
    {
        #region Private constants

        private const string FoodWasteDataProviderConnectionStringSettingsName = "OSDevGrp.OSIntranet.Repositories.DataProviders.FoodWasteDataProvider";

        #endregion

        /// <summary>
        /// Tests that the constructor initialize a data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodWasteDataProvider()
        {
            using (var foodWasteDataProvider = new FoodWasteDataProvider(ConfigurationManager.ConnectionStrings[FoodWasteDataProviderConnectionStringSettingsName]))
            {
                Assert.That(foodWasteDataProvider, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that the constructor throws abn ArgumentNullException if the settings for the configuration string is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfConnectionStringSettingsIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new FoodWasteDataProvider(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("connectionStringSettings"));
            Assert.That(exception.InnerException, Is.Null);
        }

    }
}
