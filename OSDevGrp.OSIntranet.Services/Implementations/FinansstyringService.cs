using System;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Services;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.Services.Implementations
{
    /// <summary>
    /// Service til finansstyring.
    /// </summary>
    public class FinansstyringService : IntranetServiceBase, IFinansstyringService
    {
        #region Private variables

        private readonly ICommandBus _commandBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner service til finansstyring.
        /// </summary>
        /// <param name="commandBus">Implementering af en CommandBus.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public FinansstyringService(ICommandBus commandBus)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            _commandBus = commandBus;
        }

        #endregion

        #region IFinansstyringService Members

        /// <summary>
        /// Opretter en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <returns>Svar fra oprettelse af en bogføringslinje.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BogføringslinjeOpretResponse BogføringslinjeOpret(BogføringslinjeOpretCommand command)
        {
            try
            {
                return _commandBus.Publish<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(command);
            }
            catch (IntranetRepositoryException ex)
            {
                throw CreateIntranetRepositoryFault(ex);
            }
            catch (IntranetBusinessException ex)
            {
                throw CreateIntranetBusinessFault(ex);
            }
            catch (IntranetSystemException ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
            catch (Exception ex)
            {
                throw CreateIntranetSystemFault(ex);
            }
        }

        #endregion
    }
}