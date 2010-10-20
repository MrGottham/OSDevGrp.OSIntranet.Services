﻿using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for adressekartoteket.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.AdresseRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IAdresseRepositoryService : IRepositoryService
    {
        /// <summary>
        /// Henter alle personer.
        /// </summary>
        /// <param name="personGetAllQuery">Query til forespørgelse efter alle personer.</param>
        /// <returns>Alle personer.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<PersonView> PersonGetAll(PersonGetAllQuery personGetAllQuery);

        /// <summary>
        /// Henter en given person.
        /// </summary>
        /// <param name="personGetByNummerQuery">Query til forespørgelse efter en given person.</param>
        /// <returns>Person.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PersonView PersonGetByNummer(PersonGetByNummerQuery personGetByNummerQuery);

        /// <summary>
        /// Henter alle firmaer.
        /// </summary>
        /// <param name="firmaGetAllQuery">Query til forespørgelse efter alle firmaer.</param>
        /// <returns>Alle firmaer.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<FirmaView> FirmaGetAll(FirmaGetAllQuery firmaGetAllQuery);

        /// <summary>
        /// Henter et givent firma.
        /// </summary>
        /// <param name="firmaGetByNummerQuery">Query til forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FirmaView FirmaGetByNummer(FirmaGetByNummerQuery firmaGetByNummerQuery);

        /// <summary>
        /// Henter alle adresser til en adresseliste.
        /// </summary>
        /// <param name="adresselisteGetAllQuery">Query til forespørgelse efter alle adresser til en adresseliste.</param>
        /// <returns>Alle adresser til en adresseliste.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<AdresselisteView> AdresselisteGetAll(AdresselisteGetAllQuery adresselisteGetAllQuery);

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <param name="postnummerGetAllQuery">Query til forespørgelse efter alle postnumre.</param>
        /// <returns>Alle postnumre.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<PostnummerView> PostnummerGetAll(PostnummerGetAllQuery postnummerGetAllQuery);

        /// <summary>
        /// Henter alle postnumre for en given landekode.
        /// </summary>
        /// <param name="postnummerGetByLandekodeQuery">Query til forespørgelse efter alle postnumre for en given landekode.</param>
        /// <returns>Alle postnumre for den givne landekode.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<PostnummerView> PostnummerGetAllByLandekode(PostnummerGetByLandekodeQuery postnummerGetByLandekodeQuery);

        /// <summary>
        /// Henter bynavnet til et givent postnummer på en given landekode.
        /// </summary>
        /// <param name="bynavnGetByLandekodeAndPostnummerQuery">Query til forespørgelse efter bynavnet for et givent postnummer på en given landekode.</param>
        /// <returns>Landekode, postnummer og bynavn</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PostnummerView BynavnGetByLandekodeAndPostnummre(BynavnGetByLandekodeAndPostnummerQuery bynavnGetByLandekodeAndPostnummerQuery);

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="adressegruppeGetAllQuery">Query til forespørgelse efter alle adressegrupper.</param>
        /// <returns>Alle adressegrupper.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<AdressegruppeView> AdressegruppeGetAll(AdressegruppeGetAllQuery adressegruppeGetAllQuery);

        /// <summary>
        /// Henter en given adressegruppe.
        /// </summary>
        /// <param name="adressegruppeGetByNummerQuery">Query til forespørgelse efter en given adressegruppe.</param>
        /// <returns>Adressegruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        AdressegruppeView AdressegruppeGetByNummer(AdressegruppeGetByNummerQuery adressegruppeGetByNummerQuery);

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="betalingsbetingelseGetAllQuery">Query til forespørgelse efter alle betalingsbetingelser.</param>
        /// <returns>Alle betalingsbetingelser.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<BetalingsbetingelseView> BetalingsbetingelseGetAll(BetalingsbetingelseGetAllQuery betalingsbetingelseGetAllQuery);

        /// <summary>
        /// Henter en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseGetByNummerQuery">Query til forespørgelse efter en given betalingsbetingelse.</param>
        /// <returns>Betalingsbetingelse.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BetalingsbetingelseView BetalingsbetingelseGetByNummer(BetalingsbetingelseGetByNummerQuery betalingsbetingelseGetByNummerQuery);
    }
}
