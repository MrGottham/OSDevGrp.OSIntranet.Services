﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        #region Constructor

        /// <summary>
        /// Creates an object mapper which can map objects in the food waste domain.
        /// </summary>
        static FoodWasteObjectMapper()
        {
            Mapper.CreateMap<IFoodGroup, IFoodGroupProxy>()
                .ConvertUsing(m =>
                {
                    if (m as IFoodGroupProxy != null)
                    {
                        return (IFoodGroupProxy) m;
                    }
                    var parentFoodGroupProxy = m.Parent == null ? null : (m.Parent as IFoodGroupProxy) != null ? (IFoodGroupProxy) m.Parent : Mapper.Map<IFoodGroup, IFoodGroupProxy>(m.Parent);
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
                        foodGroupProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                    }
                    foreach (var foreignKey in m.ForeignKeys)
                    {
                        if (foreignKey as IForeignKeyProxy != null)
                        {
                            foodGroupProxy.ForeignKeyAdd(foreignKey);
                            continue;
                        }
                        foodGroupProxy.ForeignKeyAdd(Mapper.Map<IForeignKey, IForeignKeyProxy>(foreignKey));
                    }
                    return foodGroupProxy;
                });

            Mapper.CreateMap<IForeignKey, IForeignKeyProxy>()
                .ConvertUsing(m =>
                {
                    if (m as IForeignKeyProxy != null)
                    {
                        return (IForeignKeyProxy) m;
                    }
                    var dataProider = (m.DataProvider as IDataProviderProxy) != null ? (IDataProviderProxy) m.DataProvider : Mapper.Map<IDataProvider, IDataProviderProxy>(m.DataProvider);
                    var foreignKeyProxy = new ForeignKeyProxy(dataProider, m.ForeignKeyForIdentifier, m.ForeignKeyForTypes, m.ForeignKeyValue)
                    {
                        Identifier = m.Identifier
                    };
                    return foreignKeyProxy;
                });

            Mapper.CreateMap<IDataProvider, DataProviderView>()
                .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(m => m.DataSourceStatement, opt => opt.MapFrom(s => s.DataSourceStatement != null ? s.DataSourceStatement.Value : string.Empty));

            Mapper.CreateMap<IDataProvider, DataProviderSystemView>()
                .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(m => m.DataSourceStatementIdentifier, opt => opt.MapFrom(s => s.DataSourceStatementIdentifier))
                .ForMember(m => m.DataSourceStatements, opt => opt.MapFrom(s => Mapper.Map<IEnumerable<ITranslation>, IEnumerable<TranslationSystemView>>(s.DataSourceStatements)));

            Mapper.CreateMap<IDataProvider, IDataProviderProxy>()
                .ConvertUsing(m =>
                {
                    if (m as IDataProviderProxy != null)
                    {
                        return (IDataProviderProxy) m;
                    }
                    var dataProviderProxy = new DataProviderProxy(m.Name, m.DataSourceStatementIdentifier)
                    {
                        Identifier = m.Identifier,
                    };
                    foreach (var translation in m.Translations)
                    {
                        if (translation as ITranslationProxy != null)
                        {
                            dataProviderProxy.TranslationAdd(translation);
                            continue;
                        }
                        dataProviderProxy.TranslationAdd(Mapper.Map<ITranslation, ITranslationProxy>(translation));
                    }
                    return dataProviderProxy;
                });

            Mapper.CreateMap<ITranslation, TranslationSystemView>()
                .ForMember(m => m.TranslationIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                .ForMember(m => m.TranslationOfIdentifier, opt => opt.MapFrom(s => s.TranslationOfIdentifier))
                .ForMember(m => m.TranslationInfo, opt => opt.MapFrom(s => Mapper.Map<ITranslationInfo, TranslationInfoSystemView>(s.TranslationInfo)))
                .ForMember(m => m.Translation, opt => opt.MapFrom(s => s.Value));

            Mapper.CreateMap<ITranslation, ITranslationProxy>()
                .ConvertUsing(m =>
                {
                    if (m as ITranslationProxy != null)
                    {
                        return (ITranslationProxy) m;
                    }
                    var translationInfoProxy = (m.TranslationInfo as ITranslationInfoProxy) != null ? (ITranslationInfoProxy) m.TranslationInfo : Mapper.Map<ITranslationInfo, ITranslationInfoProxy>(m.TranslationInfo);
                    return new TranslationProxy(m.TranslationOfIdentifier, translationInfoProxy, m.Value)
                    {
                        Identifier = m.Identifier
                    };
                });

            Mapper.CreateMap<ITranslationInfo, TranslationInfoSystemView>()
                .ForMember(m => m.TranslationInfoIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                .ForMember(m => m.CultureName, opt => opt.MapFrom(s => s.CultureName));

            Mapper.CreateMap<ITranslationInfo, ITranslationInfoProxy>()
                .ConvertUsing(m => (m as ITranslationInfoProxy) != null ? (ITranslationInfoProxy) m : new TranslationInfoProxy(m.CultureName) {Identifier = m.Identifier});

            Mapper.CreateMap<IIdentifiable, ServiceReceiptResponse>()
                .ConvertUsing(m => new ServiceReceiptResponse
                {
                    Identifier = m.Identifier,
                    EventDate = DateTime.Now
                });

            Mapper.AssertConfigurationIsValid();
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
