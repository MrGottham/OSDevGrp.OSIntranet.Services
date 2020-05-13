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
                    .ConvertUsing(s => ToAdresse(s));

                config.CreateMap<PersonView, AdresseBase>()
                    .ConvertUsing(s => ToAdresse(s));

                config.CreateMap<FirmaView, AdresseBase>()
                    .ConvertUsing(s => ToAdresse(s));

                config.CreateMap<PostnummerView, Postnummer>()
                    .ConvertUsing(s => new Postnummer(s.Landekode, s.Postnummer, s.Bynavn));

                config.CreateMap<AdressegruppeView, Adressegruppe>()
                    .ConvertUsing(s => new Adressegruppe(s.Nummer, s.Navn, s.AdressegruppeOswebdb));

                config.CreateMap<BetalingsbetingelseView, Betalingsbetingelse>()
                    .ConvertUsing(s => new Betalingsbetingelse(s.Nummer, s.Navn));

                config.CreateMap<RegnskabListeView, Regnskab>()
                    .ConvertUsing(s => ToRegnskab(s));

                config.CreateMap<RegnskabView, Regnskab>()
                    .ConvertUsing(s => ToRegnskab(s));

                config.CreateMap<KontoListeView, Konto>()
                    .ConvertUsing(s => ToKonto(s));

                config.CreateMap<KontoView, Konto>()
                    .ConvertUsing(s => ToKonto(s));

                config.CreateMap<KreditoplysningerView, Kreditoplysninger>()
                    .ConvertUsing(s => new Kreditoplysninger(s.År, s.Måned, s.Kredit));

                config.CreateMap<BudgetkontoListeView, Budgetkonto>()
                    .ConvertUsing(s => ToBudgetkonto(s));

                config.CreateMap<BudgetkontoView, Budgetkonto>()
                    .ConvertUsing(s => ToBudgetkonto(s));

                config.CreateMap<BudgetoplysningerView, Budgetoplysninger>()
                    .ConvertUsing(s => new Budgetoplysninger(s.År, s.Måned, s.Indtægter, s.Udgifter));

                config.CreateMap<BogføringslinjeView, Bogføringslinje>()
                    .ConvertUsing(s => ToBogføringslinje(s));

                config.CreateMap<KontogruppeView, Kontogruppe>()
                    .ConvertUsing(s => ToKontogruppe(s));

                config.CreateMap<BudgetkontogruppeView, Budgetkontogruppe>()
                    .ConvertUsing(s => new Budgetkontogruppe(s.Nummer, s.Navn));

                config.CreateMap<BrevhovedView, Brevhoved>()
                    .ConvertUsing(s => ToBrevhoved(s));

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

        #region Private methods

        private AdresseBase ToAdresse(AdressereferenceView source)
        {
            throw new NotSupportedException();
        }

        private AdresseBase ToAdresse(PersonView source)
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
                    adressegruppe = GetAdressegruppeCallback(source.Adressegruppe.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Adressegruppe).Name, source.Adressegruppe.Nummer), ex);
                }
            }

            var person = new Person(source.Nummer, source.Navn, adressegruppe);
            person.SætAdresseoplysninger(source.Adresse1, source.Adresse2, source.PostnummerBy);
            person.SætTelefon(source.Telefon, source.Mobil);
            person.SætFødselsdato(source.Fødselsdato);
            person.SætBekendtskab(source.Bekendtskab);
            person.SætMailadresse(source.Mailadresse);
            person.SætWebadresse(source.Webadresse);
            if (source.Betalingsbetingelse != null)
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
                        betalingsbetingelse = GetBetalingsbetingelseCallback(source.Betalingsbetingelse.Nummer);
                    }
                    catch (IntranetRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Betalingsbetingelse).Name, source.Betalingsbetingelse.Nummer), ex);
                    }
                }

                person.SætBetalingsbetingelse(betalingsbetingelse);
            }

            person.SætUdlånsfrist(source.Udlånsfrist);
            person.SætFilofaxAdresselabel(source.FilofaxAdresselabel);
            if (source.Firma != null)
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
                        firma = (Firma) GetAdresseBaseCallback(source.Firma.Nummer);
                    }
                    catch (IntranetRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Firma).Name, source.Firma.Nummer), ex);
                    }
                }

                firma.TilføjPerson(person);
            }

            return person;
        }

        private AdresseBase ToAdresse(FirmaView source)
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
                    adressegruppe = GetAdressegruppeCallback(source.Adressegruppe.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Adressegruppe).Name, source.Adressegruppe.Nummer), ex);
                }
            }

            var firma = new Firma(source.Nummer, source.Navn, adressegruppe);
            firma.SætAdresseoplysninger(source.Adresse1, source.Adresse2, source.PostnummerBy);
            firma.SætTelefon(source.Telefon1, source.Telefon2, source.Telefax);
            firma.SætBekendtskab(source.Bekendtskab);
            firma.SætMailadresse(source.Mailadresse);
            firma.SætWebadresse(source.Webadresse);
            if (source.Betalingsbetingelse != null)
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
                        betalingsbetingelse = GetBetalingsbetingelseCallback(source.Betalingsbetingelse.Nummer);
                    }
                    catch (IntranetRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Betalingsbetingelse).Name, source.Betalingsbetingelse.Nummer), ex);
                    }
                }

                firma.SætBetalingsbetingelse(betalingsbetingelse);
            }

            firma.SætUdlånsfrist(source.Udlånsfrist);
            firma.SætFilofaxAdresselabel(source.FilofaxAdresselabel);
            return firma;
        }

        private Regnskab ToRegnskab(RegnskabListeView source)
        {
            var regnskab = new Regnskab(source.Nummer, source.Navn);
            if (source.Brevhoved != null && source.Brevhoved.Nummer != 0)
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
                        brevhoved = GetBrevhovedCallback(source.Brevhoved.Nummer);
                    }
                    catch (IntranetRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Brevhoved).Name, source.Brevhoved.Nummer), ex);
                    }
                }

                regnskab.SætBrevhoved(brevhoved);
            }

            return regnskab;
        }

        private Regnskab ToRegnskab(RegnskabView source)
        {
            var regnskab = new Regnskab(source.Nummer, source.Navn);
            if (source.Brevhoved != null && source.Brevhoved.Nummer != 0)
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
                        brevhoved = GetBrevhovedCallback(source.Brevhoved.Nummer);
                    }
                    catch (IntranetRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Brevhoved).Name, source.Brevhoved.Nummer), ex);
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
                foreach (var konto in BuildMany<KontoView, Konto>(source.Konti))
                {
                    regnskab.TilføjKonto(konto);
                }

                foreach (var budgetkonto in BuildMany<BudgetkontoView, Budgetkonto>(source.Budgetkonti))
                {
                    regnskab.TilføjKonto(budgetkonto);
                }

                BuildMany<BogføringslinjeView, Bogføringslinje>(source.Konti.SelectMany(m => m.Bogføringslinjer));
            }

            return regnskab;
        }

        private Konto ToKonto(KontoListeView source)
        {
            throw new NotSupportedException();
        }

        private Konto ToKonto(KontoView source)
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
                    regnskab = GetRegnskabCallback(source.Regnskab.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Regnskab).Name, source.Regnskab.Nummer), ex);
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
                    kontogruppe = GetKontogruppeCallback(source.Kontogruppe.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Kontogruppe).Name, source.Kontogruppe.Nummer), ex);
                }
            }

            var konto = new Konto(regnskab, source.Kontonummer, source.Kontonavn, kontogruppe);
            konto.SætBeskrivelse(source.Beskrivelse);
            konto.SætNote(source.Note);
            foreach (var kreditoplysninger in BuildMany<KreditoplysningerView, Kreditoplysninger>(source.Kreditoplysninger))
            {
                konto.TilføjKreditoplysninger(kreditoplysninger);
            }

            return konto;
        }

        private Budgetkonto ToBudgetkonto(BudgetkontoListeView source)
        {
            throw new NotSupportedException();
        }

        private Budgetkonto ToBudgetkonto(BudgetkontoView source)
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
                    regnskab = GetRegnskabCallback(source.Regnskab.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Regnskab).Name, source.Regnskab.Nummer), ex);
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
                    budgetkontogruppe = GetBudgetkontogruppeCallback(source.Budgetkontogruppe.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Budgetkontogruppe).Name, source.Budgetkontogruppe.Nummer), ex);
                }
            }

            var budgetkonto = new Budgetkonto(regnskab, source.Kontonummer, source.Kontonavn, budgetkontogruppe);
            budgetkonto.SætBeskrivelse(source.Beskrivelse);
            budgetkonto.SætNote(source.Note);
            foreach (var budgetoplysninger in BuildMany<BudgetoplysningerView, Budgetoplysninger>(source.Budgetoplysninger))
            {
                budgetkonto.TilføjBudgetoplysninger(budgetoplysninger);
            }

            return budgetkonto;
        }

        private Bogføringslinje ToBogføringslinje(BogføringslinjeView source)
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
                    regnskab = GetRegnskabCallback(source.Konto.Regnskab.Nummer);
                }
                catch (IntranetRepositoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Regnskab).Name, source.Konto.Regnskab.Nummer), ex);
                }
            }

            Konto konto;
            try
            {
                konto = regnskab.Konti
                    .OfType<Konto>()
                    .Single(m => String.Compare(m.Kontonummer, source.Konto.Kontonummer, StringComparison.Ordinal) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Konto).Name, source.Konto.Kontonummer), ex);
            }

            Budgetkonto budgetkonto = null;
            if (source.Budgetkonto != null && string.IsNullOrEmpty(source.Budgetkonto.Kontonummer) == false)
            {
                try
                {
                    budgetkonto = regnskab.Konti
                        .OfType<Budgetkonto>()
                        .Single(m => String.Compare(m.Kontonummer, source.Budgetkonto.Kontonummer, StringComparison.Ordinal) == 0);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Budgetkonto).Name, source.Budgetkonto.Kontonummer), ex);
                }
            }

            AdresseBase adresse = null;
            if (source.Adresse != null && source.Adresse.Nummer != 0)
            {
                lock (SyncRoot)
                {
                    if (GetAdresseBaseCallback == null)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate, "GetAdresseBaseCallback"));
                    }

                    try
                    {
                        adresse = GetAdresseBaseCallback(source.Adresse.Nummer);
                    }
                    catch (IntranetRepositoryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(AdresseBase).Name, source.Adresse.Nummer), ex);
                    }
                }
            }

            var bogføringslinje = new Bogføringslinje(source.Løbenummer, source.Dato, source.Bilag, source.Tekst, source.Debit, source.Kredit);
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
        }

        private Kontogruppe ToKontogruppe(KontogruppeView source)
        {
            var kontogruppe = new Kontogruppe(source.Nummer, source.Navn, KontogruppeType.Aktiver);
            switch (source.KontogruppeType)
            {
                case DataAccess.Contracts.Enums.KontogruppeType.Aktiver:
                    kontogruppe.SætKontogruppeType(KontogruppeType.Aktiver);
                    break;

                case DataAccess.Contracts.Enums.KontogruppeType.Passiver:
                    kontogruppe.SætKontogruppeType(KontogruppeType.Passiver);
                    break;

                default:
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, source.KontogruppeType, "KontogruppeType", MethodBase.GetCurrentMethod().Name));
            }

            return kontogruppe;
        }

        private Brevhoved ToBrevhoved(BrevhovedView source)
        {
            var brevhoved = new Brevhoved(source.Nummer, source.Navn);
            brevhoved.SætLinje1(source.Linje1);
            brevhoved.SætLinje2(source.Linje2);
            brevhoved.SætLinje3(source.Linje3);
            brevhoved.SætLinje4(source.Linje4);
            brevhoved.SætLinje5(source.Linje5);
            brevhoved.SætLinje6(source.Linje6);
            brevhoved.SætLinje7(source.Linje7);
            brevhoved.SætCvrNr(source.CvrNr);
            return brevhoved;
        }

        #endregion
    }
}