using System;
using System.Reflection;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies
{
    /// <summary>
    /// Hjælpeklasse til en data proxy.
    /// </summary>
    public static class DataProxyHelper
    {
        /// <summary>
        /// Sætter værdi for en given variable i en data proxy.
        /// </summary>
        /// <typeparam name="TValue">Typen på værdien, der skal sættes.</typeparam>
        /// <param name="dataProxy">Data proxy, hvorpå værdi skal sættes.</param>
        /// <param name="fieldName">Navnet på variablen, hvis værdi skal sættes.</param>
        /// <param name="value">Værdi.</param>
        public static void SetFieldValue<TValue>(this IDataProxyBase dataProxy, string fieldName, TValue value)
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException("dataProxy");
            }
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            if (Equals(value, null))
            {
                throw new ArgumentNullException("value");
            }

            var field = dataProxy.GetType().GetField(fieldName,
                                                     BindingFlags.Instance | BindingFlags.NonPublic |
                                                     BindingFlags.Public);
            if (field != null)
            {
                field.SetValue(dataProxy, value);
                return;
            }

            var baseType = dataProxy.GetType().BaseType;
            if (baseType != null)
            {
                field = baseType.GetField(fieldName,
                                          BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (field != null)
                {
                    field.SetValue(dataProxy, value);
                    return;
                }
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, fieldName,
                                                                           "fieldName"));
        }

        /// <summary>
        /// Returnerer en nullable streng, som kan benyttes i SQL.
        /// </summary>
        /// <param name="dataProxy">Data proxy, hvorpå strengen skal returneres.</param>
        /// <param name="value">Værdi for den nullable streng.</param>
        /// <returns>Streng, som kan benyttes </returns>
        public static string GetNullableSqlString(this IDataProxyBase dataProxy, string value)
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException("dataProxy");
            }
            return value == null ? "NULL" : string.Format("'{0}'", value);
        }

        /// <summary>
        /// Henter og returnerer en given data proxy gennem en data provider.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy, som skal hentes.</typeparam>
        /// <param name="dataProxy">Data proxy, hvorfra en given data proxy skal hentes.</param>
        /// <param name="dataProvider">Data provider, der skal hente data proxy.</param>
        /// <param name="queryForDataProxy">Data proxy indeholdende de nødvendige værdier til at hente al data gennem data provideren.</param>
        /// <param name="callerName">Navn på metoden, der kalder.</param>
        /// <returns>Data proxy indeholdende</returns>
        public static TDataProxy Get<TDataProxy>(this IDataProxyBase dataProxy, IDataProviderBase dataProvider, TDataProxy queryForDataProxy, string callerName) where TDataProxy : class, IDataProxyBase, new()
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException("dataProxy");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (queryForDataProxy == null)
            {
                throw new ArgumentNullException("queryForDataProxy");
            }
            if (string.IsNullOrEmpty(callerName))
            {
                throw new ArgumentNullException("callerName");
            }
            try
            {
                return dataProvider.Get(queryForDataProxy);
            }
            catch (IntranetRepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.RepositoryError, callerName, ex.Message), ex);
            }
        }
    }
}
