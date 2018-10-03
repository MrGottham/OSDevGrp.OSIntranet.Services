using System;
using System.Data;
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
        public static void SetFieldValue<TValue, TDataReader, TDbCommand>(this IDataProxyBase<TDataReader, TDbCommand> dataProxy, string fieldName, TValue value) where TDataReader : IDataReader where TDbCommand : IDbCommand
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException(nameof(dataProxy));
            }
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }
            if (Equals(value, null))
            {
                throw new ArgumentNullException(nameof(value));
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
        /// <typeparam name="TDataReader">Type of the data reader which supports this data proxy.</typeparam>
        /// <typeparam name="TDbCommand">Type of the database command which can be used for SQL statements.</typeparam>
        /// <param name="dataProxy">Data proxy, hvorpå strengen skal returneres.</param>
        /// <param name="value">Værdi for den nullable streng.</param>
        /// <returns>Streng, som kan benyttes.</returns>
        public static string GetNullableSqlString<TDataReader, TDbCommand>(this IDataProxyBase<TDataReader, TDbCommand> dataProxy, string value) where TDataReader : IDataReader where TDbCommand : IDbCommand
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException(nameof(dataProxy));
            }
            return string.IsNullOrWhiteSpace(value) ? "NULL" : $"'{value}'";
        }

        /// <summary>
        /// Henter og returnerer en given data proxy gennem en data provider.
        /// </summary>
        /// <typeparam name="TDataProxy">Typen på data proxy, som skal hentes.</typeparam>
        /// <typeparam name="TDataReader">Type of the data reader which supports this data proxy.</typeparam>
        /// <typeparam name="TDbCommand">Type of the database command which can be used for SQL statements.</typeparam>
        /// <param name="dataProxy">Data proxy, hvorfra en given data proxy skal hentes.</param>
        /// <param name="dataProvider">Data provider, der skal hente data proxy.</param>
        /// <param name="queryForDataProxy">Data proxy indeholdende de nødvendige værdier til at hente al data gennem data provideren.</param>
        /// <param name="callerName">Navn på metoden, der kalder.</param>
        /// <returns>Data proxy indeholdende</returns>
        public static TDataProxy Get<TDataProxy, TDataReader, TDbCommand>(this IDataProxyBase<TDataReader, TDbCommand> dataProxy, IDataProviderBase<TDataReader, TDbCommand> dataProvider, TDataProxy queryForDataProxy, string callerName) where TDataProxy : class, IDataProxyBase<TDataReader, TDbCommand>, new() where TDataReader : IDataReader where TDbCommand : IDbCommand
        {
            if (dataProxy == null)
            {
                throw new ArgumentNullException(nameof(dataProxy));
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }
            if (queryForDataProxy == null)
            {
                throw new ArgumentNullException(nameof(queryForDataProxy));
            }
            if (string.IsNullOrEmpty(callerName))
            {
                throw new ArgumentNullException(nameof(callerName));
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
