using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Logic executor which can execute basic logic.
    /// </summary>
    public class LogicExecuter : ILogicExecuter
    {
        #region Private variables

        private readonly ICommandBus _commandBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a logic executor which can execute basic logic.
        /// </summary>
        /// <param name="commandBus">Implementation of a command bus which can publish commands.</param>
        public LogicExecuter(ICommandBus commandBus)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            _commandBus = commandBus;
        }

        #endregion
    }
}
