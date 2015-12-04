using System;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for an exception builder.
    /// </summary>
    public interface IExceptionBuilder
    {
        /// <summary>
        /// Build an exception which should be thrown.
        /// </summary>
        /// <param name="exception">Exception that was occurred.</param>
        /// <param name="method">Method where the exception occurred.</param>
        /// <returns>Exception to throw.</returns>
        Exception Build(Exception exception, MethodBase method);
    }
}
