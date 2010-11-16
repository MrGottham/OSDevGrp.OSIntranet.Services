using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.Contracts;
using OSDevGrp.OSIntranet.Contracts.Faults;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Basisklasse for en service til OS Intranet.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class IntranetServiceBase
    {
        #region Methods

        /// <summary>
        /// Danner en fault for en fejl, som er opstået i repositories under OS Intranet.
        /// </summary>
        /// <param name="exception">Exception, som er opstået i repositories.</param>
        /// <returns>Fault.</returns>
        protected FaultException<IntranetRepositoryFault> CreateIntranetRepositoryFault(IntranetRepositoryException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var fault = new IntranetRepositoryFault
                            {
                                Message = exception.Message,
                                ExceptionMessages = FormatExceptionMessage(exception)
                            };
            return new FaultException<IntranetRepositoryFault>(fault);
        }

        /// <summary>
        /// Danner en fault for en fejl, som er opstået i forretningslogik under OS Intranet.
        /// </summary>
        /// <param name="exception">Exception, som er opstået i forretningslogik.</param>
        /// <returns>Fault.</returns>
        protected FaultException<IntranetBusinessFault> CreateIntranetBusinessFault(IntranetBusinessException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var fault = new IntranetBusinessFault
                            {
                                Message = exception.Message,
                                ExceptionMessages = FormatExceptionMessage(exception)
                            };
            return new FaultException<IntranetBusinessFault>(fault);
        }

        /// <summary>
        /// Danner en fault for en systemfejl, som er opstået i OS Intranet.
        /// </summary>
        /// <param name="exception">Exception, som er opstået som systemfejl.</param>
        /// <returns>Fault.</returns>
        protected FaultException<IntranetSystemFault> CreateIntranetSystemFault(IntranetSystemException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var fault = new IntranetSystemFault
                            {
                                Message = exception.Message,
                                ExceptionMessages = FormatExceptionMessage(exception)
                            };
            return new FaultException<IntranetSystemFault>(fault);
        }

        /// <summary>
        /// Danner en fault for en fejl, som er opstået i OS Intranet.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Fault.</returns>
        protected FaultException<IntranetSystemFault> CreateIntranetSystemFault(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            var fault = new IntranetSystemFault
                            {
                                Message = exception.Message,
                                ExceptionMessages = FormatExceptionMessage(exception)
                            };
            return new FaultException<IntranetSystemFault>(fault);
        }

        /// <summary>
        /// Formaterer fejlbeskeder på en exception og dens inner exceptions.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Formaterer fejlbesekder.</returns>
        private static string FormatExceptionMessage(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (exception.InnerException != null)
            {
                return string.Format("{0}\r\n\r\n{1}", exception.Message,
                                     FormatExceptionMessage(exception.InnerException));
            }
            return exception.Message;
        }

        #endregion
    }
}