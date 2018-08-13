using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Repositories.FoodWaste
{
    /// <summary>
    /// Integration tests for the repository which can access household data for the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class HouseholdDataRepositoryTests
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepository;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _householdDataRepository = container.Resolve<IHouseholdDataRepository>();
        }

        /// <summary>
        /// Tests that Get gets a given household.
        /// </summary>
        [Test]
        public void TestThatGetGetsHousehold()
        {
            var fixture = new Fixture();
            var household = _householdDataRepository.Insert(new Household(fixture.Create<string>()));
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                var result = _householdDataRepository.Get<IHousehold>(household.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(result.Identifier.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(result.Identifier.Value, Is.EqualTo(household.Identifier.Value));
            }
            finally
            {
                _householdDataRepository.Delete(household);
            }
        }

        /// <summary>
        /// Tests that Update updates a given household.
        /// </summary>
        [Test]
        public void TestThatUpdateUpdatesHousehold()
        {
            var fixture = new Fixture();
            var household = _householdDataRepository.Insert(new Household(fixture.Create<string>()));
            try
            {
                Assert.That(household.Description, Is.Null);

                household.Description = fixture.Create<string>();
                _householdDataRepository.Update(household);

                // ReSharper disable PossibleInvalidOperationException
                var result = _householdDataRepository.Get<IHousehold>(household.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Description, Is.Not.Null);
                Assert.That(result.Description, Is.Not.Empty);
                Assert.That(result.Description, Is.EqualTo(household.Description));
            }
            finally
            {
                _householdDataRepository.Delete(household);
            }
        }

        /// <summary>
        /// Tests that Get gets a given storage.
        /// </summary>
        [Test]
        public void TestThatGetGetsStorage()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IStorageType storageType = _householdDataRepository.Get<IStorageType>(StorageType.IdentifierForRefrigerator);
            Assert.That(storageType, Is.Not.Null);

            IHousehold household = _householdDataRepository.Insert(new Household(fixture.Create<string>()));
            try
            {
                IStorage storage = _householdDataRepository.Insert(new Storage(household, random.Next(1, 100), storageType, random.Next(storageType.TemperatureRange.StartValue, storageType.TemperatureRange.EndValue), DateTime.Now));
                try
                {
                    // ReSharper disable PossibleInvalidOperationException
                    IStorage result = _householdDataRepository.Get<IStorage>(storage.Identifier.Value);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.Identifier, Is.Not.Null);
                    // ReSharper disable ConditionIsAlwaysTrueOrFalse
                    Assert.That(result.Identifier.HasValue, Is.True);
                    // ReSharper restore ConditionIsAlwaysTrueOrFalse
                    Assert.That(result.Identifier.Value, Is.EqualTo(storage.Identifier.Value));
                }
                finally
                {
                    _householdDataRepository.Delete(storage);
                }
            }
            finally
            {
                _householdDataRepository.Delete(household);
            }
        }

        /// <summary>
        /// Tests that Get updates a given storage.
        /// </summary>
        [Test]
        public void TestThatGetUpdatesStorage()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            IStorageType storageType = _householdDataRepository.Get<IStorageType>(StorageType.IdentifierForRefrigerator);
            Assert.That(storageType, Is.Not.Null);

            IHousehold household = _householdDataRepository.Insert(new Household(fixture.Create<string>()));
            try
            {
                IStorage storage = _householdDataRepository.Insert(new Storage(household, random.Next(1, 100), storageType, random.Next(storageType.TemperatureRange.StartValue, storageType.TemperatureRange.EndValue), DateTime.Now));
                try
                {
                    Assert.That(storage.Description, Is.Null);

                    storage.Description = fixture.Create<string>();
                    _householdDataRepository.Update(storage);

                    // ReSharper disable PossibleInvalidOperationException
                    IStorage result = _householdDataRepository.Get<IStorage>(storage.Identifier.Value);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result.Description, Is.Not.Null);
                    Assert.That(result.Description, Is.Not.Empty);
                    Assert.That(result.Description, Is.EqualTo(storage.Description));
                }
                finally
                {
                    _householdDataRepository.Delete(storage);
                }
            }
            finally
            {
                _householdDataRepository.Delete(household);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress returns the household member when the mail address does exist.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsHouseholdMemberWhenMailAddressDoesExist()
        {
            var mailAddress = $"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk";
            var householdMember = _householdDataRepository.Insert(new HouseholdMember(mailAddress));
            try
            {
                var result = _householdDataRepository.HouseholdMemberGetByMailAddress(mailAddress);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.MailAddress, Is.Not.Null);
                Assert.That(result.MailAddress, Is.Not.Empty);
                Assert.That(result.MailAddress, Is.EqualTo(mailAddress));
            }
            finally
            {
                _householdDataRepository.Delete(householdMember);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberGetByMailAddress returns null when the mail address does not exist.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsNullWhenMailAddressDoesNotExist()
        {
            var mailAddress = $"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk";
            var result = _householdDataRepository.HouseholdMemberGetByMailAddress(mailAddress);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that Get gets a given household member.
        /// </summary>
        [Test]
        public void TestThatGetGetsHouseholdMember()
        {
            var householdMember = _householdDataRepository.Insert(new HouseholdMember($"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk"));
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                var result = _householdDataRepository.Get<IHouseholdMember>(householdMember.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(result.Identifier.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(result.Identifier.Value, Is.EqualTo(householdMember.Identifier.Value));
            }
            finally
            {
                _householdDataRepository.Delete(householdMember);
            }
        }

        /// <summary>
        /// Tests that Update updates a given household member.
        /// </summary>
        [Test]
        public void TestThatUpdateUpdatesHouseholdMember()
        {
            var householdMember = _householdDataRepository.Insert(new HouseholdMember($"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk"));
            try
            {
                householdMember.ActivationTime = DateTime.Now;
                _householdDataRepository.Update(householdMember);

                // ReSharper disable PossibleInvalidOperationException
                var result = _householdDataRepository.Get<IHouseholdMember>(householdMember.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ActivationTime, Is.Not.Null);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(result.ActivationTime.HasValue, Is.True);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
                Assert.That(result.ActivationTime.Value, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            }
            finally
            {
                _householdDataRepository.Delete(householdMember);
            }
        }
    }
}
