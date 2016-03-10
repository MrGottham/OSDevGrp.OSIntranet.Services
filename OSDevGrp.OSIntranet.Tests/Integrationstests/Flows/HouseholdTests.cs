using System;
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
                var householdAddServiceReceipt = _householdDataService.HouseholdAdd(householdAddCommand);
                try
                {
                    Assert.That(householdAddServiceReceipt, Is.Not.Null);
                    Assert.That(householdAddServiceReceipt.Identifier, Is.Not.Null);
                    Assert.That(householdAddServiceReceipt.Identifier.HasValue, Is.True);
                    Assert.That(householdAddServiceReceipt.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);

                    // ReSharper disable PossibleInvalidOperationException
                    var householdIdentifier = householdAddServiceReceipt.Identifier.Value;
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(householdAddServiceReceipt.Identifier.Value, Is.Not.EqualTo(default(Guid)));

                    var householdMemberIsCreatedBooleanResponse = _householdDataService.HouseholdMemberIsCreated(new HouseholdMemberIsCreatedQuery());
                    Assert.That(householdMemberIsCreatedBooleanResponse, Is.Not.Null);
                    Assert.That(householdMemberIsCreatedBooleanResponse.Result, Is.True);

                    var householdMemberIsActivatedBooleanResponse = _householdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery());
                    Assert.That(householdMemberIsActivatedBooleanResponse, Is.Not.Null);
                    Assert.That(householdMemberIsActivatedBooleanResponse.Result, Is.False);

                    var householdMemberActivateCommand = new HouseholdMemberActivateCommand
                    {
                        ActivationCode = _householdDataRepository.HouseholdMemberGetByMailAddress(executor.MailAddress).ActivationCode
                    };
                    var householdMemberActivateServiceReceipt = _householdDataService.HouseholdMemberActivate(householdMemberActivateCommand);
                    Assert.That(householdMemberActivateServiceReceipt, Is.Not.Null);
                    Assert.That(householdMemberActivateServiceReceipt.Identifier, Is.Not.Null);
                    Assert.That(householdMemberActivateServiceReceipt.Identifier.HasValue, Is.True);
                    Assert.That(householdMemberActivateServiceReceipt.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);

                    householdMemberIsActivatedBooleanResponse = _householdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery());
                    Assert.That(householdMemberIsActivatedBooleanResponse, Is.Not.Null);
                    Assert.That(householdMemberIsActivatedBooleanResponse.Result, Is.True);

                    var householdMemberHasAcceptedPrivacyPolicyBooleanResponse = _householdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery());
                    Assert.That(householdMemberHasAcceptedPrivacyPolicyBooleanResponse, Is.Not.Null);
                    Assert.That(householdMemberHasAcceptedPrivacyPolicyBooleanResponse.Result, Is.False);

                    var householdMemberAcceptPrivacyPolicyServiceReceipt = _householdDataService.HouseholdMemberAcceptPrivacyPolicy(new HouseholdMemberAcceptPrivacyPolicyCommand());
                    Assert.That(householdMemberAcceptPrivacyPolicyServiceReceipt, Is.Not.Null);
                    Assert.That(householdMemberAcceptPrivacyPolicyServiceReceipt.Identifier, Is.Not.Null);
                    Assert.That(householdMemberAcceptPrivacyPolicyServiceReceipt.Identifier.HasValue, Is.True);
                    Assert.That(householdMemberAcceptPrivacyPolicyServiceReceipt.EventDate, Is.EqualTo(DateTime.Now).Within(5).Seconds);

                    householdMemberHasAcceptedPrivacyPolicyBooleanResponse = _householdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery());
                    Assert.That(householdMemberHasAcceptedPrivacyPolicyBooleanResponse, Is.Not.Null);
                    Assert.That(householdMemberHasAcceptedPrivacyPolicyBooleanResponse.Result, Is.True);
                }
                finally
                {
                    if (householdAddServiceReceipt != null && householdAddServiceReceipt.Identifier.HasValue)
                    {
                        _householdDataRepository.Delete(_householdDataRepository.Get<IHousehold>(householdAddServiceReceipt.Identifier.Value));
                    }
                }
            }
        }
    }
}
