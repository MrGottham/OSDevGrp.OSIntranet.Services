using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Tests the data proxy to a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberProxyTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        /// <summary>
        /// Setup each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        /// <summary>
        /// Tests that the constructor initialize a data proxy to a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberProxy()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);
            Assert.That(sut.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(sut.MailAddress, Is.Null);
            Assert.That(sut.Membership, Is.EqualTo(Membership.Basic));
            Assert.That(sut.MembershipExpireTime, Is.Null);
            Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            Assert.That(sut.MembershipHasExpired, Is.True);
            Assert.That(sut.CanRenewMembership, Is.False);
            Assert.That(sut.CanUpgradeMembership, Is.True);
            Assert.That(sut.ActivationCode, Is.Null);
            Assert.That(sut.ActivationTime, Is.Null);
            Assert.That(sut.ActivationTime.HasValue, Is.False);
            Assert.That(sut.IsActivated, Is.False);
            Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(sut.IsPrivacyPolicyAccepted, Is.False);
            Assert.That(sut.HasReachedHouseholdLimit, Is.False);
            Assert.That(sut.CanCreateStorage, Is.False);
            Assert.That(sut.CanUpdateStorage, Is.True);
            Assert.That(sut.CanDeleteStorage, Is.False);
            Assert.That(sut.CreationTime, Is.EqualTo(DateTime.MinValue));
            Assert.That(sut.UpgradeableMemberships, Is.Not.Null);
            Assert.That(sut.UpgradeableMemberships, Is.Not.Empty);
            Assert.That(sut.UpgradeableMemberships.Count(), Is.EqualTo(2));
            Assert.That(sut.UpgradeableMemberships.Contains(Membership.Basic), Is.False);
            Assert.That(sut.UpgradeableMemberships.Contains(Membership.Deluxe), Is.True);
            Assert.That(sut.UpgradeableMemberships.Contains(Membership.Premium), Is.True);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);
            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Empty);
        }

        /// <summary>
        /// Test that Households maps households into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatHouseholdsMapsHouseholdsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdMemberIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(householdMemberIdentifier);

            IHouseholdProxy[] householdProxyCollection = _fixture.Build<HouseholdProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(1, 7))
                .Cast<IHouseholdProxy>()
                .ToArray();
            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdProxyCollection).ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households, Is.EqualTo(householdProxyCollection.OrderByDescending(m => m.CreationTime).Take(1).ToList()));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Test that Households maps households into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatHouseholdsMapsHouseholdsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdMemberIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(householdMemberIdentifier);

            IHouseholdProxy[] householdProxyCollection = _fixture.Build<HouseholdProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(1, 7))
                .Cast<IHouseholdProxy>()
                .ToArray();
            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdProxyCollection).ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            IHouseholdMemberProxy result = sut.Create(dataReader, dataProvider, "HouseholdMemberIdentifier", "MailAddress", "Membership", "MembershipExpireTime", "ActivationCode", "ActivationTime", "PrivacyPolicyAcceptedTime", "CreationTime");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Households, Is.Not.Null);
            Assert.That(result.Households, Is.Not.Empty);
            Assert.That(result.Households, Is.EqualTo(householdProxyCollection.OrderByDescending(m => m.CreationTime).Take(1).ToList()));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Test that Payments maps payment into the proxy when Create has been called and MapData has not been called.
        /// </summary>
        [Test]
        public void TestThatPaymentsMapsPaymentsIntoProxyWhenMapDataHasBeenCalledAndMapRelationsHasNotBeenCalled()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdMemberIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(householdMemberIdentifier);

            IEnumerable<PaymentProxy> paymentProxyCollection = BuildPaymentProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(paymentProxyCollection: paymentProxyCollection);

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Payments, Is.Not.Null);
            Assert.That(sut.Payments, Is.Not.Empty);
            Assert.That(sut.Payments, Is.EqualTo(paymentProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT p.PaymentIdentifier,p.StakeholderIdentifier,p.StakeholderType,p.DataProviderIdentifier,p.PaymentTime,p.PaymentReference,p.PaymentReceipt,p.CreationTime,hm.MailAddress AS HouseholdMemberMailAddress,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.CreationTime AS HouseholdMemberCreationTime,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,dp.Name AS DataProviderName,dp.HandlesPayments AS DataProviderHandlesPayments,dp.DataSourceStatementIdentifier AS DataProviderDataSourceStatementIdentifier FROM Payments AS p LEFT JOIN HouseholdMembers AS hm ON hm.HouseholdMemberIdentifier=p.StakeholderIdentifier INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=p.DataProviderIdentifier WHERE p.StakeholderIdentifier=@stakeholderIdentifier")
                .AddCharDataParameter("@stakeholderIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<PaymentProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Test that Payments maps payment into the proxy when MapData has been called and MapRelations has not been called.
        /// </summary>
        [Test]
        public void TestThatPaymentsMapsPaymentsIntoProxyWhenCreateHasBeenCalledAndMapDataHasNotBeenCalled()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdMemberIdentifier = Guid.NewGuid();
            MySqlDataReader dataReader = CreateMySqlDataReader(householdMemberIdentifier);

            IEnumerable<PaymentProxy> paymentProxyCollection = BuildPaymentProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(paymentProxyCollection: paymentProxyCollection);

            IHouseholdMemberProxy result = sut.Create(dataReader, dataProvider, "HouseholdMemberIdentifier", "MailAddress", "Membership", "MembershipExpireTime", "ActivationCode", "ActivationTime", "PrivacyPolicyAcceptedTime", "CreationTime");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Payments, Is.Not.Null);
            Assert.That(result.Payments, Is.Not.Empty);
            Assert.That(result.Payments, Is.EqualTo(paymentProxyCollection));

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT p.PaymentIdentifier,p.StakeholderIdentifier,p.StakeholderType,p.DataProviderIdentifier,p.PaymentTime,p.PaymentReference,p.PaymentReceipt,p.CreationTime,hm.MailAddress AS HouseholdMemberMailAddress,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.CreationTime AS HouseholdMemberCreationTime,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,dp.Name AS DataProviderName,dp.HandlesPayments AS DataProviderHandlesPayments,dp.DataSourceStatementIdentifier AS DataProviderDataSourceStatementIdentifier FROM Payments AS p LEFT JOIN HouseholdMembers AS hm ON hm.HouseholdMemberIdentifier=p.StakeholderIdentifier INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=p.DataProviderIdentifier WHERE p.StakeholderIdentifier=@stakeholderIdentifier")
                .AddCharDataParameter("@stakeholderIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<PaymentProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that getter for UniqueId throws an IntranetRepositoryException when the household member has no identifier.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterThrowsIntranetRepositoryExceptionWhenHouseholdMemberProxyHasNoIdentifier()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            // ReSharper disable UnusedVariable
            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => {var uniqueId = sut.UniqueId;});
            // ReSharper restore UnusedVariable

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that getter for UniqueId gets the unique identifier for the household member.
        /// </summary>
        [Test]
        public void TestThatUniqueIdGetterGetsUniqueIdentificationForHouseholdMemberProxy()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
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
            IHouseholdMemberProxy sut = CreateSut();
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
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapData(CreateMySqlDataReader(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapData maps data into the proxy.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatMapDataMapsDataIntoProxy(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdMemberIdentifier = Guid.NewGuid();
            // ReSharper disable StringLiteralTypo
            string mailAddress = $"test.{householdMemberIdentifier.ToString("D").ToLower()}@osdevgrp.dk";
            // ReSharper restore StringLiteralTypo
            DateTime? membershipExpireTime = hasMembershipExpireTime ? DateTime.Now.AddYears(1) : (DateTime?) null;
            string activationCode = _fixture.Create<string>();
            DateTime? activationTime = hasActivationTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime? privacyPolicyAcceptedTime = hasPrivacyPolicyAcceptedTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime creationTime = DateTime.Now.AddDays(_random.Next(14, 31) * -1).AddMinutes(_random.Next(-120, 120));
            MySqlDataReader dataReader = CreateMySqlDataReader(householdMemberIdentifier, mailAddress, membership, membershipExpireTime, activationCode, activationTime, privacyPolicyAcceptedTime, creationTime);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapData(dataReader, dataProvider);

            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Identifier, Is.EqualTo(householdMemberIdentifier));
            Assert.That(sut.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(sut.MailAddress, Is.Not.Null);
            Assert.That(sut.MailAddress, Is.Not.Empty);
            Assert.That(sut.MailAddress, Is.EqualTo(mailAddress));
            Assert.That(sut.Membership, Is.EqualTo(membership));
            if (membershipExpireTime.HasValue)
            {
                Assert.That(sut.MembershipExpireTime, Is.Not.Null);
                Assert.That(sut.MembershipExpireTime, Is.EqualTo(membershipExpireTime.Value).Within(1).Milliseconds);
            }
            else
            {
                Assert.That(sut.MembershipExpireTime, Is.Null);
                Assert.That(sut.MembershipExpireTime.HasValue, Is.False);
            }
            Assert.That(sut.ActivationCode, Is.Not.Null);
            Assert.That(sut.ActivationCode, Is.Not.Empty);
            Assert.That(sut.ActivationCode, Is.EqualTo(activationCode));
            if (activationTime.HasValue)
            {
                Assert.That(sut.ActivationTime, Is.Not.Null);
                Assert.That(sut.ActivationTime, Is.EqualTo(activationTime.Value).Within(1).Milliseconds);
            }
            else
            {
                Assert.That(sut.ActivationTime, Is.Null);
                Assert.That(sut.ActivationTime.HasValue, Is.False);
            }
            if (privacyPolicyAcceptedTime.HasValue)
            {
                Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(sut.PrivacyPolicyAcceptedTime, Is.EqualTo(privacyPolicyAcceptedTime.Value).Within(1).Milliseconds);
            }
            else
            {
                Assert.That(sut.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(sut.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
            Assert.That(sut.CreationTime, Is.EqualTo(creationTime).Within(1).Milliseconds);

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("MailAddress")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("Membership")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("MembershipExpireTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(3)), opt => opt.Repeat.Once());
            if (membershipExpireTime.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(3)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(3)));
            }
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ActivationCode")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("ActivationTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(5)), opt => opt.Repeat.Once());
            if (activationTime.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(5)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(5)));
            }
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("PrivacyPolicyAcceptedTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            if (privacyPolicyAcceptedTime.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(6)));
            }
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that MapRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatMapRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.MapRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that MapRelations does not clone the data provider.
        /// </summary>
        [Test]
        public void TestThatMapRelationsDoesNotCloneDataProvider()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            sut.MapRelations(dataProvider);

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Tests that SaveRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.SaveRelations(null, _fixture.Create<bool>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when the identifier for the household member is null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(CreateFoodWasteDataProvider(), _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations throws an IntranetRepositoryException when one of the households has an identifier equal to null.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsThrowsIntranetRepositoryExceptionWhenOneHouseholdIdentifierIsNull()
        {
            Guid householdMemberIdentifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(householdMemberIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            HouseholdProxy householdProxy = BuildHouseholdProxy(false);

            sut.HouseholdAdd(householdProxy);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Contains(householdProxy), Is.True);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.SaveRelations(dataProvider, _fixture.Create<bool>()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, householdProxy.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that SaveRelations inserts the missing bindings between the given household member and their households.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsInsertsMissingMemberOfHouseholds()
        {
            Guid householdMemberIdentifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(householdMemberIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            Guid householdIdentifier = Guid.NewGuid();
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);

            sut.HouseholdAdd(householdProxy);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Contains(householdProxy), Is.True);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Matches(proxy =>
                    proxy != null && proxy.Identifier != null &&
                    proxy.HouseholdMember != null && proxy.HouseholdMember == sut &&
                    proxy.HouseholdMemberIdentifier != null && proxy.HouseholdMemberIdentifier == householdMemberIdentifier &&
                    proxy.Household != null && proxy.Household == householdProxy &&
                    proxy.HouseholdIdentifier != null && proxy.HouseholdIdentifier == householdIdentifier &&
                    proxy.CreationTime >= DateTime.Now.AddSeconds(-3) && proxy.CreationTime <= DateTime.Now.AddSeconds(3)))
                , opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations does not inserts the existing bindings between the given household member and their households.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDoesNotInsertsExistingMemberOfHouseholds()
        {
            Guid householdMemberIdentifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(householdMemberIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            Guid householdIdentifier = Guid.NewGuid();
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);

            sut.HouseholdAdd(householdProxy);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Contains(householdProxy), Is.True);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Once());

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.Anything));
            dataProvider.AssertWasNotCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.Anything));
        }

        /// <summary>
        /// Tests that SaveRelations deletes the removed bindings between the given household member and their households.
        /// </summary>
        [Test]
        public void TestThatSaveRelationsDeletesRemovedMemberOfHouseholds()
        {
            Guid householdMemberIdentifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(householdMemberIdentifier);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            Guid householdIdentifier = Guid.NewGuid();
            HouseholdProxy householdProxy = BuildHouseholdProxy(identifier: householdIdentifier);

            sut.HouseholdAdd(householdProxy);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Not.Empty);
            Assert.That(sut.Households.Contains(householdProxy), Is.True);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdProxy);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            sut.HouseholdRemove(householdProxy);
            Assert.That(sut.Households, Is.Not.Null);
            Assert.That(sut.Households, Is.Empty);

            sut.SaveRelations(dataProvider, _fixture.Create<bool>());

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(5));

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdMemberIdentifier", householdMemberIdentifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Times(3));

            dataProvider.AssertWasNotCalled(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.Anything));

            dataProvider.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Matches(proxy =>
                    proxy != null && proxy.Identifier != null &&
                    proxy.HouseholdMember != null && proxy.HouseholdMember == sut &&
                    proxy.HouseholdMemberIdentifier != null && proxy.HouseholdMemberIdentifier == householdMemberIdentifier &&
                    proxy.Household != null && proxy.Household == householdProxy &&
                    proxy.HouseholdIdentifier != null && proxy.HouseholdIdentifier == householdIdentifier &&
                    proxy.CreationTime >= DateTime.Now.AddSeconds(-3) && proxy.CreationTime <= DateTime.Now.AddSeconds(3))),
                opt => opt.Repeat.Once());

            dataProvider.AssertWasCalled(m => m.Delete(Arg<IHouseholdProxy>.Is.Equal(householdProxy)), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DeleteRelations throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.DeleteRelations(null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that DeleteRelations throws an IntranetRepositoryException when the identifier for the household member is null.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsThrowsIntranetRepositoryExceptionWhenIdentifierIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Identifier, Is.Null);
            Assert.That(sut.Identifier.HasValue, Is.False);

            IntranetRepositoryException result = Assert.Throws<IntranetRepositoryException>(() => sut.DeleteRelations(CreateFoodWasteDataProvider()));

            TestHelper.AssertIntranetRepositoryExceptionIsValid(result, ExceptionMessage.IllegalValue, sut.Identifier, "Identifier");
        }

        /// <summary>
        /// Tests that DeleteRelations calls Clone on the data provider two time.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsCloneOnDataProviderTwoTime()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut);
            IEnumerable<PaymentProxy> paymentProxyCollection = new List<PaymentProxy>(0);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection, paymentProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Clone(), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the bindings which bind a given household member to all the households on which there is a membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetMemberOfHouseholdProxies()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut);
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.DeleteRelations(dataProvider);

            // ReSharper disable StringLiteralTypo
            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddCharDataParameter("@householdMemberIdentifier", identifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each binding which bind a given household member to all the households on which there is a membership.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachMemberOfHouseholdProxy()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IHouseholdProxy[] householdProxyCollection = _fixture.Build<HouseholdProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(1, 7))
                .Cast<IHouseholdProxy>()
                .ToArray();
            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = BuildMemberOfHouseholdProxyCollection(sut, householdProxyCollection).ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(memberOfHouseholdProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<MemberOfHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(memberOfHouseholdProxyCollection.Length));
            dataProvider.AssertWasCalled(m => m.Delete(Arg<IHouseholdProxy>.Is.NotNull), opt => opt.Repeat.Times(householdProxyCollection.Length));
        }

        /// <summary>
        /// Tests that DeleteRelations calls GetCollection on the data provider to get the payments made by the household member.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsGetCollectionOnDataProviderToGetPayments()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            IEnumerable<PaymentProxy> paymentProxyCollection = BuildPaymentProxyCollection();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(paymentProxyCollection: paymentProxyCollection);

            sut.DeleteRelations(dataProvider);

            IDbCommandTestExecutor commandTester = new DbCommandTestBuilder("SELECT p.PaymentIdentifier,p.StakeholderIdentifier,p.StakeholderType,p.DataProviderIdentifier,p.PaymentTime,p.PaymentReference,p.PaymentReceipt,p.CreationTime,hm.MailAddress AS HouseholdMemberMailAddress,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.CreationTime AS HouseholdMemberCreationTime,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,dp.Name AS DataProviderName,dp.HandlesPayments AS DataProviderHandlesPayments,dp.DataSourceStatementIdentifier AS DataProviderDataSourceStatementIdentifier FROM Payments AS p LEFT JOIN HouseholdMembers AS hm ON hm.HouseholdMemberIdentifier=p.StakeholderIdentifier INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=p.DataProviderIdentifier WHERE p.StakeholderIdentifier=@stakeholderIdentifier")
                .AddCharDataParameter("@stakeholderIdentifier", identifier)
                .Build();
            dataProvider.AssertWasCalled(m => m.GetCollection<PaymentProxy>(Arg<MySqlCommand>.Matches(cmd => commandTester.Run(cmd))), opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that DeleteRelations calls Delete on the data provider for each payment made by the household member.
        /// </summary>
        [Test]
        public void TestThatDeleteRelationsCallsDeleteOnDataProviderForEachPayment()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            PaymentProxy[] paymentProxyCollection = BuildPaymentProxyCollection().ToArray();
            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider(paymentProxyCollection: paymentProxyCollection);

            sut.DeleteRelations(dataProvider);

            dataProvider.AssertWasCalled(m => m.Delete(Arg<PaymentProxy>.Is.NotNull), opt => opt.Repeat.Times(paymentProxyCollection.Length));
        }

        /// <summary>
        /// Tests that CreateGetCommand returns the SQL command for selecting the given household member.
        /// </summary>
        [Test]
        public void TestThatCreateGetCommandReturnsSqlCommand()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("SELECT HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime FROM HouseholdMembers WHERE HouseholdMemberIdentifier=@householdMemberIdentifier")
                .AddCharDataParameter("@householdMemberIdentifier", identifier)
                .Build()
                .Run(sut.CreateGetCommand());
        }

        /// <summary>
        /// Tests that CreateInsertCommand returns the SQL command to insert a household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatCreateInsertCommandReturnsSqlCommandForInsert(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            Guid identifier = Guid.NewGuid();
            // ReSharper disable StringLiteralTypo
            string mailAddress = $"test.{identifier.ToString("D").ToLower()}@osdevgrp.dk";
            // ReSharper restore StringLiteralTypo
            DateTime? membershipExpireTime = hasMembershipExpireTime ? DateTime.Now.AddYears(1) : (DateTime?) null;
            string activationCode = _fixture.Create<string>();
            DateTime? activationTime = hasActivationTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime? privacyPolicyAcceptedTime = hasPrivacyPolicyAcceptedTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime creationTime = DateTime.Now.AddDays(_random.Next(14, 31) * -1).AddMinutes(_random.Next(-120, 120));
            IHouseholdMemberProxy sut = CreateSut(identifier, mailAddress, membership, membershipExpireTime, activationCode, activationTime, privacyPolicyAcceptedTime, creationTime);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("INSERT INTO HouseholdMembers (HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime) VALUES(@householdMemberIdentifier,@mailAddress,@membership,@membershipExpireTime,@activationCode,@activationTime,@privacyPolicyAcceptedTime,@creationTime)")
                .AddCharDataParameter("@householdMemberIdentifier", identifier)
                .AddVarCharDataParameter("@mailAddress", mailAddress, 128)
                .AddTinyIntDataParameter("@membership", (int) membership, 4)
                .AddDateTimeDataParameter("@membershipExpireTime", membershipExpireTime?.ToUniversalTime(), true)
                .AddVarCharDataParameter("@activationCode", activationCode, 64)
                .AddDateTimeDataParameter("@activationTime", activationTime?.ToUniversalTime(), true)
                .AddDateTimeDataParameter("@privacyPolicyAcceptedTime", privacyPolicyAcceptedTime?.ToUniversalTime(), true)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateInsertCommand());
        }

        /// <summary>
        /// Tests that CreateUpdateCommand returns the SQL command to update a household member.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatCreateUpdateCommandReturnsSqlCommandForUpdate(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            Guid identifier = Guid.NewGuid();
            // ReSharper disable StringLiteralTypo
            string mailAddress = $"test.{identifier.ToString("D").ToLower()}@osdevgrp.dk";
            // ReSharper restore StringLiteralTypo
            DateTime? membershipExpireTime = hasMembershipExpireTime ? DateTime.Now.AddYears(1) : (DateTime?) null;
            string activationCode = _fixture.Create<string>();
            DateTime? activationTime = hasActivationTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime? privacyPolicyAcceptedTime = hasPrivacyPolicyAcceptedTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime creationTime = DateTime.Now.AddDays(_random.Next(14, 31) * -1).AddMinutes(_random.Next(-120, 120));
            IHouseholdMemberProxy sut = CreateSut(identifier, mailAddress, membership, membershipExpireTime, activationCode, activationTime, privacyPolicyAcceptedTime, creationTime);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("UPDATE HouseholdMembers SET MailAddress=@mailAddress,Membership=@membership,MembershipExpireTime=@membershipExpireTime,ActivationCode=@activationCode,ActivationTime=@activationTime,PrivacyPolicyAcceptedTime=@privacyPolicyAcceptedTime,CreationTime=@creationTime WHERE HouseholdMemberIdentifier=@householdMemberIdentifier")
                .AddCharDataParameter("@householdMemberIdentifier", identifier)
                .AddVarCharDataParameter("@mailAddress", mailAddress, 128)
                .AddTinyIntDataParameter("@membership", (int) membership, 4)
                .AddDateTimeDataParameter("@membershipExpireTime", membershipExpireTime?.ToUniversalTime(), true)
                .AddVarCharDataParameter("@activationCode", activationCode, 64)
                .AddDateTimeDataParameter("@activationTime", activationTime?.ToUniversalTime(), true)
                .AddDateTimeDataParameter("@privacyPolicyAcceptedTime", privacyPolicyAcceptedTime?.ToUniversalTime(), true)
                .AddDateTimeDataParameter("@creationTime", creationTime.ToUniversalTime())
                .Build()
                .Run(sut.CreateUpdateCommand());
        }

        /// <summary>
        /// Tests that CreateDeleteCommand returns the SQL command to delete a household member.
        /// </summary>
        [Test]
        public void TestThatCreateDeleteCommandReturnsSqlCommandForDelete()
        {
            Guid identifier = Guid.NewGuid();

            IHouseholdMemberProxy sut = CreateSut(identifier);
            Assert.That(sut, Is.Not.Null);

            new DbCommandTestBuilder("DELETE FROM HouseholdMembers WHERE HouseholdMemberIdentifier=@householdMemberIdentifier")
                .AddCharDataParameter("@householdMemberIdentifier", identifier)
                .Build()
                .Run(sut.CreateDeleteCommand());
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data reader is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataReaderIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, CreateFoodWasteDataProvider(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataReader");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the data provider is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfDataProviderIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), null, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>()));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "dataProvider");
        }

        /// <summary>
        /// Tests that Create throws an ArgumentNullException if the column name collection is null.
        /// </summary>
        [Test]
        public void TestThatCreateThrowsArgumentNullExceptionIfColumnNameCollectionIsNull()
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(CreateMySqlDataReader(), CreateFoodWasteDataProvider(), null));

            TestHelper.AssertArgumentNullExceptionIsValid(result, "columnNameCollection");
        }

        /// <summary>
        /// Tests that Create creates a data proxy to a given household member with values from the data reader.
        /// </summary>
        [Test]
        [TestCase(Membership.Basic, true, true, true)]
        [TestCase(Membership.Basic, true, true, false)]
        [TestCase(Membership.Basic, true, false, true)]
        [TestCase(Membership.Basic, true, false, false)]
        [TestCase(Membership.Basic, false, true, true)]
        [TestCase(Membership.Basic, false, true, false)]
        [TestCase(Membership.Basic, false, false, true)]
        [TestCase(Membership.Basic, false, false, false)]
        [TestCase(Membership.Deluxe, true, true, true)]
        [TestCase(Membership.Deluxe, true, true, false)]
        [TestCase(Membership.Deluxe, true, false, true)]
        [TestCase(Membership.Deluxe, true, false, false)]
        [TestCase(Membership.Premium, true, true, true)]
        [TestCase(Membership.Premium, true, true, false)]
        [TestCase(Membership.Premium, true, false, true)]
        [TestCase(Membership.Premium, true, false, false)]
        public void TestThatCreateCreatesProxy(Membership membership, bool hasMembershipExpireTime, bool hasActivationTime, bool hasPrivacyPolicyAcceptedTime)
        {
            IHouseholdMemberProxy sut = CreateSut();
            Assert.That(sut, Is.Not.Null);

            Guid householdMemberIdentifier = Guid.NewGuid();
            // ReSharper disable StringLiteralTypo
            string mailAddress = $"test.{householdMemberIdentifier.ToString("D").ToLower()}@osdevgrp.dk";
            // ReSharper restore StringLiteralTypo
            DateTime? membershipExpireTime = hasMembershipExpireTime ? DateTime.Now.AddYears(1) : (DateTime?) null;
            string activationCode = _fixture.Create<string>();
            DateTime? activationTime = hasActivationTime? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime? privacyPolicyAcceptedTime = hasPrivacyPolicyAcceptedTime ? DateTime.Now.AddDays(_random.Next(1, 7) * -1).AddMinutes(_random.Next(-120, 120)) : (DateTime?) null;
            DateTime creationTime = DateTime.Now.AddDays(_random.Next(14, 31) * -1).AddMinutes(_random.Next(-120, 120));
            MySqlDataReader dataReader = CreateMySqlDataReader(householdMemberIdentifier, mailAddress, membership, membershipExpireTime, activationCode, activationTime, privacyPolicyAcceptedTime, creationTime);

            IFoodWasteDataProvider dataProvider = CreateFoodWasteDataProvider();

            IHouseholdMemberProxy result = sut.Create(dataReader, dataProvider, "HouseholdMemberIdentifier", "MailAddress", "Membership", "MembershipExpireTime", "ActivationCode", "ActivationTime", "PrivacyPolicyAcceptedTime", "CreationTime");
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(householdMemberIdentifier));
            Assert.That(result.StakeholderType, Is.EqualTo(StakeholderType.HouseholdMember));
            Assert.That(result.MailAddress, Is.Not.Null);
            Assert.That(result.MailAddress, Is.Not.Empty);
            Assert.That(result.MailAddress, Is.EqualTo(mailAddress));
            Assert.That(result.Membership, Is.EqualTo(membership));
            if (membershipExpireTime.HasValue)
            {
                Assert.That(result.MembershipExpireTime, Is.Not.Null);
                Assert.That(result.MembershipExpireTime, Is.EqualTo(membershipExpireTime.Value).Within(1).Milliseconds);
            }
            else
            {
                Assert.That(result.MembershipExpireTime, Is.Null);
                Assert.That(result.MembershipExpireTime.HasValue, Is.False);
            }
            Assert.That(result.ActivationCode, Is.Not.Null);
            Assert.That(result.ActivationCode, Is.Not.Empty);
            Assert.That(result.ActivationCode, Is.EqualTo(activationCode));
            if (activationTime.HasValue)
            {
                Assert.That(result.ActivationTime, Is.Not.Null);
                Assert.That(result.ActivationTime, Is.EqualTo(activationTime.Value).Within(1).Milliseconds);
            }
            else
            {
                Assert.That(result.ActivationTime, Is.Null);
                Assert.That(result.ActivationTime.HasValue, Is.False);
            }
            if (privacyPolicyAcceptedTime.HasValue)
            {
                Assert.That(result.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(result.PrivacyPolicyAcceptedTime, Is.EqualTo(privacyPolicyAcceptedTime.Value).Within(1).Milliseconds);
            }
            else
            {
                Assert.That(result.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(result.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
            Assert.That(result.CreationTime, Is.EqualTo(creationTime).Within(1).Milliseconds);

            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("MailAddress")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetInt16(Arg<string>.Is.Equal("Membership")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("MembershipExpireTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(3)), opt => opt.Repeat.Once());
            if (membershipExpireTime.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(3)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(3)));
            }
            dataReader.AssertWasCalled(m => m.GetString(Arg<string>.Is.Equal("ActivationCode")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("ActivationTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(5)), opt => opt.Repeat.Once());
            if (activationTime.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(5)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(5)));
            }
            dataReader.AssertWasCalled(m => m.GetOrdinal(Arg<string>.Is.Equal("PrivacyPolicyAcceptedTime")), opt => opt.Repeat.Once());
            dataReader.AssertWasCalled(m => m.IsDBNull(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            if (privacyPolicyAcceptedTime.HasValue)
            {
                dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(6)), opt => opt.Repeat.Once());
            }
            else
            {
                dataReader.AssertWasNotCalled(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(6)));
            }
            dataReader.AssertWasCalled(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")), opt => opt.Repeat.Once());

            dataProvider.AssertWasNotCalled(m => m.Clone());
        }

        /// <summary>
        /// Creates an instance of a data proxy to a household member.
        /// </summary>
        /// <returns>Instance of a data proxy to a household member.</returns>
        private IHouseholdMemberProxy CreateSut()
        {
            return new HouseholdMemberProxy();
        }

        /// <summary>
        /// Creates an instance of a data proxy to a household member.
        /// </summary>
        /// <returns>Instance of a data proxy to a household member.</returns>
        private IHouseholdMemberProxy CreateSut(Guid identifier)
        {
            return new HouseholdMemberProxy
            {
                Identifier = identifier
            };
        }

        /// <summary>
        /// Creates an instance of a data proxy to a household member.
        /// </summary>
        /// <returns>Instance of a data proxy to a household member.</returns>
        private IHouseholdMemberProxy CreateSut(Guid identifier, string mailAddress, Membership membership, DateTime? membershipExpireTime, string activationCode, DateTime? activationTime, DateTime? privacyPolicyAcceptedTime, DateTime creationTime)
        {
            return new HouseholdMemberProxy(mailAddress, membership, membershipExpireTime, activationCode, creationTime)
            {
                Identifier = identifier,
                ActivationTime = activationTime,
                PrivacyPolicyAcceptedTime = privacyPolicyAcceptedTime,
            };
        }

        /// <summary>
        /// Creates a stub for the MySQL data reader.
        /// </summary>
        /// <returns>Stub for the MySQL data reader.</returns>
        private MySqlDataReader CreateMySqlDataReader(Guid? householdMemberIdentifier = null, string mailAddress = null, Membership? membership = null, DateTime? membershipExpireTime = null, string activationCode = null, DateTime? activationTime = null, DateTime? privacyPolicyAcceptedTime = null, DateTime? creationTime = null)
        {
            MySqlDataReader mySqlDataReaderMock = MockRepository.GenerateStub<MySqlDataReader>();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("HouseholdMemberIdentifier")))
                .Return(householdMemberIdentifier.HasValue ? householdMemberIdentifier.Value.ToString("D").ToUpper() : Guid.NewGuid().ToString("D").ToUpper())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("MailAddress")))
                // ReSharper disable StringLiteralTypo
                .Return(mailAddress ?? $"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk")
                // ReSharper restore StringLiteralTypo
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetInt16(Arg<string>.Is.Equal("Membership")))
                .Return((short) (membership ?? _fixture.Create<Membership>()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("MembershipExpireTime")))
                .Return(3)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(3)))
                .Return(membershipExpireTime.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(3)))
                .Return(new MySqlDateTime((membershipExpireTime ?? DateTime.Now).ToUniversalTime()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetString(Arg<string>.Is.Equal("ActivationCode")))
                .Return(activationCode ?? _fixture.Create<string>())
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("ActivationTime")))
                .Return(5)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(5)))
                .Return(activationTime.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(5)))
                .Return(new MySqlDateTime((activationTime ?? DateTime.Now).ToUniversalTime()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetOrdinal(Arg<string>.Is.Equal("PrivacyPolicyAcceptedTime")))
                .Return(6)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.IsDBNull(Arg<int>.Is.Equal(6)))
                .Return(privacyPolicyAcceptedTime.HasValue == false)
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<int>.Is.Equal(6)))
                .Return(new MySqlDateTime((privacyPolicyAcceptedTime ?? DateTime.Now).ToUniversalTime()))
                .Repeat.Any();
            mySqlDataReaderMock.Stub(m => m.GetMySqlDateTime(Arg<string>.Is.Equal("CreationTime")))
                .Return(new MySqlDateTime((creationTime ?? DateTime.Now).ToUniversalTime()))
                .Repeat.Any();
            return mySqlDataReaderMock;
        }

        /// <summary>
        /// Creates a mockup for the data provider which can access data in the food waste repository.
        /// </summary>
        /// <returns>Mockup for the data provider which can access data in the food waste repository.</returns>
        private IFoodWasteDataProvider CreateFoodWasteDataProvider(IEnumerable<MemberOfHouseholdProxy> memberOfHouseholdProxyCollection = null, IEnumerable<PaymentProxy> paymentProxyCollection = null)
        {
            IFoodWasteDataProvider foodWasteDataProvider = MockRepository.GenerateMock<IFoodWasteDataProvider>();
            foodWasteDataProvider.Stub(m => m.Clone())
                .Return(foodWasteDataProvider)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<MemberOfHouseholdProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(memberOfHouseholdProxyCollection ?? BuildMemberOfHouseholdProxyCollection(CreateSut(Guid.NewGuid())))
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.Add(Arg<MemberOfHouseholdProxy>.Is.TypeOf))
                .WhenCalled(e => e.ReturnValue = (MemberOfHouseholdProxy) e.Arguments.ElementAt(0))
                .Return(null)
                .Repeat.Any();
            foodWasteDataProvider.Stub(m => m.GetCollection<PaymentProxy>(Arg<MySqlCommand>.Is.Anything))
                .Return(paymentProxyCollection ?? BuildPaymentProxyCollection())
                .Repeat.Any();
            return foodWasteDataProvider;
        }

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        /// <param name="hasIdentifier">Indicates whether the data proxy has an identifier.</param>
        /// <param name="identifier">The identifier for the data proxy.</param>
        /// <returns>Data proxy to a household.</returns>
        private HouseholdProxy BuildHouseholdProxy(bool hasIdentifier = true, Guid? identifier = null)
        {
            return new HouseholdProxy
            {
                Identifier = hasIdentifier ? identifier ?? Guid.NewGuid() : (Guid?) null
            };
        }

        /// <summary>
        /// Creates a collection of data proxies which bind a given household member to some households.
        /// </summary>
        /// <returns>Collection of data proxies which bind a given household member to some households.</returns>
        private IEnumerable<MemberOfHouseholdProxy> BuildMemberOfHouseholdProxyCollection(IHouseholdMemberProxy householdMemberProxy, params IHouseholdProxy[] householdProxyCollection)
        {
            ArgumentNullGuard.NotNull(householdMemberProxy, nameof(householdMemberProxy));

            if (householdProxyCollection == null || householdProxyCollection.Any() == false)
            {
                return new List<MemberOfHouseholdProxy>(0);
            }

            return householdProxyCollection
                .Select(householdProxy => new MemberOfHouseholdProxy(householdMemberProxy, householdProxy, DateTime.Now) {Identifier = Guid.NewGuid()})
                .ToList();
        }

        /// <summary>
        /// Creates a collection of data proxies to some payments from a stakeholder.
        /// </summary>
        /// <returns>Collection of data proxies to some payments from a stakeholder.</returns>
        private IEnumerable<PaymentProxy> BuildPaymentProxyCollection()
        {
            return _fixture.Build<PaymentProxy>()
                .With(m => m.Identifier, Guid.NewGuid())
                .CreateMany(_random.Next(5, 15))
                .ToList();
        }
    }
}
