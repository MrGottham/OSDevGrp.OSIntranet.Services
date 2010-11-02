using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Businessexception.
    /// </summary>
    [Serializable]
    public class IntranetBusinessException : Exception
    {
        #region Constructors

        /// <summary>
        /// Danner en businessexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public IntranetBusinessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en businessexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetBusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Danner en businessexception.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected IntranetBusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
