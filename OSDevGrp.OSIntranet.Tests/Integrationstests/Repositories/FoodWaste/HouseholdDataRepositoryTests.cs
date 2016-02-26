using System;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Ploeh.AutoFixture;

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
        [TestFixtureSetUp]
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
                Assert.That(result.Identifier.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(result.Identifier.Value, Is.EqualTo(household.Identifier.Value));
                // ReSharper restore PossibleInvalidOperationException
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
        /// Tests that HouseholdMemberGetByMailAddress returns the household member when the mail address does exist.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberGetByMailAddressReturnsHouseholdMemberWhenMailAddressDoesExist()
        {
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower());
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
            var mailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower());
            var result = _householdDataRepository.HouseholdMemberGetByMailAddress(mailAddress);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that Get gets a given household member.
        /// </summary>
        [Test]
        public void TestThatGetGetsHouseholdMember()
        {
            var householdMember = _householdDataRepository.Insert(new HouseholdMember(string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower())));
            try
            {
                // ReSharper disable PossibleInvalidOperationException
                var result = _householdDataRepository.Get<IHouseholdMember>(householdMember.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Identifier, Is.Not.Null);
                Assert.That(result.Identifier.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(result.Identifier.Value, Is.EqualTo(householdMember.Identifier.Value));
                // ReSharper restore PossibleInvalidOperationException
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
            var householdMember = _householdDataRepository.Insert(new HouseholdMember(string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower())));
            try
            {
                householdMember.ActivationTime = DateTime.Now;
                _householdDataRepository.Update(householdMember);

                // ReSharper disable PossibleInvalidOperationException
                var result = _householdDataRepository.Get<IHouseholdMember>(householdMember.Identifier.Value);
                // ReSharper restore PossibleInvalidOperationException
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ActivationTime, Is.Not.Null);
                Assert.That(result.ActivationTime.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(result.ActivationTime.Value, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                // ReSharper restore PossibleInvalidOperationException
            }
            finally
            {
                _householdDataRepository.Delete(householdMember);
            }
        }
    }
}
