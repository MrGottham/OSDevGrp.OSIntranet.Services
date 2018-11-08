using System;
using AutoFixture;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a given relation between a food item and a food group in the food waste domain.
    /// </summary>
    [TestFixture]
    public class FoodItemGroupProxyTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a given relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeFoodItemGroupProxy()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.FoodItem, Is.Null);
            Assert.That(sut.FoodItemIdentifier, Is.Null);
            Assert.That(sut.FoodItemIdentifier.HasValue, Is.False);
            Assert.That(sut.FoodGroup, Is.Null);
            Assert.That(sut.FoodGroupIdentifier, Is.Null);
            Assert.That(sut.FoodGroupIdentifier.HasValue, Is.False);
            Assert.That(sut.IsPrimary, Is.False);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the relation between a food item and a food group has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenFoodItemGroupProxyHasNoIdentifier()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForFoodItemGroupProxy()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, CreateFoodWasteDataProvider()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataMapsDataIntoProxy(bool isPrimary)
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid foodItemGroupIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(foodItemGroupIdentifier, isPrimary);

            Guid foodItemIdentifier = Guid.NewGuid();
            FoodItemProxy foodItemProxy = BuildFoodItemProxy(identifier: foodItemIdentifier);
            Guid foodGroupIdentifier = Guid.NewGuid();
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy(identifier: foodGroupIdentifier);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(foodItemProxy, foodGroupProxy);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(foodItemGroupIdentifier));
            Assert.That(sut.FoodItem, Is.Not.Null);
            Assert.That(sut.FoodItem, Is.EqualTo(foodItemProxy));
            Assert.That(sut.FoodItemIdentifier, Is.Not.Null);
            Assert.That(sut.FoodItemIdentifier, Is.EqualTo(foodItemIdentifier));
            Assert.That(sut.FoodGroup, Is.Not.Null);
            Assert.That(sut.FoodGroup, Is.EqualTo(foodGroupProxy));
            Assert.That(sut.FoodGroupIdentifier, Is.Not.Null);
            Assert.That(sut.FoodGroupIdentifier, Is.EqualTo(foodGroupIdentifier));
            Assert.That(sut.IsPrimary, Is.EqualTo(isPrimary));

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("FoodItemGroupIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt32(Arg<string>.Is.Equal("IsPrimary")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IFoodItemProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 2 &&
                                               e[0] == "FoodItemIdentifier" &&
                                               e[1] == "FoodItemIsActive")),
                opt => opt.Repeat.Once());
            dataProvider.AssertWasCalled(m => m.Create(
                    Arg<IFoodGroupProxy>.Is.TypeOf,
                    Arg<MySqlDataReader>.Is.Equal(dataReader),
                    Arg<string[]>.Matches(e => e != null && e.Length == 3 &&
                                               e[0] == "FoodGroupIdentifier" &&
                                               e[1] == "FoodGroupParentIdentifier" &&
                                               e[2] == "FoodGroupIsActive")),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the relation between a food item and a food group is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the translation is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IFoodItemGroupProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IFoodItemGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT fig.FoodItemGroupIdentifier,fig.FoodItemIdentifier,fig.FoodGroupIdentifier,fig.IsPrimary,fi.IsActive AS FoodItemIsActive,fg.ParentIdentifier AS FoodGroupParentIdentifier,fg.IsActive AS FoodGroupIsActive,pfg.ParentIdentifier AS FoodGroupParentsParentIdentifier,pfg.IsActive AS FoodGroupParentsParentIsActive FROM FoodItemGroups AS fig INNER JOIN FoodItems AS fi ON fi.FoodItemIdentifier=fig.FoodItemIdentifier INNER JOIN FoodGroups AS fg ON fg.FoodGroupIdentifier=fig.FoodGroupIdentifier LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier WHERE fig.FoodItemGroupIdentifier=@foodItemGroupIdentifier")
                .AddCharDataParameter("@foodItemGroupIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand throws an IntranetRepositoryException when the identifier for the food item on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatCreateInsertCommandThrowsIntranetRepositoryExceptionWhenFoodItemIdentifierOnFoodItemGroupHasNoValue()
        {
            FoodItemProxy foodItemProxy = BuildFoodItemProxy(false);
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy();

            IFoodItemGroupProxy sut = CreateSut(Guid.NewGuid(), foodItemProxy, foodGroupProxy, _fixture.Create<bool>());
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var insertCommand = sut.CreateInsertCommand();});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.FoodItemIdentifier, "FoodItemIdentifier");
        }

        /// <summary>
        /// Tests that CreateInsertCommand throws an IntranetRepositoryException when the identifier for the food group on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatCreateInsertCommandThrowsIntranetRepositoryExceptionWhenFoodGroupIdentifierOnFoodItemGroupHasNoValue()
        {
            FoodItemProxy foodItemProxy = BuildFoodItemProxy();
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy(false);

            IFoodItemGroupProxy sut = CreateSut(Guid.NewGuid(), foodItemProxy, foodGroupProxy, _fixture.Create<bool>());
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var insertCommand = sut.CreateInsertCommand();});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.FoodGroupIdentifier, "FoodGroupIdentifier");
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert this relation between a food item and a food group.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(bool isPrimary)
        {
            Guid identifier = new Guid();

            Guid foodItemIdentifier = Guid.NewGuid();
            FoodItemProxy foodItemProxy = BuildFoodItemProxy(identifier: foodItemIdentifier);

            Guid foodGroupIdentifier = Guid.NewGuid();
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy(identifier: foodGroupIdentifier);

            IFoodItemGroupProxy sut = CreateSut(identifier, foodItemProxy, foodGroupProxy, isPrimary);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO FoodItemGroups (FoodItemGroupIdentifier,FoodItemIdentifier,FoodGroupIdentifier,IsPrimary) VALUES(@foodItemGroupIdentifier,@foodItemIdentifier,@foodGroupIdentifier,@isPrimary)")
                .AddCharDataParameter("@foodItemGroupIdentifier", identifier)
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .AddCharDataParameter("@foodGroupIdentifier", foodGroupIdentifier)
                .AddBitDataParameter("@isPrimary", isPrimary)
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand throws an IntranetRepositoryException when the identifier for the food item on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatCreateUpdateCommandThrowsIntranetRepositoryExceptionWhenFoodItemIdentifierOnFoodItemGroupHasNoValue()
        {
            FoodItemProxy foodItemProxy = BuildFoodItemProxy(false);
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy();

            IFoodItemGroupProxy sut = CreateSut(Guid.NewGuid(), foodItemProxy, foodGroupProxy, _fixture.Create<bool>());
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var updateCommand = sut.CreateUpdateCommand();});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.FoodItemIdentifier, "FoodItemIdentifier");
        }

        /// <summary>
        /// Tests that CreateUpdateCommand throws an IntranetRepositoryException when the identifier for the food group on the given relation between a food item and a food group has no value.
        /// </summary>
        [Test]
        public void TestThatCreateUpdateCommandThrowsIntranetRepositoryExceptionWhenFoodGroupIdentifierOnFoodItemGroupHasNoValue()
        {
            FoodItemProxy foodItemProxy = BuildFoodItemProxy();
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy(false);

            IFoodItemGroupProxy sut = CreateSut(Guid.NewGuid(), foodItemProxy, foodGroupProxy, _fixture.Create<bool>());
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var updateCommand = sut.CreateUpdateCommand();});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.FoodGroupIdentifier, "FoodGroupIdentifier");
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update this relation between a food item and a food group.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(bool isPrimary)
        {
            Guid identifier = new Guid();

            Guid foodItemIdentifier = Guid.NewGuid();
            FoodItemProxy foodItemProxy = BuildFoodItemProxy(identifier: foodItemIdentifier);

            Guid foodGroupIdentifier = Guid.NewGuid();
            FoodGroupProxy foodGroupProxy = BuildFoodGroupProxy(identifier: foodGroupIdentifier);

            IFoodItemGroupProxy sut = CreateSut(identifier, foodItemProxy, foodGroupProxy, isPrimary);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE FoodItemGroups SET FoodItemIdentifier=@foodItemIdentifier,FoodGroupIdentifier=@foodGroupIdentifier,IsPrimary=@isPrimary WHERE FoodItemGroupIdentifier=@foodItemGroupIdentifier")
                .AddCharDataParameter("@foodItemGroupIdentifier", identifier)
                .AddCharDataParameter("@foodItemIdentifier", foodItemIdentifier)
                .AddCharDataParameter("@foodGroupIdentifier", foodGroupIdentifier)
                .AddBitDataParameter("@isPrimary", isPrimary)
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete this relation between a food item and a food group.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = new Guid();

            IFoodItemGroupProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM FoodItemGroups WHERE FoodItemGroupIdentifier=@foodItemGroupIdentifier")
                .AddCharDataParameter("@foodItemGroupIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Creates an instance of a data proxy for a given relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy for a given relation between a food item and a food group in the food waste domain.</returns>
        private IFoodItemGroupProxy CreateSut()
        {
            return new FoodItemGroupProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy for a given relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy for a given relation between a food item and a food group in the food waste domain.</returns>
        private IFoodItemGroupProxy CreateSut(Guid identifier)
        {
            return new FoodItemGroupProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy for a given relation between a food item and a food group in the food waste domain.
        /// </summary>
        /// <returns>Instance of a data proxy for a given relation between a food item and a food group in the food waste domain.</returns>
        private IFoodItemGroupProxy CreateSut(Guid identifier, FoodItemProxy foodItemProxy, FoodGroupProxy foodGroupProxy, bool isPrimary)
        {
            return new FoodItemGroupProxy(foodItemProxy, foodGroupProxy)
            {
                Identifier = identifier,
                IsPrimary = isPrimary
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? foodItemGroupIdentifier = null, bool? isPrimary = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("FoodItemGroupIdentifier")))
                .Return(foodItemGroupIdentifier.HasValue ? foodItemGroupIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt32(Arg<string>.Is.Equal("IsPrimary")))
                .Return(Convert.ToInt32(isPrimary ?? _fixture.Create<bool>()))
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(FoodItemProxy foodItemProxy = null, FoodGroupProxy foodGroupProxy = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IFoodItemProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(foodItemProxy ?? BuildFoodItemProxy())
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Create(Arg<IFoodGroupProxy>.Is.TypeOf, Arg<MySqlDataReader>.Is.Anything, Arg<string[]>.Is.Anything))
                .Return(foodGroupProxy ?? BuildFoodGroupProxy())
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Creates a data proxy for a food item.
        /// </summary>
        /// <param name="hasIdentifier">Indicates whether the data proxy has an identifier.</param>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy for the food item.</returns>
        private FoodItemProxy BuildFoodItemProxy(bool hasIdentifier = true, Guid? identifier = null)
        {
            return new FoodItemProxy
            {
                Identifier = hasIdentifier ? identifier ?? Guid.NewGuid() : (Guid?) null
            };
        }

        /// <summary>
        /// Creates a data proxy for a food group.
        /// </summary>
        /// <param name="hasIdentifier">Indicates whether the data proxy has an identifier.</param>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy for the food group.</returns>
        private FoodGroupProxy BuildFoodGroupProxy(bool hasIdentifier = true, Guid? identifier = null)
        {
            return new FoodGroupProxy
            {
                Identifier = hasIdentifier ? identifier ?? Guid.NewGuid() : (Guid?) null
            };
        }
    }
}
