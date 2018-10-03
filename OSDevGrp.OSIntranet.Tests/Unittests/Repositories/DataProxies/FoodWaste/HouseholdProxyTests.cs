using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Test the data proxy to a household.
    /// </summary>
    [TestFixture]
    public class HouseholdProxyTests
    {
        /// <summary>
        /// Tests that the constructor initialize a data proxy to a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdProxy()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Name, Is.Null);
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the household has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenHouseholdProxyHasNoIdentifier()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => { sut.UniqueId.ToUpper(); });
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the household.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForHouseholdProxy()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.Value, Is.EqualTo(identifier));

            string uniqueId = sut.UniqueId;
            Assert.That(uniqueId, Is.Not.Null);
            Assert.That(uniqueId, Is.Not.Empty);
            Assert.That(uniqueId, Is.EqualTo(identifier.ToString("D").ToUpper()));
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an ArgumentNullException when the given household is null.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsArgumentNullExceptionWhenHouseholdIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => ((HouseholdProxy) sut).GetSqlQueryForId(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "household");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId throws an IntranetRepositoryException when the identifier on the given household has no value.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdThrowsIntranetRepositoryExceptionWhenIdentifierOnHouseholdHasNoValue()
        {
            IHousehold householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(null)
                .Repeat.Any();

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => ((HouseholdProxy) sut).GetSqlQueryForId(householdMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, householdMock.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that GetSqlQueryForId returns the SQL statement for selecting the given household.
        /// </summary>
        [Test]
        public void TestThatGetSqlQueryForIdReturnsSqlQueryForId()
        {
            Guid identifier = Guid.NewGuid();

            IHousehold householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            string result = ((HouseholdProxy)sut).GetSqlQueryForId(householdMock);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"SELECT HouseholdIdentifier,Name,Descr,CreationTime FROM Households WHERE HouseholdIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Tests that GetSqlCommandForInsert returns the SQL statement to insert a household.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForInsertReturnsSqlCommandForInsert(bool hasDescription)
        {
            Fixture fixture = new Fixture();
            
            Guid identifier = Guid.NewGuid();
            string name = fixture.Create<string>();
            string description = hasDescription ? fixture.Create<string>() : null;
            string descriptionAsSql = hasDescription ? $"'{description}'" : "NULL";
            DateTime creationTime = DateTime.Now;

            IHouseholdProxy sut = CreateSut(identifier, name, description, creationTime);
            Assert.That(sut, Is.Not.Null);

            string result = ((HouseholdProxy) sut).GetSqlCommandForInsert();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"INSERT INTO Households (HouseholdIdentifier,Name,Descr,CreationTime) VALUES('{identifier.ToString("D").ToUpper()}','{name}',{descriptionAsSql},{DataRepositoryHelper.GetSqlValueForDateTime(creationTime)})"));
        }

        /// <summary>
        /// Tests that GetSqlCommandForUpdate returns the SQL statement to update a household.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatGetSqlCommandForUpdateReturnsSqlCommandForUpdate(bool hasDescription)
        {
            Fixture fixture = new Fixture();

            Guid identifier = Guid.NewGuid();
            string name = fixture.Create<string>();
            string description = hasDescription ? fixture.Create<string>() : null;
            string descriptionAsSql = hasDescription ? $"'{description}'" : "NULL";
            DateTime creationTime = DateTime.Now;

            IHouseholdProxy sut = CreateSut(identifier, name, description, creationTime);
            Assert.That(sut, Is.Not.Null);

            string result = ((HouseholdProxy) sut).GetSqlCommandForUpdate();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"UPDATE Households SET Name='{name}',Descr={descriptionAsSql},CreationTime={DataRepositoryHelper.GetSqlValueForDateTime(creationTime)} WHERE HouseholdIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Tests that GetSqlCommandForDelete returns the SQL statement to delete a household.
        /// </summary>
        [Test]
        public void TestThatGetSqlCommandForDeleteReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            string result = ((HouseholdProxy) sut).GetSqlCommandForDelete();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo($"DELETE FROM Households WHERE HouseholdIdentifier='{identifier.ToString("D").ToUpper()}'"));
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(null, MockRepository.GenerateMock<IMySqlDataProvider>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that MapData throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapDataThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(MockRepository.GenerateStub<MySqlDataReader>(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatMapDataMapsDataIntoProxy(bool hasDescription)
        {
            Fixture fixture = new Fixture();

            Guid identifier = Guid.NewGuid();
            string name = fixture.Create<string>();
            string description = hasDescription ? fixture.Create<string>() : null;
            DateTime creationTime = DateTime.Now;

            MySqlDataReader mySqlDataReaderStub = CreateMySqlDataReaderStub(identifier, name, description, creationTime);

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture);

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.Name, Is.Null);
            Assert.That(sut.Description, Is.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.MapData(mySqlDataReaderStub, dataProviderMock);

            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.Value, Is.EqualTo(identifier));
            Assert.That(sut.Name, Is.Not.Null);
            Assert.That(sut.Name, Is.Not.Empty);
            Assert.That(sut.Name, Is.EqualTo(name));
            if (hasDescription)
            {
                Assert.That(sut.Description, Is.Not.Null);
                Assert.That(sut.Description, Is.Not.Empty);
                Assert.That(sut.Description, Is.EqualTo(description));
            }
            else
            {
                Assert.That(sut.Description, Is.Null);
            }
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime));
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == $"SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdIdentifier='{identifier.ToString("D").ToUpper()}' ORDER BY CreationTime DESC")));
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
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
            Fixture fixture = new Fixture();

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the household is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            Fixture fixture = new Fixture();

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(MockRepository.GenerateStub<IMySqlDataProvider>(), fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when one of the household members has an identifier equal to null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenOneHouseholdMemberIdentifierIsNull()
        {
            Fixture fixture = new Fixture();

            IHouseholdMember householdMemberMock = CreateHouseholdMemberMock();

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture);

            IHouseholdProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMemberMock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(1));
            Assert.That(sut.HouseholdMembers.Contains(householdMemberMock), Is.True);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(dataProviderMock, fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, householdMemberMock.Identifier, "Identifier");

            dataProviderMock.AssertWasNotCalled(m => m.Clone());
            dataProviderMock.AssertWasNotCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing bindings between the given household and who is member of the household.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingMemberOfHouseholds()
        {
            Fixture fixture = new Fixture();

            Guid householdMemberIdentifier = Guid.NewGuid();
            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture, new List<MemberOfHouseholdProxy>(0));

            Guid householdIdentifier = Guid.NewGuid();
            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMemberMock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(1));
            Assert.That(sut.HouseholdMembers.Contains(householdMemberMock), Is.True);

            sut.SaveRelations(dataProviderMock, fixture.Create<bool>());

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
            dataProviderMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == $"SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdIdentifier='{householdIdentifier.ToString("D").ToUpper()}' ORDER BY CreationTime DESC")), opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Matches(proxy =>
                    proxy != null &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.Identifier != null && proxy.Identifier.HasValue && proxy.Identifier.Value != default(Guid) &&
                    // ReSharper restore MergeSequentialChecks
                    proxy.HouseholdMember != null && proxy.HouseholdMember == householdMemberMock &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.HouseholdMemberIdentifier != null && proxy.HouseholdMemberIdentifier.HasValue && proxy.HouseholdMemberIdentifier.Value == householdMemberIdentifier &&
                    // ReSharper restore MergeSequentialChecks
                    proxy.Household != null && proxy.Household == sut &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.HouseholdIdentifier != null && proxy.HouseholdIdentifier.HasValue && proxy.HouseholdIdentifier.Value == householdIdentifier)),
                    // ReSharper restore MergeSequentialChecks
                opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasNotCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations does not inserts the existing bindings between the given household and who is member of the household.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDoesNotInsertsExistingMemberOfHouseholds()
        {
            Fixture fixture = new Fixture();

            Guid householdMemberIdentifier = Guid.NewGuid();
            IHouseholdMember householdMemberMock = CreateHouseholdMemberMock(householdMemberIdentifier);

            Guid householdIdentifier = Guid.NewGuid();
            MemberOfHouseholdProxy memberOfHouseholdProxy = CreateMemberOfHouseholdProxy(fixture, householdMemberIdentifier, householdIdentifier);

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture, new List<MemberOfHouseholdProxy> {memberOfHouseholdProxy});

            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMemberMock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(1));
            Assert.That(sut.HouseholdMembers.Contains(householdMemberMock), Is.True);

            sut.SaveRelations(dataProviderMock, fixture.Create<bool>());

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == $"SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdIdentifier='{householdIdentifier.ToString("D").ToUpper()}' ORDER BY CreationTime DESC")), opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasNotCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.Anything));
            dataProviderMock.AssertWasNotCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations deletes the removed bindings between the given household and who is member of the household.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDeletesRemovedMemberOfHouseholds()
        {
            Fixture fixture = new Fixture();

            Guid householdMemberIdentifier = Guid.NewGuid();
            IHouseholdMember householdMemberMock = CreateHouseholdMemberMock(householdMemberIdentifier);

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture, new List<MemberOfHouseholdProxy>(0));

            Guid householdIdentifier = Guid.NewGuid();
            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.HouseholdMemberAdd(householdMemberMock);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Not.Empty);
            Assert.That(sut.HouseholdMembers.Count(), Is.EqualTo(1));
            Assert.That(sut.HouseholdMembers.Contains(householdMemberMock), Is.True);

            sut.SaveRelations(dataProviderMock, fixture.Create<bool>());

            IHouseholdMember householdMemberToDelete = sut.HouseholdMembers.FirstOrDefault();
            Assert.That(householdMemberToDelete, Is.Not.Null);

            sut.HouseholdMemberRemove(householdMemberToDelete);
            Assert.That(sut.HouseholdMembers, Is.Not.Null);
            Assert.That(sut.HouseholdMembers, Is.Empty);

            sut.SaveRelations(dataProviderMock, fixture.Create<bool>());

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(7));
            dataProviderMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == $"SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdIdentifier='{householdIdentifier.ToString("D").ToUpper()}' ORDER BY CreationTime DESC")), opt => opt.Repeat.Times(3));
            dataProviderMock.AssertWasCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Matches(proxy =>
                    proxy != null &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.Identifier != null && proxy.Identifier.HasValue && proxy.Identifier.Value != default(Guid) &&
                    // ReSharper restore MergeSequentialChecks
                    proxy.HouseholdMember != null && proxy.HouseholdMember == householdMemberMock &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.HouseholdMemberIdentifier != null && proxy.HouseholdMemberIdentifier.HasValue && proxy.HouseholdMemberIdentifier.Value == householdMemberIdentifier &&
                    // ReSharper restore MergeSequentialChecks
                    proxy.Household != null && proxy.Household == sut &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.HouseholdIdentifier != null && proxy.HouseholdIdentifier.HasValue && proxy.HouseholdIdentifier.Value == householdIdentifier)),
                    // ReSharper restore MergeSequentialChecks
                opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasCalled(m => m.Get(Arg<HouseholdMemberProxy>.Matches(proxy =>
                    proxy != null &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.Identifier != null && proxy.Identifier.HasValue && proxy.Identifier.Value == householdMemberIdentifier)),
                    // ReSharper restore MergeSequentialChecks
                opt => opt.Repeat.Times(1));
            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<IHouseholdMemberProxy>.Matches(proxy =>
                    proxy != null &&
                    // ReSharper disable MergeSequentialChecks
                    proxy.Identifier != null && proxy.Identifier.HasValue && proxy.Identifier.Value == householdMemberIdentifier)),
                    // ReSharper restore MergeSequentialChecks
                opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the household is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            Fixture fixture = new Fixture();

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture);

            IHouseholdProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(dataProviderMock));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider one time.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProviderOneTime()
        {
            Fixture fixture = new Fixture();

            IFoodWasteDataProvider dataProviderMock = CreateDataProviderMock(fixture, new List<MemberOfHouseholdProxy>(0));

            IHouseholdProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            sut.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the bindings which bind a given household to all the household members who has membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetMemberOfHouseholdProxies()
        {
            Fixture fixture = new Fixture();

            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture, new List<MemberOfHouseholdProxy>(0));

            Guid householdIdentifier = Guid.NewGuid();
            IHouseholdProxy sut = CreateSut(householdIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            sut.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => cmd.CommandText == $"SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdIdentifier='{householdIdentifier.ToString("D").ToUpper()}' ORDER BY CreationTime DESC")));
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each binding which bind a given household to all the household members who has membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachMemberOfHouseholdProxy()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            int memberOfHouseholdCount = random.Next(7, 15);
            List<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = new List<MemberOfHouseholdProxy>(memberOfHouseholdCount);
            while (memberOfHouseholdProxyCollection.Count < memberOfHouseholdCount)
            {
                IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                householdMemberMock.Stub(m => m.Identifier)
                    .Return(Guid.NewGuid())
                    .Repeat.Any();
                IHousehold householdMock = MockRepository.GenerateMock<IHousehold>();
                householdMock.Stub(m => m.Identifier)
                    .Return(Guid.NewGuid())
                    .Repeat.Any();
                memberOfHouseholdProxyCollection.Add(new MemberOfHouseholdProxy(householdMemberMock, householdMock));
            }
            IMySqlDataProvider dataProviderMock = CreateDataProviderMock(fixture, memberOfHouseholdProxyCollection);

            IHouseholdProxy sut = CreateSut(Guid.NewGuid());
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(sut.Identifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            sut.DeleteRelations(dataProviderMock);

            dataProviderMock.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(memberOfHouseholdCount));
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given household which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given household which should be used for unit testing.</returns>
        private static IHouseholdProxy CreateSut(Guid? householdIdentifier = null)
        {
            return new HouseholdProxy
            {
                Identifier = householdIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of the data proxy to a given household which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of the data proxy to a given household which should be used for unit testing.</returns>
        private static IHouseholdProxy CreateSut(Guid householdIdentifier, string name, string description, DateTime creationTime)
        {
            return new HouseholdProxy(name, description, creationTime)
            {
                Identifier = householdIdentifier
            };
        }

        /// <summary>
        /// Creates an instance of a MySQL data reader which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a MySQL data reader which should be used for unit testing.</returns>
        private static MySqlDataReader CreateMySqlDataReaderStub(Guid householdIdentifier, string name, string description, DateTime creationTime)
        {
            MySqlDataReader mySqlDataReaderStub = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdIdentifier")))
                .Return(householdIdentifier.ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<string>.Is.Equal("Name")))
                .Return(name)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("Descr")))
                .Return(2)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(2)))
                .Return(string.IsNullOrWhiteSpace(description))
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetString(Arg<int>.Is.Equal(2)))
                .Return(description)
                .Repeat.Any();
            mySqlDataReaderStub.Stub(m => m.GetDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(creationTime.ToUniversalTime())
                .Repeat.Any();
            return mySqlDataReaderStub;
        }

        /// <summary>
        /// Creates an instance of a data provider which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a data provider which should be used for unit testing</returns>
        private static IFoodWasteDataProvider CreateDataProviderMock(Fixture fixture, List<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = null)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            if (memberOfHouseholdProxyCollection == null)
            {
                Random random = new Random(fixture.Create<int>());

                memberOfHouseholdProxyCollection = new List<MemberOfHouseholdProxy>(random.Next(7, 15));
                while (memberOfHouseholdProxyCollection.Count < memberOfHouseholdProxyCollection.Capacity)
                {
                    IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
                    householdMemberMock.Stub(m => m.Identifier)
                        .Return(Guid.NewGuid())
                        .Repeat.Any();
                    IHousehold householdMock = MockRepository.GenerateMock<IHousehold>();
                    householdMock.Stub(m => m.Identifier)
                        .Return(Guid.NewGuid())
                        .Repeat.Any();
                    memberOfHouseholdProxyCollection.Add(new MemberOfHouseholdProxy(householdMemberMock, householdMock));
                }
            }

            IFoodWasteDataProvider dataProviderMock = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            dataProviderMock.Stub(m => m.Clone())
                .Return(dataProviderMock)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(memberOfHouseholdProxyCollection)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Get(Arg<HouseholdMemberProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    HouseholdMemberProxy proxy = (HouseholdMemberProxy) e.Arguments.ElementAt(0);
                    e.ReturnValue = new HouseholdMemberProxy($"test.{Guid.NewGuid():D}@osdevgrp.dk", Membership.Basic, null, fixture.Create<string>(), DateTime.Now)
                    {
                        Identifier = proxy.Identifier
                    };
                })
                .Return(null)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    MemberOfHouseholdProxy proxy = (MemberOfHouseholdProxy) e.Arguments.ElementAt(0);
                    memberOfHouseholdProxyCollection.Add(proxy);
                    e.ReturnValue = proxy;
                })
                .Return(null)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.NotNull))
                .WhenCalled(e =>
                {
                    MemberOfHouseholdProxy proxy = (MemberOfHouseholdProxy) e.Arguments.ElementAt(0);
                    memberOfHouseholdProxyCollection.Remove(proxy);
                })
                .Repeat.Any();
            return dataProviderMock;
        }

        /// <summary>
        /// Creates a mockup for a household member which should be used for unit testing.
        /// </summary>
        /// <returns>Mockup for a household member which should be used for unit testing.</returns>
        private static IHouseholdMember CreateHouseholdMemberMock(Guid? householdMemberIdentifier = null)
        {
            IHouseholdMember householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(householdMemberIdentifier)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.Households)
                .Return(new List<IHousehold>(0))
                .Repeat.Any();
            return householdMemberMock;
        }

        /// <summary>
        /// Creates an instance of a member of household proxy which should be used for unit testing.
        /// </summary>
        /// <returns>Instance of a member of household proxy which should be used for unit testing.</returns>
        private static MemberOfHouseholdProxy CreateMemberOfHouseholdProxy(Fixture fixture, Guid householdMemberIdentifier, Guid householdIdentifier)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return fixture.Build<MemberOfHouseholdProxy>()
                .With(m => m.HouseholdMemberIdentifier, householdMemberIdentifier)
                .With(m => m.HouseholdIdentifier, householdIdentifier)
                .Create();
        }
    }
}
