using System;
using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;
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
                config.CreateMap<AdresseBase, TelefonlisteView>()
                    .ConvertUsing(s => ToTelefonlisteView(s));

                config.CreateMap<Person, TelefonlisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil));

                config.CreateMap<Firma, TelefonlisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2));

                config.CreateMap<AdresseBase, AdressekontolisteView>()
                    .ConvertUsing(s => ToAdressekontolisteView(s));

                config.CreateMap<Person, AdressekontolisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

                config.CreateMap<Firma, AdressekontolisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

                config.CreateMap<AdresseBase, AdressekontoView>()
                    .ConvertUsing(s => ToAdressekontoView(s));

                config.CreateMap<Person, AdressekontoView>()
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

                config .CreateMap<Firma, AdressekontoView>()
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

                config.CreateMap<AdresseBase, DebitorlisteView>()
                    .ConvertUsing(s => ToDebitorlisteView(s));

                config.CreateMap<Person, DebitorlisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

                config.CreateMap<Firma, DebitorlisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

                config.CreateMap<AdresseBase, DebitorView>()
                    .ConvertUsing(s => ToDebitorView(s));

                config.CreateMap<Person, DebitorView>()
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

                config.CreateMap<Firma, DebitorView>()
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

                config.CreateMap<AdresseBase, KreditorlisteView>()
                    .ConvertUsing(s => ToKreditorlisteView(s));

                config.CreateMap<Person, KreditorlisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? s.Mobil : s.Telefon))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon) ? null : s.Mobil))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

                config.CreateMap<Firma, KreditorlisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.PrimærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? s.Telefon2 : s.Telefon1))
                    .ForMember(x => x.SekundærTelefon, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Telefon1) ? null : s.Telefon2))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato));

                config.CreateMap<AdresseBase, KreditorView>()
                    .ConvertUsing(s => ToKreditorView(s));

                config.CreateMap<Person, KreditorView>()
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

                config.CreateMap<Firma, KreditorView>()
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

                config.CreateMap<Betalingsbetingelse, BetalingsbetingelseView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

                config.CreateMap<Regnskab, RegnskabslisteView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.Brevhoved, opt => opt.MapFrom(s => s.Brevhoved));

                config.CreateMap<KontoBase, KontoBaseView>()
                    .ConvertUsing(s => ToKontoBaseView(s));

                config.CreateMap<Konto, KontoplanView>()
                    .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                    .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                    .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                    .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                    .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                    .ForMember(x => x.Kontogruppe, opt => opt.MapFrom(s => s.Kontogruppe))
                    .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.KreditPrStatusdato))
                    .ForMember(x => x.Saldo, opt => opt.MapFrom(s => s.SaldoPrStatusdato))
                    .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato));

                config.CreateMap<Konto, KontoView>()
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

                config.CreateMap<Kreditoplysninger, KreditoplysningerView>()
                    .ForMember(x => x.År, opt => opt.MapFrom(s => s.År))
                    .ForMember(x => x.Måned, opt => opt.MapFrom(s => s.Måned))
                    .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.Kredit));

                config.CreateMap<Budgetkonto, BudgetkontoplanView>()
                    .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                    .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                    .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                    .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                    .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                    .ForMember(x => x.Budgetkontogruppe, opt => opt.MapFrom(s => s.Budgetkontogruppe))
                    .ForMember(x => x.Budget, opt => opt.MapFrom(s => s.BudgetPrStatusdato))
                    .ForMember(x => x.BudgetSidsteMåned, opt => opt.MapFrom(s => s.BudgetSidsteMåned))
                    .ForMember(x => x.BudgetÅrTilDato, opt => opt.MapFrom(s => s.BudgetÅrTilStatusdato))
                    .ForMember(x => x.BudgetSidsteÅr, opt => opt.MapFrom(s => s.BudgetSidsteÅr))
                    .ForMember(x => x.Bogført, opt => opt.MapFrom(s => s.BogførtPrStatusdato))
                    .ForMember(x => x.BogførtSidsteMåned, opt => opt.MapFrom(s => s.BogførtSidsteMåned))
                    .ForMember(x => x.BogførtÅrTilDato, opt => opt.MapFrom(s => s.BogførtÅrTilStatusdato))
                    .ForMember(x => x.BogførtSidsteÅr, opt => opt.MapFrom(s => s.BogførtSidsteÅr))
                    .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato));

                config.CreateMap<Budgetkonto, BudgetkontoView>()
                    .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                    .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                    .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                    .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                    .ForMember(x => x.Notat, opt => opt.MapFrom(s => s.Note))
                    .ForMember(x => x.Budgetkontogruppe, opt => opt.MapFrom(s => s.Budgetkontogruppe))
                    .ForMember(x => x.Budget, opt => opt.MapFrom(s => s.BudgetPrStatusdato))
                    .ForMember(x => x.BudgetSidsteMåned, opt => opt.MapFrom(s => s.BudgetSidsteMåned))
                    .ForMember(x => x.BudgetÅrTilDato, opt => opt.MapFrom(s => s.BudgetÅrTilStatusdato))
                    .ForMember(x => x.BudgetSidsteÅr, opt => opt.MapFrom(s => s.BudgetSidsteÅr))
                    .ForMember(x => x.Bogført, opt => opt.MapFrom(s => s.BogførtPrStatusdato))
                    .ForMember(x => x.BogførtSidsteMåned, opt => opt.MapFrom(s => s.BogførtSidsteMåned))
                    .ForMember(x => x.BogførtÅrTilDato, opt => opt.MapFrom(s => s.BogførtÅrTilStatusdato))
                    .ForMember(x => x.BogførtSidsteÅr, opt => opt.MapFrom(s => s.BogførtSidsteÅr))
                    .ForMember(x => x.Disponibel, opt => opt.MapFrom(s => s.DisponibelPrStatusdato))
                    .ForMember(x => x.Budgetoplysninger, opt => opt.MapFrom(s => s.Budgetoplysninger));

                config.CreateMap<Budgetoplysninger, BudgetoplysningerView>()
                    .ForMember(x => x.År, opt => opt.MapFrom(s => s.År))
                    .ForMember(x => x.Måned, opt => opt.MapFrom(s => s.Måned))
                    .ForMember(x => x.Budget, opt => opt.MapFrom(s => s.Budget))
                    .ForMember(x => x.Bogført, opt => opt.MapFrom(s => s.BogførtPrStatusdato));

                config.CreateMap<Bogføringslinje, BogføringslinjeView>()
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

                config.CreateMap<IBogføringsresultat, BogføringslinjeOpretResponse>()
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

                config.CreateMap<IBogføringsadvarsel, BogføringsadvarselResponse>()
                    .ForMember(x => x.Advarsel, opt => opt.MapFrom(s => s.Advarsel))
                    .ForMember(x => x.Konto, opt => opt.MapFrom(s => s.Konto))
                    .ForMember(x => x.Beløb, opt => opt.MapFrom(s => s.Beløb));

                config.CreateMap<Kontogruppe, KontogruppeView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                    .ForMember(x => x.ErAktiver, opt => opt.MapFrom(s => s.KontogruppeType == KontogruppeType.Aktiver))
                    .ForMember(x => x.ErPassiver, opt => opt.MapFrom(s => s.KontogruppeType == KontogruppeType.Passiver));

                config.CreateMap<Budgetkontogruppe, BudgetkontogruppeView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                    .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

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

                config.CreateMap<Brevhoved, BrevhovedreferenceView>()
                    .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
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

        #region Private methods

        private static TelefonlisteView ToTelefonlisteView(AdresseBase source)
        {
            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, TelefonlisteView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, TelefonlisteView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static AdressekontolisteView ToAdressekontolisteView(AdresseBase source)
        {
            if (source == null)
            {
                return null;
            }

            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, AdressekontolisteView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, AdressekontolisteView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static AdressekontoView ToAdressekontoView(AdresseBase source)
        {
            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, AdressekontoView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, AdressekontoView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static DebitorlisteView ToDebitorlisteView(AdresseBase source)
        {
            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, DebitorlisteView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, DebitorlisteView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static DebitorView ToDebitorView(AdresseBase source)
        {
            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, DebitorView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, DebitorView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static KreditorlisteView ToKreditorlisteView(AdresseBase source)
        {
            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, KreditorlisteView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, KreditorlisteView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static KreditorView ToKreditorView(AdresseBase source)
        {
            var mapper = new ObjectMapper();
            Person person = source as Person;
            if (person != null)
            {
                return mapper.Map<Person, KreditorView>(person);
            }

            Firma firma = source as Firma;
            if (firma != null)
            {
                return mapper.Map<Firma, KreditorView>(firma);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        private static KontoBaseView ToKontoBaseView(KontoBase source)
        {
            var objectMapper = new ObjectMapper();
            Konto konto = source as Konto;
            if (konto != null)
            {
                return objectMapper.Map<Konto, KontoView>(konto);
            }

            Budgetkonto budgetkonto = source as Budgetkonto;
            if (budgetkonto != null)
            {
                return objectMapper.Map<Budgetkonto, BudgetkontoView>(budgetkonto);
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, source.GetType().Name));
        }

        #endregion
    }
}