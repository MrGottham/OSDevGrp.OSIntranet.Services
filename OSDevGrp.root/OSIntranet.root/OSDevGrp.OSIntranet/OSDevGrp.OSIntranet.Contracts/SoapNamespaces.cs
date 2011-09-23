namespace OSDevGrp.OSIntranet.Contracts
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
        /// Navn på service til adressekartotek.
        /// </summary>
        public const string AdressekartotekServiceName = "AdressekartotekService";

        /// <summary>
        /// Navn på service til finansstyring.
        /// </summary>
        public const string FinansstyringServiceName = "FinansstyringService";

        /// <summary>
        /// Navn på service til fælles elementer.
        /// </summary>
        public const string CommonServiceName = "CommonService";
    }
}
