using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integration tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteHouseHoldServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "FoodWasteHouseHoldServiceIntegrationstest";

        #endregion

        #region Private variables

        private ChannelFactory<IFoodWasteHouseHoldService> _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            _channelFactory = new ChannelFactory<IFoodWasteHouseHoldService>(ClientEndpointName);
            if (_channelFactory.Credentials == null)
            {
                return;
            }
            _channelFactory.Credentials.UserName.UserName = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower());
        }

        /// <summary>
        /// Tests that FoodItemCollectionGet gets the collection of food items.
        /// </summary>
        [Test]
        public void TestThatFoodItemCollectionGetGetsFoodItemCollection()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var foodGroupTreeGetQuery = new FoodGroupTreeGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var foodGroupTree = client.FoodGroupTreeGet(foodGroupTreeGetQuery);
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
                        var foodItemCollection = client.FoodItemCollectionGet(foodItemCollectionGetQuery);
                        Assert.That(foodItemCollection, Is.Not.Null);
                        Assert.That(foodItemCollection.FoodItems, Is.Not.Null);
                        Assert.That(foodItemCollection.FoodItems.Count(), Is.GreaterThanOrEqualTo(0));
                        Assert.That(foodItemCollection.DataProvider, Is.Not.Null);
                    }
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that FoodGroupTreeGet gets the tree of food groups.
        /// </summary>
        [Test]
        public void TestThatFoodGroupTreeGetGetsFoodGroupTree()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var query = new FoodGroupTreeGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var foodGroupTree = client.FoodGroupTreeGet(query);
                    Assert.That(foodGroupTree, Is.Not.Null);
                    Assert.That(foodGroupTree.FoodGroups, Is.Not.Null);
                    Assert.That(foodGroupTree.FoodGroups.Count(), Is.GreaterThanOrEqualTo(0));
                    Assert.That(foodGroupTree.DataProvider, Is.Not.Null);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that PrivacyPolicyGet gets the privacy policy.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyGetGetsStaticTextViewForPrivacyPolicy()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
                var translationInfoSystemViewCollection = client.TranslationInfoGetAll(translationInfoCollectionGetQuery);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);

                foreach (var translationInfoSystemView in translationInfoSystemViewCollection)
                {
                    var privacyPolicyGetQuery = new PrivacyPolicyGetQuery
                    {
                        TranslationInfoIdentifier = translationInfoSystemView.TranslationInfoIdentifier
                    };
                    var staticTextView = client.PrivacyPolicyGet(privacyPolicyGetQuery);
                    Assert.That(staticTextView, Is.Not.Null);
                    Assert.That(staticTextView.StaticTextType, Is.EqualTo((int) StaticTextType.PrivacyPolicy));
                    Assert.That(staticTextView.SubjectTranslation, Is.Not.Null);
                    Assert.That(staticTextView.SubjectTranslation, Is.Not.Empty);
                    Assert.That(staticTextView.BodyTranslation, Is.Not.Null);
                    Assert.That(staticTextView.BodyTranslation, Is.Not.Empty);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that TranslationInfoGetAll gets all the tranlation informations.
        /// </summary>
        [Test]
        public void TestThatTranslationInfoGetAllGetsTranslationInfoSystemViewCollection()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollectionGetQuery = new TranslationInfoCollectionGetQuery();
                var translationInfoSystemViewCollection = client.TranslationInfoGetAll(translationInfoCollectionGetQuery);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Null);
                Assert.That(translationInfoSystemViewCollection, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }
    }
}
