using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Domain.FoodWaste
{
    /// <summary>
    /// Functionality for building domain object mockups for the food waste domain.
    /// </summary>
    public static class DomainObjectMockBuilder
    {
        /// <summary>
        /// Build a mockup for a household.
        /// </summary>
        /// <returns>Mockup for a household.</returns>
        public static IHousehold BuildHouseholdMock(Guid? householdIdentifier = null, IHouseholdMember householdMember = null)
        {
            var fixture = new Fixture();
            var householdMock = MockRepository.GenerateMock<IHousehold>();
            householdMock.Stub(m => m.Identifier)
                .Return(householdIdentifier ?? Guid.NewGuid())
                .Repeat.Any();
            householdMock.Stub(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            householdMock.Stub(m => m.Description)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            householdMock.Stub(m => m.CreationTime)
                .Return(DateTime.Today)
                .Repeat.Any();
            householdMock.Stub(m => m.HouseholdMembers)
                .Return(new List<IHouseholdMember> {householdMember ?? BuildHouseholdMemberMock()})
                .Repeat.Any();
            return householdMock;
        }

        /// <summary>
        /// Build a collection of mockups for some households.
        /// </summary>
        /// <returns>Collection of mockups for some households.</returns>
        public static IEnumerable<IHousehold> BuildHouseholdMockCollection(Membership membership = Membership.Basic, IHouseholdMember householdMember = null)
        {
            int numberOfHouseholds;
            switch (membership)
            {
                case Membership.Basic:
                    numberOfHouseholds = 1;
                    break;

                case Membership.Deluxe:
                    numberOfHouseholds = 2;
                    break;

                case Membership.Premium:
                    numberOfHouseholds = 3;
                    break;

                default:
                    throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, membership, "membership", MethodBase.GetCurrentMethod().Name));
            }
            var householdCollection = new List<IHousehold>(numberOfHouseholds);
            while (householdCollection.Count < numberOfHouseholds)
            {
                householdCollection.Add(BuildHouseholdMock(householdMember: householdMember));
            }
            return householdCollection;
        }

        /// <summary>
        /// Build a mockup for a storage.
        /// </summary>
        /// <returns>Mockup for a storage.</returns>
        public static IStorage BuildStorageMock(IHousehold household = null, int? sortOrder = null, IStorageType storageType = null, bool hasDescription = true)
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            if (storageType == null)
            {
                storageType = BuildStorageTypeMock();
            }

            IStorage storage = MockRepository.GenerateMock<IStorage>();
            storage.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            storage.Stub(m => m.Household)
                .Return(household ?? BuildHouseholdMock())
                .Repeat.Any();
            storage.Stub(m => m.SortOrder)
                .Return(sortOrder ?? random.Next(1, 100))
                .Repeat.Any();
            storage.Stub(m => m.StorageType)
                .Return(storageType)
                .Repeat.Any();
            storage.Stub(m => m.Description)
                .Return(hasDescription ? fixture.Create<string>() : null)
                .Repeat.Any();
            storage.Stub(m => m.Temperature)
                .Return(random.Next(storageType.TemperatureRange.StartValue, storageType.TemperatureRange.EndValue))
                .Repeat.Any();
            storage.Stub(m => m.CreationTime)
                .Return(DateTime.Now)
                .Repeat.Any();

            return storage;
        }

        /// <summary>
        /// Build a collection of mockups for some storages.
        /// </summary>
        /// <returns>Collection of mockups for some storages.</returns>
        public static IEnumerable<IStorage> BuildStorageMockCollection(IHousehold household = null)
        {
            return new List<IStorage>
            {
                BuildStorageMock(household, 1, BuildStorageTypeMock(StorageType.IdentifierForRefrigerator, 1)),
                BuildStorageMock(household, 2, BuildStorageTypeMock(StorageType.IdentifierForFreezer, 2)),
                BuildStorageMock(household, 3, BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets, 3)),
                BuildStorageMock(household, 4, BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket, 4))
            };
        }

        /// <summary>
        /// Build a mockup for a storage type.
        /// </summary>
        /// <returns>Mockup for a storage type.</returns>
        public static IStorageType BuildStorageTypeMock(Guid? storageTypeIdentifier = null, int? sortOrder = null)
        {
            Fixture fixture = new Fixture();

            Guid identifier = storageTypeIdentifier ?? Guid.NewGuid();

            IStorageType storageType = MockRepository.GenerateMock<IStorageType>();
            storageType.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            storageType.Stub(m => m.SortOrder)
                .Return(sortOrder ?? fixture.Create<int>())
                .Repeat.Any();
            storageType.Stub(m => m.Temperature)
                .Return(fixture.Create<int>())
                .Repeat.Any();
            storageType.Stub(m => m.TemperatureRange)
                .Return(BuildIntRange())
                .Repeat.Any();
            storageType.Stub(m => m.Creatable)
                .Return(fixture.Create<bool>())
                .Repeat.Any();
            storageType.Stub(m => m.Editable)
                .Return(fixture.Create<bool>())
                .Repeat.Any();
            storageType.Stub(m => m.Deletable)
                .Return(fixture.Create<bool>())
                .Repeat.Any();
            storageType.Stub(m => m.Translation)
                .Return(BuildTranslationMock(identifier))
                .Repeat.Any();
            storageType.Stub(m => m.Translations)
                .Return(BuildTranslationMockCollection(identifier))
                .Repeat.Any();

            return storageType;
        }

        /// <summary>
        /// Build a collection of mockups for some storages types.
        /// </summary>
        /// <returns>Collection of mockups for some storages types.</returns>
        public static IEnumerable<IStorageType> BuildStorageTypeMockCollection()
        {
            return new List<IStorageType>
            {
                BuildStorageTypeMock(StorageType.IdentifierForRefrigerator, 1),
                BuildStorageTypeMock(StorageType.IdentifierForFreezer, 2),
                BuildStorageTypeMock(StorageType.IdentifierForKitchenCabinets, 3),
                BuildStorageTypeMock(StorageType.IdentifierForShoppingBasket, 4)
            };
        }

        /// <summary>
        /// Build a mockup for a household member.
        /// </summary>
        /// <returns>Mockup for a household member.</returns>
        public static IHouseholdMember BuildHouseholdMemberMock(Membership membership = Membership.Basic, bool isActivated = true, bool isPrivacyPolictyAccepted = true, bool canRenewMembership = false, bool canUpgradeMembership = true, bool hasReachedHouseholdLimit = false, IEnumerable<Membership> upgradeableMemberships = null, bool membershipHasExpired = true)
        {
            var fixture = new Fixture();
            var identfier = Guid.NewGuid();
            var mailAddress = $"test.{identfier.ToString("D").ToLower()}@osdevgrp.dk";
            var householdMemberMock = MockRepository.GenerateMock<IHouseholdMember>();
            householdMemberMock.Stub(m => m.Identifier)
                .Return(identfier)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.StakeholderType)
                .Return(StakeholderType.HouseholdMember)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.Membership)
                .Return(membership)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.MembershipExpireTime)
                .Return(membership == Membership.Basic ? null : (DateTime?) DateTime.Now.AddYears(1))
                .Repeat.Any();
            householdMemberMock.Stub(m => m.MembershipHasExpired)
                .Return(membershipHasExpired)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.CanRenewMembership)
                .Return(canRenewMembership)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.CanUpgradeMembership)
                .Return(canUpgradeMembership)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.ActivationCode)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            householdMemberMock.Stub(m => m.ActivationTime)
                .Return(isActivated ? (DateTime?) DateTime.Today : null)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.IsActivated)
                .Return(isActivated)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.PrivacyPolicyAcceptedTime)
                .Return(isPrivacyPolictyAccepted ? (DateTime?) DateTime.Today : null)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.IsPrivacyPolictyAccepted)
                .Return(isPrivacyPolictyAccepted)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.HasReachedHouseholdLimit)
                .Return(hasReachedHouseholdLimit)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.CreationTime)
                .Return(DateTime.Today)
                .Repeat.Any();
            householdMemberMock.Stub(m => m.UpgradeableMemberships)
                .Return(upgradeableMemberships ?? new List<Membership> {Membership.Deluxe, Membership.Deluxe})
                .Repeat.Any();
            householdMemberMock.Stub(m => m.Households)
                .Return(BuildHouseholdMockCollection(membership, householdMemberMock))
                .Repeat.Any();
            householdMemberMock.Stub(m => m.Payments)
                .Return(BuildPaymentMockCollection(householdMemberMock))
                .Repeat.Any();
            return householdMemberMock;
        }

        /// <summary>
        /// Build a collection of mockups for some household members.
        /// </summary>
        /// <returns>Collection of mockups for some household members.</returns>
        public static IEnumerable<IHouseholdMember> BuildHouseholdMemberMockCollection()
        {
            return new List<IHouseholdMember>
            {
                BuildHouseholdMemberMock(),
                BuildHouseholdMemberMock(),
                BuildHouseholdMemberMock(),
                BuildHouseholdMemberMock(),
                BuildHouseholdMemberMock()
            };
        }

        /// <summary>
        /// Build a mockup for a payment made by a stakeholder.
        /// </summary>
        /// <returns>Mockup for a payment made by a stakeholder</returns>
        public static IPayment BuildPaymentMock(IStakeholder stakeholder = null, bool hasPaymentReceipt = true)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var paymentMock = MockRepository.GenerateMock<IPayment>();
            paymentMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            paymentMock.Stub(m => m.Stakeholder)
                .Return(stakeholder ?? BuildStakeholderMock())
                .Repeat.Any();
            paymentMock.Stub(m => m.DataProvider)
                .Return(BuildDataProviderMock(true))
                .Repeat.Any();
            paymentMock.Stub(m => m.PaymentTime)
                .Return(DateTime.Now.AddDays(random.Next(1, 365)*-1).AddMinutes(random.Next(120, 240)))
                .Repeat.Any();
            paymentMock.Stub(m => m.PaymentReference)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            paymentMock.Stub(m => m.PaymentReceipt)
                .Return(hasPaymentReceipt ? fixture.CreateMany<byte>(random.Next(1024, 4096)).ToArray() : null)
                .Repeat.Any();
            paymentMock.Stub(m => m.CreationTime)
                .Return(DateTime.Now)
                .Repeat.Any();
            return paymentMock;
        }

        /// <summary>
        /// Build a collection of mockups for some payments made by a stakeholder.
        /// </summary>
        /// <returns>Collection of mockups for some payments made by a stakeholder.</returns>
        public static IEnumerable<IPayment> BuildPaymentMockCollection(IStakeholder stakeholder = null, bool hasPaymentReceipt = true)
        {
            return new List<IPayment>
            {
                BuildPaymentMock(stakeholder, hasPaymentReceipt),
                BuildPaymentMock(stakeholder, hasPaymentReceipt),
                BuildPaymentMock(stakeholder, hasPaymentReceipt)
            };
        }

        /// <summary>
        /// Build a mockup for an internal or external stakeholder.
        /// </summary>
        /// <returns>Mockup for an internal or external stakeholder.</returns>
        public static IStakeholder BuildStakeholderMock(StakeholderType stakeholderType = StakeholderType.HouseholdMember)
        {
            var identfier = Guid.NewGuid();
            var mailAddress = $"test.{identfier.ToString("D").ToLower()}@osdevgrp.dk";
            var stakeholderMock = MockRepository.GenerateMock<IStakeholder>();
            stakeholderMock.Stub(m => m.Identifier)
                .Return(identfier)
                .Repeat.Any();
            stakeholderMock.Stub(m => m.StakeholderType)
                .Return(stakeholderType)
                .Repeat.Any();
            stakeholderMock.Stub(m => m.MailAddress)
                .Return(mailAddress)
                .Repeat.Any();
            return stakeholderMock;
        }

        /// <summary>
        /// Build a collection of mockups for some internal or external stakeholders.
        /// </summary>
        /// <returns>Collection of mockups for some internal or external stakeholders.</returns>
        public static IEnumerable<IStakeholder> BuildStakeholderMockCollection(StakeholderType stakeholderType = StakeholderType.HouseholdMember)
        {
            return new List<IStakeholder>
            {
                BuildStakeholderMock(stakeholderType),
                BuildStakeholderMock(stakeholderType),
                BuildStakeholderMock(stakeholderType)
            };
        }

        /// <summary>
        /// Build a mockup for a food item.
        /// </summary>
        /// <param name="isActive">Indicates whether the food item should be active or inactive.</param>
        /// <param name="dataProvider">Data provider who owns one of the foreign keys.</param>
        /// <param name="translations">Collection of translation mockups for the food item.</param>
        /// <returns>Mockup for a food item.</returns>
        public static IFoodItem BuildFoodItemMock(bool isActive = true, IDataProvider dataProvider = null, IEnumerable<ITranslation> translations = null)
        {
            var identifier = Guid.NewGuid();
            var primaryFoodGroupMock = BuildFoodGroupMock();
            var foodGroupMockCollection = new List<IFoodGroup>
            {
                primaryFoodGroupMock,
                BuildFoodGroupMock(),
                BuildFoodGroupMock()
            };
            var foodItemMock = MockRepository.GenerateMock<IFoodItem>();
            foodItemMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            foodItemMock.Stub(m => m.PrimaryFoodGroup)
                .Return(primaryFoodGroupMock)
                .Repeat.Any();
            foodItemMock.Stub(m => m.IsActive)
                .Return(isActive)
                .Repeat.Any();
            foodItemMock.Stub(m => m.FoodGroups)
                .Return(foodGroupMockCollection)
                .Repeat.Any();
            foodItemMock.Stub(m => m.Translation)
                .Return(BuildTranslationMock(identifier))
                .Repeat.Any();
            foodItemMock.Stub(m => m.Translations)
                .Return(translations ?? BuildTranslationMockCollection(identifier))
                .Repeat.Any();
            foodItemMock.Stub(m => m.ForeignKeys)
                .Return(BuildForeignKeyMockCollection(identifier, typeof (IFoodItem), dataProvider))
                .Repeat.Any();
            return foodItemMock;
        }

        /// <summary>
        /// Build a collection of mockups for some food items.
        /// </summary>
        /// <returns>Collection of mockups for some food items.</returns>
        public static IEnumerable<IFoodItem> BuildFoodItemMockCollection()
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var result = new List<IFoodItem>(random.Next(10, 25));
            while (result.Count < result.Capacity)
            {
                result.Add(BuildFoodItemMock());
            }
            return result;
        }

        /// <summary>
        /// Build a mockup for a food group.
        /// </summary>
        /// <param name="parentMock">Mockup for the parent food group.</param>
        /// <param name="isActive">Indicates whether the food group should be active or inactive.</param>
        /// <param name="dataProvider">Data provider who owns one of the foreign keys.</param>
        /// <param name="translations">Collection for translation mockups for the food group.</param>
        /// <returns>Mockup for a food group.</returns>
        public static IFoodGroup BuildFoodGroupMock(IFoodGroup parentMock = null, bool isActive = true, IDataProvider dataProvider = null, IEnumerable<ITranslation> translations = null)
        {
            var identifier = Guid.NewGuid();
            var foodGroupMock = MockRepository.GenerateMock<IFoodGroup>();
            foodGroupMock.Stub(m => m.Identifier)
                .Return(identifier)
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Parent)
                .Return(parentMock)
                .Repeat.Any();
            foodGroupMock.Stub(m => m.IsActive)
                .Return(isActive)
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Children)
                .Return(parentMock != null ? new List<IFoodGroup>(0) : BuildFoodGroupMockCollection(foodGroupMock))
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Translation)
                .Return(BuildTranslationMock(identifier))
                .Repeat.Any();
            foodGroupMock.Stub(m => m.Translations)
                .Return(translations ?? BuildTranslationMockCollection(identifier))
                .Repeat.Any();
            foodGroupMock.Stub(m => m.ForeignKeys)
                .Return(BuildForeignKeyMockCollection(identifier, typeof (IFoodGroup), dataProvider))
                .Repeat.Any();
            return foodGroupMock;
        }

        /// <summary>
        /// Build a collection of mockups for some food groups.
        /// </summary>
        /// <param name="parentMock">Mockup for the parent food group.</param>
        /// <returns>Collection of mockups for some food groups.</returns>
        public static IEnumerable<IFoodGroup> BuildFoodGroupMockCollection(IFoodGroup parentMock = null)
        {
            var fixture = new Fixture();
            var random = new Random(fixture.Create<int>());
            var result = new List<IFoodGroup>(random.Next(1, 5));
            while (result.Count < result.Capacity)
            {
                result.Add(BuildFoodGroupMock(parentMock));
            }
            return result;
        }

        /// <summary>
        /// Build a collection of mockups for a foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has the foreign keys.</param>
        /// <param name="foreignKeyForType">Type on which has the foreign keys.</param>
        /// <param name="dataProvider">Data provider who owns one of the foreign keys.</param>
        /// <returns>Collection of mockups for a foreign key to a domain object in the food waste domain.</returns>
        public static IEnumerable<IForeignKey> BuildForeignKeyMockCollection(Guid foreignKeyForIdentifier, Type foreignKeyForType, IDataProvider dataProvider = null)
        {
            return new List<IForeignKey>
            {
                BuildForeignKeyMock(foreignKeyForIdentifier, foreignKeyForType, dataProvider),
                BuildForeignKeyMock(foreignKeyForIdentifier, foreignKeyForType),
                BuildForeignKeyMock(foreignKeyForIdentifier, foreignKeyForType)
            };
        }

        /// <summary>
        /// Build a mockup for a foreign key to a domain object in the food waste domain.
        /// </summary>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has the foreign key.</param>
        /// <param name="foreignKeyForType">Type on which has the foreign key.</param>
        /// <param name="dataProvider">Data provider who owns the foreign key.</param>
        /// <returns>Mockup for a foreign key to a to a domain object in the food waste domain.</returns>
        public static IForeignKey BuildForeignKeyMock(Guid foreignKeyForIdentifier, Type foreignKeyForType, IDataProvider dataProvider = null)
        {
            if (foreignKeyForType == null)
            {
                throw new ArgumentNullException(nameof(foreignKeyForType));
            }
            var fixture = new Fixture();
            var foreignKeyMock = MockRepository.GenerateMock<IForeignKey>();
            foreignKeyMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.DataProvider)
                .Return(dataProvider ?? BuildDataProviderMock())
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.ForeignKeyForIdentifier)
                .Return(foreignKeyForIdentifier)
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.ForeignKeyForTypes)
                .Return(new List<Type> {typeof (IDomainObject), typeof (IIdentifiable), foreignKeyForType})
                .Repeat.Any();
            foreignKeyMock.Stub(m => m.ForeignKeyValue)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return foreignKeyMock;
        }

        /// <summary>
        /// Build a collection of mockups for all the static text used by the food waste domain.
        /// </summary>
        /// <returns>Collection of mockups for all the static text used by the food waste domain.</returns>
        public static IEnumerable<IStaticText> BuildStaticTextMockCollection()
        {
            return Enum.GetValues(typeof (StaticTextType))
                .Cast<StaticTextType>()
                .Select(BuildStaticTextMock)
                .ToList();
        }

        /// <summary>
        /// Build a mockup for a static text used by the food waste domain.
        /// </summary>
        /// <param name="staticTextType">Type of the static text.</param>
        /// <returns>Mockup for a static text used by the food waste domain.</returns>
        public static IStaticText BuildStaticTextMock(StaticTextType staticTextType = StaticTextType.WelcomeLetter)
        {
            var subjectTranslationIdentifier = Guid.NewGuid();
            var subjectTranslation = BuildTranslationMock(subjectTranslationIdentifier);
            var bodyTranslationIdentifer = Guid.NewGuid();
            var bodyTranslation = BuildTranslationMock(bodyTranslationIdentifer);
            var staticTextMock = MockRepository.GenerateMock<IStaticText>();
            staticTextMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            staticTextMock.Stub(m => m.Type)
                .Return(staticTextType)
                .Repeat.Any();
            staticTextMock.Stub(m => m.Translation)
                .Return(null)
                .Repeat.Any();
            staticTextMock.Stub(m => m.Translations)
                .Return(new List<ITranslation> {subjectTranslation, bodyTranslation})
                .Repeat.Any();
            staticTextMock.Stub(m => m.SubjectTranslationIdentifier)
                .Return(subjectTranslationIdentifier)
                .Repeat.Any();
            staticTextMock.Stub(m => m.SubjectTranslation)
                .Return(subjectTranslation)
                .Repeat.Any();
            staticTextMock.Stub(m => m.SubjectTranslations)
                .Return(new List<ITranslation> {subjectTranslation})
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslationIdentifier)
                .Return(bodyTranslationIdentifer)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslation)
                .Return(bodyTranslation)
                .Repeat.Any();
            staticTextMock.Stub(m => m.BodyTranslations)
                .Return(new List<ITranslation> {bodyTranslation})
                .Repeat.Any();
            return staticTextMock;
        }

        /// <summary>
        /// Build a mockup for a data provider.
        /// </summary>
        /// <param name="handlesPayments">Indication of whether the data provider handles payments.</param>
        /// <returns>Mockup for a data provider.</returns>
        public static IDataProvider BuildDataProviderMock(bool handlesPayments = false)
        {
            var fixture = new Fixture();
            var dataSourceStatementIdentifier = Guid.NewGuid();
            var dataProviderMock = MockRepository.GenerateMock<IDataProvider>();
            dataProviderMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Translation)
                .Return(BuildTranslationMock(dataSourceStatementIdentifier))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Translations)
                .Return(BuildTranslationMockCollection(dataSourceStatementIdentifier))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.Name)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            dataProviderMock.Stub(m => m.HandlesPayments)
                .Return(handlesPayments)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.DataSourceStatementIdentifier)
                .Return(dataSourceStatementIdentifier)
                .Repeat.Any();
            dataProviderMock.Stub(m => m.DataSourceStatement)
                .Return(BuildTranslationMock(dataSourceStatementIdentifier))
                .Repeat.Any();
            dataProviderMock.Stub(m => m.DataSourceStatements)
                .Return(BuildTranslationMockCollection(dataSourceStatementIdentifier))
                .Repeat.Any();
            return dataProviderMock;
        }

        /// <summary>
        /// Build a collection of mockups for some data providers.
        /// </summary>
        /// <param name="handlesPayments">Indication of whether the data provider handles payments.</param>
        /// <returns>Collection of mockups for some data providers.</returns>
        public static IEnumerable<IDataProvider> BuildDataProviderMockCollection(bool handlesPayments = false)
        {
            return new List<IDataProvider>
            {
                BuildDataProviderMock(handlesPayments),
                BuildDataProviderMock(handlesPayments),
                BuildDataProviderMock(handlesPayments)
            };
        }

        /// <summary>
        /// Build a mockup for a translation.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which are translated by the translation.</param>
        /// <returns>Mockup for a translation.</returns>
        public static ITranslation BuildTranslationMock(Guid translationOfIdentifier)
        {
            return BuildTranslationMock(Thread.CurrentThread.CurrentUICulture.Name, translationOfIdentifier);
        }

        /// <summary>
        /// Build a mockup for a translation.
        /// </summary>
        /// <param name="cultureName">Name for the culture which are used for translation.</param>
        /// <param name="translationOfIdentifier">Identifier for the domain object which are translated by the translation.</param>
        /// <returns>Mockup for a translation.</returns>
        public static ITranslation BuildTranslationMock(string cultureName, Guid translationOfIdentifier)
        {
            var fixture = new Fixture();
            var translationMock = MockRepository.GenerateMock<ITranslation>();
            translationMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationOfIdentifier)
                .Return(translationOfIdentifier)
                .Repeat.Any();
            translationMock.Stub(m => m.TranslationInfo)
                .Return(BuildTranslationInfoMock(cultureName))
                .Repeat.Any();
            translationMock.Stub(m => m.Value)
                .Return(fixture.Create<string>())
                .Repeat.Any();
            return translationMock;
        }

        /// <summary>
        /// Build a collection of mockups for some translations.
        /// </summary>
        /// <param name="translationOfIdentifier">Identifier for the domain object which are translated by the translations.</param>
        /// <returns>Collection of mockups for some translations.</returns>
        public static IEnumerable<ITranslation> BuildTranslationMockCollection(Guid translationOfIdentifier)
        {
            return new List<ITranslation>
            {
                BuildTranslationMock("da-DK", translationOfIdentifier),
                BuildTranslationMock("en-US", translationOfIdentifier)
            };
        }

        /// <summary>
        /// Build a mockup for translation informations for the current cultue.
        /// </summary>
        /// <returns>Mockup for translation informations for the current culture.</returns>
        public static ITranslationInfo BuildTranslationInfoMock()
        {
            return BuildTranslationInfoMock(Thread.CurrentThread.CurrentUICulture.Name);
        }

        /// <summary>
        /// Build a mockup for translation informations.
        /// </summary>
        /// <returns>Mockup for translation informations.</returns>
        public static ITranslationInfo BuildTranslationInfoMock(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                throw new ArgumentNullException(nameof(cultureName));
            }
            var translationInfoMock = MockRepository.GenerateMock<ITranslationInfo>();
            translationInfoMock.Stub(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            translationInfoMock.Stub(m => m.CultureName)
                .Return(cultureName)
                .Repeat.Any();
            translationInfoMock.Stub(m => m.CultureInfo)
                .Return(new CultureInfo(cultureName))
                .Repeat.Any();
            return translationInfoMock;
        }

        /// <summary>
        /// Build a collection of mockups for translation informations.
        /// </summary>
        /// <returns>Collection of mockups for translation informations.</returns>
        public static IEnumerable<ITranslationInfo> BuildTranslationInfoMockCollection()
        {
            return new List<ITranslationInfo>
            {
                BuildTranslationInfoMock("da-DK"),
                BuildTranslationInfoMock("en-US")
            };
        }

        /// <summary>
        /// Build a mockup for an identifiable domain object in the food waste domain.
        /// </summary>
        /// <returns>Mockup for an identifiable domain object in the food waste domain.</returns>
        public static IIdentifiable BuildIdentifiableMock()
        {
            var identifibaleMock = MockRepository.GenerateMock<IIdentifiable>();
            identifibaleMock.Expect(m => m.Identifier)
                .Return(Guid.NewGuid())
                .Repeat.Any();
            return identifibaleMock;
        }

        /// <summary>
        /// Build a mockup for an integer range.
        /// </summary>
        /// <returns>Mockup for an integer range.</returns>
        public static IRange<int> BuildIntRange()
        {
            Fixture fixture = new Fixture();
            Random random = new Random(fixture.Create<int>());

            int startValue = random.Next(1, 100);
            int endValue = startValue += random.Next(1, 100);

            IRange<int> intRangeMock = MockRepository.GenerateMock<IRange<int>>();
            intRangeMock.Stub(m => m.StartValue)
                .Return(startValue)
                .Repeat.Any();
            intRangeMock.Stub(m => m.EndValue)
                .Return(endValue)
                .Repeat.Any();

            return intRangeMock;
        }
    }
}
