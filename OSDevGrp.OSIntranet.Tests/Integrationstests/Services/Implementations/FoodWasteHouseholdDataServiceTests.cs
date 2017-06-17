using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integration tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteHouseholdDataServiceTests
    {
        #region Private variables

        private IFoodWasteHouseholdDataService _foodWasteHouseholdDataService;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [SetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _foodWasteHouseholdDataService = container.Resolve<IFoodWasteHouseholdDataService>();
        }

        /// <summary>
        /// Tests that StorageTypeGetAll gets the collection of all storages types.
        /// </summary>
        [Test]
        public void TestThatStorageTypeGetAllGetGetsStorageTypeViewCollection()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (TranslationInfoSystemView translationInfo in translationInfoCollection)
            {
                StorageTypeCollectionGetQuery storageTypeCollectionGetQuery = new StorageTypeCollectionGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                IList<StorageTypeView> storageTypeCollection = new List<StorageTypeView>(_foodWasteHouseholdDataService.StorageTypeGetAll(storageTypeCollectionGetQuery));
                Assert.That(storageTypeCollection, Is.Not.Null);
                Assert.That(storageTypeCollection, Is.Not.Empty);
                Assert.That(storageTypeCollection.Count, Is.EqualTo(4));
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForRefrigerator), Is.Not.Null);
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForFreezer), Is.Not.Null);
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForKitchenCabinets), Is.Not.Null);
                Assert.That(storageTypeCollection.SingleOrDefault(m => m.StorageTypeIdentifier == StorageType.IdentifierForShoppingBasket), Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that HouseholdDataGet throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (TranslationInfoSystemView translationInfo in translationInfoCollection)
                {
                    HouseholdDataGetQuery householdDataGetQuery = new HouseholdDataGetQuery
                    {
                        HouseholdIdentifier = Guid.NewGuid(),
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdDataGet(householdDataGetQuery));

                    TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdDataGet", ExceptionMessage.HouseholdMemberNotCreated);
                }
            }
        }

        /// <summary>
        /// Tests that HouseholdAdd throws an FaultException when the household could not be created.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsFaultExceptionWhenHouseholdCouldNotBeCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    HouseholdAddCommand householdAddCommand = new HouseholdAddCommand
                    {
                        Name = null,
                        Description = null,
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdAdd(householdAddCommand));

                    TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdAdd", ExceptionMessage.ValueMustBeGivenForProperty, "Name");
                }
            }
        }

        /// <summary>
        /// Tests that HouseholdUpdate throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                HouseholdUpdateCommand householdUpdateCommand = new HouseholdUpdateCommand
                {
                    HouseholdIdentifier = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString("D"),
                    Description = null
                };
                FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdUpdate(householdUpdateCommand));

                TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdUpdate", ExceptionMessage.HouseholdMemberNotCreated);
            }
        }

        /// <summary>
        /// Tests that HouseholdAddHouseholdMember throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    HouseholdAddHouseholdMemberCommand householdAddHouseholdMemberCommand = new HouseholdAddHouseholdMemberCommand
                    {
                        HouseholdIdentifier = Guid.NewGuid(),
                        MailAddress = $"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk",
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdAddHouseholdMember(householdAddHouseholdMemberCommand));

                    TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdAddHouseholdMember", ExceptionMessage.HouseholdMemberNotCreated);
                }
            }
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                HouseholdRemoveHouseholdMemberCommand householdRemoveHouseholdMemberCommand = new HouseholdRemoveHouseholdMemberCommand
                {
                    HouseholdIdentifier = Guid.NewGuid(),
                    MailAddress = $"test.{Guid.NewGuid().ToString("D").ToLower()}@osdevgrp.dk"
                };
                FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdRemoveHouseholdMember(householdRemoveHouseholdMemberCommand));

                TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdRemoveHouseholdMember", ExceptionMessage.HouseholdMemberNotCreated);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated returns boolean result where the result is false.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedReturnsBooleanResultWhereResultIsFalse()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                var booleanResult = _foodWasteHouseholdDataService.HouseholdMemberIsCreated(new HouseholdMemberIsCreatedQuery());
                Assert.That(booleanResult, Is.Not.Null);
                Assert.That(booleanResult.Result, Is.EqualTo(false));
                Assert.That(booleanResult.EventDate, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery()));

                TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdMemberIsActivated", ExceptionMessage.HouseholdMemberNotCreated);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery()));

                TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdMemberHasAcceptedPrivacyPolicy", ExceptionMessage.HouseholdMemberNotCreated);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberDataGet throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    HouseholdMemberDataGetQuery householdMemberDataGetQuery = new HouseholdMemberDataGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdMemberDataGet(householdMemberDataGetQuery));

                    TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdMemberDataGet", ExceptionMessage.HouseholdMemberNotCreated);
                }
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                HouseholdMemberActivateCommand householdMemberActivateCommand = new HouseholdMemberActivateCommand
                {
                    ActivationCode = Guid.NewGuid().ToString("N")
                };
                FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdMemberActivate(householdMemberActivateCommand));

                TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdMemberActivate", ExceptionMessage.HouseholdMemberNotCreated);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdMemberAcceptPrivacyPolicy(new HouseholdMemberAcceptPrivacyPolicyCommand()));

                TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdMemberAcceptPrivacyPolicy", ExceptionMessage.HouseholdMemberNotCreated);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberUpgradeMembership throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatHouseholdMemberUpgradeMembershipThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated(Membership upgradeToMembership)
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var dataProviderWhoHandlesPaymentsCollectionGetQuery = new DataProviderWhoHandlesPaymentsCollectionGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    IList<DataProviderView> dataProviderCollection = new List<DataProviderView>(_foodWasteHouseholdDataService.DataProviderWhoHandlesPaymentsCollectionGet(dataProviderWhoHandlesPaymentsCollectionGetQuery));
                    Assert.That(dataProviderCollection, Is.Not.Null);
                    Assert.That(dataProviderCollection, Is.Not.Empty);

                    foreach (var dataProvider in dataProviderCollection)
                    {
                        HouseholdMemberUpgradeMembershipCommand householdMemberUpgradeMembershipCommand = new HouseholdMemberUpgradeMembershipCommand
                        {
                            Membership = upgradeToMembership.ToString(),
                            DataProviderIdentifier = dataProvider.DataProviderIdentifier,
                            PaymentTime = DateTime.Now,
                            PaymentReference = Guid.NewGuid().ToString("D").ToUpper(),
                            PaymentReceipt = null
                        };
                        FaultException<FoodWasteFault> faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => _foodWasteHouseholdDataService.HouseholdMemberUpgradeMembership(householdMemberUpgradeMembershipCommand));

                        TestHelper.AssertFaultExceptionWithFoodWasteFault(faultException, FoodWasteFaultType.BusinessFault, SoapNamespaces.FoodWasteHouseholdDataServiceName, "HouseholdMemberUpgradeMembership", ExceptionMessage.HouseholdMemberNotCreated);
                    }
                }
            }
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet gets the collection of food items.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetGetsFoodItemCollection()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var foodGroupTreeGetQuery = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteHouseholdDataService.FoodGroupTreeGet(foodGroupTreeGetQuery);
                Assert.That(foodGroupTree, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);

                var foodGroupIdentifiers = new List<Guid?> {null};
                if (foodGroupTree.FoodGroups.Any())
                {
                    foodGroupIdentifiers.Add(foodGroupTree.FoodGroups.Take(1).ElementAt(0).FoodGroupIdentifier);
                }
                foreach (var foodGroupIdentifier in foodGroupIdentifiers)
                {
                    var foodItemCollectionGetQuery = new FoodItemCollectionGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier,
                        FoodGroupIdentifier = foodGroupIdentifier
                    };
                    var foodItemCollection = _foodWasteHouseholdDataService.FoodItemCollectionGet(foodItemCollectionGetQuery);
                    Assert.That(foodItemCollection, Is.Not.Null);
                    Assert.That(foodItemCollection.FoodItems, Is.Not.Null);
                    Assert.That(foodItemCollection.FoodItems.Count(), Is.GreaterThanOrEqualTo(0));
                    Assert.That(foodItemCollection.DataProvider, Is.Not.Null);
                }
            }
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet gets the tree of food groups.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetGetsFoodGroupTree()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var query = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteHouseholdDataService.FoodGroupTreeGet(query);
                Assert.That(foodGroupTree, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);
                Assert.That(foodGroupTree.FoodGroups.Count(), Is.GreaterThanOrEqualTo(0));
                Assert.That(foodGroupTree.DataProvider, Is.Not.Null);
            }
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet gets the privacy policy.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetGetsStaticTextViewForPrivacyPolicy()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfoSystemView in translationInfoCollection)
            {
                var privacyPolicyGetQuery = new PrivacyPolicyGetQuery
                {
                    TranslationInfoIdentifier = translationInfoSystemView.TranslationInfoIdentifier
                };
                var staticTextView = _foodWasteHouseholdDataService.PrivacyPolicyGet(privacyPolicyGetQuery);
                Assert.That(staticTextView, Is.Not.Null);
                Assert.That(staticTextView.StaticTextType, Is.EqualTo((int) StaticTextType.PrivacyPolicy));
                Assert.That(staticTextView.SubjectTranslation, Is.Not.Null);
                Assert.That(staticTextView.SubjectTranslation, Is.Not.Empty);
                Assert.That(staticTextView.BodyTranslation, Is.Not.Null);
                Assert.That(staticTextView.BodyTranslation, Is.Not.Empty);
            }
        }

        /// <summary>
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet gets all the data providers who handles payments.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetGetsDataProviderWhoHandlesPaymentsCollecti()
        {
            IList<TranslationInfoSystemView> translationInfoCollection = GetTranslationInfoCollection();
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var dataProviderWhoHandlesPaymentsCollectionGetQuery = new DataProviderWhoHandlesPaymentsCollectionGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                IList<DataProviderView> dataProviderWhoHandlesPaymentsCollection = new List<DataProviderView>(_foodWasteHouseholdDataService.DataProviderWhoHandlesPaymentsCollectionGet(dataProviderWhoHandlesPaymentsCollectionGetQuery));
                Assert.That(dataProviderWhoHandlesPaymentsCollection, Is.Not.Null);
                Assert.That(dataProviderWhoHandlesPaymentsCollection, Is.Not.Empty);
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            TranslationInfoCollectionGetQuery translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            IList<TranslationInfoSystemView> translationInfoSystemViewCollection = new List<TranslationInfoSystemView>(_foodWasteHouseholdDataService.TranslationInfoGetAll(translationInfoCollectionGetQuery));
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
        }

        /// <summary>
        /// Gets all the translatios informations.
        /// </summary>
        /// <returns>Colleciton of all the translatios informations.</returns>
        private IList<TranslationInfoSystemView> GetTranslationInfoCollection()
        {
            TranslationInfoCollectionGetQuery translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            return new List<TranslationInfoSystemView>(_foodWasteHouseholdDataService.TranslationInfoGetAll(translationInfoCollectionGetQuery));
        }
    }
}
