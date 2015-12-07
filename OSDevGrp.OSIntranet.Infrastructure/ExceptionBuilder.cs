using System;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Infrastructure
{
    /// <summary>
    /// Exception builder.
    /// </summary>
    public class ExceptionBuilder : IExceptionBuilder
    {
        #region Methods

        /// <summary>
        /// Build an exception which should be thrown.
        /// </summary>
        /// <param name="exception">Exception that was occurred.</param>
        /// <param name="method">Method where the exception occurred.</param>
        /// <returns>Exception to throw.</returns>
        public virtual Exception Build(Exception exception, MethodBase method)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (exception is IntranetRepositoryException)
            {
                return exception;
            }
            if (exception is IntranetBusinessException)
            {
                return exception;
            }
            if (exception is IntranetSystemException)
            {
                return exception;
            }

            var methodClassType = method.ReflectedType;
            var commandHandlerInterface = methodClassType.GetInterface(typeof (ICommandHandler<>).Name);
            if (commandHandlerInterface != null && commandHandlerInterface.IsGenericType && commandHandlerInterface.GenericTypeArguments.Length == 1)
            {
                return new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue, commandHandlerInterface.GetGenericArguments().ElementAt(0).Name, exception.Message), exception);
            }
            commandHandlerInterface = methodClassType.GetInterface(typeof (ICommandHandler<,>).Name);
            if (commandHandlerInterface != null && commandHandlerInterface.IsGenericType && commandHandlerInterface.GenericTypeArguments.Length == 2)
            {
                throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue, commandHandlerInterface.GetGenericArguments().ElementAt(0).Name, commandHandlerInterface.GetGenericArguments().ElementAt(1).Name, exception.Message), exception);
            }

            return exception;
        }

        #endregion
    }
}
