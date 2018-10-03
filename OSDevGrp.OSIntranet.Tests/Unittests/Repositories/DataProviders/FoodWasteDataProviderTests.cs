using System;
using System.Configuration;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;

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
            using (IFoodWasteDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that the constructor throws abn ArgumentNullException if the settings for the configuration string is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionIfConnectionStringSettingsIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => new FoodWasteDataProvider(null));
            // ReSharper restore ObjectCreationAsStatement

            TestHelper.AssertArgumentNullExceptionIsValid(result, "connectionStringSettings");
        }

        /// <summary>
        /// Tests that Clone clones the data provider which can access data in the food waste repository.
        /// </summary>
        [Test]
        public void TestThatCloneClonesFoodWasteDataProvider()
        {
            using (IFoodWasteDataProvider sut = CreateSut())
            {
                Assert.That(sut, Is.Not.Null);

                using (IFoodWasteDataProvider clone = sut.Clone() as IFoodWasteDataProvider)
                {
                    Assert.That(clone, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Creates an instance of the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Instance of the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateSut()
        {
            return new FoodWasteDataProvider(ConfigurationManager.ConnectionStrings[FoodWasteDataProviderConnectionStringSettingsName]);
        }
    }
}
