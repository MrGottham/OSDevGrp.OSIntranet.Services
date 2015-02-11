using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Security.Core
{
    /// <summary>
    /// Helper functionality for certificates.
    /// </summary>
    internal static class CertificateHelper
    {
        #region Methods

        /// <summary>
        /// Gets a certificate.
        /// </summary>
        /// <param name="storeName">Certificate store name.</param>
        /// <param name="storeLocation">Certificate location.</param>
        /// <param name="subjectName">Subject name for the certificate to get.</param>
        /// <returns>Certificate.</returns>
        internal static X509Certificate2 GetCertificate(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            if (string.IsNullOrEmpty(subjectName))
            {
                throw new ArgumentNullException("subjectName");
            }
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            try
            {
                foreach (var certificate in store.Certificates.OfType<X509Certificate2>())
                {
                    if (certificate.SubjectName.Name == null)
                    {
                        continue;
                    }
                    if (string.Compare(certificate.SubjectName.Name, subjectName, StringComparison.Ordinal) == 0)
                    {
                        return new X509Certificate2(certificate);
                    }
                }
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.CertificateWasNotFound, subjectName));
            }
            finally
            {
                store.Close();
            }
        }

        #endregion
    }
}
