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
        [SetUp]
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
        /// Tests that HouseholdDataGet throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdDataGetThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var householdDataGetQuery = new HouseholdDataGetQuery
                    {
                        HouseholdIdentifier = Guid.NewGuid(),
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdDataGet(householdDataGetQuery));
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
                    Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdDataGet"));
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdAdd throws an FaultException when the household could not be created.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddThrowsFaultExceptionWhenHouseholdCouldNotBeCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var householdAddCommand = new HouseholdAddCommand
                    {
                        Name = null,
                        Description = null,
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdAdd(householdAddCommand));
                    Assert.That(faultException, Is.Not.Null);
                    Assert.That(faultException.Detail, Is.Not.Null);
                    Assert.That(faultException.Detail.FaultType, Is.EqualTo(FoodWasteFaultType.BusinessFault));
                    Assert.That(faultException.Detail.ErrorMessage, Is.Not.Null);
                    Assert.That(faultException.Detail.ErrorMessage, Is.Not.Empty);
                    Assert.That(faultException.Detail.ErrorMessage, Is.EqualTo(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Name")));
                    Assert.That(faultException.Detail.ServiceName, Is.Not.Null);
                    Assert.That(faultException.Detail.ServiceName, Is.Not.Empty);
                    Assert.That(faultException.Detail.ServiceName, Is.EqualTo(SoapNamespaces.FoodWasteHouseholdDataServiceName));
                    Assert.That(faultException.Detail.ServiceMethod, Is.Not.Null);
                    Assert.That(faultException.Detail.ServiceMethod, Is.Not.Empty);
                    Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdAdd"));
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdUpdate throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdUpdateThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var householdUpdateCommand = new HouseholdUpdateCommand
                {
                    HouseholdIdentifier = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString("D"),
                    Description = null
                };
                var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdUpdate(householdUpdateCommand));
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
                Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdUpdate"));
                Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdAddHouseholdMember throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdAddHouseholdMemberThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var householdAddHouseholdMemberCommand = new HouseholdAddHouseholdMemberCommand
                    {
                        HouseholdIdentifier = Guid.NewGuid(),
                        MailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower()),
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdAddHouseholdMember(householdAddHouseholdMemberCommand));
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
                    Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdAddHouseholdMember"));
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
                }
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
        }

        /// <summary>
        /// Tests that HouseholdRemoveHouseholdMember throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdRemoveHouseholdMemberThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var householdRemoveHouseholdMemberCommand = new HouseholdRemoveHouseholdMemberCommand
                {
                    HouseholdIdentifier = Guid.NewGuid(),
                    MailAddress = string.Format("test.{0}@osdevgrp.dk", Guid.NewGuid().ToString("D").ToLower())
                };
                var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdRemoveHouseholdMember(householdRemoveHouseholdMemberCommand));
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
                Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdRemoveHouseholdMember"));
                Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
            }
            finally
            {
                ChannelTools.CloseChannel(client);
            }
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
        /// Tests that HouseholdMemberDataGet throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberDataGetThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var householdMemberDataGetQuery = new HouseholdMemberDataGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdMemberDataGet(householdMemberDataGetQuery));
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
                    Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdMemberDataGet"));
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                    Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
                }
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
        /// Tests that HouseholdMemberUpgradeMembership throws an FaultException when the household member has not been created.
        /// </summary>
        [Test]
        [TestCase(Membership.Deluxe)]
        [TestCase(Membership.Premium)]
        public void TestThatHouseholdMemberUpgradeMembershipThrowsFaultExceptionWhenHouseholdMemberHasNotBeenCreated(Membership upgradeToMembership)
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                using (new ClaimsPrincipalTestExecutor())
                {
                    var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                    Assert.That(translationInfoCollection, Is.Not.Null);
                    Assert.That(translationInfoCollection, Is.Not.Empty);

                    foreach (var translationInfo in translationInfoCollection)
                    {
                        var dataProviderWhoHandlesPaymentsCollectionGetQuery = new DataProviderWhoHandlesPaymentsCollectionGetQuery
                        {
                            TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                        };
                        var dataProviderCollection = client.DataProviderWhoHandlesPaymentsCollectionGet(dataProviderWhoHandlesPaymentsCollectionGetQuery);
                        Assert.That(dataProviderCollection, Is.Not.Null);
                        Assert.That(dataProviderCollection, Is.Not.Empty);

                        foreach (var dataProvider in dataProviderCollection)
                        {
                            var householdMemberUpgradeMembershipCommand = new HouseholdMemberUpgradeMembershipCommand
                            {
                                Membership = upgradeToMembership.ToString(),
                                DataProviderIdentifier = dataProvider.DataProviderIdentifier,
                                PaymentTime = DateTime.Now,
                                PaymentReference = Guid.NewGuid().ToString("D").ToUpper(),
                                PaymentReceipt = null
                            };
                            var faultException = Assert.Throws<FaultException<FoodWasteFault>>(() => client.HouseholdMemberUpgradeMembership(householdMemberUpgradeMembershipCommand));
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
                            Assert.That(faultException.Detail.ServiceMethod, Is.EqualTo("HouseholdMemberUpgradeMembership"));
                            Assert.That(faultException.Detail.StackTrace, Is.Not.Null);
                            Assert.That(faultException.Detail.StackTrace, Is.Not.Empty);
                        }
                    }
                }
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
        /// Tests that DataProviderWhoHandlesPaymentsCollectionGet gets all the data providers who handles payments.
        /// </summary>
        [Test]
        public void TestThatDataProviderWhoHandlesPaymentsCollectionGetGetsDataProviderWhoHandlesPaymentsCollecti()
        {
            var client = _channelFactory.CreateChannel();
            try
            {
                var translationInfoCollection = client.TranslationInfoGetAll(new TranslationInfoCollectionGetQuery());
                Assert.That(translationInfoCollection, Is.Not.Null);
                Assert.That(translationInfoCollection, Is.Not.Empty);

                foreach (var translationInfo in translationInfoCollection)
                {
                    var dataProviderWhoHandlesPaymentsCollectionGetQuery = new DataProviderWhoHandlesPaymentsCollectionGetQuery
                    {
                        TranslationInfoIdentifier = translationInfo.TranslationInfoIdentifier
                    };
                    var dataProviderWhoHandlesPaymentsCollection = client.DataProviderWhoHandlesPaymentsCollectionGet(dataProviderWhoHandlesPaymentsCollectionGetQuery);
                    Assert.That(dataProviderWhoHandlesPaymentsCollection, Is.Not.Null);
                    Assert.That(dataProviderWhoHandlesPaymentsCollection, Is.Not.Empty);
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
