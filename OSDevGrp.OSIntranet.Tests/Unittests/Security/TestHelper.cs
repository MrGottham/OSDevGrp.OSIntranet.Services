using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Security
{
    /// <summary>
    /// Helper for tests.
    /// </summary>
    internal static class TestHelper
    {
        /// <summary>
        /// Gets an certificate.
        /// </summary>
        /// <param name="subjectName">Subject name for the certificate.</param>
        /// <returns>Certificate.</returns>
        internal static X509Certificate2 GetCertificate(string subjectName)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            try
            {
                return store.Certificates
                    .OfType<X509Certificate2>()
                    .FirstOrDefault(c => string.Compare(c.SubjectName.Name, subjectName, StringComparison.Ordinal) == 0);
            }
            finally
            {
                store.Close();
            }
        }
    }
}
