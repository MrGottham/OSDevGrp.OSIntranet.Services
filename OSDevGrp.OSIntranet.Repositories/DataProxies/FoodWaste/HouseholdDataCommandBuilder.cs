using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/> and the <see cref="Repositories.FoodWaste.HouseholdDataRepository"/>.
    /// </summary>
    internal class HouseholdDataCommandBuilder : FoodWasteCommandBuilder
    {
        #region Constructor

        /// <summary>
        /// Creates an instance of the internal builder which can build a MySQL command for SQL statements used by the <see cref="DataProviders.FoodWasteDataProvider"/> and the <see cref="Repositories.FoodWaste.HouseholdDataRepository"/>.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement for the MySQL command.</param>
        /// <param name="timeout">Wait time (in seconds) before terminating the attempt to execute a command and generating an error.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="sqlStatement"/> is null, empty or white space.</exception>
        internal HouseholdDataCommandBuilder(string sqlStatement, int timeout = 30)
            : base(sqlStatement, timeout)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a household identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the household identifier.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddHouseholdIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@householdIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a household member identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the household member identifier.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddHouseholdMemberIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@householdMemberIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a mail address parameter to the command.
        /// </summary>
        /// <param name="value">The value for the mail address.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null, empty or white space.</exception>
        internal HouseholdDataCommandBuilder AddMailAddressParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@mailAddress", value, 128);
            return this;
        }

        /// <summary>
        /// Adds a membership parameter to the command.
        /// </summary>
        /// <param name="value">The value for the membership.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddMembershipParameter(Membership value)
        {
            AddTinyIntParameter("@membership", (int) value, 4);
            return this;
        }

        /// <summary>
        /// Adds a time parameter for when the membership expires the to the command.
        /// </summary>
        /// <param name="value">The value for when the membership expires.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddMembershipExpireTimeParameter(DateTime? value)
        {
            AddDateTimeParameter("@membershipExpireTime", value, true);
            return this;
        }

        /// <summary>
        /// Adds an activation code parameter to the command.
        /// </summary>
        /// <param name="value">The value for the activation code.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null, empty or white space.</exception>
        internal HouseholdDataCommandBuilder AddActivationCodeParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@activationCode", value, 64);
            return this;
        }

        /// <summary>
        /// Adds a time parameter for when the activation has occured the to the command.
        /// </summary>
        /// <param name="value">The value for when the activation has occured.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddActivationTimeParameter(DateTime? value)
        {
            AddDateTimeParameter("@activationTime", value, true);
            return this;
        }

        /// <summary>
        /// Adds a time parameter for when the privacy policy has been accepted the to the command.
        /// </summary>
        /// <param name="value">The value for when when the privacy policy has been accepted.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddPrivacyPolicyAcceptedTimeParameter(DateTime? value)
        {
            AddDateTimeParameter("@privacyPolicyAcceptedTime", value, true);
            return this;
        }

        /// <summary>
        /// Adds an identifier which describe a binding between a household member and a household parameter to the command.
        /// </summary>
        /// <param name="value">The value for the identifier which describe a binding between a household member and a household.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddMemberOfHouseholdIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@memberOfHouseholdIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a payment identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the payment identifier.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddPaymentIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@paymentIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a payment time parameter to the command.
        /// </summary>
        /// <param name="value">The value for the payment time.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddPaymentTimeParameter(DateTime value)
        {
            AddDateTimeParameter("@paymentTime", value);
            return this;
        }

        /// <summary>
        /// Adds a payment reference parameter to the command.
        /// </summary>
        /// <param name="value">The value for the payment reference.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null, empty or white space.</exception>
        internal HouseholdDataCommandBuilder AddPaymentReferenceParameter(string value)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(value, nameof(value));

            AddVarCharParameter("@paymentReference", value, 128);
            return this;
        }

        /// <summary>
        /// Adds a payment receipt parameter to the command.
        /// </summary>
        /// <param name="value">The value for the payment receipt.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddPaymentReceiptParameter(IEnumerable<byte> value)
        {
            AddLongTextParameter("@paymentReceipt", value, true);
            return this;
        }

        /// <summary>
        /// Adds a stakeholder identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the stakeholder identifier.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddStakeholderIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@stakeholderIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a stakeholder type parameter to the command.
        /// </summary>
        /// <param name="value">The value for the stakeholder type.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddStakeholderTypeParameter(StakeholderType value)
        {
            AddTinyIntParameter("@stakeholderType", (short) value, 4);
            return this;
        }

        /// <summary>
        /// Adds a data provider identifier parameter to the command.
        /// </summary>
        /// <param name="value">The value for the data provider identifier.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddDataProviderIdentifierParameter(Guid? value)
        {
            AddIdentifierParameter("@dataProviderIdentifier", value);
            return this;
        }

        /// <summary>
        /// Adds a creation time parameter to the command.
        /// </summary>
        /// <param name="value">The value for the creation time.</param>
        /// <returns>This instance of the <see cref="HouseholdDataCommandBuilder"/>.</returns>
        internal HouseholdDataCommandBuilder AddCreationTimeParameter(DateTime value)
        {
            AddDateTimeParameter("@creationTime", value);
            return this;
        }

        #endregion
    }
}
