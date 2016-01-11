using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using NUnit.Framework;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Contracts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Tests.Integrationstests.Services.ClientCalls
{
    /// <summary>
    /// Integration tests the service which can access and modify data on a house hold in the food waste domain.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FoodWasteHouseholdDataServiceTests
    {
        #region Private constants

        private const string ClientEndpointName = "FoodWasteHouseholdDataServiceIntegrationstest";

        #endregion

        #region Private variables

        private ChannelFactory<IFoodWasteHouseholdDataService> _channelFactory;

        #endregion

        /// <summary>
        /// Opsætning af test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetUp()
        {
            _channelFactory = new ChannelFactory<IFoodWasteHouseholdDataService>(ClientEndpointName);
            if (_channelFactory.Credentials == null)
            {
                return;
            }
            _channelFactory.Credentials.UserName.UserName = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower());
        }

        /// <summary>
        /// Tests that HouseholdMemberIsCreated returns boolean result where the result is false.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsCreatedReturnsBooleanResultWhereResultIsFalse()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var booleanResult = client.HouseholdMemberIsCreated(new HouseholdMemberIsCreatedQuery());
                Assert.That(booleanResult, Is.Not.Null);
                Assert.That(booleanResult.Result, Is.EqualTo(false));
                Assert.That(booleanResult.EventDate, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberIsActivated throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIsActivatedThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdMemberIsActivated(new HouseholdMemberIsActivatedQuery()));
                Assert.That(faultException, Is.Not.Null);
                Assert.That(faultException.Detail, Is.Not.Null);
                Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.BusinessFault));
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
                Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated)));
                Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceName, Is.EqualTo(SoapNamespaces.FoodWasteHouseholdDataServiceName));
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdMemberIsActivated"));
                Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberHasAcceptedPrivacyPolicy throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberHasAcceptedPrivacyPolicyThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdMemberHasAcceptedPrivacyPolicy(new HouseholdMemberHasAcceptedPrivacyPolicyQuery()));
                Assert.That(faultException, Is.Not.Null);
                Assert.That(faultException.Detail, Is.Not.Null);
                Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.BusinessFault));
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
                Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated)));
                Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceName, Is.EqualTo(SoapNamespaces.FoodWasteHouseholdDataServiceName));
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdMemberHasAcceptedPrivacyPolicy"));
                Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberActivate throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberActivateThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var householdMemberActivateCommand = new HouseholdMemberActivateCommand
                {
                    ActivationCode = Guid.NewGuid().ToString("N")
                };
                var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdMemberActivate(householdMemberActivateCommand));
                Assert.That(faultException, Is.Not.Null);
                Assert.That(faultException.Detail, Is.Not.Null);
                Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.BusinessFault));
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
                Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated)));
                Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceName, Is.EqualTo(SoapNamespaces.FoodWasteHouseholdDataServiceName));
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdMemberActivate"));
                Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdMemberAcceptPrivacyPolicy throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberAcceptPrivacyPolicyThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdMemberAcceptPrivacyPolicy(new HouseholdMemberAcceptPrivacyPolicyCommand()));
                Assert.That(faultException, Is.Not.Null);
                Assert.That(faultException.Detail, Is.Not.Null);
                Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.BusinessFault));
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
                Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
                Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated)));
                Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceName, Is.EqualTo(SoapNamespaces.FoodWasteHouseholdDataServiceName));
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
                Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
                Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdMemberAcceptPrivacyPolicy"));
                Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
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
