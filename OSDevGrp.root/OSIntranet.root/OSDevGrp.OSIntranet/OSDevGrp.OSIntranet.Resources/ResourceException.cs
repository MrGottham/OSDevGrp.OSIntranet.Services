using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Resources
{
    /// <summary>
    /// Resourceexception.
    /// </summary>
    [Serializable]
    public class ResourceException : Exception
    {
        #region Constructors

        /// <summary>
        /// Danner en resourceexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ResourceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en resourceexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ResourceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Danner en resourceexception.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        protected ResourceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
