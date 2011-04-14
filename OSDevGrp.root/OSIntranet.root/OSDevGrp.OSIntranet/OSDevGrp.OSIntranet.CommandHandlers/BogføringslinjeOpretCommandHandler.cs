using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
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
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Konto konto;
            Budgetkonto budgetkonto;
            AdresseBase adressekonto;
            EvaluateCommand(command, out konto, out budgetkonto, out adressekonto);

            var bogføringslinje = new Bogføringslinje(int.MaxValue, command.Dato, command.Bilag, command.Tekst,
                                                      command.Debit, command.Kredit);
            _finansstyringRepository.BogføringslinjeAdd(bogføringslinje.Dato, bogføringslinje.Bilag, konto,
                                                        bogføringslinje.Tekst, budgetkonto, bogføringslinje.Debit,
                                                        bogføringslinje.Kredit, adressekonto);

            konto.TilføjBogføringslinje(bogføringslinje);
            konto.Calculate(bogføringslinje.Dato, bogføringslinje.Løbenummer);
            if (budgetkonto != null)
            {
                budgetkonto.TilføjBogføringslinje(bogføringslinje);
                budgetkonto.Calculate(bogføringslinje.Dato, bogføringslinje.Løbenummer);
            }
            if (adressekonto != null)
            {
                adressekonto.TilføjBogføringslinje(bogføringslinje);
                adressekonto.Calculate(bogføringslinje.Dato, bogføringslinje.Løbenummer);
            }

            return CreateResponse(konto, budgetkonto);
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

        /// <summary>
        /// Evaluerer kommando til oprettelse af en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <param name="konto">Returnering af konto, hvorpå bogføringslinjen skal bogføres.</param>
        /// <param name="budgetkonto">Returnering af budgetkonto, hvorpå bogføringslinjen skal bogføres.</param>
        /// <param name="adressekonto">Returnering af adressekonto, hvorpå bogføringslinjen skal bogføres.</param>
        private void EvaluateCommand(BogføringslinjeOpretCommand command, out Konto konto, out Budgetkonto budgetkonto, out AdresseBase adressekonto)
        {
            var adresser = _adresseRepository.AdresseGetAll();
            var regnskab = _finansstyringRepository.RegnskabGet(command.Regnskabsnummer, nummer =>
                                                                                             {
                                                                                                 var adresse = adresser.SingleOrDefault(m => m.Nummer == nummer);
                                                                                                 if (adresse == null)
                                                                                                 {
                                                                                                     var message = Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, "AdresseBase", nummer);
                                                                                                     throw new IntranetRepositoryException(message);
                                                                                                 }
                                                                                                 return adresse;
                                                                                             });
            var currentTime = DateTime.Now;
            if (command.Dato.Date < currentTime.AddDays(_konfigurationRepository.DageForBogføringsperiode*-1).Date)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld,
                                                                                 _konfigurationRepository.
                                                                                     DageForBogføringsperiode));
            }
            if (command.Dato.Date > currentTime.Date)
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateIsForwardInTime));
            }
            if (string.IsNullOrEmpty(command.Kontonummer))
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.BalanceLineAccountNumberMissing));
            }
            try
            {
                konto = regnskab.Konti.OfType<Konto>().Single(m => m.Kontonummer == command.Kontonummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Konto),
                                                 command.Kontonummer), ex);
            }
            if (string.IsNullOrEmpty(command.Tekst))
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineTextMissing));
            }
            budgetkonto = null;
            if (!string.IsNullOrEmpty(command.Budgetkontonummer))
            {
                try
                {
                    budgetkonto = regnskab.Konti
                        .OfType<Budgetkonto>()
                        .Single(m => m.Kontonummer == command.Budgetkontonummer);
                }
                catch (Exception ex)
                {
                    throw new IntranetRepositoryException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto),
                                                     command.Budgetkontonummer), ex);
                }
            }
            if (command.Debit < 0M)
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero));
            }
            if (command.Kredit < 0M)
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero));
            }
            if (command.Debit == 0M && command.Kredit == 0M)
            {
                throw new IntranetBusinessException(
                    Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueMissing));
            }
            adressekonto = null;
            if (command.Adressekonto == 0)
            {
                return;
            }
            try
            {
                adressekonto = adresser.Single(m => m.Nummer == command.Adressekonto);
            }
            catch (Exception ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (AdresseBase),
                                                 command.Adressekonto), ex);
            }
        }

        /// <summary>
        /// Danner svar for oprettelse af bogføringslinje.
        /// </summary>
        /// <param name="konto">Konto.</param>
        /// <param name="budgetkonto">Budgetkonto.</param>
        /// <returns>Svar for oprettelse af bogføringslinje.</returns>
        private static BogføringslinjeOpretResponse CreateResponse(Konto konto, Budgetkonto budgetkonto)
        {
            var response = new BogføringslinjeOpretResponse
                               {
                                   Advarsler = new List<string>()
                               };
            return response;
        }
    }
}
