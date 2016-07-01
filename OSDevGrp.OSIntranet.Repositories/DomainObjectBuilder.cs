using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Domæneobjekt bygger.
    /// </summary>
    public class DomainObjectBuilder : IDomainObjectBuilder
    {
        #region Private variables

        private Func<int, AdresseBase> _getAdresseBaseCallback;
        private Func<int, Adressegruppe> _getAdressegruppeCallback;
        private Func<int, Betalingsbetingelse> _getBetalingsbetingelseCallback;
        private Func<int, Regnskab> _getRegnskabCallback;
        private Func<int, Kontogruppe> _getKontogruppeCallback;
        private Func<int, Budgetkontogruppe> _getBudgetkontogruppeCallback;
        private Func<int, Brevhoved> _getBrevhovedCallback;
        private readonly IMapper _mapper;
        private readonly IList<Regnskab> _regnskaber = new List<Regnskab>();
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner domæneobjekt bygger.
        /// </summary>
        public DomainObjectBuilder()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.CreateMap<AdressereferenceView, AdresseBase>()
                    .ConvertUsing(s => { throw new NotSupportedException(); });

                config.CreateMap<PersonView, AdresseBase>()
                    .ConvertUsing(s =>
                    {
                        Adressegruppe adressegruppe;
                        lock (SyncRoot)
                        {
                            if (GetAdressegruppeCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetAdressegruppeCallback"));
                            }
                            try
                            {
                                adressegruppe = GetAdressegruppeCallback(s.Adressegruppe.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Adressegruppe).Name, s.Adressegruppe.Nummer), ex);
                            }
                        }
                        var person = new Person(s.Nummer, s.Navn, adressegruppe);
                        person.SætAdresseoplysninger(s.Adresse1, s.Adresse2, s.PostnummerBy);
                        person.SætTelefon(s.Telefon, s.Mobil);
                        person.SætFødselsdato(s.Fødselsdato);
                        person.SætBekendtskab(s.Bekendtskab);
                        person.SætMailadresse(s.Mailadresse);
                        person.SætWebadresse(s.Webadresse);
                        if (s.Betalingsbetingelse != null)
                        {
                            Betalingsbetingelse betalingsbetingelse;
                            lock (SyncRoot)
                            {
                                if (GetBetalingsbetingelseCallback == null)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetBetalingsbetingelseCallback"));
                                }
                                try
                                {
                                    betalingsbetingelse = GetBetalingsbetingelseCallback(s.Betalingsbetingelse.Nummer);
                                }
                                catch (IntranetRepositoryException)
                                {
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Betalingsbetingelse).Name, s.Betalingsbetingelse.Nummer), ex);
                                }
                            }
                            person.SætBetalingsbetingelse(betalingsbetingelse);
                        }
                        person.SætUdlånsfrist(s.Udlånsfrist);
                        person.SætFilofaxAdresselabel(s.FilofaxAdresselabel);
                        if (s.Firma != null)
                        {
                            Firma firma;
                            lock (SyncRoot)
                            {
                                if (GetAdresseBaseCallback == null)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetAdresseBaseCallback"));
                                }
                                try
                                {
                                    firma = (Firma) GetAdresseBaseCallback(s.Firma.Nummer);
                                }
                                catch (IntranetRepositoryException)
                                {
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Firma).Name, s.Firma.Nummer), ex);
                                }
                            }
                            firma.TilføjPerson(person);
                        }
                        return person;
                    });

                config.CreateMap<FirmaView, AdresseBase>()
                    .ConvertUsing(s =>
                    {
                        Adressegruppe adressegruppe;
                        lock (SyncRoot)
                        {
                            if (GetAdressegruppeCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetAdressegruppeCallback"));
                            }
                            try
                            {
                                adressegruppe = GetAdressegruppeCallback(s.Adressegruppe.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Adressegruppe).Name, s.Adressegruppe.Nummer), ex);
                            }
                        }
                        var firma = new Firma(s.Nummer, s.Navn, adressegruppe);
                        firma.SætAdresseoplysninger(s.Adresse1, s.Adresse2, s.PostnummerBy);
                        firma.SætTelefon(s.Telefon1, s.Telefon2, s.Telefax);
                        firma.SætBekendtskab(s.Bekendtskab);
                        firma.SætMailadresse(s.Mailadresse);
                        firma.SætWebadresse(s.Webadresse);
                        if (s.Betalingsbetingelse != null)
                        {
                            Betalingsbetingelse betalingsbetingelse;
                            lock (SyncRoot)
                            {
                                if (GetBetalingsbetingelseCallback == null)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetBetalingsbetingelseCallback"));
                                }
                                try
                                {
                                    betalingsbetingelse = GetBetalingsbetingelseCallback(s.Betalingsbetingelse.Nummer);
                                }
                                catch (IntranetRepositoryException)
                                {
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Betalingsbetingelse).Name, s.Betalingsbetingelse.Nummer), ex);
                                }
                            }
                            firma.SætBetalingsbetingelse(betalingsbetingelse);
                        }
                        firma.SætUdlånsfrist(s.Udlånsfrist);
                        firma.SætFilofaxAdresselabel(s.FilofaxAdresselabel);
                        return firma;
                    });

                config.CreateMap<PostnummerView, Postnummer>()
                    .ConvertUsing(s => new Postnummer(s.Landekode, s.Postnummer, s.Bynavn));

                config.CreateMap<AdressegruppeView, Adressegruppe>()
                    .ConvertUsing(s => new Adressegruppe(s.Nummer, s.Navn, s.AdressegruppeOswebdb));

                config.CreateMap<BetalingsbetingelseView, Betalingsbetingelse>()
                    .ConvertUsing(s => new Betalingsbetingelse(s.Nummer, s.Navn));

                config.CreateMap<RegnskabListeView, Regnskab>()
                    .ConvertUsing(s =>
                    {
                        var regnskab = new Regnskab(s.Nummer, s.Navn);
                        if (s.Brevhoved != null && s.Brevhoved.Nummer != 0)
                        {
                            Brevhoved brevhoved;
                            lock (SyncRoot)
                            {
                                if (GetBrevhovedCallback == null)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetBrevhovedCallback"));
                                }
                                try
                                {
                                    brevhoved = GetBrevhovedCallback(s.Brevhoved.Nummer);
                                }
                                catch (IntranetRepositoryException)
                                {
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Brevhoved).Name, s.Brevhoved.Nummer), ex);
                                }
                            }
                            regnskab.SætBrevhoved(brevhoved);
                        }
                        return regnskab;
                    });

                config.CreateMap<RegnskabView, Regnskab>()
                    .ConvertUsing(s =>
                    {
                        var regnskab = new Regnskab(s.Nummer, s.Navn);
                        if (s.Brevhoved != null && s.Brevhoved.Nummer != 0)
                        {
                            Brevhoved brevhoved;
                            lock (SyncRoot)
                            {
                                if (GetBrevhovedCallback == null)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetBrevhovedCallback"));
                                }
                                try
                                {
                                    brevhoved = GetBrevhovedCallback(s.Brevhoved.Nummer);
                                }
                                catch (IntranetRepositoryException)
                                {
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Brevhoved).Name, s.Brevhoved.Nummer), ex);
                                }
                            }
                            regnskab.SætBrevhoved(brevhoved);
                        }
                        lock (SyncRoot)
                        {
                            var cached = _regnskaber.SingleOrDefault(m => m.Nummer == regnskab.Nummer);
                            if (cached != null)
                            {
                                _regnskaber.Remove(cached);
                            }
                            _regnskaber.Add(regnskab);
                            var regnskabslisteHelper = new RegnskabslisteHelper(_regnskaber);
                            GetRegnskabCallback = regnskabslisteHelper.GetById;
                            foreach (var konto in BuildMany<KontoView, Konto>(s.Konti))
                            {
                                regnskab.TilføjKonto(konto);
                            }
                            foreach (var budgetkonto in BuildMany<BudgetkontoView, Budgetkonto>(s.Budgetkonti))
                            {
                                regnskab.TilføjKonto(budgetkonto);
                            }
                            BuildMany<BogføringslinjeView, Bogføringslinje>(s.Konti.SelectMany(m => m.Bogføringslinjer));
                        }
                        return regnskab;
                    });

                config.CreateMap<KontoListeView, Konto>()
                    .ConvertUsing(s => { throw new NotSupportedException(); });

                config.CreateMap<KontoView, Konto>()
                    .ConvertUsing(s =>
                    {
                        Regnskab regnskab;
                        lock (SyncRoot)
                        {
                            if (GetRegnskabCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetRegnskabCallback"));
                            }
                            try
                            {
                                regnskab = GetRegnskabCallback(s.Regnskab.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Regnskab).Name, s.Regnskab.Nummer), ex);
                            }
                        }
                        Kontogruppe kontogruppe;
                        lock (SyncRoot)
                        {
                            if (GetKontogruppeCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetKontogruppeCallback"));
                            }
                            try
                            {
                                kontogruppe = GetKontogruppeCallback(s.Kontogruppe.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Kontogruppe).Name, s.Kontogruppe.Nummer), ex);
                            }
                        }
                        var konto = new Konto(regnskab, s.Kontonummer, s.Kontonavn, kontogruppe);
                        konto.SætBeskrivelse(s.Beskrivelse);
                        konto.SætNote(s.Note);
                        foreach (var kreditoplysninger in BuildMany<KreditoplysningerView, Kreditoplysninger>(s.Kreditoplysninger))
                        {
                            konto.TilføjKreditoplysninger(kreditoplysninger);
                        }
                        return konto;
                    });

                config.CreateMap<KreditoplysningerView, Kreditoplysninger>()
                    .ConvertUsing(s => new Kreditoplysninger(s.År, s.Måned, s.Kredit));

                config.CreateMap<BudgetkontoListeView, Budgetkonto>()
                    .ConvertUsing(s => { throw new NotSupportedException(); });

                config.CreateMap<BudgetkontoView, Budgetkonto>()
                    .ConvertUsing(s =>
                    {
                        Regnskab regnskab;
                        lock (SyncRoot)
                        {
                            if (GetRegnskabCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetRegnskabCallback"));
                            }
                            try
                            {
                                regnskab = GetRegnskabCallback(s.Regnskab.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Regnskab).Name, s.Regnskab.Nummer), ex);
                            }
                        }
                        Budgetkontogruppe budgetkontogruppe;
                        lock (SyncRoot)
                        {
                            if (GetBudgetkontogruppeCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetBudgetkontogruppeCallback"));
                            }
                            try
                            {
                                budgetkontogruppe = GetBudgetkontogruppeCallback(s.Budgetkontogruppe.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkontogruppe).Name, s.Budgetkontogruppe.Nummer), ex);
                            }
                        }
                        var budgetkonto = new Budgetkonto(regnskab, s.Kontonummer, s.Kontonavn, budgetkontogruppe);
                        budgetkonto.SætBeskrivelse(s.Beskrivelse);
                        budgetkonto.SætNote(s.Note);
                        foreach (var budgetoplysninger in BuildMany<BudgetoplysningerView, Budgetoplysninger>(s.Budgetoplysninger))
                        {
                            budgetkonto.TilføjBudgetoplysninger(budgetoplysninger);
                        }
                        return budgetkonto;
                    });

                config.CreateMap<BudgetoplysningerView, Budgetoplysninger>()
                    .ConvertUsing(s => new Budgetoplysninger(s.År, s.Måned, s.Indtægter, s.Udgifter));

                config.CreateMap<BogføringslinjeView, Bogføringslinje>()
                    .ConvertUsing(s =>
                    {
                        Regnskab regnskab;
                        lock (SyncRoot)
                        {
                            if (GetRegnskabCallback == null)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetRegnskabCallback"));
                            }
                            try
                            {
                                regnskab = GetRegnskabCallback(s.Konto.Regnskab.Nummer);
                            }
                            catch (IntranetRepositoryException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Regnskab).Name, s.Konto.Regnskab.Nummer), ex);
                            }
                        }
                        Konto konto;
                        try
                        {
                            konto = regnskab.Konti
                                .OfType<Konto>()
                                .Single(m => String.Compare(m.Kontonummer, s.Konto.Kontonummer, StringComparison.Ordinal) == 0);
                        }
                        catch (InvalidOperationException ex)
                        {
                            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Konto).Name, s.Konto.Kontonummer), ex);
                        }
                        Budgetkonto budgetkonto = null;
                        if (s.Budgetkonto != null && string.IsNullOrEmpty(s.Budgetkonto.Kontonummer) == false)
                        {
                            try
                            {
                                budgetkonto = regnskab.Konti
                                    .OfType<Budgetkonto>()
                                    .Single(m => String.Compare(m.Kontonummer, s.Budgetkonto.Kontonummer, StringComparison.Ordinal) == 0);
                            }
                            catch (InvalidOperationException ex)
                            {
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto).Name, s.Budgetkonto.Kontonummer), ex);
                            }
                        }
                        AdresseBase adresse = null;
                        if (s.Adresse != null && s.Adresse.Nummer != 0)
                        {
                            lock (SyncRoot)
                            {
                                if (GetAdresseBaseCallback == null)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetAdresseBaseCallback"));
                                }
                                try
                                {
                                    adresse = GetAdresseBaseCallback(s.Adresse.Nummer);
                                }
                                catch (IntranetRepositoryException)
                                {
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (AdresseBase).Name, s.Adresse.Nummer), ex);
                                }
                            }
                        }
                        var bogføringslinje = new Bogføringslinje(s.Løbenummer, s.Dato, s.Bilag, s.Tekst, s.Debit, s.Kredit);
                        konto.TilføjBogføringslinje(bogføringslinje);
                        if (budgetkonto != null)
                        {
                            budgetkonto.TilføjBogføringslinje(bogføringslinje);
                        }
                        if (adresse != null)
                        {
                            adresse.TilføjBogføringslinje(bogføringslinje);
                        }
                        return bogføringslinje;
                    });

                config.CreateMap<KontogruppeView, Kontogruppe>()
                    .ConvertUsing(s =>
                    {
                        var kontogruppe = new Kontogruppe(s.Nummer, s.Navn, KontogruppeType.Aktiver);
                        switch (s.KontogruppeType)
                        {
                            case DataAccess.Contracts.Enums.KontogruppeType.Aktiver:
                                kontogruppe.SætKontogruppeType(KontogruppeType.Aktiver);
                                break;

                            case DataAccess.Contracts.Enums.KontogruppeType.Passiver:
                                kontogruppe.SætKontogruppeType(KontogruppeType.Passiver);
                                break;

                            default:
                                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, s.KontogruppeType, "KontogruppeType", MethodBase.GetCurrentMethod().Name));
                        }
                        return kontogruppe;
                    });

                config.CreateMap<BudgetkontogruppeView, Budgetkontogruppe>()
                    .ConvertUsing(s => new Budgetkontogruppe(s.Nummer, s.Navn));

                config.CreateMap<BrevhovedView, Brevhoved>()
                    .ConvertUsing(s =>
                    {
                        var brevhoved = new Brevhoved(s.Nummer, s.Navn);
                        brevhoved.SætLinje1(s.Linje1);
                        brevhoved.SætLinje2(s.Linje2);
                        brevhoved.SætLinje3(s.Linje3);
                        brevhoved.SætLinje4(s.Linje4);
                        brevhoved.SætLinje5(s.Linje5);
                        brevhoved.SætLinje6(s.Linje6);
                        brevhoved.SætLinje7(s.Linje7);
                        brevhoved.SætCvrNr(s.CvrNr);
                        return brevhoved;
                    });

                config.CreateMap<BrevhovedreferenceView, Brevhoved>()
                    .ConvertUsing(s => new Brevhoved(s.Nummer, s.Navn));
            });
            
            mapperConfiguration.AssertConfigurationIsValid();

            _mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region IDomainObjectBuilder Members

        /// <summary>
        /// Callbackmetode, som domæneobjekbyggeren benytter til at hente en given adresse.
        /// </summary>
        public Func<int, AdresseBase> GetAdresseBaseCallback
        {
            get
            {
                return _getAdresseBaseCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getAdresseBaseCallback = value;
                }
            }
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given adressegruppe.
        /// </summary>
        public Func<int, Adressegruppe> GetAdressegruppeCallback
        {
            get
            {
                return _getAdressegruppeCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getAdressegruppeCallback = value;
                }
            }
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given betalingsbetingelse.
        /// </summary>
        public Func<int, Betalingsbetingelse> GetBetalingsbetingelseCallback
        {
            get
            {
                return _getBetalingsbetingelseCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getBetalingsbetingelseCallback = value;
                }
            }
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente et givent regnskab.
        /// </summary>
        public Func<int, Regnskab> GetRegnskabCallback
        {
            get
            {
                return _getRegnskabCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getRegnskabCallback = value;
                }
            }
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given kontogruppe.
        /// </summary>
        public Func<int, Kontogruppe> GetKontogruppeCallback
        {
            get
            {
                return _getKontogruppeCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getKontogruppeCallback = value;
                }
            }
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given gruppe til budgetkonti.
        /// </summary>
        public Func<int, Budgetkontogruppe> GetBudgetkontogruppeCallback
        {
            get
            {
                return _getBudgetkontogruppeCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getBudgetkontogruppeCallback = value;
                }
            }
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente et givent brevhoved.
        /// </summary>
        public Func<int, Brevhoved> GetBrevhovedCallback
        {
            get
            {
                return _getBrevhovedCallback;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (SyncRoot)
                {
                    _getBrevhovedCallback = value;
                }
            }
        }

        /// <summary>
        /// Bygger objekt i domænemodellen.
        /// </summary>
        /// <typeparam name="TSource">Typen på objektet, hvorfra domæneobjektet skal bygges.</typeparam>
        /// <typeparam name="TDomainObject">Typen på domæneobjektet.</typeparam>
        /// <param name="source">Objektet, hvorfra domæneobjektet skal bygges.</param>
        /// <returns>Domæneobjekt.</returns>
        public TDomainObject Build<TSource, TDomainObject>(TSource source)
        {
            if (Equals(source, null))
            {
                throw new ArgumentNullException("source");
            }
            try
            {
                return _mapper.Map<TSource, TDomainObject>(source);
            }
            catch (AutoMapperMappingException ex)
            {
                if (ex.InnerException is IntranetRepositoryException)
                {
                    throw ex.InnerException;
                }
                var errorMessage = new StringBuilder(ex.Message);

                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    if (innerException is AutoMapperMappingException)
                    {
                        innerException = innerException.InnerException;
                        continue;
                    }
                    errorMessage.AppendFormat(" -> {0}", innerException.Message);
                    if (innerException is IntranetRepositoryException)
                    {
                        innerException = null;
                        continue;
                    }
                    innerException = innerException.InnerException;
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name, errorMessage), ex);
            }
        }

        /// <summary>
        /// Bygger flere instansere af et objekt i domænemodellen.
        /// </summary>
        /// <typeparam name="TSource">Typen på objekterne, hvorfra domæneobjekter skal bygges.</typeparam>
        /// <typeparam name="TDomainObject">Typen på domæneobjekterne.</typeparam>
        /// <param name="sources">Objekter, hvorfra domæneobjekter skal bygges.</param>
        /// <returns>Domæneobjekter.</returns>
        public IEnumerable<TDomainObject> BuildMany<TSource, TDomainObject>(IEnumerable<TSource> sources)
        {
            return Build<IEnumerable<TSource>, IEnumerable<TDomainObject>>(sources);
        }

        #endregion
    }
}
