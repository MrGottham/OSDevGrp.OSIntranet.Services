using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories
{
    /// <summary>
    /// Repository til adressekartoteket.
    /// </summary>
    public class AdresseRepository : IAdresseRepository
    {
        #region Private constants

        private const string EndpointConfigurationName = "AdresseRepositoryService";

        #endregion

        #region Private variables

        private readonly IChannelFactory _channelFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository til adressekartoteket.
        /// </summary>
        /// <param name="channelFactory">Implementering af en ChannelFactory.</param>
        public AdresseRepository(IChannelFactory channelFactory)
        {
            if (channelFactory == null)
            {
                throw new ArgumentNullException("channelFactory");
            }
            _channelFactory = channelFactory;
        }

        #endregion

        #region IAdresseRepository Members

        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <returns>Liste af adresser.</returns>
        public IList<AdresseBase> AdresseGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                // Henter alle adressegrupper.
                var adressegruppeQuery = new AdressegruppeGetAllQuery();
                var adressegruppeViews = channel.AdressegruppeGetAll(adressegruppeQuery);
                // Henter alle betalingsbetingelser.
                var betalingsbetingelseQuery = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelseViews = channel.BetalingsbetingelseGetAll(betalingsbetingelseQuery);
                // Henter alle firmaadresser.
                var firmaQuery = new FirmaGetAllQuery();
                var firmaViews = channel.FirmaGetAll(firmaQuery);
                // Henter alle personadresser.
                var personQuery = new PersonGetAllQuery();
                var personViews = channel.PersonGetAll(personQuery);
                // Mapper views til adresser.
                var adresser = new List<AdresseBase>();
                adresser.AddRange(
                    firmaViews.Select(
                        firmaView =>
                        MapFirma(firmaView, adressegruppeViews.Select(MapAdressegruppe).ToList(),
                                 betalingsbetingelseViews.Select(MapBetalingsbetingelse).ToList())).ToList());
                adresser.AddRange(
                    personViews.Select(
                        personView =>
                        MapPerson(personView, adresser.OfType<Firma>().ToList(),
                                  adressegruppeViews.Select(MapAdressegruppe).ToList(),
                                  betalingsbetingelseViews.Select(MapBetalingsbetingelse).ToList())).ToList());
                return adresser.OrderBy(m => m, new AdresseComparer()).ToList();
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <returns>Liste af postnumre.</returns>
        public IList<Postnummer> PostnummerGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new PostnummerGetAllQuery();
                var postnummerViews = channel.PostnummerGetAll(query);
                return postnummerViews.Select(MapPostnummer).ToList();
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste af adressegrupper.</returns>
        public IList<Adressegruppe> AdressegruppeGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new AdressegruppeGetAllQuery();
                var adressegruppeViews = channel.AdressegruppeGetAll(query);
                return adressegruppeViews.Select(MapAdressegruppe).ToList();
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <returns>Liste af betalingsbetingelser.</returns>
        public IList<Betalingsbetingelse> BetalingsbetingelseGetAll()
        {
            var channel = _channelFactory.CreateChannel<IAdresseRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelseViews = channel.BetalingsbetingelseGetAll(query);
                return betalingsbetingelseViews.Select(MapBetalingsbetingelse).ToList();
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (FaultException ex)
            {
                throw new IntranetRepositoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, MethodBase.GetCurrentMethod().Name,
                                                 ex.Message), ex);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Mapper et firmaview til et firma.
        /// </summary>
        /// <param name="firmaView">Firmaview.</param>
        /// <param name="adressegrupper">Adressegrupper.</param>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        /// <returns>Firma.</returns>
        private static Firma MapFirma(FirmaView firmaView, IEnumerable<Adressegruppe> adressegrupper, IEnumerable<Betalingsbetingelse> betalingsbetingelser)
        {
            if (firmaView == null)
            {
                throw new ArgumentNullException("firmaView");
            }
            if (adressegrupper == null)
            {
                throw new ArgumentNullException("adressegrupper");
            }
            if (betalingsbetingelser == null)
            {
                throw new ArgumentNullException("betalingsbetingelser");
            }
            Adressegruppe adressegruppe;
            try
            {
                adressegruppe = adressegrupper.Single(m => m.Nummer == firmaView.Adressegruppe.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Adressegruppe),
                                                 firmaView.Adressegruppe.Nummer), ex);
            }
            var firma = new Firma(firmaView.Nummer, firmaView.Navn, adressegruppe);
            firma.SætAdresseoplysninger(firmaView.Adresse1, firmaView.Adresse2, firmaView.PostnummerBy);
            firma.SætTelefon(firmaView.Telefon1, firmaView.Telefon2, firmaView.Telefax);
            firma.SætBekendtskab(firmaView.Bekendtskab);
            firma.SætMailadresse(firmaView.Mailadresse);
            firma.SætWebadresse(firmaView.Webadresse);
            if (firmaView.Betalingsbetingelse != null)
            {
                Betalingsbetingelse betalingsbetingelse;
                try
                {
                    betalingsbetingelse =
                        betalingsbetingelser.Single(m => m.Nummer == firmaView.Betalingsbetingelse.Nummer);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IntranetRepositoryException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Betalingsbetingelse),
                                                     firmaView.Betalingsbetingelse.Nummer), ex);
                }
                firma.SætBetalingsbetingelse(betalingsbetingelse);
            }
            firma.SætUdlånsfrist(firmaView.Udlånsfrist);
            firma.SætFilofaxAdresselabel(firmaView.FilofaxAdresselabel);
            return firma;
        }

        /// <summary>
        /// Mapper et personview til en person.
        /// </summary>
        /// <param name="personView">Personview.</param>
        /// <param name="firmaer">Firmaer.</param>
        /// <param name="adressegrupper">Adressegrupper.</param>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        /// <returns>Person.</returns>
        private static Person MapPerson(PersonView personView, IEnumerable<Firma> firmaer, IEnumerable<Adressegruppe> adressegrupper, IEnumerable<Betalingsbetingelse> betalingsbetingelser)
        {
            if (personView == null)
            {
                throw new ArgumentNullException("personView");
            }
            if (firmaer == null)
            {
                throw new ArgumentNullException("firmaer");
            }
            if (adressegrupper == null)
            {
                throw new ArgumentNullException("adressegrupper");
            }
            if (betalingsbetingelser == null)
            {
                throw new ArgumentNullException("betalingsbetingelser");
            }
            Adressegruppe adressegruppe;
            try
            {
                adressegruppe = adressegrupper.Single(m => m.Nummer == personView.Adressegruppe.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof(Adressegruppe),
                                                 personView.Adressegruppe.Nummer), ex);
            }
            var person = new Person(personView.Nummer, personView.Navn, adressegruppe);

            person.SætAdresseoplysninger(personView.Adresse1, personView.Adresse2, personView.PostnummerBy);
            person.SætTelefon(personView.Telefon, personView.Mobil);
            person.SætFødselsdato(personView.Fødselsdato);
            person.SætBekendtskab(personView.Bekendtskab);
            person.SætMailadresse(personView.Mailadresse);
            person.SætWebadresse(personView.Webadresse);
            if (personView.Betalingsbetingelse != null)
            {
                Betalingsbetingelse betalingsbetingelse;
                try
                {
                    betalingsbetingelse =
                        betalingsbetingelser.Single(m => m.Nummer == personView.Betalingsbetingelse.Nummer);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IntranetRepositoryException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Betalingsbetingelse),
                                                     personView.Betalingsbetingelse.Nummer), ex);
                }
                person.SætBetalingsbetingelse(betalingsbetingelse);
            }
            person.SætUdlånsfrist(personView.Udlånsfrist);
            person.SætFilofaxAdresselabel(personView.FilofaxAdresselabel);
            if (personView.Firma != null)
            {
                Firma firma;
                try
                {
                    firma = firmaer.Single(m => m.Nummer == personView.Firma.Nummer);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IntranetRepositoryException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Firma),
                                                     personView.Firma.Nummer), ex);
                }
                firma.TilføjPerson(person);
            }
            return person;
        }

        /// <summary>
        /// Mapper et postnummerview til et postnummer.
        /// </summary>
        /// <param name="postnummerView">Postnummerview.</param>
        /// <returns>Postnummer.</returns>
        private static Postnummer MapPostnummer(PostnummerView postnummerView)
        {
            if (postnummerView == null)
            {
                throw new ArgumentNullException("postnummerView");
            }
            return new Postnummer(postnummerView.Landekode, postnummerView.Postnummer, postnummerView.Bynavn);
        }

        /// <summary>
        /// Mapper et adressegruppeview til en adressegruppe.
        /// </summary>
        /// <param name="adressegruppeView">Adressegruppeview.</param>
        /// <returns>Adressegruppe.</returns>
        private static Adressegruppe MapAdressegruppe(AdressegruppeView adressegruppeView)
        {
            if (adressegruppeView == null)
            {
                throw new ArgumentNullException("adressegruppeView");
            }
            return new Adressegruppe(adressegruppeView.Nummer, adressegruppeView.Navn,
                                     adressegruppeView.AdressegruppeOswebdb);
        }

        /// <summary>
        /// Mapper et betalingsbetingelseview til en betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseView">Betalingsbetingelseview.</param>
        /// <returns>Betalingsbetingelse.</returns>
        private static Betalingsbetingelse MapBetalingsbetingelse(TabelView betalingsbetingelseView)
        {
            if (betalingsbetingelseView == null)
            {
                throw new ArgumentNullException("betalingsbetingelseView");
            }
            return new Betalingsbetingelse(betalingsbetingelseView.Nummer, betalingsbetingelseView.Navn);
        }

        #endregion
    }
}
