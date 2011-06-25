using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
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
        private Func<int, Kontogruppe> _getKontogruppeCallback;
        private Func<int, Budgetkontogruppe> _getBudgetkontogruppeCallback;
        private Func<int, Brevhoved> _getBrevhovedCallback;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner domæneobjekt bygger.
        /// </summary>
        public DomainObjectBuilder()
        {
            Mapper.CreateMap<PersonView, AdresseBase>()
                .ConvertUsing(s =>
                                  {
                                      if (GetAdressegruppeCallback == null)
                                      {
                                          throw new IntranetRepositoryException(
                                              Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate,
                                                                           "GetAdressegruppeCallback"));
                                      }
                                      Adressegruppe adressegruppe;
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
                                          throw new IntranetRepositoryException(
                                              Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                           typeof(Adressegruppe),
                                                                           s.Adressegruppe.Nummer), ex);
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
                                          if (GetBetalingsbetingelseCallback == null)
                                          {
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(
                                                      ExceptionMessage.NoRegistrationForDelegate,
                                                      "GetBetalingsbetingelseCallback"));
                                          }
                                          Betalingsbetingelse betalingsbetingelse;
                                          try
                                          {
                                              betalingsbetingelse =
                                                  GetBetalingsbetingelseCallback(s.Betalingsbetingelse.Nummer);
                                          }
                                          catch (IntranetRepositoryException)
                                          {
                                              throw;
                                          }
                                          catch (Exception ex)
                                          {
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                               typeof (Betalingsbetingelse),
                                                                               s.Betalingsbetingelse.Nummer), ex);
                                          }
                                          person.SætBetalingsbetingelse(betalingsbetingelse);
                                      }
                                      person.SætUdlånsfrist(s.Udlånsfrist);
                                      person.SætFilofaxAdresselabel(s.FilofaxAdresselabel);
                                      if (s.Firma != null)
                                      {
                                          if (GetAdresseBaseCallback == null)
                                          {
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(
                                                      ExceptionMessage.NoRegistrationForDelegate,
                                                      "GetAdresseBaseCallback"));
                                          }
                                          Firma firma;
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
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                               typeof (Firma), s.Firma.Nummer), ex);
                                          }
                                          firma.TilføjPerson(person);
                                      }
                                      return person;
                                  });

            Mapper.CreateMap<FirmaView, AdresseBase>()
                .ConvertUsing(s =>
                                  {
                                      if (GetAdressegruppeCallback == null)
                                      {
                                          throw new IntranetRepositoryException(
                                              Resource.GetExceptionMessage(ExceptionMessage.NoRegistrationForDelegate,
                                                                           "GetAdressegruppeCallback"));
                                      }
                                      Adressegruppe adressegruppe;
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
                                          throw new IntranetRepositoryException(
                                              Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                           typeof (Adressegruppe),
                                                                           s.Adressegruppe.Nummer), ex);
                                      }
                                      var firma = new Firma(s.Nummer, s.Navn, adressegruppe);
                                      firma.SætAdresseoplysninger(s.Adresse1, s.Adresse2, s.PostnummerBy);
                                      firma.SætTelefon(s.Telefon1, s.Telefon2, s.Telefax);
                                      firma.SætBekendtskab(s.Bekendtskab);
                                      firma.SætMailadresse(s.Mailadresse);
                                      firma.SætWebadresse(s.Webadresse);
                                      if (s.Betalingsbetingelse != null)
                                      {
                                          if (GetBetalingsbetingelseCallback == null)
                                          {
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(
                                                      ExceptionMessage.NoRegistrationForDelegate,
                                                      "GetBetalingsbetingelseCallback"));
                                          }
                                          Betalingsbetingelse betalingsbetingelse;
                                          try
                                          {
                                              betalingsbetingelse =
                                                  GetBetalingsbetingelseCallback(s.Betalingsbetingelse.Nummer);
                                          }
                                          catch (IntranetRepositoryException)
                                          {
                                              throw;
                                          }
                                          catch (Exception ex)
                                          {
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                               typeof (Betalingsbetingelse),
                                                                               s.Betalingsbetingelse.Nummer), ex);
                                          }
                                          firma.SætBetalingsbetingelse(betalingsbetingelse);
                                      }
                                      firma.SætUdlånsfrist(s.Udlånsfrist);
                                      firma.SætFilofaxAdresselabel(s.FilofaxAdresselabel);
                                      return firma;
                                  });

            Mapper.CreateMap<PostnummerView, Postnummer>()
                .ConvertUsing(s => new Postnummer(s.Landekode, s.Postnummer, s.Bynavn));

            Mapper.CreateMap<AdressegruppeView, Adressegruppe>()
                .ConvertUsing(s => new Adressegruppe(s.Nummer, s.Navn, s.AdressegruppeOswebdb));

            Mapper.CreateMap<BetalingsbetingelseView, Betalingsbetingelse>()
                .ConvertUsing(s => new Betalingsbetingelse(s.Nummer, s.Navn));

            Mapper.CreateMap<RegnskabListeView, Regnskab>()
                .ConvertUsing(s => new Regnskab(s.Nummer, s.Navn));

            Mapper.CreateMap<KontogruppeView, Kontogruppe>()
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
                                              throw new IntranetRepositoryException(
                                                  Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue,
                                                                               s.KontogruppeType, "KontogruppeType",
                                                                               MethodBase.GetCurrentMethod().Name));
                                      }
                                      return kontogruppe;
                                  });

            Mapper.CreateMap<BudgetkontogruppeView, Budgetkontogruppe>()
                .ConvertUsing(s => new Budgetkontogruppe(s.Nummer, s.Navn));

            Mapper.AssertConfigurationIsValid();
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
                _getAdresseBaseCallback = value;
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
                _getAdressegruppeCallback = value;
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
                _getBetalingsbetingelseCallback = value;
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
                _getKontogruppeCallback = value;
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
                _getBudgetkontogruppeCallback = value;
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
                _getBrevhovedCallback = value;
            }
        }

        /// <summary>
        /// Sætter adresser til brug ved bygning af domæneobjekter.
        /// </summary>
        /// <param name="adresser">Adresser til brug ved bygning af domæneobjekter.</param>
        public void SætAdresser(IEnumerable<AdresseBase> adresser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sætter adressegrupper til brug ved bygning af domæneobjekter.
        /// </summary>
        /// <param name="adressegrupper">Adressegrupper til brug ved bygning af domæneobjekter.</param>
        public void SætAdressegrupper(IEnumerable<Adressegruppe> adressegrupper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sætter betalingsbetingelser til brug ved bygning af domæneobjekter.
        /// </summary>
        /// <param name="betalingsbetingelser">Betalingsbetingelser til brug ved bygning af domæneobjekter.</param>
        public void SætBetalingsbetingelser(IEnumerable<Betalingsbetingelse> betalingsbetingelser)
        {
            throw new NotImplementedException();
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
                return Mapper.Map<TSource, TDomainObject>(source);
            }
            catch (AutoMapperMappingException ex)
            {
                if (ex.InnerException != null && ex.InnerException is IntranetRepositoryException)
                {
                    throw ex.InnerException;
                }
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
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
