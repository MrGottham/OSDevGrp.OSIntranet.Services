using System;
using AutoMapper;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using IObjectMapper=OSDevGrp.OSIntranet.Infrastructure.Interfaces.IObjectMapper;

namespace OSDevGrp.OSIntranet.Infrastructure
{
    /// <summary>
    /// Implementering af objektmapperen, der bruger AutoMapper. 
    /// Objektmapperen mapperobjekter fra en type til en andentype, 
    /// som typisk bruges ved konvertering af objekter i 
    /// domænemodellen til objekter i viewmodellen. 
    /// </summary>
    public class ObjectMapper : IObjectMapper
    {
        #region private variables

        private static readonly IMapper Mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Alle mapninger sættes op i den statiske 
        /// constructor af objektet.
        /// </summary>
        static ObjectMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<IBrugeraftale, KalenderbrugerAftaleView>()
                    .ForMember(x => x.System, opt => opt.MapFrom(s => s.Aftale.System))
                    .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Aftale.Id))
                    .ForMember(x => x.FraTidspunkt, opt => opt.MapFrom(s => s.Aftale.FraTidspunkt))
                    .ForMember(x => x.TilTidspunkt, opt => opt.MapFrom(s => s.Aftale.TilTidspunkt))
                    .ForMember(x => x.Emne, opt => opt.MapFrom(s => s.Aftale.Emne))
                    .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Aftale.Notat))
                    .ForMember(x => x.Offentlig, opt => opt.MapFrom(s => s.Offentligtgørelse))
                    .ForMember(x => x.Privat, opt => opt.MapFrom(s => s.Privat))
                    .ForMember(x => x.Alarm, opt => opt.MapFrom(s => s.Alarm))
                    .ForMember(x => x.Udført, opt => opt.MapFrom(s => s.Udført))
                    .ForMember(x => x.Eksporteres, opt => opt.MapFrom(s => s.Eksporter))
                    .ForMember(x => x.Eksporteret, opt => opt.MapFrom(s => s.Eksporteret))
                    .ForMember(x => x.Deltagere, opt => opt.MapFrom(s => s.Aftale.Deltagere));

                config.CreateMap<IBrugeraftale, KalenderbrugerView>()
                    .ForMember(x => x.System, opt => opt.MapFrom(s => s.Bruger.System))
                    .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Bruger.Id))
                    .ForMember(x => x.Initialer, opt => opt.MapFrom(s => s.Bruger.Initialer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Bruger.Navn));

                config.CreateMap<IBruger, KalenderbrugerView>()
                    .ForMember(x => x.System, opt => opt.MapFrom(s => s.System))
                    .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
                    .ForMember(x => x.Initialer, opt => opt.MapFrom(s => s.Initialer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

                config.CreateMap<ISystem, SystemView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Titel, opt => opt.MapFrom(s => s.Titel))
                    .ForMember(x => x.Kalender, opt => opt.MapFrom(s => s.Kalender));
            });

            mapperConfiguration.AssertConfigurationIsValid();

            Mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region IObjectMapper Members

        /// <summary>
        /// Mapper et objekt af typen TSource til et objekt af typen TDestination.
        /// </summary>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            if (Equals(source, null))
            {
                throw new ArgumentNullException(nameof(source));
            }
            return Mapper.Map<TSource, TDestination>(source);
        }

        #endregion
    }
}