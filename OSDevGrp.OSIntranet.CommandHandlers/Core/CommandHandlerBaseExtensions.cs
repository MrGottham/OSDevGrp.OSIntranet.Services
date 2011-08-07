using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Extensions til basisklassen CommandHandlerBase.
    /// </summary>
    public static class CommandHandlerBaseExtensions
    {
        #region Methods

        /// <summary>
        /// Mapper et domæneobjekt til et view.
        /// </summary>
        /// <typeparam name="TDomainObject">Typen på domæneobjektet, der skal mappes.</typeparam>
        /// <typeparam name="TView">Typen på viewet, der skal mappes til.</typeparam>
        /// <param name="commandHandler">CommandHandler, hvorpå metoden skal udføres.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        /// <param name="domainObject">Domæneobjektet, der skal mappes.</param>
        /// <returns>View.</returns>
        public static TView Map<TDomainObject, TView>(this CommandHandlerBase commandHandler, IObjectMapper objectMapper, TDomainObject domainObject)
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException("commandHandler");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            if (Equals(domainObject, null))
            {
                throw new ArgumentNullException("domainObject");
            }
            return objectMapper.Map<TDomainObject, TView>(domainObject);
        }

        /// <summary>
        /// Mapper et eller flere domæneobjekter til et eller flere views.
        /// </summary>
        /// <typeparam name="TDomainObject">Typen på domæneobjekter, der skal mappes.</typeparam>
        /// <typeparam name="TView">Typen på views, der skal mappes til.</typeparam>
        /// <param name="commandHandler">CommandHandler, hvorpå metoden skal udføres.</param>
        /// <param name="objectMapper">Domæneobjektet, der skal mappes.</param>
        /// <param name="domainObjects">Domæneobjekter, der skal mappes.</param>
        /// <returns>Views.</returns>
        public static IEnumerable<TView> MapMany<TDomainObject, TView>(this CommandHandlerBase commandHandler, IObjectMapper objectMapper, IEnumerable<TDomainObject> domainObjects)
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException("commandHandler");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            if (Equals(domainObjects, null))
            {
                throw new ArgumentNullException("domainObjects");
            }
            return objectMapper.Map<IEnumerable<TDomainObject>, IEnumerable<TView>>(domainObjects);
        }

        /// <summary>
        /// Håndterer en kastet exception fra en commandhandler, hvorefter der returneres en IntranetSystemException på baggrund heraf.
        /// </summary>
        /// <typeparam name="TCommand">Typen på kommandoen, som er behandlet af commandhandleren, der kaster exception.</typeparam>
        /// <param name="commandHandler">CommandHandler, der kaster exception.</param>
        /// <param name="command">Command.</param>
        /// <param name="exception">Kastet exception.</param>
        /// <returns>IntranetSystemException for den kastede exception.</returns>
        public static IntranetSystemException CreateIntranetSystemExceptionException<TCommand>(this CommandHandlerBase commandHandler, TCommand command, Exception exception) where TCommand : ICommand
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException("commandHandler");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            return
                new IntranetSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithoutReturnValue,
                                                 typeof (TCommand), exception.Message));
        }

        /// <summary>
        /// Håndterer en kastet exception fra en commandhandler, hvorefter der returneres en IntranetSystemException på baggrund heraf.
        /// </summary>
        /// <typeparam name="TCommand">Typen på kommandoen, som er behandlet af commandhandleren, der kaster exception.</typeparam>
        /// <typeparam name="TResponse">Type på svaret, som returneres fra commandhandleren, der kaster exception.</typeparam>
        /// <param name="commandHandler">CommandHandler, der kaster exception.</param>
        /// <param name="command">Kommando.</param>
        /// <param name="exception">Kastet exception.</param>
        /// <returns>IntranetSystemException for den kastede exception.</returns>
        public static IntranetSystemException CreateIntranetSystemExceptionException<TCommand, TResponse>(this CommandHandlerBase commandHandler, TCommand command, Exception exception) where TCommand : ICommand
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException("commandHandler");
            }
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            return
                new IntranetSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue,
                                                 typeof (TCommand), typeof (TResponse), exception.Message));
        }

        #endregion
    }
}
