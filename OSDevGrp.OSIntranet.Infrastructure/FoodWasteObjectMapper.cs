using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using AutoMapper;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
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

        private static readonly IMapper Mapper;

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
                    .ForMember(m => m.HouseholdIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Description, opt => opt.MapFrom(s => s.Description));

                config.CreateMap<IHouseholdMember, HouseholdMemberIdentificationView>()
                    .ForMember(m => m.HouseholdMemberIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.MailAddress, opt => opt.MapFrom(s => s.MailAddress));

                config.CreateMap<IHouseholdMember, HouseholdMemberView>()
                    .ForMember(m => m.HouseholdMemberIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.MailAddress, opt => opt.MapFrom(s => s.MailAddress))
                    .ForMember(m => m.Membership, opt => opt.MapFrom(s => s.Membership))
                    .ForMember(m => m.MembershipExpireTime, opt => opt.MapFrom(s => s.MembershipExpireTime))
                    .ForMember(m => m.ActivationTime, opt => opt.MapFrom(s => s.ActivationTime))
                    .ForMember(m => m.IsActivated, opt => opt.MapFrom(s => s.IsActivated))
                    .ForMember(m => m.PrivacyPolicyAcceptedTime, opt => opt.MapFrom(s => s.PrivacyPolicyAcceptedTime))
                    .ForMember(m => m.IsPrivacyPolictyAccepted, opt => opt.MapFrom(s => s.IsPrivacyPolictyAccepted))
                    .ForMember(m => m.CreationTime, opt => opt.MapFrom(s => s.CreationTime))
                    .ForMember(m => m.Households, opt => opt.MapFrom(s => s.Households));

                config.CreateMap<IHouseholdMember, IHouseholdMemberProxy>()
                    .ConstructUsing(m =>
                    {
                        if (m as IHouseholdMemberProxy != null)
                        {
                            return (IHouseholdMemberProxy) m;
                        }
                        var householdMemberProxy = new HouseholdMemberProxy(m.MailAddress, m.Membership, m.MembershipExpireTime, m.ActivationCode, m.CreationTime)
                        {
                            Identifier = m.Identifier,
                            ActivationTime = m.ActivationTime,
                            PrivacyPolicyAcceptedTime = m.PrivacyPolicyAcceptedTime
                        };
                        foreach (var household in m.Households)
                        {
                            // TODO: Convert household to proxy and add (modify the test).
                        }
                        foreach (var payment in m.Payments)
                        {
                            if (payment as IPaymentProxy != null)
                            {
                                householdMemberProxy.PaymentAdd(payment);
                                continue;
                            }
                            var dataProviderProxy = payment.DataProvider as IDataProviderProxy;
                            if (dataProviderProxy == null && Mapper != null)
                            {
                                dataProviderProxy = Mapper.Map<IDataProvider, IDataProviderProxy>(payment.DataProvider);
                            }
                            var paymentProxy = new PaymentProxy(householdMemberProxy, dataProviderProxy, payment.PaymentTime, payment.PaymentReference, payment.PaymentReceipt, payment.CreationTime)
                            {
                                Identifier = payment.Identifier
                            };
                            householdMemberProxy.PaymentAdd(paymentProxy);
                        }
                        return householdMemberProxy;
                    });

                config.CreateMap<IPayment, IPaymentProxy>()
                    .ConstructUsing(m =>
                    {
                        if (m as IPaymentProxy != null)
                        {
                            return (IPaymentProxy) m;
                        }
                        IStakeholder stakeholderProxy = null;
                        if (m.Stakeholder as IHouseholdMember != null)
                        {
                            stakeholderProxy = m.Stakeholder as IHouseholdMemberProxy;
                            if (stakeholderProxy == null && Mapper != null)
                            {
                                stakeholderProxy = Mapper.Map<IHouseholdMember, IHouseholdMemberProxy>((IHouseholdMember) m.Stakeholder);
                            }
                        }
                        var dataProviderProxy = m.DataProvider as IDataProviderProxy;
                        if (dataProviderProxy == null && Mapper != null)
                        {
                            dataProviderProxy = Mapper.Map<IDataProvider, IDataProviderProxy>(m.DataProvider);
                        }
                        return new PaymentProxy(stakeholderProxy, dataProviderProxy, m.PaymentTime, m.PaymentReference, m.PaymentReceipt, m.CreationTime)
                        {
                            Identifier = m.Identifier
                        };
                    });

                config.CreateMap<IFoodItemCollection, FoodItemCollectionView>()
                    .ForMember(m => m.FoodItems, opt => opt.MapFrom<IEnumerable<IFoodItem>>(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodItemCollection, FoodItemCollectionSystemView>()
                    .ForMember(m => m.FoodItems, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodItem, FoodItemIdentificationView>()
                    .ForMember(m => m.FoodItemIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty));

                config.CreateMap<IFoodItem, FoodItemView>()
                    .ForMember(m => m.FoodItemIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.PrimaryFoodGroup, opt => opt.MapFrom(s => s.PrimaryFoodGroup))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s.FoodGroups));

                config.CreateMap<IFoodItem, FoodItemSystemView>()
                    .ForMember(m => m.FoodItemIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.PrimaryFoodGroup, opt => opt.MapFrom(s => s.PrimaryFoodGroup))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s.FoodGroups))
                    .ForMember(m => m.Translations, opt => opt.MapFrom(s => s.Translations))
                    .ForMember(m => m.ForeignKeys, opt => opt.MapFrom(s => s.ForeignKeys));

                config.CreateMap<IFoodItem, IFoodItemProxy>()
                    .ConvertUsing(m =>
                    {
                        if (m as IFoodItemProxy != null)
                        {
                            return (IFoodItemProxy) m;
                        }
                        IFoodGroupProxy primaryFoodGroup = null;
                        if (m.PrimaryFoodGroup != null)
                        {
                            primaryFoodGroup = m.PrimaryFoodGroup as IFoodGroupProxy;
                            if (primaryFoodGroup == null && Mapper != null)
                            {
                                primaryFoodGroup = Mapper.Map<IFoodGroup, IFoodGroupProxy>(m.PrimaryFoodGroup);
                            }
                        }
                        var foodItemProxy = new FoodItemProxy(primaryFoodGroup)
                        {
                            Identifier = m.Identifier,
                            IsActive = m.IsActive
                        };
                        foreach (var foodGroup in m.FoodGroups)
                        {
                            if (primaryFoodGroup != null && primaryFoodGroup.Identifier == foodGroup.Identifier)
                            {
                                continue;
                            }
                            if (foodGroup as IFoodGroupProxy != null)
                            {
                                foodItemProxy.FoodGroupAdd(foodGroup);
                                continue;
                            }
                            if (Mapper != null)
                            {
                                foodItemProxy.FoodGroupAdd(Mapper.Map<IFoodGroup, IFoodGroupProxy>(foodGroup));
                            }
                        }
                        foreach (var translation in m.Translations)
                        {
                            if (translation as ITranslationProxy != null)
                            {
                                foodItemProxy.TranslationAdd(translation);
                                continue;
                            }
                            if (Mapper != null)
                            {
                                foodItemProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                            }
                        }
                        foreach (var foreignKey in m.ForeignKeys)
                        {
                            if (foreignKey as IForeignKeyProxy != null)
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
                    });

                config.CreateMap<IFoodGroupCollection, FoodGroupTreeView>()
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodGroupCollection, FoodGroupTreeSystemView>()
                    .ForMember(m => m.FoodGroups, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider));

                config.CreateMap<IFoodGroup, FoodGroupIdentificationView>()
                    .ForMember(m => m.FoodGroupIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty));

                config.CreateMap<IFoodGroup, FoodGroupView>()
                    .ForMember(m => m.FoodGroupIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.Parent, opt => opt.MapFrom(s => s.Parent))
                    .ForMember(m => m.Children, opt => opt.MapFrom(s => s.Children));

                config.CreateMap<IFoodGroup, FoodGroupSystemView>()
                    .ForMember(m => m.FoodGroupIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Translation != null ? s.Translation.Value : string.Empty))
                    .ForMember(m => m.IsActive, opt => opt.MapFrom(s => s.IsActive))
                    .ForMember(m => m.Parent, opt => opt.MapFrom(s => s.Parent))
                    .ForMember(m => m.Translations, opt => opt.MapFrom(s => s.Translations))
                    .ForMember(m => m.ForeignKeys, opt => opt.MapFrom(s => s.ForeignKeys))
                    .ForMember(m => m.Children, opt => opt.MapFrom(s => s.Children));

                config.CreateMap<IFoodGroup, IFoodGroupProxy>()
                    .ConvertUsing(m =>
                    {
                        if (m as IFoodGroupProxy != null)
                        {
                            return (IFoodGroupProxy) m;
                        }
                        IFoodGroupProxy parentFoodGroupProxy = null;
                        if (m.Parent != null)
                        {
                            parentFoodGroupProxy = m.Parent as IFoodGroupProxy;
                            if (parentFoodGroupProxy == null && Mapper != null)
                            {
                                parentFoodGroupProxy = Mapper.Map<IFoodGroup, IFoodGroupProxy>(m.Parent);
                            }
                        }
                        var childFoodGroupProxyCollection = new List<IFoodGroupProxy>();
                        foreach (var child in m.Children)
                        {
                            if (child as IFoodGroupProxy != null)
                            {
                                childFoodGroupProxyCollection.Add((IFoodGroupProxy) child);
                                continue;
                            }
                            var childFoodGroupProxy = new FoodGroupProxy
                            {
                                Identifier = m.Identifier,
                                Parent = parentFoodGroupProxy,
                                IsActive = m.IsActive
                            };
                            childFoodGroupProxyCollection.Add(childFoodGroupProxy);
                        }
                        var foodGroupProxy = new FoodGroupProxy(childFoodGroupProxyCollection)
                        {
                            Identifier = m.Identifier,
                            Parent = parentFoodGroupProxy,
                            IsActive = m.IsActive
                        };
                        foreach (var translation in m.Translations)
                        {
                            if (translation as ITranslationProxy != null)
                            {
                                foodGroupProxy.TranslationAdd(translation);
                                continue;
                            }
                            if (Mapper != null)
                            {
                                foodGroupProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                            }
                        }
                        foreach (var foreignKey in m.ForeignKeys)
                        {
                            if (foreignKey as IForeignKeyProxy != null)
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
                    });

                config.CreateMap<IForeignKey, ForeignKeyView>()
                    .ForMember(m => m.ForeignKeyIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider))
                    .ForMember(m => m.ForeignKeyForIdentifier, opt => opt.MapFrom(s => s.ForeignKeyForIdentifier))
                    .ForMember(m => m.ForeignKey, opt => opt.MapFrom(s => s.ForeignKeyValue));

                config.CreateMap<IForeignKey, ForeignKeySystemView>()
                    .ForMember(m => m.ForeignKeyIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.DataProvider, opt => opt.MapFrom(s => s.DataProvider))
                    .ForMember(m => m.ForeignKeyForIdentifier, opt => opt.MapFrom(s => s.ForeignKeyForIdentifier))
                    .ForMember(m => m.ForeignKey, opt => opt.MapFrom(s => s.ForeignKeyValue));

                config.CreateMap<IForeignKey, IForeignKeyProxy>()
                    .ConvertUsing(m =>
                    {
                        if (m as IForeignKeyProxy != null)
                        {
                            return (IForeignKeyProxy) m;
                        }
                        var dataProvider = m.DataProvider as IDataProviderProxy;
                        if (dataProvider == null && Mapper != null)
                        {
                            dataProvider = Mapper.Map<IDataProvider, IDataProviderProxy>(m.DataProvider);
                        }
                        var foreignKeyProxy = new ForeignKeyProxy(dataProvider, m.ForeignKeyForIdentifier, m.ForeignKeyForTypes, m.ForeignKeyValue)
                        {
                            Identifier = m.Identifier
                        };
                        return foreignKeyProxy;
                    });

                config.CreateMap<IStaticText, StaticTextView>()
                    .ForMember(m => m.StaticTextIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.StaticTextType, opt => opt.MapFrom(s => (int) s.Type))
                    .ForMember(m => m.SubjectTranslation, opt => opt.MapFrom(s => s.SubjectTranslation != null ? s.SubjectTranslation.Value : string.Empty))
                    .ForMember(m => m.BodyTranslation, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? s.BodyTranslation != null ? s.BodyTranslation.Value : string.Empty : null));

                config.CreateMap<IStaticText, StaticTextSystemView>()
                    .ForMember(m => m.StaticTextIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.StaticTextType, opt => opt.MapFrom(s => (int) s.Type))
                    .ForMember(m => m.SubjectTranslationIdentifier, opt => opt.MapFrom(s => s.SubjectTranslationIdentifier))
                    .ForMember(m => m.SubjectTranslation, opt => opt.MapFrom(s => s.SubjectTranslation != null ? s.SubjectTranslation.Value : string.Empty))
                    .ForMember(m => m.SubjectTranslations, opt => opt.MapFrom(s => s.SubjectTranslations))
                    .ForMember(m => m.BodyTranslationIdentifier, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? (Guid?) s.BodyTranslationIdentifier.Value : null))
                    .ForMember(m => m.BodyTranslation, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? s.BodyTranslation != null ? s.BodyTranslation.Value : string.Empty : null))
                    .ForMember(m => m.BodyTranslations, opt => opt.MapFrom(s => s.BodyTranslationIdentifier.HasValue ? s.BodyTranslations : null));

                config.CreateMap<IStaticText, IStaticTextProxy>()
                    .ConvertUsing(m =>
                    {
                        if (m as IStaticTextProxy != null)
                        {
                            return (IStaticTextProxy) m;
                        }
                        var staticTextProxy = new StaticTextProxy(m.Type, m.SubjectTranslationIdentifier, m.BodyTranslationIdentifier)
                        {
                            Identifier = m.Identifier
                        };
                        foreach (var translation in m.Translations)
                        {
                            if (translation as ITranslationProxy != null)
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
                    });

                config.CreateMap<IDataProvider, DataProviderView>()
                    .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(m => m.DataSourceStatement, opt => opt.MapFrom(s => s.DataSourceStatement != null ? s.DataSourceStatement.Value : string.Empty));

                config.CreateMap<IDataProvider, DataProviderSystemView>()
                    .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                    .ForMember(m => m.HandlesPayments, opt => opt.MapFrom(s => s.HandlesPayments))
                    .ForMember(m => m.DataSourceStatementIdentifier, opt => opt.MapFrom(s => s.DataSourceStatementIdentifier))
                    .ForMember(m => m.DataSourceStatements, opt => opt.MapFrom(s => s.DataSourceStatements));

                config.CreateMap<IDataProvider, IDataProviderProxy>()
                    .ConvertUsing(m =>
                    {
                        if (m as IDataProviderProxy != null)
                        {
                            return (IDataProviderProxy) m;
                        }
                        var dataProviderProxy = new DataProviderProxy(m.Name, m.HandlesPayments, m.DataSourceStatementIdentifier)
                        {
                            Identifier = m.Identifier
                        };
                        foreach (var translation in m.Translations)
                        {
                            if (translation as ITranslationProxy != null)
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
                    });

                config.CreateMap<ITranslation, TranslationSystemView>()
                    .ForMember(m => m.TranslationIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.TranslationOfIdentifier, opt => opt.MapFrom(s => s.TranslationOfIdentifier))
                    .ForMember(m => m.TranslationInfo, opt => opt.MapFrom(s => s.TranslationInfo))
                    .ForMember(m => m.Translation, opt => opt.MapFrom(s => s.Value));

                config.CreateMap<ITranslation, ITranslationProxy>()
                    .ConvertUsing(m =>
                    {
                        if (m as ITranslationProxy != null)
                        {
                            return (ITranslationProxy) m;
                        }
                        var translationInfoProxy = m.TranslationInfo as ITranslationInfoProxy;
                        if (translationInfoProxy == null && Mapper != null)
                        {
                            translationInfoProxy = Mapper.Map<ITranslationInfo, ITranslationInfoProxy>(m.TranslationInfo);
                        }
                        return new TranslationProxy(m.TranslationOfIdentifier, translationInfoProxy, m.Value)
                        {
                            Identifier = m.Identifier
                        };
                    });

                config.CreateMap<ITranslationInfo, TranslationInfoSystemView>()
                    .ForMember(m => m.TranslationInfoIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                    .ForMember(m => m.CultureName, opt => opt.MapFrom(s => s.CultureName));

                config.CreateMap<ITranslationInfo, ITranslationInfoProxy>()
                    .ConvertUsing(m => (m as ITranslationInfoProxy) != null ? (ITranslationInfoProxy) m : new TranslationInfoProxy(m.CultureName) {Identifier = m.Identifier});

                config.CreateMap<IIdentifiable, ServiceReceiptResponse>()
                    .ForMember(m => m.Identifier, opt => opt.MapFrom(s => s.Identifier))
                    .ForMember(m => m.EventDate, opt => opt.MapFrom(s => DateTime.Now));

                config.CreateMap<bool, BooleanResultResponse>()
                    .ForMember(m => m.Result, opt => opt.MapFrom(s => s))
                    .ForMember(m => m.EventDate, opt => opt.MapFrom(s => DateTime.Now));
            });

            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Properties

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
                throw new ArgumentNullException("source");
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

        #endregion
    }
}
