﻿using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Contracts.Responses;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers.Core
{
    /// <summary>
    /// Basic functionality which can handle a command for modifying some data on a household member.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command for modifying some data on a household member.</typeparam>
    public abstract class HouseholdMemberDataModificationCommandHandlerBase<TCommand> : FoodWasteHouseholdDataCommandHandlerBase, ICommandHandler<TCommand, ServiceReceiptResponse> where TCommand : HouseholdMemberDataModificationCommandBase
    {
        #region Constructor

        /// <summary>
        /// Creates the basic functionality which can handle a command for modifying some data on a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        protected HouseholdMemberDataModificationCommandHandlerBase(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IExceptionBuilder exceptionBuilder) 
            : base(householdDataRepository, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
            ArgumentNullGuard.NotNull(claimValueProvider, nameof(claimValueProvider));

            ClaimValueProvider = claimValueProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether the household member should be activated to execute the command handled by this command handler.
        /// </summary>
        public virtual bool ShouldBeActivated => true;

        /// <summary>
        /// Gets whether the household member should have accepted the privacy policy to execute the command handled by this command handler.
        /// </summary>
        public virtual bool ShouldHaveAcceptedPrivacyPolicy => true;

        /// <summary>
        /// Gets the required membership which the household member should have to execute the command handled by this command handler.
        /// </summary>
        public virtual Membership RequiredMembership => Membership.Basic;

        /// <summary>
        /// Gets the provider which can resolve values from the current users claims.
        /// </summary>
        protected virtual IClaimValueProvider ClaimValueProvider { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the functionality which can handle a command for modifying some data on a household member.
        /// </summary>
        /// <param name="command">Command for modifying some data on a household member.</param>
        /// <returns>Service receipt.</returns>
        public virtual ServiceReceiptResponse Execute(TCommand command)
        {
            ArgumentNullGuard.NotNull(command, nameof(command));

            IHouseholdMember householdMember = HouseholdDataRepository.HouseholdMemberGetByMailAddress(ClaimValueProvider.MailAddress);

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(householdMember), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotCreated)))
                .IsSatisfiedBy(() => ShouldBeActivated == false || householdMember.IsActivated, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberNotActivated)))
                .IsSatisfiedBy(() => ShouldHaveAcceptedPrivacyPolicy == false || householdMember.IsPrivacyPolicyAccepted, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotAcceptedPrivacyPolicy)))
                .IsSatisfiedBy(() => householdMember.HasRequiredMembership(RequiredMembership), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.HouseholdMemberHasNotRequiredMembership)))
                .Evaluate();

            AddValidationRules(householdMember, command, Specification);
            
            Specification.Evaluate();

            IIdentifiable identifiableDomainObject = ModifyData(householdMember, command);

            return ObjectMapper.Map<IIdentifiable, ServiceReceiptResponse>(identifiableDomainObject);
        }

        /// <summary>
        /// Handles an exception which has occurred when executing the functionality which can handle a command for modifying some data on a household member.
        /// </summary>
        /// <param name="command">Command for modifying some data on a household member.</param>
        /// <param name="exception">Exception.</param>
        public virtual void HandleException(TCommand command, Exception exception)
        {
            throw ExceptionBuilder.Build(exception, MethodBase.GetCurrentMethod());
        }

        /// <summary>
        /// Adds validation rules to the specification which encapsulates validation rules.
        /// </summary>
        /// <param name="householdMember">Household member for which to modify data.</param>
        /// <param name="command">Command for modifying some data on a household member.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public abstract void AddValidationRules(IHouseholdMember householdMember, TCommand command, ISpecification specification);

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="householdMember">Household member for which to modify data.</param>
        /// <param name="command">Command for modifying some data on a household member.</param>
        /// <returns>An identifiable domain object in the food waste domain.</returns>
        public abstract IIdentifiable ModifyData(IHouseholdMember householdMember, TCommand command);

        #endregion
    }
}
