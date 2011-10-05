using System;
using System.Reflection;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
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
    }
}
