using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Contracts.Views;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
    /// </summary>
    public class BogføringslinjeOpretCommandHandler : RegnskabCommandHandlerBase, ICommandHandler<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>
    {
        #region Private variables

        private readonly IKonfigurationRepository _konfigurationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner CommandHandler til håndtering af kommandoen: BogføringslinjeOpretCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer i domænet.</param>
        /// <param name="konfigurationRepository">Implementering af konfigurationsrepository.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BogføringslinjeOpretCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IKonfigurationRepository konfigurationRepository, IObjectMapper objectMapper)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper)
        {
            if (konfigurationRepository == null)
            {
                throw new ArgumentNullException("konfigurationRepository");
            }
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

            var bogføringslinje = Repository.BogføringslinjeAdd(command.Dato, command.Bilag, konto, command.Tekst,
                                                                budgetkonto, command.Debit, command.Kredit, adressekonto);

            throw new NotImplementedException();


            /*



            var bogføringslinje = new Bogføringslinje(int.MaxValue, command.Dato, command.Bilag, command.Tekst,
                                                      command.Debit, command.Kredit);
            Repository.BogføringslinjeAdd(bogføringslinje.Dato, bogføringslinje.Bilag, konto, bogføringslinje.Tekst,
                                          budgetkonto, bogføringslinje.Debit, bogføringslinje.Kredit, adressekonto);

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
            */
        }

        /// <summary>
        /// Exceptionhandling ved oprettelse af en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <param name="exception">Exception.</param>
        [RethrowException(typeof(IntranetRepositoryException), typeof(IntranetBusinessException), typeof(IntranetSystemException))]
        public void HandleException(BogføringslinjeOpretCommand command, Exception exception)
        {
            throw this.CreateIntranetSystemExceptionException<BogføringslinjeOpretCommand, BogføringslinjeOpretResponse>(command, exception);
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
            var adresselisteHelper = new AdresselisteHelper(AdresseRepository.AdresseGetAll());
            var brevhovedlisteHelper = new BrevhovedlisteHelper(FællesRepository.BrevhovedGetAll());
            var regnskab = Repository.RegnskabGet(command.Regnskabsnummer, brevhovedlisteHelper.GetById,
                                                  adresselisteHelper.GetById);
            var currentTime = DateTime.Now;
            if (command.Dato.Date < currentTime.AddDays(_konfigurationRepository.DageForBogføringsperiode * -1).Date)
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
                konto = regnskab.Konti.OfType<Konto>().Single(m => m.Kontonummer.CompareTo(command.Kontonummer) == 0);
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
                    budgetkonto =
                        regnskab.Konti.OfType<Budgetkonto>().Single(
                            m => m.Kontonummer.CompareTo(command.Budgetkontonummer) == 0);
                }
                catch (InvalidOperationException ex)
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
            adressekonto = adresselisteHelper.GetById(command.Adressekonto);
        }

        /// <summary>
        /// Danner svar for oprettelse af bogføringslinje.
        /// </summary>
        /// <param name="konto">Konto.</param>
        /// <param name="budgetkonto">Budgetkonto.</param>
        /// <returns>Svar for oprettelse af bogføringslinje.</returns>
        private BogføringslinjeOpretResponse CreateResponse(Konto konto, Budgetkonto budgetkonto)
        {
            var advarsler = new List<BogføringsadvarselResponse>();
            if (konto.DisponibelPrStatusdato < 0M)
            {
                var advarsel = new BogføringsadvarselResponse
                                   {
                                       Advarsel = Resource.GetExceptionMessage(ExceptionMessage.AccountIsOverdrawn),
                                       Konto = ObjectMapper.Map<Konto, KontoView>(konto),
                                       Beløb = Math.Abs(konto.DisponibelPrStatusdato)
                                   };
                advarsler.Add(advarsel);
            }
            if (budgetkonto != null)
            {
                if (budgetkonto.BudgetPrStatusdato <= 0M && budgetkonto.DisponibelPrStatusdato < 0M)
                {
                    var advarsel = new BogføringsadvarselResponse
                                       {
                                           Advarsel = Resource.GetExceptionMessage(ExceptionMessage.BudgetAccountIsOverdrawn),
                                           Konto = ObjectMapper.Map<Budgetkonto, BudgetkontoView>(budgetkonto),
                                           Beløb = Math.Abs(budgetkonto.DisponibelPrStatusdato)
                                       };
                    advarsler.Add(advarsel);
                }
            }
            var response = new BogføringslinjeOpretResponse
                               {
                                   Advarsler = advarsler
                               };
            return response;
        }
    }
}
