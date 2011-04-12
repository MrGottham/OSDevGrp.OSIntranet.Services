using System;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    public class BogføringslinjeOpretCommandHandler : CommandHandlerNonTransactionalBase, ICommandHandler<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;
        private readonly IKonfigurationRepository _konfigurationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="konfigurationRepository">Implementering af konfigurationsrepository.</param>
        public BogføringslinjeOpretCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IKonfigurationRepository konfigurationRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (konfigurationRepository == null)
            {
                throw new ArgumentNullException("konfigurationRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _adresseRepository = adresseRepository;
            _konfigurationRepository = konfigurationRepository;
        }

        #endregion

        #region ICommandHandler<BogføringslinjeOpretCommand,BogføringslinjeOpretResponse> Members

        /// <summary>
        /// Oprettelse af en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <returns>Svar for oprettelse af en bogføringslinje.</returns>
        public BogføringslinjeOpretResponse Execute(BogføringslinjeOpretCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Exceptionhandling ved oprettelse af en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <param name="exception">Exception.</param>
        [RethrowException(typeof(IntranetRepositoryException), typeof(IntranetBusinessException), typeof(IntranetSystemException))]
        public void HandleException(BogføringslinjeOpretCommand command, Exception exception)
        {
            throw new IntranetSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandlerWithReturnValue,
                                             typeof (BogføringslinjeOpretCommand), typeof (BogføringslinjeOpretResponse),
                                             exception.Message));
        }

        #endregion
    }
}
