﻿using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using IObjectMapper = OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.IObjectMapper;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Infrastructure
{
    /// <summary>
    /// Implementering af objektmapperen, der bruger AutoMapper.
    /// Objektmapperen mapperobjekter fra en type til en andentype,
    /// som typisk bruges ved konvertering af objekter i 
    /// domænemodellen til objekter i viewmodellen.
    /// </summary>
    public class ObjectMapper : IObjectMapper
    {
        #region Constructor

        /// <summary>
        /// Alle mapninger sættes op i den statiske
        /// constructor af objektet.
        /// </summary>
        static ObjectMapper()
        {
            Mapper.CreateMap<AdresseBase, AdresselisteView>()
                .ConvertUsing(s =>
                                  {
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, AdresselisteView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, AdresselisteView>(s as Firma);
                                      }
                                      throw new DataAccessSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, AdresselisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adressegruppe, opt => opt.MapFrom(s => s.Adressegruppe))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.Telefon, opt => opt.MapFrom(s => s.Telefon))
                .ForMember(x => x.Mobil, opt => opt.MapFrom(s => s.Mobil))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse));

            Mapper.CreateMap<Firma, AdresselisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adressegruppe, opt => opt.MapFrom(s => s.Adressegruppe))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.Telefon, opt => opt.MapFrom(s => s.Telefon1))
                .ForMember(x => x.Mobil, opt => opt.Ignore())
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse));

            Mapper.CreateMap<Postnummer, PostnummerView>()
                .ForMember(x => x.Landekode, opt => opt.MapFrom(s => s.Landekode))
                .ForMember(x => x.Postnummer, opt => opt.MapFrom(s => s.Postnr))
                .ForMember(x => x.Bynavn, opt => opt.MapFrom(s => s.By));

            Mapper.CreateMap<Adressegruppe, AdressegruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.AdressegruppeOswebdb, opt => opt.MapFrom(s => s.AdressegruppeOswebdb));

            Mapper.CreateMap<Betalingsbetingelse, BetalingsbetingelseView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.AssertConfigurationIsValid();
        }

        #endregion

        #region IObjectMapper Members

        /// <summary>
        /// Mapper et objekt af typen TSource til et objekt af typen TDestination.
        /// </summary>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        #endregion
    }
}
