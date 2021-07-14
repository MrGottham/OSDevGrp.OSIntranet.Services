using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoMapper;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Infrastructure
{
    /// <summary>
    /// Object mapper which can map objects in the food waste domain.
    /// </summary>
    public class FoodWasteObjectMapper : IFoodWasteObjectMapper
    {
        #region Private variables

        private static IHouseholdProxy _mappingHousehold;
        private static IHouseholdMemberProxy _mappingHouseholdMember;
        private static readonly IMapper Mapper;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an object mapper which can map objects in the food waste domain.
        /// </summary>
        static FoodWasteObjectMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<IHousehold, HouseholdIdentificationView>()
                    .ForMember(m => m.HouseholdIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name));

                config.CreateMap<IHousehold, HouseholdView>()
                    .ForMember(m => m.HouseholdIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(m => m.Description, opt => opt.MapFrom(s => s.Description))
                    .ForMember(m => m.CreationTime, opt => opt.MapFrom(s => s.CreationTime))
                    .ForMember(m => m.HouseholdMembers, opt => opt.MapFrom(s => s.HouseholdMembers))
                    .ForMember(m => m.Storages, opt => opt.MapFrom(s => s.Storages));

                config.CreateMap<IHousehold, IHouseholdProxy>()
                    .ConvertUsing(m => ToHouseholdProxy(m));

                config.CreateMap<IStorageType, StorageTypeIdentificationView>()
                    .ForMember(m => m.StorageTypeIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty));

                config.CreateMap<IStorageType, StorageTypeView>()
                    .ForMember(m => m.StorageTypeIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.SortOrder, opt => opt.MapFrom(s => s.SortOrder))
                    .ForMember(m => m.Temperature, opt => opt.MapFrom(s => s.Temperature))
                    .ForMember(m => m.TemperatureRange, opt => opt.MapFrom(s => s.TemperatureRange))
                    .ForMember(m => m.Creatable, opt => opt.MapFrom(s => s.Creatable))
                    .ForMember(m => m.Editable, opt => opt.MapFrom(s => s.Editable))
                    .ForMember(m => m.Deletable, opt => opt.MapFrom(s => s.Deletable));

                config.CreateMap<IStorageType, StorageTypeSystemView>()
                    .ForMember(m => m.StorageTypeIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.SortOrder, opt => opt.MapFrom(s => s.SortOrder))
                    .ForMember(m => m.Temperature, opt => opt.MapFrom(s => s.Temperature))
                    .ForMember(m => m.TemperatureRange, opt => opt.MapFrom(s => s.TemperatureRange))
                    .ForMember(m => m.Creatable, opt => opt.MapFrom(s => s.Creatable))
                    .ForMember(m => m.Editable, opt => opt.MapFrom(s => s.Editable))
                    .ForMember(m => m.Deletable, opt => opt.MapFrom(s => s.Deletable))
                    .ForMember(m => m.Translations, opt => opt.MapFrom(s => s.Translations));

                config.CreateMap<IStorage, StorageIdentificationView>()
                    .ForMember(m => m.StorageIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.SortOrder, opt => opt.MapFrom(s => s.SortOrder));

                config.CreateMap<IStorage, StorageView>()
                    .ForMember(m => m.StorageIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Household, opt => opt.MapFrom(s => s.Household))
                    .ForMember(m => m.SortOrder, opt => opt.MapFrom(s => s.SortOrder))
                    .ForMember(m => m.StorageType, opt => opt.MapFrom(s => s.StorageType))
                    .ForMember(m => m.Description, opt =>
                    {
                        opt.Condition(s => string.IsNullOrWhiteSpace(s.Description) == false);
                        opt.MapFrom(s => s.Description);
                    })
                    .ForMember(m => m.Temperature, opt => opt.MapFrom(s => s.Temperature))
                    .ForMember(m => m.CreationTime, opt => opt.MapFrom(s => s.CreationTime));

                config.CreateMap<IStorage, IStorageProxy>()
                    .ConvertUsing(m => ToStorageProxy(m));

                config.CreateMap<IStorageType, IStorageTypeProxy>()
                    .ConvertUsing(m => ToStorageTypeProxy(m));

                config.CreateMap<IHouseholdMember, HouseholdMemberIdentificationView>()
                    .ForMember(m => m.HouseholdMemberIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.MailAddress, opt => opt.MapFrom(s => s.MailAddress));

                config.CreateMap<IHouseholdMember, HouseholdMemberView>()
                    .ForMember(m => m.HouseholdMemberIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.MailAddress, opt => opt.MapFrom(s => s.MailAddress))
                    .ForMember(m => m.Membership, opt => opt.MapFrom(s => s.Membership))
                    .ForMember(m => m.MembershipExpireTime, opt =>
                    {
                        opt.Condition(s => s.MembershipHasExpired == false);
                        opt.MapFrom(s => s.MembershipExpireTime);
                    })
                    .ForMember(m => m.CanRenewMembership, opt => opt.MapFrom(s => s.CanRenewMembership))
                    .ForMember(m => m.CanUpgradeMembership, opt => opt.MapFrom(s => s.CanUpgradeMembership))
                    .ForMember(m => m.ActivationTime, opt => opt.MapFrom(s => s.ActivationTime))
                    .ForMember(m => m.IsActivated, opt => opt.MapFrom(s => s.IsActivated))
                    .ForMember(m => m.PrivacyPolicyAcceptedTime, opt => opt.MapFrom(s => s.PrivacyPolicyAcceptedTime))
                    .ForMember(m => m.IsPrivacyPolicyAccepted, opt => opt.MapFrom(s => s.IsPrivacyPolicyAccepted))
                    .ForMember(m => m.HasReachedHouseholdLimit, opt => opt.MapFrom(s => s.HasReachedHouseholdLimit))
                    .ForMember(m => m.CanCreateStorage, opt => opt.MapFrom(s => s.CanCreateStorage))
                    .ForMember(m => m.CanUpdateStorage, opt => opt.MapFrom(s => s.CanUpdateStorage))
                    .ForMember(m => m.CanDeleteStorage, opt => opt.MapFrom(s => s.CanDeleteStorage))
                    .ForMember(m => m.UpgradeableMemberships, opt => opt.MapFrom(s => s.UpgradeableMemberships))
                    .ForMember(m => m.CreationTime, opt => opt.MapFrom(s => s.CreationTime))
                    .ForMember(m => m.Households, opt => opt.MapFrom(s => s.Households))
                    .ForMember(m => m.Payments, opt => opt.MapFrom(s => s.Payments));

                config.CreateMap<IHouseholdMember, IHouseholdMemberProxy>()
                    .ConvertUsing(m => ToHouseholdMemberProxy(m));

                config.CreateMap<IPayment, PaymentView>()
                    .ForMember(m => m.PaymentIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Stakeholder, opt => opt.MapFrom(s => s.Stakeholder))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider))
                    .ForMember(m => m.PaymentTime, opt => opt.MapFrom(s => s.PaymentTime))
                    .ForMember(m => m.PaymentReference, opt => opt.MapFrom(s => s.PaymentReference))
                    .ForMember(m => m.PaymentReceipt, opt => opt.MapFrom(s => s.PaymentReceipt == null ? null : Convert.ToBase64String(s.PaymentReceipt.ToArray())))
                    .ForMember(m => m.CreationTime, opt => opt.MapFrom(s => s.CreationTime));

                config.CreateMap<IPayment, IPaymentProxy>()
                    .ConvertUsing(m => ToPaymentProxy(m));

                config.CreateMap<IStakeholder, StakeholderView>()
                    .ForMember(m => m.StakeholderIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.MailAddress, opt => opt.MapFrom(s => s.MailAddress));

                config.CreateMap<IFoodItemCollection, FoodItemCollectionView>()
                    .ForMember(m => m.FoodItems, opt => opt.MapFrom<IEnumerable<IFoodItem>>(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodItemCollection, FoodItemCollectionSystemView>()
                    .ForMember(m => m.FoodItems, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodItem, FoodItemIdentificationView>()
                    .ForMember(m => m.FoodItemIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty));

                config.CreateMap<IFoodItem, FoodItemView>()
                    .ForMember(m => m.FoodItemIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.PrimaryFoodGroup, opt => opt.MapFrom(s => s.PrimaryFoodGroup))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s.FoodGroups));

                config.CreateMap<IFoodItem, FoodItemSystemView>()
                    .ForMember(m => m.FoodItemIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.PrimaryFoodGroup, opt => opt.MapFrom(s => s.PrimaryFoodGroup))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s.FoodGroups))
                    .ForMember(m => m.Translations, opt => opt.MapFrom(s => s.Translations))
                    .ForMember(m => m.ForeignKeys, opt => opt.MapFrom(s => s.ForeignKeys));

                config.CreateMap<IFoodItem, IFoodItemProxy>()
                    .ConvertUsing(m => ToFoodItemProxy(m));

                config.CreateMap<IFoodGroupCollection, FoodGroupTreeView>()
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodGroupCollection, FoodGroupTreeSystemView>()
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodGroup, FoodGroupIdentificationView>()
                    .ForMember(m => m.FoodGroupIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty));

                config.CreateMap<IFoodGroup, FoodGroupView>()
                    .ForMember(m => m.FoodGroupIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.Parent, opt =>
                    {
                        opt.Condition(s => s.Parent != null);
                        opt.MapFrom(s => s.Parent);
                    })
                    .ForMember(m => m.Children, opt => opt.MapFrom(s => s.Children));

                config.CreateMap<IFoodGroup, FoodGroupSystemView>()
                    .ForMember(m => m.FoodGroupIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.Parent, opt =>
                    {
                        opt.Condition(s => s.Parent != null);
                        opt.MapFrom(s => s.Parent);
                    })
                    .ForMember(m => m.Translations, opt => opt.MapFrom(s => s.Translations))
                    .ForMember(m => m.ForeignKeys, opt => opt.MapFrom(s => s.ForeignKeys))
                    .ForMember(m => m.Children, opt => opt.MapFrom(s => s.Children));

                config.CreateMap<IFoodGroup, IFoodGroupProxy>()
                    .ConvertUsing(m => ToFoodGroupProxy(m));

                config.CreateMap<IForeignKey, ForeignKeyView>()
                    .ForMember(m => m.ForeignKeyIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider))
                    .ForMember(m => m.ForeignKeyForIdentifier, opt => opt.MapFrom(s => s.ForeignKeyForIdentifier))
                    .ForMember(m => m.ForeignKey, opt => opt.MapFrom(s => s.ForeignKeyValue));

                config.CreateMap<IForeignKey, ForeignKeySystemView>()
                    .ForMember(m => m.ForeignKeyIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider))
                    .ForMember(m => m.ForeignKeyForIdentifier, opt => opt.MapFrom(s => s.ForeignKeyForIdentifier))
                    .ForMember(m => m.ForeignKey, opt => opt.MapFrom(s => s.ForeignKeyValue));

                config.CreateMap<IForeignKey, IForeignKeyProxy>()
                    .ConvertUsing(m => ToForeignKeyProxy(m));

                config.CreateMap<IStaticText, StaticTextView>()
                    .ForMember(m => m.StaticTextIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.StaticTextType, opt => opt.MapFrom(s => (int) s.Type))
                    .ForMember(m => m.SubjectTranslation, opt => opt.MapFrom(s => s.SubjectTranslation != null ? s.SubjectTranslation.Value : string.Empty))
                    .ForMember(m => m.BodyTranslation, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? s.BodyTranslation != null ? s.BodyTranslation.Value : string.Empty : null));

                config.CreateMap<IStaticText, StaticTextSystemView>()
                    .ForMember(m => m.StaticTextIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.StaticTextType, opt => opt.MapFrom(s => (int) s.Type))
                    .ForMember(m => m.SubjectTranslationIdentifier, opt => opt.MapFrom(s => s.SubjectTranslationIdentifier))
                    .ForMember(m => m.SubjectTranslation, opt => opt.MapFrom(s => s.SubjectTranslation != null ? s.SubjectTranslation.Value : string.Empty))
                    .ForMember(m => m.SubjectTranslations, opt => opt.MapFrom(s => s.SubjectTranslations))
                    .ForMember(m => m.BodyTranslationIdentifier, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? (Guid?) s.BodyTranslationIdentifier.Value : null))
                    .ForMember(m => m.BodyTranslation, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? s.BodyTranslation != null ? s.BodyTranslation.Value : string.Empty : null))
                    .ForMember(m => m.BodyTranslations, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? s.BodyTranslations : null));

                config.CreateMap<IStaticText, IStaticTextProxy>()
                    .ConvertUsing(m => ToStaticTextProxy(m));

                config.CreateMap<IDataProvider, DataProviderView>()
                    .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(m => m.DataSourceStatement, opt => opt.MapFrom(s => s.DataSourceStatement != null ? s.DataSourceStatement.Value : string.Empty));

                config.CreateMap<IDataProvider, DataProviderSystemView>()
                    .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(m => m.HandlesPayments, opt => opt.MapFrom(s => s.HandlesPayments))
                    .ForMember(m => m.DataSourceStatementIdentifier, opt => opt.MapFrom(s => s.DataSourceStatementIdentifier))
                    .ForMember(m => m.DataSourceStatements, opt => opt.MapFrom(s => s.DataSourceStatements));

                config.CreateMap<IDataProvider, IDataProviderProxy>()
                    .ConvertUsing(m => ToDataProviderProxy(m));

                config.CreateMap<ITranslation, TranslationSystemView>()
                    .ForMember(m => m.TranslationIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.TranslationOfIdentifier, opt => opt.MapFrom(s => s.TranslationOfIdentifier))
                    .ForMember(m => m.TranslationInfo, opt => opt.MapFrom(s => s.TranslationInfo))
                    .ForMember(m => m.Translation, opt => opt.MapFrom(s => s.Value));

                config.CreateMap<ITranslation, ITranslationProxy>()
                    .ConvertUsing(m => ToTranslationProxy(m));

                config.CreateMap<ITranslationInfo, TranslationInfoSystemView>()
                    .ForMember(m => m.TranslationInfoIdentifier, opt => opt.MapFrom(s => s.Identifier ?? Guid.Empty))
                    .ForMember(m => m.CultureName, opt => opt.MapFrom(s => s.CultureName));

                config.CreateMap<ITranslationInfo, ITranslationInfoProxy>()
                    .ConvertUsing(m => ToTranslationInfoProxy(m));

                config.CreateMap<IIdentifiable, ServiceReceiptResponse>()
                    .ForMember(m => m.Identifier, opt => opt.MapFrom(s => s.Identifier))
                    .ForMember(m => m.EventDate, opt => opt.MapFrom(s => DateTime.Now));

                config.CreateMap<IRange<int>, IntRangeView>()
                    .ForMember(m => m.StartValue, opt => opt.MapFrom(s => s.StartValue))
                    .ForMember(m => m.EndValue, opt => opt.MapFrom(s => s.EndValue));

                config.CreateMap<bool, BooleanResultResponse>()
                    .ForMember(m => m.Result, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.EventDate, opt => opt.MapFrom(s => DateTime.Now));
            });

            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Maps a source object to a destination object.
        /// </summary>
        /// <typeparam name="TSource">Type for the source object to map.</typeparam>
        /// <typeparam name="TDestination">Type for the destination object.</typeparam>
        /// <param name="source">Source object to map.</param>
        /// <param name="translationCulture">Culture information used to translation.</param>
        /// <returns>Destination object mapped from the source object.</returns>
        public TDestination Map<TSource, TDestination>(TSource source, CultureInfo translationCulture = null)
        {
            if (Equals(source, null))
            {
                throw new ArgumentNullException(nameof(source));
            }
            var identifiable = source as IIdentifiable;
            if (identifiable != null && identifiable.Identifier.HasValue == false)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, identifiable.Identifier, "Identifier"));
            }
            var translatable = source as ITranslatable;
            if (translatable != null)
            {
                translatable.Translate(translationCulture ?? Thread.CurrentThread.CurrentUICulture);
            }
            var foreignKeyable = source as IForeignKeyable;
            if (foreignKeyable != null)
            {
                foreach (var foreignKey in foreignKeyable.ForeignKeys.Where(m => m.DataProvider != null && m.DataProvider.Translation == null))
                {
                    foreignKey.DataProvider.Translate(translationCulture ?? Thread.CurrentThread.CurrentUICulture);
                }
            }
            var translatableCollection = source as IEnumerable<ITranslatable>;
            if (translatableCollection != null)
            {
                foreach (var t in translatableCollection)
                {
                    t.Translate(translationCulture ?? Thread.CurrentThread.CurrentUICulture);
                }
            }
            return Mapper.Map<TSource, TDestination>(source);
        }

        private static IHouseholdProxy ToHouseholdProxy(IHousehold source)
        {
            IHouseholdProxy householdProxy = source as IHouseholdProxy;
            if (householdProxy != null)
            {
                return householdProxy;
            }

            householdProxy = new HouseholdProxy(source.Name, source.Description, source.CreationTime)
            {
                Identifier = source.Identifier
            };
            lock (SyncRoot)
            {
                _mappingHousehold = householdProxy;
                try
                {
                    if (_mappingHouseholdMember == null)
                    {
                        foreach (var householdMember in source.HouseholdMembers)
                        {
                            if (householdMember is IHouseholdMemberProxy)
                            {
                                if (householdProxy.HouseholdMembers.Contains(householdMember))
                                {
                                    continue;
                                }

                                householdProxy.HouseholdMemberAdd(householdMember);
                                continue;
                            }

                            if (Mapper != null)
                            {
                                var householdMemberProxy = Mapper.Map<IHouseholdMember, IHouseholdMemberProxy>(householdMember);
                                if (householdProxy.HouseholdMembers.Contains(householdMemberProxy))
                                {
                                    continue;
                                }

                                householdProxy.HouseholdMemberAdd(householdMemberProxy);
                            }
                        }
                    }
                    else if (householdProxy.HouseholdMembers.Contains(_mappingHouseholdMember) == false)
                    {
                        householdProxy.HouseholdMemberAdd(_mappingHouseholdMember);
                    }

                    foreach (IStorage storage in source.Storages.OrderBy(n => n.SortOrder).ThenByDescending(n => n.CreationTime))
                    {
                        if (storage is IStorageProxy)
                        {
                            if (householdProxy.Storages.Contains(storage))
                            {
                                continue;
                            }

                            householdProxy.StorageAdd(storage);
                            continue;
                        }

                        if (Mapper != null)
                        {
                            IStorageProxy storageProxy = Mapper.Map<IStorage, IStorageProxy>(storage);
                            if (householdProxy.Storages.Contains(storageProxy))
                            {
                                continue;
                            }

                            householdProxy.StorageAdd(storageProxy);
                        }
                    }
                }
                finally
                {
                    _mappingHousehold = null;
                }
            }

            return householdProxy;
        }

        private static IStorageProxy ToStorageProxy(IStorage source)
        {
            IStorageProxy storageProxy = source as IStorageProxy;
            if (storageProxy != null)
            {
                return storageProxy;
            }

            IHouseholdProxy householdProxy;
            lock (SyncRoot)
            {
                if (_mappingHousehold == null)
                {
                    householdProxy = source.Household as IHouseholdProxy;
                    if (householdProxy == null && Mapper != null)
                    {
                        householdProxy = Mapper.Map<IHousehold, IHouseholdProxy>(source.Household);
                    }
                }
                else
                {
                    householdProxy = _mappingHousehold;
                }
            }

            IStorageTypeProxy storageTypeProxy = source.StorageType as IStorageTypeProxy;
            if (storageTypeProxy == null && Mapper != null)
            {
                storageTypeProxy = Mapper.Map<IStorageType, IStorageTypeProxy>(source.StorageType);
            }

            return new StorageProxy(householdProxy, source.SortOrder, storageTypeProxy, source.Temperature, source.CreationTime, source.Description)
            {
                Identifier = source.Identifier
            };
        }

        private static IStorageTypeProxy ToStorageTypeProxy(IStorageType source)
        {
            IStorageTypeProxy storageTypeProxy = source as IStorageTypeProxy;
            if (storageTypeProxy != null)
            {
                return storageTypeProxy;
            }

            IRange<int> temperatureRange = new Range<int>(source.TemperatureRange.StartValue, source.TemperatureRange.EndValue);
            storageTypeProxy = new StorageTypeProxy(source.SortOrder, source.Temperature, temperatureRange, source.Creatable, source.Editable, source.Deletable)
            {
                Identifier = source.Identifier
            };
            foreach (ITranslation translation in source.Translations)
            {
                if (translation is ITranslationProxy)
                {
                    storageTypeProxy.TranslationAdd(translation);
                    continue;
                }

                if (Mapper != null)
                {
                    storageTypeProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                }
            }

            return storageTypeProxy;
        }

        private static IHouseholdMemberProxy ToHouseholdMemberProxy(IHouseholdMember source)
        {
            IHouseholdMemberProxy householdMemberProxy = source as IHouseholdMemberProxy;
            if (householdMemberProxy != null)
            {
                return householdMemberProxy;
            }

            householdMemberProxy = new HouseholdMemberProxy(source.MailAddress, source.Membership, source.MembershipExpireTime, source.ActivationCode, source.CreationTime)
            {
                Identifier = source.Identifier,
                ActivationTime = source.ActivationTime,
                PrivacyPolicyAcceptedTime = source.PrivacyPolicyAcceptedTime
            };
            lock (SyncRoot)
            {
                _mappingHouseholdMember = householdMemberProxy;
                try
                {
                    if (_mappingHousehold == null)
                    {
                        foreach (var household in source.Households)
                        {
                            if (household is IHouseholdProxy)
                            {
                                if (householdMemberProxy.Households.Contains(household))
                                {
                                    continue;
                                }

                                householdMemberProxy.HouseholdAdd(household);
                                continue;
                            }

                            if (Mapper != null)
                            {
                                var householdProxy = Mapper.Map<IHousehold, IHouseholdProxy>(household);
                                if (householdMemberProxy.Households.Contains(householdProxy))
                                {
                                    continue;
                                }

                                householdMemberProxy.HouseholdAdd(householdProxy);
                            }
                        }
                    }
                    else if (householdMemberProxy.Households.Contains(_mappingHousehold) == false)
                    {
                        householdMemberProxy.HouseholdAdd(_mappingHousehold);
                    }

                    foreach (var payment in source.Payments)
                    {
                        if (payment is IPaymentProxy)
                        {
                            householdMemberProxy.PaymentAdd(payment);
                            continue;
                        }

                        if (Mapper != null)
                        {
                            householdMemberProxy.PaymentAdd(Mapper.Map<IPayment, IPaymentProxy>(payment));
                        }
                    }
                }
                finally
                {
                    _mappingHouseholdMember = null;
                }
            }

            return householdMemberProxy;
        }

        private static IPaymentProxy ToPaymentProxy(IPayment source)
        {
            IPaymentProxy paymentProxy = source as IPaymentProxy;
            if (paymentProxy != null)
            {
                return paymentProxy;
            }

            IStakeholder stakeholderProxy = null;
            if (source.Stakeholder != null)
            {
                switch (source.Stakeholder.StakeholderType)
                {
                    case StakeholderType.HouseholdMember:
                        lock (SyncRoot)
                        {
                            if (_mappingHouseholdMember != null)
                            {
                                stakeholderProxy = _mappingHouseholdMember;
                            }
                            else if (source.Stakeholder is IHouseholdMember)
                            {
                                stakeholderProxy = source.Stakeholder as IHouseholdMemberProxy;
                                if (stakeholderProxy == null && Mapper != null)
                                {
                                    stakeholderProxy = Mapper.Map<IHouseholdMember, IHouseholdMemberProxy>((IHouseholdMember) source.Stakeholder);
                                }
                            }
                        }

                        break;
                }
            }

            var dataProviderProxy = source.DataProvider as IDataProviderProxy;
            if (dataProviderProxy == null && Mapper != null)
            {
                dataProviderProxy = Mapper.Map<IDataProvider, IDataProviderProxy>(source.DataProvider);
            }

            return new PaymentProxy(stakeholderProxy, dataProviderProxy, source.PaymentTime, source.PaymentReference, source.PaymentReceipt, source.CreationTime)
            {
                Identifier = source.Identifier
            };
        }

        private static IFoodItemProxy ToFoodItemProxy(IFoodItem source)
        {
            IFoodItemProxy foodItemProxy = source as IFoodItemProxy;
            if (foodItemProxy != null)
            {
                return foodItemProxy;
            }

            IFoodGroupProxy primaryFoodGroup = null;
            if (source.PrimaryFoodGroup != null)
            {
                primaryFoodGroup = source.PrimaryFoodGroup as IFoodGroupProxy;
                if (primaryFoodGroup == null && Mapper != null)
                {
                    primaryFoodGroup = Mapper.Map<IFoodGroup, IFoodGroupProxy>(source.PrimaryFoodGroup);
                }
            }

            foodItemProxy = new FoodItemProxy(primaryFoodGroup)
            {
                Identifier = source.Identifier,
                IsActive = source.IsActive
            };
            foreach (var foodGroup in source.FoodGroups)
            {
                if (primaryFoodGroup != null && primaryFoodGroup.Identifier == foodGroup.Identifier)
                {
                    continue;
                }

                if (foodGroup is IFoodGroupProxy)
                {
                    foodItemProxy.FoodGroupAdd(foodGroup);
                    continue;
                }

                if (Mapper != null)
                {
                    foodItemProxy.FoodGroupAdd(Mapper.Map<IFoodGroup, IFoodGroupProxy>(foodGroup));
                }
            }

            foreach (var translation in source.Translations)
            {
                if (translation is ITranslationProxy)
                {
                    foodItemProxy.TranslationAdd(translation);
                    continue;
                }

                if (Mapper != null)
                {
                    foodItemProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                }
            }

            foreach (var foreignKey in source.ForeignKeys)
            {
                if (foreignKey is IForeignKeyProxy)
                {
                    foodItemProxy.ForeignKeyAdd(foreignKey);
                    continue;
                }

                if (Mapper != null)
                {
                    foodItemProxy.ForeignKeyAdd(Mapper.Map<IForeignKey, IForeignKeyProxy>(foreignKey));
                }
            }

            return foodItemProxy;
        }

        private static IFoodGroupProxy ToFoodGroupProxy(IFoodGroup source)
        {
            IFoodGroupProxy foodGroupProxy = source as IFoodGroupProxy;
            if (foodGroupProxy != null)
            {
                return (IFoodGroupProxy) source;
            }

            IFoodGroupProxy parentFoodGroupProxy = null;
            if (source.Parent != null)
            {
                parentFoodGroupProxy = source.Parent as IFoodGroupProxy;
                if (parentFoodGroupProxy == null && Mapper != null)
                {
                    parentFoodGroupProxy = Mapper.Map<IFoodGroup, IFoodGroupProxy>(source.Parent);
                }
            }

            var childFoodGroupProxyCollection = new List<IFoodGroupProxy>();
            foreach (var child in source.Children)
            {
                IFoodGroupProxy childFoodGroupProxy = child as IFoodGroupProxy;
                if (childFoodGroupProxy != null)
                {
                    childFoodGroupProxyCollection.Add((IFoodGroupProxy) child);
                    continue;
                }

                childFoodGroupProxy = new FoodGroupProxy
                {
                    Identifier = source.Identifier,
                    Parent = parentFoodGroupProxy,
                    IsActive = source.IsActive
                };
                childFoodGroupProxyCollection.Add(childFoodGroupProxy);
            }

            foodGroupProxy = new FoodGroupProxy(childFoodGroupProxyCollection)
            {
                Identifier = source.Identifier,
                Parent = parentFoodGroupProxy,
                IsActive = source.IsActive
            };
            foreach (var translation in source.Translations)
            {
                if (translation is ITranslationProxy)
                {
                    foodGroupProxy.TranslationAdd(translation);
                    continue;
                }

                if (Mapper != null)
                {
                    foodGroupProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                }
            }

            foreach (var foreignKey in source.ForeignKeys)
            {
                if (foreignKey is IForeignKeyProxy)
                {
                    foodGroupProxy.ForeignKeyAdd(foreignKey);
                    continue;
                }

                if (Mapper != null)
                {
                    foodGroupProxy.ForeignKeyAdd(Mapper.Map<IForeignKey, IForeignKeyProxy>(foreignKey));
                }
            }

            return foodGroupProxy;
        }

        private static IForeignKeyProxy ToForeignKeyProxy(IForeignKey source)
        {
            IForeignKeyProxy foreignKeyProxy = source as IForeignKeyProxy;
            if (foreignKeyProxy != null)
            {
                return foreignKeyProxy;
            }

            var dataProvider = source.DataProvider as IDataProviderProxy;
            if (dataProvider == null && Mapper != null)
            {
                dataProvider = Mapper.Map<IDataProvider, IDataProviderProxy>(source.DataProvider);
            }

            foreignKeyProxy = new ForeignKeyProxy(dataProvider, source.ForeignKeyForIdentifier, source.ForeignKeyForTypes, source.ForeignKeyValue)
            {
                Identifier = source.Identifier
            };
            return foreignKeyProxy;
        }

        private static IStaticTextProxy ToStaticTextProxy(IStaticText source)
        {
            IStaticTextProxy staticTextProxy = source as IStaticTextProxy;
            if (staticTextProxy != null)
            {
                return staticTextProxy;
            }

            staticTextProxy = new StaticTextProxy(source.Type, source.SubjectTranslationIdentifier, source.BodyTranslationIdentifier)
            {
                Identifier = source.Identifier
            };
            foreach (var translation in source.Translations)
            {
                if (translation is ITranslationProxy)
                {
                    staticTextProxy.TranslationAdd(translation);
                    continue;
                }

                if (Mapper != null)
                {
                    staticTextProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                }
            }

            return staticTextProxy;
        }

        private static IDataProviderProxy ToDataProviderProxy(IDataProvider source)
        {
            IDataProviderProxy dataProviderProxy = source as IDataProviderProxy;
            if (dataProviderProxy != null)
            {
                return dataProviderProxy;
            }

            dataProviderProxy = new DataProviderProxy(source.Name, source.HandlesPayments, source.DataSourceStatementIdentifier)
            {
                Identifier = source.Identifier
            };
            foreach (var translation in source.Translations)
            {
                if (translation is ITranslationProxy)
                {
                    dataProviderProxy.TranslationAdd(translation);
                    continue;
                }

                if (Mapper != null)
                {
                    dataProviderProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                }
            }

            return dataProviderProxy;
        }

        private static ITranslationProxy ToTranslationProxy(ITranslation source)
        {
            ITranslationProxy translationProxy = source as ITranslationProxy;
            if (translationProxy != null)
            {
                return translationProxy;
            }

            var translationInfoProxy = source.TranslationInfo as ITranslationInfoProxy;
            if (translationInfoProxy == null && Mapper != null)
            {
                translationInfoProxy = Mapper.Map<ITranslationInfo, ITranslationInfoProxy>(source.TranslationInfo);
            }

            return new TranslationProxy(source.TranslationOfIdentifier, translationInfoProxy, source.Value)
            {
                Identifier = source.Identifier
            };
        }

        private static ITranslationInfoProxy ToTranslationInfoProxy(ITranslationInfo source)
        {
            ITranslationInfoProxy translationInfoProxy = source as ITranslationInfoProxy;
            return translationInfoProxy ?? new TranslationInfoProxy(source.CultureName) {Identifier = source.Identifier};
        }

        #endregion
    }
}