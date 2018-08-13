using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using AutoFixture;

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
        [SetUp]
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
                    Assert.That(householdData.Description, Is.Not.Null);
                    Assert.That(householdData.Description, Is.Not.Empty);
                    Assert.That(householdData.Description, Is.EqualTo(householdAddCommand.Description));
                    Assert.That(householdData.CreationTime, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    Assert.That(householdData.HouseholdMembers, Is.Not.Null);
                    Assert.That(householdData.HouseholdMembers, Is.Not.Empty);
                    Assert.That(householdData.HouseholdMembers.Count(), Is.EqualTo(1));
                    Assert.That(householdData.HouseholdMembers.SingleOrDefault(householdMember => string.Compare(householdMember.MailAddress, currentMailAddress, StringComparison.Ordinal) == 0), Is.Not.Null);

                    var householdUpdateCommand = new HouseholdUpdateCommand
                    {
                        HouseholdIdentifier = householdIdentifier,
                        Name = fixture.Create<string>(),
                        Description = fixture.Create<string>()
                    };
                    var householdUpdate = _householdDataService.HouseholdUpdate(householdUpdateCommand);
                    Assert.That(householdUpdate, Is.Not.Null);
                    Assert.That(householdUpdate.Identifier, Is.Not.Null);
                    Assert.That(householdUpdate.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdUpdate.Identifier.Value, Is.EqualTo(householdIdentifier));
                    // ReSharper restore PossibleInvalidOperationException

                    householdData = _householdDataService.HouseholdDataGet(householdDataGetQuery);
                    Assert.That(householdData, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
                    Assert.That(householdData.Name, Is.Not.Null);
                    Assert.That(householdData.Name, Is.Not.Empty);
                    Assert.That(householdData.Name, Is.EqualTo(householdUpdateCommand.Name));
                    Assert.That(householdData.Description, Is.Not.Null);
                    Assert.That(householdData.Description, Is.Not.Empty);
                    Assert.That(householdData.Description, Is.EqualTo(householdUpdateCommand.Description));

                    householdUpdateCommand = new HouseholdUpdateCommand
                    {
                        HouseholdIdentifier = householdIdentifier,
                        Name = fixture.Create<string>(),
                        Description = null
                    };
                    householdUpdate = _householdDataService.HouseholdUpdate(householdUpdateCommand);
                    Assert.That(householdUpdate, Is.Not.Null);
                    Assert.That(householdUpdate.Identifier, Is.Not.Null);
                    Assert.That(householdUpdate.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdUpdate.Identifier.Value, Is.EqualTo(householdIdentifier));
                    // ReSharper restore PossibleInvalidOperationException

                    householdData = _householdDataService.HouseholdDataGet(householdDataGetQuery);
                    Assert.That(householdData, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
                    Assert.That(householdData.Name, Is.Not.Null);
                    Assert.That(householdData.Name, Is.Not.Empty);
                    Assert.That(householdData.Name, Is.EqualTo(householdUpdateCommand.Name));
                    Assert.That(householdData.Description, Is.Null);

                    var secondaryMailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower());
                    var householdAddHouseholdMemberCommand = new HouseholdAddHouseholdMemberCommand
                    {
                        HouseholdIdentifier = householdIdentifier,
                        MailAddress = secondaryMailAddress,
                        TranslationInfoIdentifier = translationInfoIdentifier
                    };
                    var householdAddHouseholdMember = _householdDataService.HouseholdAddHouseholdMember(householdAddHouseholdMemberCommand);
                    Assert.That(householdAddHouseholdMember.Identifier, Is.Not.Null);
                    Assert.That(householdAddHouseholdMember.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdAddHouseholdMember.Identifier.Value, Is.EqualTo(householdIdentifier));
                    // ReSharper restore PossibleInvalidOperationException

                    householdData = _householdDataService.HouseholdDataGet(householdDataGetQuery);
                    Assert.That(householdData, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
                    Assert.That(householdData.HouseholdMembers, Is.Not.Null);
                    Assert.That(householdData.HouseholdMembers, Is.Not.Empty);
                    Assert.That(householdData.HouseholdMembers.Count(), Is.EqualTo(2));
                    Assert.That(householdData.HouseholdMembers.SingleOrDefault(householdMember => string.Compare(householdMember.MailAddress, secondaryMailAddress, StringComparison.Ordinal) == 0), Is.Not.Null);

                    var householdRemoveHouseholdMemberCommand = new HouseholdRemoveHouseholdMemberCommand
                    {
                        HouseholdIdentifier = householdIdentifier,
                        MailAddress = secondaryMailAddress
                    };
                    var householdRemoveHouseholdMember = _householdDataService.HouseholdRemoveHouseholdMember(householdRemoveHouseholdMemberCommand);
                    Assert.That(householdRemoveHouseholdMember.Identifier, Is.Not.Null);
                    Assert.That(householdRemoveHouseholdMember.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdRemoveHouseholdMember.Identifier.Value, Is.EqualTo(householdIdentifier));
                    // ReSharper restore PossibleInvalidOperationException

                    householdData = _householdDataService.HouseholdDataGet(householdDataGetQuery);
                    Assert.That(householdData, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.Not.Null);
                    Assert.That(householdData.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
                    Assert.That(householdData.HouseholdMembers, Is.Not.Null);
                    Assert.That(householdData.HouseholdMembers, Is.Not.Empty);
                    Assert.That(householdData.HouseholdMembers.Count(), Is.EqualTo(1));
                    Assert.That(householdData.HouseholdMembers.SingleOrDefault(householdMember => string.Compare(householdMember.MailAddress, secondaryMailAddress, StringComparison.Ordinal) == 0), Is.Null);
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
