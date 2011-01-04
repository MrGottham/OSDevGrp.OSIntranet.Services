using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
using IObjectMapper = OSDevGrp.OSIntranet.Infrastructure.Interfaces.IObjectMapper;

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
        #region Constructor

        /// <summary>
        /// Alle mapninger sættes op i den statiske 
        /// constructor af objektet.
        /// </summary>
        static ObjectMapper()
        {
            Mapper.CreateMap<AdresseBase, TelefonlisteView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, TelefonlisteView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, TelefonlisteView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, TelefonlisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil));

            Mapper.CreateMap<Firma, TelefonlisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2));

            Mapper.CreateMap<AdresseBase, DebitorlisteView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, DebitorlisteView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, DebitorlisteView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, DebitorlisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Firma, DebitorlisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<AdresseBase, KreditorlisteView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, KreditorlisteView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, KreditorlisteView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, KreditorlisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Firma, KreditorlisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Betalingsbetingelse, BetalingsbetingelseView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Regnskab, RegnskabslisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Konto, KontoplanView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Kontogruppe, opt => opt.MapFrom(s => s.Kontogruppe))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.KreditPrStatusdato))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato))
                .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato));

            Mapper.CreateMap<Konto, KontoView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Kontogruppe, opt => opt.MapFrom(s => s.Kontogruppe))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.KreditPrStatusdato))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato))
                .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato))
                .ForMember(x => x.Kreditoplysninger, opt => opt.MapFrom(s => s.Kreditoplysninger));

            Mapper.CreateMap<Kreditoplysninger, KreditoplysningerView>()
                .ForMember(x => x.År, opt => opt.MapFrom(s => s.År))
                .ForMember(x => x.Måned, opt => opt.MapFrom(s => s.Måned))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.Kredit));

            Mapper.CreateMap<Budgetkonto, BudgetkontoplanView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Budgetkontogruppe, opt => opt.MapFrom(s => s.Budgetkontogruppe))
                .ForMember(x => x.Budget, opt => opt.MapFrom(s => s.BudgetPrStatusdato))
                .ForMember(x => x.Bogført, opt => opt.MapFrom(s => s.BogførtPrStatusdato))
                .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato));

            Mapper.CreateMap<Budgetkonto, BudgetkontoView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Budgetkontogruppe, opt => opt.MapFrom(s => s.Budgetkontogruppe))
                .ForMember(x => x.Budget, opt => opt.MapFrom(s => s.BudgetPrStatusdato))
                .ForMember(x => x.Bogført, opt => opt.MapFrom(s => s.BogførtPrStatusdato))
                .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato))
                .ForMember(x => x.Budgetoplysninger, opt => opt.MapFrom(s => s.Budgetoplysninger));

            Mapper.CreateMap<Budgetoplysninger, BudgetoplysningerView>()
                .ForMember(x => x.År, opt => opt.MapFrom(s => s.År))
                .ForMember(x => x.Måned, opt => opt.MapFrom(s => s.Måned))
                .ForMember(x => x.Budget, opt => opt.MapFrom(s => s.Budget))
                .ForMember(x => x.Bogført, opt => opt.MapFrom(s => s.BogførtPrStatusdato));

            Mapper.CreateMap<Kontogruppe, KontogruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.ErAktiver, opt => opt.MapFrom(s => s.KontogruppeType == KontogruppeType.Aktiver))
                .ForMember(x => x.ErPassiver, opt => opt.MapFrom(s => s.KontogruppeType == KontogruppeType.Passiver));

            Mapper.CreateMap<Budgetkontogruppe, BudgetkontogruppeView>()
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
