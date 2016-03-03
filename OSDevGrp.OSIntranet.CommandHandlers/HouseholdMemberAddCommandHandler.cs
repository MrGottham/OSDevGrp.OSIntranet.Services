using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Dispatchers;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which handles a command for adding a household member.
    /// </summary>
    public class HouseholdMemberAddCommandHandler : FoodWasteHouseholdDataCommandHandlerBase, ICommandHandler<HouseholdMemberAddCommand, ServiceReceiptResponse>
    {
        #region Private variables

        private readonly IDomainObjectValidations _domainObjectValidations;
        private readonly IWelcomeLetterDispatcher _welcomeLetterDispatcher;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a command handler which handles a command for adding a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of the repository which can access household data for the food waste domain.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of the common validations.</param>
        /// <param name="domainObjectValidations">Implementation of the common validations used by domain objects in the food waste domain.</param>
        /// <param name="welcomeLetterDispatcher">Implementation of the dispatcher which can dispatch the welcome letter to a household member.</param>
        /// <param name="exceptionBuilder">Implementation of the builder which can build exceptions.</param>
        public HouseholdMemberAddCommandHandler(IHouseholdDataRepository householdDataRepository, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IDomainObjectValidations domainObjectValidations, IWelcomeLetterDispatcher welcomeLetterDispatcher, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
            if (domainObjectValidations == null)
            {
                throw new ArgumentNullException("domainObjectValidations");
            }
            if (welcomeLetterDispatcher == null)
            {
                throw new ArgumentNullException("welcomeLetterDispatcher");
            }
            _domainObjectValidations = domainObjectValidations;
            _welcomeLetterDispatcher = welcomeLetterDispatcher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the command which adds a household member.
        /// </summary>
        /// <param name="command">Command which adds a household member.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(HouseholdMemberAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var translationInfo = HouseholdDataRepository.Get<ITranslationInfo>(command.TranslationInfoIdentifier);

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(translationInfo), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.TranslationInfoIdentifier)))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.MailAddress), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "MailAddress")))
                .IsSatisfiedBy(() => CommonValidations.IsLengthValid(command.MailAddress, 1, 128), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.LengthForPropertyIsInvalid, "MailAddress", 1, 128)))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.MailAddress) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "MailAddress")))
                .IsSatisfiedBy(() => _domainObjectValidations.IsMailAddress(command.MailAddress), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, command.MailAddress, "MailAddress")))
                .Evaluate();

            var householdMember = HouseholdDataRepository.Insert<IHouseholdMember>(new HouseholdMember(command.MailAddress));

            _welcomeLetterDispatcher.Dispatch(householdMember, householdMember, translationInfo);

            return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(householdMember, translationInfo.CultureInfo);
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the command.
        /// </summary>
        /// <param name="command">Command which adds a household member.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(HouseholdMemberAddCommand command, Exception exception)
        {
            throw ExceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}
