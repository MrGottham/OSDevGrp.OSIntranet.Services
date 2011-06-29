using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    /// <summary>
    /// Interface til at bygge objekter i domænemodellen.
    /// </summary>
    public interface IDomainObjectBuilder
    {
        /// <summary>
        /// Callbackmetode, som domæneobjekbyggeren benytter til at hente en given adresse.
        /// </summary>
        Func<int, AdresseBase> GetAdresseBaseCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given adressegruppe.
        /// </summary>
        Func<int, Adressegruppe> GetAdressegruppeCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given betalingsbetingelse.
        /// </summary>
        Func<int, Betalingsbetingelse> GetBetalingsbetingelseCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente et givent regnskab.
        /// </summary>
        Func<int, Regnskab> GetRegnskabCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given kontogruppe.
        /// </summary>
        Func<int, Kontogruppe> GetKontogruppeCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente en given gruppe til budgetkonti.
        /// </summary>
        Func<int, Budgetkontogruppe> GetBudgetkontogruppeCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Callbackmetode, som domæneobjektbyggeren benytter til at hente et givent brevhoved.
        /// </summary>
        Func<int, Brevhoved> GetBrevhovedCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Bygger objekt i domænemodellen.
        /// </summary>
        /// <typeparam name="TSource">Typen på objektet, hvorfra domæneobjektet skal bygges.</typeparam>
        /// <typeparam name="TDomainObject">Typen på domæneobjektet.</typeparam>
        /// <param name="source">Objektet, hvorfra domæneobjektet skal bygges.</param>
        /// <returns>Domæneobjekt.</returns>
        TDomainObject Build<TSource, TDomainObject>(TSource source);

        /// <summary>
        /// Bygger flere instansere af et objekt i domænemodellen.
        /// </summary>
        /// <typeparam name="TSource">Typen på objekterne, hvorfra domæneobjekter skal bygges.</typeparam>
        /// <typeparam name="TDomainObject">Typen på domæneobjekterne.</typeparam>
        /// <param name="sources">Objekter, hvorfra domæneobjekter skal bygges.</param>
        /// <returns>Domæneobjekter.</returns>
        IEnumerable<TDomainObject> BuildMany<TSource, TDomainObject>(IEnumerable<TSource> sources);
    }
}
