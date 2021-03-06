﻿using System;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
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
        /// <param name="exceptionBuilder">Implementering af builderen, der kan bygge exceptions.</param>
        public BogføringslinjeOpretCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IKonfigurationRepository konfigurationRepository, IObjectMapper objectMapper, IExceptionBuilder exceptionBuilder)
            : base(finansstyringRepository, adresseRepository, fællesRepository, objectMapper, exceptionBuilder)
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

            var bogføringslinje = Repository.BogføringslinjeAdd(command.Dato, command.Bilag, konto, command.Tekst, budgetkonto, command.Debit, command.Kredit, adressekonto);

            IBogføringsresultat bogføringsresultat = new Bogføringsresultat(bogføringslinje);

            return ObjectMapper.Map<IBogføringsresultat, BogføringslinjeOpretResponse>(bogføringsresultat);
        }

        /// <summary>
        /// Exceptionhandling ved oprettelse af en bogføringslinje.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <param name="exception">Exception.</param>
        public void HandleException(BogføringslinjeOpretCommand command, Exception exception)
        {
            throw ExceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
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
            var regnskab = Repository.RegnskabGet(command.Regnskabsnummer, brevhovedlisteHelper.GetById, adresselisteHelper.GetById);
            
            var currentTime = DateTime.Now;
            if (command.Dato.Date < currentTime.AddDays(_konfigurationRepository.DageForBogføringsperiode*-1).Date)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateToOld, _konfigurationRepository. DageForBogføringsperiode));
            }
            if (command.Dato.Date > currentTime.Date)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineDateIsForwardInTime));
            }
            
            if (string.IsNullOrEmpty(command.Kontonummer))
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineAccountNumberMissing));
            }
            try
            {
                konto = regnskab.Konti.OfType<Konto>().Single(m => String.Compare(m.Kontonummer, command.Kontonummer, StringComparison.Ordinal) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Konto).Name, command.Kontonummer), ex);
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
                    budgetkonto = regnskab.Konti.OfType<Budgetkonto>().Single(m => String.Compare(m.Kontonummer, command.Budgetkontonummer, StringComparison.Ordinal) == 0);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkonto).Name, command.Budgetkontonummer), ex);
                }
            }

            if (command.Debit < 0M)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero));
            }
            if (command.Kredit < 0M)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueBelowZero));
            }
            if (command.Debit == 0M && command.Kredit == 0M)
            {
                throw new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.BalanceLineValueMissing));
            }
            
            adressekonto = null;
            if (command.Adressekonto == 0)
            {
                return;
            }
            adressekonto = adresselisteHelper.GetById(command.Adressekonto);
        }
    }
}
