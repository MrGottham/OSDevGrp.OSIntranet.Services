using System;
using System.Reflection;
using System.ServiceModel;

namespace OSDevGrp.OSIntranet.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface for a builder which can a FaultException with FaultDetails.
    /// </summary>
    /// <typeparam name="TFaultDetails">Type of the fault details.</typeparam>
    public interface IFaultExceptionBuilder<TFaultDetails>
    {
        /// <summary>
        /// Build and returns a FaultException with FaultDetails.
        /// </summary>
        /// <param name="exception">Exception which the FaultException should be based on.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="methodInfo">Method informations.</param>
        /// <returns>FaultException with FaultDetails.</returns>
        FaultException<TFaultDetails> Build(Exception exception, string serviceName, MethodBase methodInfo);
    }
}
