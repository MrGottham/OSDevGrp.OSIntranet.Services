using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.QueryHandlers;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.QueryHandlers
{
    /// <summary>
    /// Tests the query handler which handle a query for getting a collection of storage types for system usage.
    /// </summary>
    [TestFixture]
    public class StorageTypeCollectionGetQueryHandlerForSystemViewTests
    {
        #region Private varibales

        private ISystemDataRepository _systemDataRepositoryMock;
        private IFoodWasteObjectMapper _objectMapperMock;

        #endregion

        /// <summary>
        /// Test that the constructor initilize the query handler which handle a query for getting a collection of storage types for system usage.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeStorageTypeCollectionGetQueryHandlerForSystemView()
        {
            StorageTypeCollectionGetQueryHandlerForSystemView sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
        }

        /// <summary>
        /// Creates an instanse of the query handler which handle a query for getting a collection of storage types for system usage.
        /// </summary>
        /// <returns>Instanse of the query handler which handle a query for getting a collection of storage types for system usage.</returns>
        private StorageTypeCollectionGetQueryHandlerForSystemView CreateSut()
        {
            _systemDataRepositoryMock = MockRepository.GenerateMock<ISystemDataRepository>();
            _objectMapperMock = MockRepository.GenerateMock<IFoodWasteObjectMapper>();

            return new StorageTypeCollectionGetQueryHandlerForSystemView(_systemDataRepositoryMock, _objectMapperMock);
        }
    }
}
