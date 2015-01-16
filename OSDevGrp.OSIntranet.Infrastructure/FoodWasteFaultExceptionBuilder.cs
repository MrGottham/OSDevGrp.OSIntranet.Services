using System;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Infrastructure
{
    /// <summary>
    /// Builder which can a FaultException with FaultDetails which food waste services can throw.
    /// </summary>
    public class FoodWasteFaultExceptionBuilder : IFaultExceptionBuilder<FoodWasteFault>
    {
        #region Methods

        /// <summary>
        /// Build and returns a FaultException with FaultDetails.
        /// </summary>
        /// <param name="exception">Exception which the FaultException should be based on.</param>
        /// <param name="serviceName">Service name.</param>
        /// <param name="methodInfo">Method informations.</param>
        /// <returns>FaultException with FaultDetails.</returns>
        public virtual FaultException<FoodWasteFault> Build(Exception exception, string serviceName, MethodBase methodInfo)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            var faultDetails = new FoodWasteFault
            {
                ErrorMessage = exception.Message,
                ServiceName = serviceName,
                ServiceMethod = methodInfo.Name,
                StackTrace = exception.StackTrace,
            };
            if (exception.GetType() == typeof (IntranetRepositoryException))
            {
                faultDetails.FaultType = FoodWasteFaultType.RepositoryFault;
            }
            else if (exception.GetType() == typeof (IntranetSystemException))
            {
                faultDetails.FaultType = FoodWasteFaultType.SystemFault;
            }
            else if (exception.GetType() == typeof (IntranetBusinessException))
            {
                faultDetails.FaultType = FoodWasteFaultType.BusinessFault;
            }
            else
            {
                faultDetails.FaultType = FoodWasteFaultType.SystemFault;
            }
            return new FaultException<FoodWasteFault>(faultDetails, exception.Message);
        }

        #endregion
    }
}
