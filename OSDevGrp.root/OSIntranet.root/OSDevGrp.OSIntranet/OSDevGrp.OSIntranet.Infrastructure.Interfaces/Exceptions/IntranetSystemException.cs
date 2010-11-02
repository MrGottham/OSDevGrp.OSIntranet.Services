using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Systemexception.
    /// </summary>
    [Serializable]
    public class IntranetSystemException : Exception
    {
        #region Constructors

        /// <summary>
        /// Danner en systemexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public IntranetSystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en systemexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public IntranetSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Danner en systemexception.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected IntranetSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
