using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
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
    /// Repository til finansstyring.
    /// </summary>
    public class FinansstyringRepository : IFinansstyringRepository
    {
        #region Private constants

        private const string EndpointConfigurationName = "FinansstyringRepositoryService";

        #endregion

        #region IFinansstyringRepository Members

        /// <summary>
        /// Henter en liste af regnskaber.
        /// </summary>
        /// <returns>Liste af regnskaber.</returns>
        public IList<Regnskab> RegnskabslisteGet()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                var regnskabQuery = new RegnskabGetAllQuery();
                var regnskabViews = channel.RegnskabGetAll(regnskabQuery);
                return regnskabViews.Select(MapRegnskab).ToList();
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
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <returns>Regnskab.</returns>
        public Regnskab RegnskabGet(int nummer)
        {
            return RegnskabGet(nummer, null);
        }

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="nummer">Unik identifikation af regnskabet.</param>
        /// <param name="callback">Callbackmetode til at hente adressen for bogføringslinjer.</param>
        /// <returns>Regnskab.</returns>
        public Regnskab RegnskabGet(int nummer, Func<int, AdresseBase> callback)
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                // Hent regnskabet.
                var regnskabQuery = new RegnskabGetByNummerQuery
                                        {
                                            Regnskabsnummer = nummer
                                        };
                var regnskabView = channel.RegnskabGetByNummer(regnskabQuery);
                // Hent alle kontogrupper.
                var kontogruppeQuery = new KontogruppeGetAllQuery();
                var kontogruppeViews = channel.KontogruppeGetAll(kontogruppeQuery);
                // Hent alle budgetkontogrupper.
                var budgetgruppeQuery = new BudgetkontogruppeGetAllQuery();
                var budgetgruppeViews = channel.BudgetkontogruppeGetAll(budgetgruppeQuery);
                // Mapning og returnering af regnskab.)
                return MapRegnskab(regnskabView, kontogruppeViews.Select(MapKontogruppe).ToList(),
                                   budgetgruppeViews.Select(MapBudgetkontogruppe), callback);
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
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste af kontogrupper.</returns>
        public IList<Kontogruppe> KontogruppeGetAll()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new KontogruppeGetAllQuery();
                var kontogruppeViews = channel.KontogruppeGetAll(query);
                return kontogruppeViews.Select(MapKontogruppe).ToList();
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
        /// Hetner alle grupper til budgetkonti.
        /// </summary>
        /// <returns>Liste af grupper til budgetkonti.</returns>
        public IList<Budgetkontogruppe> BudgetkontogruppeGetAll()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>(EndpointConfigurationName);
            try
            {
                var query = new BudgetkontogruppeGetAllQuery();
                var budgetkontogruppeViews = channel.BudgetkontogruppeGetAll(query);
                return budgetkontogruppeViews.Select(MapBudgetkontogruppe).ToList();
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

        #region Private methods

        /// <summary>
        /// Mapper et regnskabslisteview til et regnskab.
        /// </summary>
        /// <param name="regnskabListeView">Regnskabslisteview.</param>
        /// <returns>Regnskab.</returns>
        private static Regnskab MapRegnskab(RegnskabListeView regnskabListeView)
        {
            if (regnskabListeView == null)
            {
                throw new ArgumentNullException("regnskabListeView");
            }
            return new Regnskab(regnskabListeView.Nummer, regnskabListeView.Navn);
        }

        /// <summary>
        /// Mapper et regnskabsview til et regnskab.
        /// </summary>
        /// <param name="regnskabView">Regnskabsview.</param>
        /// <param name="kontogrupper">Kontogrupper.</param>
        /// <param name="budgetkontogrupper">Budgetkontogrupper.</param>
        /// <param name="callback">Callbackmetode til at hente adressen for bogføringslinjer.</param>
        /// <returns>Regnskab.</returns>
        private static Regnskab MapRegnskab(RegnskabView regnskabView, IEnumerable<Kontogruppe> kontogrupper, IEnumerable<Budgetkontogruppe> budgetkontogrupper, Func<int, AdresseBase> callback)
        {
            if (regnskabView == null)
            {
                throw new ArgumentNullException("regnskabView");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            var regnskab = new Regnskab(regnskabView.Nummer, regnskabView.Navn);
            foreach (var kontoView in regnskabView.Konti)
            {
                regnskab.TilføjKonto(MapKonto(regnskab, kontoView, kontogrupper));
            }
            foreach (var budgetkontoView in regnskabView.Budgetkonti)
            {
                regnskab.TilføjKonto(MapBudgetkonto(regnskab, budgetkontoView, budgetkontogrupper));
            }
            foreach (var bogføringslinjeView in regnskabView.Konti.SelectMany(m => m.Bogføringslinjer).ToList())
            {
                MapBogføringslinje(bogføringslinjeView, regnskab.Konti.OfType<Konto>().ToList(),
                                   regnskab.Konti.OfType<Budgetkonto>().ToList(), callback);
            }
            return regnskab;
        }

        /// <summary>
        /// Mapper et kontolisteview til en konto.
        /// </summary>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="kontoView">Kontolisteview.</param>
        /// <param name="kontogrupper">Kontogrupper.</param>
        /// <returns>Konto.</returns>
        private static Konto MapKonto(Regnskab regnskab, KontoListeView kontoView, IEnumerable<Kontogruppe> kontogrupper)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (kontoView == null)
            {
                throw new ArgumentNullException("kontoView");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            Kontogruppe kontogruppe;
            try
            {
                kontogruppe = kontogrupper.Single(m => m.Nummer == kontoView.Kontogruppe.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Kontogruppe),
                                                 kontoView.Kontogruppe.Nummer), ex);
            }
            var konto = new Konto(regnskab, kontoView.Kontonummer, kontoView.Kontonavn, kontogruppe);
            if (!string.IsNullOrEmpty(kontoView.Beskrivelse))
            {
                konto.SætBeskrivelse(kontoView.Beskrivelse);
            }
            if (!string.IsNullOrEmpty(kontoView.Note))
            {
                konto.SætNote(kontoView.Note);
            }
            return konto;
        }

        /// <summary>
        /// Mapper et kontoview til en konto.
        /// </summary>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="kontoView">Kontolisteview.</param>
        /// <param name="kontogrupper">Kontogrupper.</param>
        /// <returns>Konto.</returns>
        private static Konto MapKonto(Regnskab regnskab, KontoView kontoView, IEnumerable<Kontogruppe> kontogrupper)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (kontoView == null)
            {
                throw new ArgumentNullException("kontoView");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            var konto = MapKonto(regnskab, kontoView as KontoListeView, kontogrupper);
            foreach (var kreditoplysningerView in kontoView.Kreditoplysninger)
            {
                konto.TilføjKreditoplysninger(MapKreditoplysninger(kreditoplysningerView));
            }
            return konto;
        }

        /// <summary>
        /// Mapper et kreditoplysningsview til kreditoplysninger.
        /// </summary>
        /// <param name="kreditoplysningerView">Kreditoplysningsview.</param>
        /// <returns>Kreditoplysninger.</returns>
        private static Kreditoplysninger MapKreditoplysninger(KreditoplysningerView kreditoplysningerView)
        {
            if (kreditoplysningerView == null)
            {
                throw new ArgumentNullException("kreditoplysningerView");
            }
            return new Kreditoplysninger(kreditoplysningerView.År, kreditoplysningerView.Måned,
                                         kreditoplysningerView.Kredit);
        }

        /// <summary>
        /// Mapper et budgetkontolisteview til en budgetkonto.
        /// </summary>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="budgetkontoView">Budgetkontolisteview.</param>
        /// <param name="budgetkontogrupper">Grupper af budgetkonti.</param>
        /// <returns>Budgetkonto.</returns>
        private static Budgetkonto MapBudgetkonto(Regnskab regnskab, BudgetkontoListeView budgetkontoView, IEnumerable<Budgetkontogruppe> budgetkontogrupper)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (budgetkontoView == null)
            {
                throw new ArgumentNullException("budgetkontoView");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            Budgetkontogruppe budgetkontogruppe;
            try
            {
                budgetkontogruppe = budgetkontogrupper.Single(m => m.Nummer == budgetkontoView.Budgetkontogruppe.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Kontogruppe),
                                                 budgetkontoView.Budgetkontogruppe.Nummer), ex);
            }
            var budgetkonto = new Budgetkonto(regnskab, budgetkontoView.Kontonummer, budgetkontoView.Kontonavn,
                                              budgetkontogruppe);
            if (!string.IsNullOrEmpty(budgetkontoView.Beskrivelse))
            {
                budgetkonto.SætBeskrivelse(budgetkontoView.Beskrivelse);
            }
            if (!string.IsNullOrEmpty(budgetkontoView.Note))
            {
                budgetkonto.SætNote(budgetkontoView.Note);
            }
            return budgetkonto;
        }

        /// <summary>
        /// Mapper et budgetkontoview til en budgetkonto.
        /// </summary>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="budgetkontoView">Budgetkontoview.</param>
        /// <param name="budgetkontogrupper">Grupper af budgetkonti.</param>
        /// <returns>Budgetkonto.</returns>
        private static Budgetkonto MapBudgetkonto(Regnskab regnskab, BudgetkontoView budgetkontoView, IEnumerable<Budgetkontogruppe> budgetkontogrupper)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (budgetkontoView == null)
            {
                throw new ArgumentNullException("budgetkontoView");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            var budgetkonto = MapBudgetkonto(regnskab, budgetkontoView as BudgetkontoListeView, budgetkontogrupper);
            foreach (var budgetoplysningerView in budgetkontoView.Budgetoplysninger)
            {
                budgetkonto.TilføjBudgetoplysninger(MapBudgetoplysninger(budgetoplysningerView));
            }
            return budgetkonto;
        }

        /// <summary>
        /// Mapper et budgetoplysningsview til budgetoplysninger.
        /// </summary>
        /// <param name="budgetoplysningerView">Budgetoplysningsview.</param>
        /// <returns>Budgetoplysninger.</returns>
        private static Budgetoplysninger MapBudgetoplysninger(BudgetoplysningerView budgetoplysningerView)
        {
            if (budgetoplysningerView == null)
            {
                throw new ArgumentNullException("budgetoplysningerView");
            }
            return new Budgetoplysninger(budgetoplysningerView.År, budgetoplysningerView.Måned,
                                         budgetoplysningerView.Indtægter, budgetoplysningerView.Udgifter);
        }

        /// <summary>
        /// Mapper et bogføringslinjeview til en bogføringslinje.
        /// </summary>
        /// <param name="bogføringslinjeView">Bogføringslinjeview.</param>
        /// <param name="konti">Konti.</param>
        /// <param name="budgetkonti">Budgetkonti.</param>
        /// <param name="callback">Callbackmetode til at hente adressen for bogføringslinjer.</param>
        /// <returns>Bogføringslinje.</returns>
        private static void MapBogføringslinje(BogføringslinjeView bogføringslinjeView, IEnumerable<Konto> konti, IEnumerable<Budgetkonto> budgetkonti, Func<int, AdresseBase> callback)
        {
            if (bogføringslinjeView == null)
            {
                throw new ArgumentNullException("bogføringslinjeView");
            }
            if (konti == null)
            {
                throw new ArgumentNullException("konti");
            }
            var bogføringslinje = new Bogføringslinje(bogføringslinjeView.Løbenummer, bogføringslinjeView.Dato,
                                                      bogføringslinjeView.Bilag, bogføringslinjeView.Tekst,
                                                      bogføringslinjeView.Debit, bogføringslinjeView.Kredit);
            Konto konto;
            try
            {
                konto = konti.Single(m => m.Kontonummer.CompareTo(bogføringslinjeView.Konto.Kontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Konto),
                                                 bogføringslinjeView.Konto.Kontonummer), ex);
            }
            konto.TilføjBogføringslinje(bogføringslinje);
            if (bogføringslinjeView.Budgetkonto != null)
            {
                Budgetkonto budgetkonto;
                try
                {
                    budgetkonto = budgetkonti
                        .Single(m => m.Kontonummer.CompareTo(bogføringslinjeView.Budgetkonto.Kontonummer) == 0);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IntranetRepositoryException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto),
                                                     bogføringslinjeView.Budgetkonto.Kontonummer), ex);
                }
                budgetkonto.TilføjBogføringslinje(bogføringslinje);
            }
            if (bogføringslinjeView.Adresse == null || callback == null)
            {
                return;
            }
            var adresse = callback(bogføringslinjeView.Adresse.Nummer);
            if (adresse == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById,
                                                                                   typeof (AdresseBase),
                                                                                   bogføringslinjeView.Adresse.Nummer));
            }
            adresse.TilføjBogføringslinje(bogføringslinje);
        }

        /// <summary>
        /// Mapper et kontogruppeview til en kontogruppe.
        /// </summary>
        /// <param name="kontogruppeView">Kontogruppeview.</param>
        /// <returns>Kontogruppe.</returns>
        private static Kontogruppe MapKontogruppe(KontogruppeView kontogruppeView)
        {
            if (kontogruppeView == null)
            {
                throw new ArgumentNullException("kontogruppeView");
            }
            return new Kontogruppe(kontogruppeView.Nummer, kontogruppeView.Navn, MapKontogruppeType(kontogruppeView.KontogruppeType));
        }

        /// <summary>
        /// Mapper et kontogruppetypeview til en kontogruppetype.
        /// </summary>
        /// <param name="kontogruppeType">Kontogruppetypeview.</param>
        /// <returns>Kontogruppetype.</returns>
        private static CommonLibrary.Domain.Enums.KontogruppeType MapKontogruppeType(DataAccess.Contracts.Enums.KontogruppeType kontogruppeType)
        {
            switch (kontogruppeType)
            {
                case DataAccess.Contracts.Enums.KontogruppeType.Aktiver:
                    return CommonLibrary.Domain.Enums.KontogruppeType.Aktiver;

                case DataAccess.Contracts.Enums.KontogruppeType.Passiver:
                    return CommonLibrary.Domain.Enums.KontogruppeType.Passiver;

                default:
                    throw new IntranetRepositoryException(
                        Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, kontogruppeType,
                                                     "KontogruppeType", MethodBase.GetCurrentMethod().Name));
            }
        }

        /// <summary>
        /// Mapper et budgetkontogruppeview til en gruppe af budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeView">Budgetkontogruppeview.</param>
        /// <returns>Gruppe af budgetkonti.</returns>
        private static Budgetkontogruppe MapBudgetkontogruppe(TabelView budgetkontogruppeView)
        {
            if (budgetkontogruppeView == null)
            {
                throw new ArgumentNullException("budgetkontogruppeView");
            }
            return new Budgetkontogruppe(budgetkontogruppeView.Nummer, budgetkontogruppeView.Navn);
        }

        #endregion
    }
}
