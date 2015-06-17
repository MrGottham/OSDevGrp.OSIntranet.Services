using System;
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
            // TODO: Fix this and fix unit tests.
            Mapper.CreateMap<IFoodGroup, object>()
                .ConvertUsing(s => new object());

            Mapper.CreateMap<IDataProvider, DataProviderSystemView>()
                .ForMember(m => m.DataProviderIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                .ForMember(m => m.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(m => m.DataSourceStatementIdentifier, opt => opt.MapFrom(s => s.DataSourceStatementIdentifier))
                .ForMember(m => m.DataSourceStatements, opt => opt.MapFrom(s => Mapper.Map<IEnumerable<ITranslation>, IEnumerable<TranslationSystemView>>(s.DataSourceStatements)));

            Mapper.CreateMap<IDataProvider, IDataProviderProxy>()
                .ConvertUsing(m =>
                {
                    var dataProviderProxy = new DataProviderProxy(m.Name, m.DataSourceStatementIdentifier)
                    {
                        Identifier = m.Identifier,
                    };
                    foreach (var translation in m.Translations)
                    {
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
                    var translationInfoProxy = Mapper.Map<ITranslationInfo, ITranslationInfoProxy>(m.TranslationInfo);
                    return new TranslationProxy(m.TranslationOfIdentifier, translationInfoProxy, m.Value)
                    {
                        Identifier = m.Identifier
                    };
                });

            Mapper.CreateMap<ITranslationInfo, TranslationInfoSystemView>()
                .ForMember(m => m.TranslationInfoIdentifier, opt => opt.MapFrom(s => s.Identifier.HasValue ? s.Identifier.Value : Guid.Empty))
                .ForMember(m => m.CultureName, opt => opt.MapFrom(s => s.CultureName));

            Mapper.CreateMap<ITranslationInfo, ITranslationInfoProxy>()
                .ConvertUsing(m => new TranslationInfoProxy(m.CultureName) {Identifier = m.Identifier});

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
