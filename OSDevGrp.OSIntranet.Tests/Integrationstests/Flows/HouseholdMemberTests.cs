using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Flows
{
    /// <summary>
    /// Integration tests which tests flows with a household member.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class HouseholdMemberTests
    {
        #region Private variables

        private ILogicExecutor _logicExecutor;
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
            _logicExecutor = container.Resolve<ILogicExecutor>();
            _householdDataRepository = container.Resolve<IHouseholdDataRepository>();
            _householdDataService = container.Resolve<IFoodWasteHouseholdDataService>();
        }

        /// <summary>
        /// Tests the flow for creation of a household member.
        /// </summary>
        [Test]
        public void TestHouseholdMemberCreationFlow()
        {
            using (var executor = new ClaimsPrincipalTestExecutor())
            {
                var translationInfoCollection = _householdDataService.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                var translationInfoIdentifier = translationInfoCollection.Take(1).First().TranslationInfoIdentifier;

                var dataProviderWhoHandlesPaymentsCollectionGetQuery = new DataProviderWhoHandlesPaymentsCollectionGetQuery
                {
                    TranslationInfoIdentifier = translationInfoIdentifier
                };
                var dataProviderWhoHandlesPaymentsCollection = _householdDataService.DataProviderWhoHandlesPaymentsCollectionGet(dataProviderWhoHandlesPaymentsCollectionGetQuery);
                Assert.That(dataProviderWhoHandlesPaymentsCollection, Is.Not.Null);
                Assert.That(dataProviderWhoHandlesPaymentsCollection, Is.Not.Empty);

                var dataProviderWhoHandlesPaymentsIdentifier = dataProviderWhoHandlesPaymentsCollection.Take(1).First().DataProviderIdentifier;

                var householdMemberIdentifier = _logicExecutor.HouseholdMemberAdd(executor.MailAddress, translationInfoIdentifier);
                try
                {
                    var householdMemberIsCreated = _householdDataService.HouseholdMemberIsCreated(new HouseholdMemberIsCreatedQuery());
                    Assert.That(householdMemberIsCreated, Is.Not.Null);
                    Assert.That(householdMemberIsCreated.Result, Is.True);

                    var householdMemberIsActivated = _householdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery());
                    Assert.That(householdMemberIsActivated, Is.Not.Null);
                    Assert.That(householdMemberIsActivated.Result, Is.False);

                    var householdMemberActivateCommand = new HouseholdMemberActivateCommand
                    {
                        ActivationCode = _householdDataRepository.Get<IHouseholdMember>(householdMemberIdentifier).ActivationCode
                    };
                    var householdMemberActivate = _householdDataService.HouseholdMemberActivate(householdMemberActivateCommand);
                    Assert.That(householdMemberActivate, Is.Not.Null);
                    Assert.That(householdMemberActivate.Identifier, Is.EqualTo(householdMemberIdentifier));

                    householdMemberIsActivated = _householdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery());
                    Assert.That(householdMemberIsActivated, Is.Not.Null);
                    Assert.That(householdMemberIsActivated.Result, Is.True);

                    var householdMemberHasAcceptedPrivacyPolicy = _householdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery());
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy, Is.Not.Null);
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy.Result, Is.False);

                    var householdMemberAcceptPrivacyPolicy = _householdDataService.HouseholdMemberAcceptPrivacyPolicy(new HouseholdMemberAcceptPrivacyPolicyCommand());
                    Assert.That(householdMemberAcceptPrivacyPolicy, Is.Not.Null);
                    Assert.That(householdMemberAcceptPrivacyPolicy.Identifier, Is.EqualTo(householdMemberIdentifier));

                    householdMemberHasAcceptedPrivacyPolicy = _householdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery());
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy, Is.Not.Null);
                    Assert.That(householdMemberHasAcceptedPrivacyPolicy.Result, Is.True);

                    var householdMemberDataGetQuery = new HouseholdMemberDataGetQuery
                    {
                        TranslationInfoIdentifier = translationInfoIdentifier
                    };
                    var householdMemberData = _householdDataService.HouseholdMemberDataGet(householdMemberDataGetQuery);
                    Assert.That(householdMemberData, Is.Not.Null);
                    Assert.That(householdMemberData.HouseholdMemberIdentifier, Is.EqualTo(householdMemberIdentifier));
                    Assert.That(householdMemberData.MailAddress, Is.Not.Null);
                    Assert.That(householdMemberData.MailAddress, Is.Not.Empty);
                    Assert.That(householdMemberData.MailAddress, Is.EqualTo(executor.MailAddress));
                    Assert.That(householdMemberData.Membership, Is.Not.Null);
                    Assert.That(householdMemberData.Membership, Is.Not.Empty);
                    Assert.That(householdMemberData.Membership, Is.EqualTo(Convert.ToString(Membership.Basic)));
                    Assert.That(householdMemberData.MembershipExpireTime, Is.Null);
                    Assert.That(householdMemberData.MembershipExpireTime.HasValue, Is.False);
                    Assert.That(householdMemberData.ActivationTime, Is.Not.Null);
                    Assert.That(householdMemberData.ActivationTime.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdMemberData.ActivationTime.Value, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(householdMemberData.IsActivated, Is.True);
                    Assert.That(householdMemberData.PrivacyPolicyAcceptedTime, Is.Not.Null);
                    Assert.That(householdMemberData.PrivacyPolicyAcceptedTime.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdMemberData.PrivacyPolicyAcceptedTime.Value, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(householdMemberData.IsPrivacyPolicyAccepted, Is.True);
                    Assert.That(householdMemberData.CreationTime, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    Assert.That(householdMemberData.Households, Is.Not.Null);
                    Assert.That(householdMemberData.Households, Is.Empty);
                    Assert.That(householdMemberData.Payments, Is.Not.Null);
                    Assert.That(householdMemberData.Payments, Is.Empty);

                    var householdMemberUpgradeMembershipToDeluxePaymentReference = Guid.NewGuid().ToString("D").ToUpper();
                    var householdMemberUpgradeMembershipToDeluxeCommand = new HouseholdMemberUpgradeMembershipCommand
                    {
                        Membership = Membership.Deluxe.ToString(),
                        DataProviderIdentifier = dataProviderWhoHandlesPaymentsIdentifier,
                        PaymentTime = DateTime.Now,
                        PaymentReference = householdMemberUpgradeMembershipToDeluxePaymentReference,
                        PaymentReceipt = null
                    };
                    var householdMemberUpgradeMembershipToDeluxe = _householdDataService.HouseholdMemberUpgradeMembership(householdMemberUpgradeMembershipToDeluxeCommand);
                    Assert.That(householdMemberUpgradeMembershipToDeluxe, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToDeluxe.Identifier, Is.EqualTo(householdMemberIdentifier));

                    householdMemberData = _householdDataService.HouseholdMemberDataGet(householdMemberDataGetQuery);
                    Assert.That(householdMemberData, Is.Not.Null);
                    Assert.That(householdMemberData.Membership, Is.Not.Null);
                    Assert.That(householdMemberData.Membership, Is.Not.Empty);
                    Assert.That(householdMemberData.Membership, Is.EqualTo(Convert.ToString(Membership.Deluxe)));
                    Assert.That(householdMemberData.MembershipExpireTime, Is.Not.Null);
                    Assert.That(householdMemberData.MembershipExpireTime.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdMemberData.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(5).Seconds);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(householdMemberData.Payments, Is.Not.Null);
                    Assert.That(householdMemberData.Payments, Is.Not.Empty);

                    var householdMemberUpgradeMembershipToDeluxePayment = householdMemberData.Payments.SingleOrDefault(m => string.Compare(m.PaymentReference, householdMemberUpgradeMembershipToDeluxePaymentReference, StringComparison.Ordinal) == 0);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.PaymentIdentifier, Is.Not.EqualTo(default(Guid)));
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.Stakeholder, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.Stakeholder.StakeholderIdentifier, Is.EqualTo(householdMemberIdentifier));
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.DataProvider, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.DataProvider.DataProviderIdentifier, Is.EqualTo(dataProviderWhoHandlesPaymentsIdentifier));
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.PaymentTime, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.PaymentReference, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.PaymentReference, Is.Not.Empty);
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.PaymentReference, Is.EqualTo(householdMemberUpgradeMembershipToDeluxePaymentReference));
                    Assert.That(householdMemberUpgradeMembershipToDeluxePayment.PaymentReceipt, Is.Null);

                    var householdMemberUpgradeMembershipToPremiumPaymentReference = Guid.NewGuid().ToString("D").ToUpper();
                    var householdMemberUpgradeMembershipToPremiumPaymentReceipt = Convert.ToBase64String(Services.TestHelper.GetTestDocument().ToArray());
                    var householdMemberUpgradeMembershipToPremiumCommand = new HouseholdMemberUpgradeMembershipCommand
                    {
                        Membership = Membership.Premium.ToString(),
                        DataProviderIdentifier = dataProviderWhoHandlesPaymentsIdentifier,
                        PaymentTime = DateTime.Now,
                        PaymentReference = householdMemberUpgradeMembershipToPremiumPaymentReference,
                        PaymentReceipt = householdMemberUpgradeMembershipToPremiumPaymentReceipt
                    };
                    var householdMemberUpgradeMembershipToPremium = _householdDataService.HouseholdMemberUpgradeMembership(householdMemberUpgradeMembershipToPremiumCommand);
                    Assert.That(householdMemberUpgradeMembershipToPremium, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToPremium.Identifier, Is.EqualTo(householdMemberIdentifier));

                    householdMemberData = _householdDataService.HouseholdMemberDataGet(householdMemberDataGetQuery);
                    Assert.That(householdMemberData, Is.Not.Null);
                    Assert.That(householdMemberData.Membership, Is.Not.Null);
                    Assert.That(householdMemberData.Membership, Is.Not.Empty);
                    Assert.That(householdMemberData.Membership, Is.EqualTo(Convert.ToString(Membership.Premium)));
                    Assert.That(householdMemberData.MembershipExpireTime, Is.Not.Null);
                    Assert.That(householdMemberData.MembershipExpireTime.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdMemberData.MembershipExpireTime.Value, Is.EqualTo(DateTime.Now.AddYears(1)).Within(5).Seconds);
                    // ReSharper restore PossibleInvalidOperationException
                    Assert.That(householdMemberData.Payments, Is.Not.Null);
                    Assert.That(householdMemberData.Payments, Is.Not.Empty);

                    var householdMemberUpgradeMembershipToPremiumPayment = householdMemberData.Payments.SingleOrDefault(m => string.Compare(m.PaymentReference, householdMemberUpgradeMembershipToPremiumPaymentReference, StringComparison.Ordinal) == 0);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentIdentifier, Is.Not.EqualTo(default(Guid)));
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.Stakeholder, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.Stakeholder.StakeholderIdentifier, Is.EqualTo(householdMemberIdentifier));
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.DataProvider, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.DataProvider.DataProviderIdentifier, Is.EqualTo(dataProviderWhoHandlesPaymentsIdentifier));
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentTime, Is.EqualTo(DateTime.Now).Within(5).Seconds);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentReference, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentReference, Is.Not.Empty);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentReference, Is.EqualTo(householdMemberUpgradeMembershipToPremiumPaymentReference));
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentReceipt, Is.Not.Null);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentReceipt, Is.Not.Empty);
                    Assert.That(householdMemberUpgradeMembershipToPremiumPayment.PaymentReceipt, Is.EqualTo(householdMemberUpgradeMembershipToPremiumPaymentReceipt));

                    var householdAddCommand = new HouseholdAddCommand
                    {
                        Name = Guid.NewGuid().ToString("N"),
                        Description = null,
                        TranslationInfoIdentifier = translationInfoIdentifier
                    };
                    var householdAddServiceReceipt1 = _householdDataService.HouseholdAdd(householdAddCommand);
                    Assert.That(householdAddServiceReceipt1, Is.Not.Null);
                    Assert.That(householdAddServiceReceipt1.Identifier, Is.Not.Null);
                    Assert.That(householdAddServiceReceipt1.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdAddServiceReceipt1.Identifier.Value, Is.Not.EqualTo(default(Guid)));
                    // ReSharper restore PossibleInvalidOperationException

                    householdMemberData = _householdDataService.HouseholdMemberDataGet(householdMemberDataGetQuery);
                    Assert.That(householdMemberData, Is.Not.Null);
                    Assert.That(householdMemberData.Households, Is.Not.Null);
                    Assert.That(householdMemberData.Households, Is.Not.Empty);
                    Assert.That(householdMemberData.Households.Count(), Is.EqualTo(1));
                    Assert.That(householdMemberData.Households.SingleOrDefault(household => household.HouseholdIdentifier == householdAddServiceReceipt1.Identifier.Value), Is.Not.Null);

                    householdAddCommand = new HouseholdAddCommand
                    {
                        Name = Guid.NewGuid().ToString("N"),
                        Description = Guid.NewGuid().ToString("N"),
                        TranslationInfoIdentifier = translationInfoIdentifier
                    };
                    var householdAddServiceReceipt2 = _householdDataService.HouseholdAdd(householdAddCommand);
                    Assert.That(householdAddServiceReceipt2, Is.Not.Null);
                    Assert.That(householdAddServiceReceipt2.Identifier, Is.Not.Null);
                    Assert.That(householdAddServiceReceipt2.Identifier.HasValue, Is.True);
                    // ReSharper disable PossibleInvalidOperationException
                    Assert.That(householdAddServiceReceipt2.Identifier.Value, Is.Not.EqualTo(default(Guid)));
                    // ReSharper restore PossibleInvalidOperationException

                    householdMemberData = _householdDataService.HouseholdMemberDataGet(householdMemberDataGetQuery);
                    Assert.That(householdMemberData, Is.Not.Null);
                    Assert.That(householdMemberData.Households, Is.Not.Null);
                    Assert.That(householdMemberData.Households, Is.Not.Empty);
                    Assert.That(householdMemberData.Households.Count(), Is.EqualTo(2));
                    Assert.That(householdMemberData.Households.SingleOrDefault(household => household.HouseholdIdentifier == householdAddServiceReceipt2.Identifier.Value), Is.Not.Null);
                }
                finally
                {
                    _householdDataRepository.Delete(_householdDataRepository.Get<IHouseholdMember>(householdMemberIdentifier));
                }
            }
        }
    }
}
