﻿namespace OSDevGrp.OSIntranet.Contracts
{
    /// <summary>
    /// Indeholder en samlet registrering af de soap namespaces, 
    /// der anvendes i løsningen.
    /// </summary>
    public static class SoapNamespaces
    {
        /// <summary>
        /// Namespace for commands, queries, views m.m.
        /// </summary>
        public const string IntranetNamespace = "urn:osdevgrp:osintranet:1.0.0";

        /// <summary>
        /// Namespace for commands, queries, view etc. used by food waste services.
        /// </summary>
        public const string FoodWasteNamespace = "urn:osdevgrp:foodwaste:1.0.0";

        /// <summary>
        /// Navn på service til adressekartotek.
        /// </summary>
        public const string AdressekartotekServiceName = "AdressekartotekService";

        /// <summary>
        /// Navn på service til finansstyring.
        /// </summary>
        public const string FinansstyringServiceName = "FinansstyringService";

        /// <summary>
        /// Navn på service til kaldender.
        /// </summary>
        public const string KalenderServiceName = "KalenderService";

        /// <summary>
        /// Navn på service til fælles elementer.
        /// </summary>
        public const string CommonServiceName = "CommonService";

        /// <summary>
        /// Name for the service which can access and modify system data in the food waste domain.
        /// </summary>
        public const string FoodWasteSystemDataServiceName = "SystemDataService";

        /// <summary>
        /// Name for the service which can access and modify data on a house hold in the food waste domain.
        /// </summary>
        public const string FoodWasteHouseholdDataServiceName = "HouseholdDataService";
    }
}
