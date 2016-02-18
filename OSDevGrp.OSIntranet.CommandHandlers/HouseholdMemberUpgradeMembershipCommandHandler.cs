using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommandHandlers.Core;
using OSDevGrp.OSIntranet.CommandHandlers.Validation;
using OSDevGrp.OSIntranet.Contracts.Commands;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.CommandHandlers
{
    /// <summary>
    /// Command handler which can handles the command for upgrading the membership on the current users household member account.
    /// </summary>
    public class HouseholdMemberUpgradeMembershipCommandHandler : HouseholdMemberDataModificationCommandHandlerBase<HouseholdMemberUpgradeMembershipCommand>
    {
        #region Private variables

        private readonly IDomainObjectValidations _domainObjectValidations;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the command handler which can handles the command for upgrading the membership on the current users household member account.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data for the food waste domain.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can resolve values from the current users claims.</param>
        /// <param name="foodWasteObjectMapper">Implementation of an object mapper which can map objects in the food waste domain.</param>
        /// <param name="specification">Implementation of a specification which encapsulates validation rules.</param>
        /// <param name="commonValidations">Implementation of a common validations.</param>
        /// <param name="domainObjectValidations">Implementaion of a common validations used by domain objects in the food waste domain.</param>
        /// <param name="exceptionBuilder">Implementation of a builder which can build exceptions.</param>
        public HouseholdMemberUpgradeMembershipCommandHandler(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, IFoodWasteObjectMapper foodWasteObjectMapper, ISpecification specification, ICommonValidations commonValidations, IDomainObjectValidations domainObjectValidations, IExceptionBuilder exceptionBuilder)
            : base(householdDataRepository, claimValueProvider, foodWasteObjectMapper, specification, commonValidations, exceptionBuilder)
        {
            if (domainObjectValidations == null)
            {
                throw new ArgumentNullException("domainObjectValidations");
            }
            _domainObjectValidations = domainObjectValidations;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds validation rules for the command for upgrading the membership on the current users household member account.
        /// </summary>
        /// <param name="householdMember">Household member on which the membership should be upgraded.</param>
        /// <param name="command">Command for upgrading the membership on the current users household member account.</param>
        /// <param name="specification">Specification which encapsulates validation rules.</param>
        public override void AddValidationRules(IHouseholdMember householdMember, HouseholdMemberUpgradeMembershipCommand command, ISpecification specification)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (specification == null)
            {
                throw new ArgumentNullException("specification");
            }

            var currentMembership = householdMember.Membership;

            specification.IsSatisfiedBy(() => CommonValidations.HasValue(command.Membership), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Membership")))
                .IsSatisfiedBy(() => CommonValidations.IsLegalEnumValue(command.Membership, new List<Membership> {Membership.Deluxe, Membership.Premium}), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyIsInvalid, command.Membership, "Membership")))
                .IsSatisfiedBy(() => _domainObjectValidations.CanUpgradeMembership(currentMembership, (Membership)Enum.Parse(typeof(Membership), command.Membership)), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.MembershipCannotDowngrade)))
                .IsSatisfiedBy(() => CommonValidations.IsDateTimeInPast(command.PaymentTime), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.DateTimeValueForPropertyIsNotInPast, "PaymentTime")))
                .IsSatisfiedBy(() => CommonValidations.HasValue(command.PaymentReference), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "PaymentReference")))
                .IsSatisfiedBy(() => CommonValidations.ContainsIllegalChar(command.PaymentReference) == false, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.ValueForPropertyContainsIllegalChars, "PaymentReference")));
        }

        /// <summary>
        /// Upgrade membership on the current users household member account.
        /// </summary>
        /// <param name="householdMember">Household member on which the membership should be upgraded.</param>
        /// <param name="command">Command for upgrading the membership on the current users household member account.</param>
        /// <returns>Household member with the upgraded membership.</returns>
        public override IIdentifiable ModifyData(IHouseholdMember householdMember, HouseholdMemberUpgradeMembershipCommand command)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var dataProvider = HouseholdDataRepository.Get<IDataProvider>(command.DataProviderIdentifier);
            var handlesPayments = dataProvider != null && dataProvider.HandlesPayments;

            Specification.IsSatisfiedBy(() => CommonValidations.IsNotNull(dataProvider), new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.IdentifierUnknownToSystem, command.DataProviderIdentifier)))
                .IsSatisfiedBy(() => handlesPayments, new IntranetBusinessException(Resource.GetExceptionMessage(ExceptionMessage.DataProviderDoesNotHandlesPayments, dataProvider != null ? dataProvider.Name : null)))
                .Evaluate();

            var paymentReceipt = string.IsNullOrWhiteSpace(command.PaymentReceipt) ? null : Convert.FromBase64String(command.PaymentReceipt);
            var insertedPayment = HouseholdDataRepository.Insert<IPayment>(new Payment(householdMember, dataProvider, command.PaymentTime, command.PaymentReference, paymentReceipt));
            try
            {
                householdMember.PaymentAdd(insertedPayment);
                householdMember.MembershipApply((Membership) Enum.Parse(typeof (Membership), command.Membership));

                return HouseholdDataRepository.Update(householdMember);
            }
            catch
            {
                HouseholdDataRepository.Delete(insertedPayment);
                throw;
            }
        }

        #endregion
    }
}
