using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Repositoryexception.
    /// </summary>
    [Serializable]
    public class IntranetRepositoryException : Exception
    {
        #region Constructors

        /// <summary>
        /// Danner en repositoryexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public IntranetRepositoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en repositoryexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Danner en repositoryexception.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected IntranetRepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
