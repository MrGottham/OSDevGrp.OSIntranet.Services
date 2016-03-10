﻿using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using Ploeh.AutoFixture;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Flows
{
    /// <summary>
    /// Integration tests which tests flows with a household.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class HouseholdTests
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepository;
        private IFoodWasteHouseholdDataService _householdDataService;

        #endregion

        /// <summary>
        /// Opsætning af tests.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _householdDataRepository = container.Resolve<IHouseholdDataRepository>();
            _householdDataService = container.Resolve<IFoodWasteHouseholdDataService>();
        }

        /// <summary>
        /// Tests the flow for creation of a household.
        /// </summary>
        [Test]
        public void TestHouseholdCreationFlow()
        {
            var fixture = new Fixture();
            using (var executor = new ClaimsPrincipalTestExecutor())
            {
                var translationInfoCollection = _householdDataService.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                var translationInfoIdentifier = translationInfoCollection.Take(1).First().TranslationInfoIdentifier;

                var householdAddCommand = new HouseholdAddCommand
                {
                    Name = fixture.Create<string>(),
                    Description = fixture.Create<string>(),
                    TranslationInfoIdentifier = translationInfoIdentifier
                };
                var householdAdd = _householdDataService.HouseholdAdd(householdAddCommand);
                try
                {
                    Assert.That(householdAdd, Is.Not.Null);
                    Assert.That(householdAdd.Identifier, Is.Not.Null);
                    Assert.That(householdAdd.Identifier.HasValue, Is.True);

                    // ReSharper disable PossibleInvalidOperationException
                    var householdIdentifier = householdAdd.Identifier.Value;
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(householdIdentifier, Is.Not.EqualTo(default(Guid)));

                    var householdMemberIsCreated = _householdDataService.HouseholdMemberIsCreated(new HouseholdMemberIsCreatedQuery());
                    Assert.That(householdMemberIsCreated, Is.Not.Null);
                    Assert.That(householdMemberIsCreated.Result, Is.True);

                    var householdMemberIsActivated = _householdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery());
                    Assert.That(householdMemberIsActivated, Is.Not.Null);
                    Assert.That(householdMemberIsActivated.Result, Is.False);

                    var currentMailAddress = executor.MailAddress;
                    var householdMemberActivateCommand = new HouseholdMemberActivateCommand
                    {
                        ActivationCode = _householdDataRepository.HouseholdMemberGetByMailAddress(currentMailAddress).ActivationCode
                    };
                    var householdMemberActivate = _householdDataService.HouseholdMemberActivate(householdMemberActivateCommand);
                    Assert.That(householdMemberActivate, Is.Not.Null);
                    Assert.That(householdMemberActivate.Identifier, Is.Not.Null);
                    Assert.That(householdMemberActivate.Identifier.HasValue, Is.True);

                    householdMemberIsActivated = _householdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery());
                    Assert.That(householdMemberIsActivated, Is.Not.Null);
                    Assert.That(householdMemberIsActivated.Result, Is.True);

                    var householdMemberHasAcceptedPrivacyPolicy = _householdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery());
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy, Is.Not.Null);
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy.Result, Is.False);

                    var householdMemberAcceptPrivacyPolicy = _householdDataService.HouseholdMemberAcceptPrivacyPolicy(new HouseholdMemberAcceptPrivacyPolicyCommand());
                    Assert.That(householdMemberAcceptPrivacyPolicy, Is.Not.Null);
                    Assert.That(householdMemberAcceptPrivacyPolicy.Identifier, Is.Not.Null);
                    Assert.That(householdMemberAcceptPrivacyPolicy.Identifier.HasValue, Is.True);

                    householdMemberHasAcceptedPrivacyPolicy = _householdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery());
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy, Is.Not.Null);
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy.Result, Is.True);

                    var householdDataGetQuery = new HouseholdDataGetQuery
                    {
                        HouseholdIdentifier = householdIdentifier,
                        TranslationInfoIdentifier = translationInfoIdentifier
                    };
                    var householdData = _householdDataService.HouseholdDataGet(householdDataGetQuery);
                    Assert.That(householdData, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
                    Assert.That(householdData.Name, Is.Not.Null);
                    Assert.That(householdData.Name, Is.Not.Empty);
                    Assert.That(householdData.Name, Is.EqualTo(householdAddCommand.Name));
                    Assert.That(householdData.Name, Is.Not.Null);
                    Assert.That(householdData.Name, Is.Not.Empty);
                    Assert.That(householdData.Name, Is.EqualTo(householdAddCommand.Name));
                    Assert.That(householdData.CreationTime, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    Assert.That(householdData.HouseholdMembers, Is.Not.Null);
                    Assert.That(householdData.HouseholdMembers, Is.Not.Empty);
                    Assert.That(householdData.HouseholdMembers.Count(), Is.EqualTo(1));
                    Assert.That(householdData.HouseholdMembers.SingleOrDefault(householdMember => string.Compare(householdMember.MailAddress, currentMailAddress, StringComparison.Ordinal) == 0), Is.Not.Null);
                }
                finally
                {
                    if (householdAdd != null && householdAdd.Identifier.HasValue)
                    {
                        _householdDataRepository.Delete(_householdDataRepository.Get<IHousehold>(householdAdd.Identifier.Value));
                    }
                }
            }
        }
    }
}
