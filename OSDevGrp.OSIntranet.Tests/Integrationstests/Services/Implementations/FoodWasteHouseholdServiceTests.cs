using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.Implementations
{
    /// <summary>
    /// Integration tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteHouseholdServiceTests
    {
        #region Private variables

        private IFoodWasteHouseholdService _foodWasteHouseholdService;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            var container = ContainerFactory.Create();
            _foodWasteHouseholdService = container.Resolve<IFoodWasteHouseholdService>();
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated returns boolean result where the result is false.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedReturnsBooleanResultWhereResultIsFalse()
        {
            using (new ClaimsPrincipalTestExecutor())
            {
                var booleanResult = _foodWasteHouseholdService.HouseholdMemberIsCreated(new HouseholdMemberIsCreatedQuery());
                Assert.That(booleanResult, Is.Not.Null);
                Assert.That(booleanResult.Result, Is.EqualTo(false));
                Assert.That(booleanResult.EventDate, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            }
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet gets the collection of food items.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetGetsFoodItemCollection()
        {
            var translationInfoCollection = _foodWasteHouseholdService.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var foodGroupTreeGetQuery = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteHouseholdService.FoodGroupTreeGet(foodGroupTreeGetQuery);
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
                    var foodItemCollection = _foodWasteHouseholdService.FoodItemCollectionGet(foodItemCollectionGetQuery);
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
            var translationInfoCollection = _foodWasteHouseholdService.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
            Assert.That(translationInfoCollection, Is.Not.Null);
            Assert.That(translationInfoCollection, Is.Not.Empty);

            foreach (var translationInfo in translationInfoCollection)
            {
                var query = new FoodGroupTreeGetQuery
                {
                    TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                };
                var foodGroupTree = _foodWasteHouseholdService.FoodGroupTreeGet(query);
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
            var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            var translationInfoSystemViewCollection = _foodWasteHouseholdService.TranslationInfoGetAll(translationInfoCollectionGetQuery);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);

            foreach (var translationInfoSystemView in translationInfoSystemViewCollection)
            {
                var privacyPolicyGetQuery = new PrivacyPolicyGetQuery
                {
                    TranslationInfoIdentifier = translationInfoSystemView.TranslationInfoIdentifier
                };
                var staticTextView = _foodWasteHouseholdService.PrivacyPolicyGet(privacyPolicyGetQuery);
                Assert.That(staticTextView, Is.Not.Null);
                Assert.That(staticTextView.StaticTextType, Is.EqualTo((int) StaticTextType.PrivacyPolicy));
                Assert.That(staticTextView.SubjectTranslation, Is.Not.Null);
                Assert.That(staticTextView.SubjectTranslation, Is.Not.Empty);
                Assert.That(staticTextView.BodyTranslation, Is.Not.Null);
                Assert.That(staticTextView.BodyTranslation, Is.Not.Empty);
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
            var translationInfoSystemViewCollection = _foodWasteHouseholdService.TranslationInfoGetAll(translationInfoCollectionGetQuery);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
            Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
        }
    }
}
