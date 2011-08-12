using System;
using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
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

            Mapper.CreateMap<AdresseBase, AdressekontolisteView>()
                .ConvertUsing(s =>
                                  {
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, AdressekontolisteView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, AdressekontolisteView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, AdressekontolisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Firma, AdressekontolisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<AdresseBase, AdressekontoView>()
                .ConvertUsing(s =>
                                  {
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, AdressekontoView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, AdressekontoView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, AdressekontoView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Betalingsbetingelse, opt =>
                                                           {
                                                               opt.Condition(s => s.Betalingsbetingelse != null);
                                                               opt.MapFrom(s => s.Betalingsbetingelse);
                                                           })
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Firma, AdressekontoView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Betalingsbetingelse, opt =>
                                                           {
                                                               opt.Condition(s => s.Betalingsbetingelse != null);
                                                               opt.MapFrom(s => s.Betalingsbetingelse);
                                                           })
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<AdresseBase, DebitorlisteView>()
                .ConvertUsing(s =>
                                  {
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

            Mapper.CreateMap<AdresseBase, DebitorView>()
                .ConvertUsing(s =>
                                  {
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, DebitorView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, DebitorView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, DebitorView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Betalingsbetingelse, opt =>
                                                           {
                                                               opt.Condition(s => s.Betalingsbetingelse != null);
                                                               opt.MapFrom(s => s.Betalingsbetingelse);
                                                           })
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Firma, DebitorView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Betalingsbetingelse, opt =>
                                                           {
                                                               opt.Condition(s => s.Betalingsbetingelse != null);
                                                               opt.MapFrom(s => s.Betalingsbetingelse);
                                                           })
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<AdresseBase, KreditorlisteView>()
                .ConvertUsing(s =>
                                  {
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

            Mapper.CreateMap<AdresseBase, KreditorView>()
                .ConvertUsing(s =>
                                  {
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, KreditorView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, KreditorView>(s as Firma);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, KreditorView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Betalingsbetingelse, opt =>
                                                           {
                                                               opt.Condition(s => s.Betalingsbetingelse != null);
                                                               opt.MapFrom(s => s.Betalingsbetingelse);
                                                           })
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Firma, KreditorView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Betalingsbetingelse, opt =>
                                                           {
                                                               opt.Condition(s => s.Betalingsbetingelse != null);
                                                               opt.MapFrom(s => s.Betalingsbetingelse);
                                                           })
                .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

            Mapper.CreateMap<Betalingsbetingelse, BetalingsbetingelseView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Regnskab, RegnskabslisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Brevhoved, opt => opt.MapFrom(s => s.Brevhoved));

            Mapper.CreateMap<KontoBase, KontoBaseView>()
                .ConvertUsing(s =>
                                  {
                                      var objectMapper = new ObjectMapper();
                                      if (s is Konto)
                                      {
                                          return objectMapper.Map<Konto, KontoView>(s as Konto);
                                      }
                                      if (s is Budgetkonto)
                                      {
                                          return objectMapper.Map<Budgetkonto, BudgetkontoView>(s as Budgetkonto);
                                      }
                                      throw new IntranetSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

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

            Mapper.CreateMap<Bogføringslinje, BogføringslinjeView>()
                .ForMember(x => x.Løbenr, opt => opt.MapFrom(s => s.Løbenummer))
                .ForMember(x => x.Konto, opt => opt.MapFrom(s => s.Konto))
                .ForMember(x => x.Budgetkonto, opt =>
                                                   {
                                                       opt.Condition(s => s.Budgetkonto != null);
                                                       opt.MapFrom(s => s.Budgetkonto);
                                                   })
                .ForMember(x => x.Adressekonto, opt =>
                                                    {
                                                        opt.Condition(s => s.Adresse != null);
                                                        opt.MapFrom(s => s.Adresse);
                                                    })
                .ForMember(x => x.Dato, opt => opt.MapFrom(s => s.Dato))
                .ForMember(x => x.Bilag, opt => opt.MapFrom(s => s.Bilag))
                .ForMember(x => x.Tekst, opt => opt.MapFrom(s => s.Tekst))
                .ForMember(x => x.Debit, opt => opt.MapFrom(s => s.Debit))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.Kredit));

            Mapper.CreateMap<IBogføringsresultat, BogføringslinjeOpretResponse>()
                .ForMember(x => x.Løbenr, opt => opt.MapFrom(s => s.Bogføringslinje.Løbenummer))
                .ForMember(x => x.Konto, opt => opt.MapFrom(s => s.Bogføringslinje.Konto))
                .ForMember(x => x.Budgetkonto, opt =>
                                                   {
                                                       opt.Condition(s => s.Bogføringslinje.Budgetkonto != null);
                                                       opt.MapFrom(s => s.Bogføringslinje.Budgetkonto);
                                                   })
                .ForMember(x => x.Adressekonto, opt =>
                                                    {
                                                        opt.Condition(s => s.Bogføringslinje.Adresse != null);
                                                        opt.MapFrom(s => s.Bogføringslinje.Adresse);
                                                    })
                .ForMember(x => x.Dato, opt => opt.MapFrom(s => s.Bogføringslinje.Dato))
                .ForMember(x => x.Bilag, opt => opt.MapFrom(s => s.Bogføringslinje.Bilag))
                .ForMember(x => x.Tekst, opt => opt.MapFrom(s => s.Bogføringslinje.Tekst))
                .ForMember(x => x.Debit, opt => opt.MapFrom(s => s.Bogføringslinje.Debit))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.Bogføringslinje.Kredit))
                .ForMember(x => x.Advarsler, opt => opt.MapFrom(s => s.Advarsler));

            Mapper.CreateMap<IBogføringsadvarsel, BogføringsadvarselResponse>()
                .ForMember(x => x.Advarsel, opt => opt.MapFrom(s => s.Advarsel))
                .ForMember(x => x.Konto, opt => opt.MapFrom(s => s.Konto))
                .ForMember(x => x.Beløb, opt => opt.MapFrom(s => s.Beløb));

            Mapper.CreateMap<Kontogruppe, KontogruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.ErAktiver, opt => opt.MapFrom(s => s.KontogruppeType == KontogruppeType.Aktiver))
                .ForMember(x => x.ErPassiver, opt => opt.MapFrom(s => s.KontogruppeType == KontogruppeType.Passiver));

            Mapper.CreateMap<Budgetkontogruppe, BudgetkontogruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Brevhoved, BrevhovedreferenceView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Brevhoved, BrevhovedView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Linje1, opt => opt.MapFrom(s => s.Linje1))
                .ForMember(x => x.Linje2, opt => opt.MapFrom(s => s.Linje2))
                .ForMember(x => x.Linje3, opt => opt.MapFrom(s => s.Linje3))
                .ForMember(x => x.Linje4, opt => opt.MapFrom(s => s.Linje4))
                .ForMember(x => x.Linje5, opt => opt.MapFrom(s => s.Linje5))
                .ForMember(x => x.Linje6, opt => opt.MapFrom(s => s.Linje6))
                .ForMember(x => x.Linje7, opt => opt.MapFrom(s => s.Linje7))
                .ForMember(x => x.CvrNr, opt => opt.MapFrom(s => s.CvrNr));

            Mapper.AssertConfigurationIsValid();
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
                throw new ArgumentNullException("source");
            }
            return Mapper.Map<TSource, TDestination>(source);
        }

        #endregion
    }
}
