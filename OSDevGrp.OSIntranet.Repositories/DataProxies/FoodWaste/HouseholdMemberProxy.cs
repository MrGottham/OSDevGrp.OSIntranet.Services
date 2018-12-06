using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a household member.
    /// </summary>
    public class HouseholdMemberProxy : HouseholdMember, IHouseholdMemberProxy
    {
        #region Private variables

        private bool _householdCollectionHasBeenLoaded;
        private bool _paymentCollectionHasBeenLoaded;
        private IFoodWasteDataProvider _dataProvider;
        private readonly IList<IHousehold> _removedHouseholdCollection = new List<IHousehold>(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a household member.
        /// </summary>
        public HouseholdMemberProxy() 
        {
        }

        /// <summary>
        /// Creates a data proxy to a household member.
        /// </summary>
        /// <param name="mailAddress">Mail address for the household member.</param>
        /// <param name="membership">Membership.</param>
        /// <param name="membershipExpireTime">Date and time for when the membership expires.</param>
        /// <param name="activationCode">Activation code for the household member.</param>
        /// <param name="creationTime">Date and time for when the household member was created.</param>
        /// <param name="dataProvider">The data provider which the created data proxy should use.</param>
        public HouseholdMemberProxy(string mailAddress, Membership membership, DateTime? membershipExpireTime, string activationCode, DateTime creationTime, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider = null)
            : base(mailAddress, membership, membershipExpireTime, activationCode, creationTime)
        {
            if (dataProvider == null)
            {
                return;
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Households on which the household member has a membership.
        /// </summary>
        public override IEnumerable<IHousehold> Households
        {
            get
            {
                if (_householdCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Households;
                }
                Households = MemberOfHouseholdProxy.GetMemberOfHouseholds(_dataProvider, this)
                    .Where(m => m.Household != null)
                    .OrderByDescending(m => m.CreationTime)
                    .Take(Validator.GetHouseholdLimit(Membership))
                    .Select(m => m.Household)
                    .ToList();
                return base.Households;
            }
            protected set
            {
                base.Households = value;
                _householdCollectionHasBeenLoaded = true;
            }
        }

        /// <summary>
        /// Payments made by the household member.
        /// </summary>
        public override IEnumerable<IPayment> Payments
        {
            get
            {
                if (_paymentCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Payments;
                }
                Payments = new List<IPayment>(PaymentProxy.GetPayments(_dataProvider, Identifier.Value));
                return base.Payments;
            }
            protected set
            {
                base.Payments = value;
                _paymentCollectionHasBeenLoaded = true;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the household member.
        /// </summary>
        public virtual string UniqueId
        {
            get
            {
                if (Identifier.HasValue)
                {
                    return Identifier.Value.ToString("D").ToUpper();
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }
        }

        #endregion

        #region IDataProxyBase<MySqlDataReader, MySqlCommand> Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider));

            Identifier = GetHouseholdMemberIdentifier(dataReader, "HouseholdMemberIdentifier");
            MailAddress = GetMailAddress(dataReader, "MailAddress");
            Membership = GetMembership(dataReader, "Membership");
            MembershipExpireTime = GetMembershipExpireTime(dataReader, "MembershipExpireTime");
            ActivationCode = GetActivationCode(dataReader, "ActivationCode");
            ActivationTime = GetActivationTime(dataReader, "ActivationTime");
            PrivacyPolicyAcceptedTime = GetPrivacyPolicyAcceptedTime(dataReader, "PrivacyPolicyAcceptedTime");
            CreationTime = GetCreationTime(dataReader, "CreationTime");

            _householdCollectionHasBeenLoaded = false;
            _paymentCollectionHasBeenLoaded = false;
            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            IEnumerable<IHousehold> householdCollection = base.Households.ToArray(); // Using base.Households will not force the proxy to reload the household collection.
            IHousehold householdWithoutIdentifier = householdCollection.FirstOrDefault(household => household.Identifier.HasValue == false);
            if (householdWithoutIdentifier != null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdWithoutIdentifier.Identifier, "Identifier"));
            }

            IList<MemberOfHouseholdProxy> existingMemberOfHouseholdCollection = new List<MemberOfHouseholdProxy>(MemberOfHouseholdProxy.GetMemberOfHouseholds(dataProvider, this));
            foreach (IHousehold household in householdCollection.Where(m => m.Identifier.HasValue))
            {
                if (existingMemberOfHouseholdCollection.Any(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdIdentifier == household.Identifier))
                {
                    continue;
                }

                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    MemberOfHouseholdProxy memberOfHouseholdProxy = new MemberOfHouseholdProxy(this, household)
                    {
                        Identifier = Guid.NewGuid()
                    };
                    existingMemberOfHouseholdCollection.Add(subDataProvider.Add(memberOfHouseholdProxy));
                }
            }

            while (_removedHouseholdCollection.Count > 0)
            {
                IHousehold householdToRemove = _removedHouseholdCollection.First();
                if (householdToRemove.Identifier.HasValue == false)
                {
                    _removedHouseholdCollection.Remove(householdToRemove);
                    continue;
                }

                MemberOfHouseholdProxy memberOfHouseholdProxyToRemove = existingMemberOfHouseholdCollection.SingleOrDefault(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdIdentifier == householdToRemove.Identifier);
                if (memberOfHouseholdProxyToRemove == null)
                {
                    _removedHouseholdCollection.Remove(householdToRemove);
                    continue;
                }

                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdProxyToRemove);
                }
                HandleAffectedHousehold(dataProvider, memberOfHouseholdProxyToRemove.Household as IHouseholdProxy);
                _removedHouseholdCollection.Remove(householdToRemove);
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            foreach (IHouseholdProxy affectedHousehold in MemberOfHouseholdProxy.DeleteMemberOfHouseholds(dataProvider, this))
            {
                HandleAffectedHousehold(dataProvider, affectedHousehold);
            }

            PaymentProxy.DeletePayments(dataProvider, Identifier.Value);

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Creates the SQL statement for getting this household member.
        /// </summary>
        /// <returns>SQL statement for getting this household member.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildHouseholdDataCommandForSelecting("WHERE HouseholdMemberIdentifier=@householdMemberIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddHouseholdMemberIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this household member.
        /// </summary>
        /// <returns>SQL statement for inserting this household member.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new HouseholdDataCommandBuilder("INSERT INTO HouseholdMembers (HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime) VALUES(@householdMemberIdentifier,@mailAddress,@membership,@membershipExpireTime,@activationCode,@activationTime,@privacyPolicyAcceptedTime,@creationTime)")
                .AddHouseholdMemberIdentifierParameter(Identifier)
                .AddMailAddressParameter(MailAddress)
                .AddMembershipParameter(Membership)
                .AddMembershipExpireTimeParameter(MembershipExpireTime)
                .AddActivationCodeParameter(ActivationCode)
                .AddActivationTimeParameter(ActivationTime)
                .AddPrivacyPolicyAcceptedTimeParameter(PrivacyPolicyAcceptedTime)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this household member.
        /// </summary>
        /// <returns>SQL statement for updating this household member.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new HouseholdDataCommandBuilder("UPDATE HouseholdMembers SET MailAddress=@mailAddress,Membership=@membership,MembershipExpireTime=@membershipExpireTime,ActivationCode=@activationCode,ActivationTime=@activationTime,PrivacyPolicyAcceptedTime=@privacyPolicyAcceptedTime,CreationTime=@creationTime WHERE HouseholdMemberIdentifier=@householdMemberIdentifier")
                .AddHouseholdMemberIdentifierParameter(Identifier)
                .AddMailAddressParameter(MailAddress)
                .AddMembershipParameter(Membership)
                .AddMembershipExpireTimeParameter(MembershipExpireTime)
                .AddActivationCodeParameter(ActivationCode)
                .AddActivationTimeParameter(ActivationTime)
                .AddPrivacyPolicyAcceptedTimeParameter(PrivacyPolicyAcceptedTime)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this household member.
        /// </summary>
        /// <returns>SQL statement for deleting this household member.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new HouseholdDataCommandBuilder("DELETE FROM HouseholdMembers WHERE HouseholdMemberIdentifier=@householdMemberIdentifier")
                .AddHouseholdMemberIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IHouseholdMemberProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy to a given household member with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy to a given household member with values from the data reader.</returns>
        public virtual IHouseholdMemberProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new HouseholdMemberProxy(GetMailAddress(dataReader, columnNameCollection[1]), GetMembership(dataReader, columnNameCollection[2]), GetMembershipExpireTime(dataReader, columnNameCollection[3]), GetActivationCode(dataReader, columnNameCollection[4]), GetCreationTime(dataReader, columnNameCollection[7]), dataProvider)
            {
                Identifier = GetHouseholdMemberIdentifier(dataReader, columnNameCollection[0]),
                ActivationTime = GetActivationTime(dataReader, columnNameCollection[5]),
                PrivacyPolicyAcceptedTime = GetPrivacyPolicyAcceptedTime(dataReader, columnNameCollection[6])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes a household from the household member.
        /// </summary>
        /// <param name="household">Household where the membership for the household member should be removed.</param>
        /// <returns>Household where the membership for the household member has been removed.</returns>
        public override IHousehold HouseholdRemove(IHousehold household)
        {
            IHousehold householdToRemove = base.HouseholdRemove(household);
            if (householdToRemove != null)
            {
                _removedHouseholdCollection.Add(householdToRemove);
            }
            return householdToRemove;
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="HouseholdMemberProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="HouseholdMemberProxy"/>.</returns>
        internal static MySqlCommand BuildHouseholdDataCommandForSelecting(string whereClause = null, Action<HouseholdDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime FROM HouseholdMembers");
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                selectStatementBuilder.Append($" {whereClause}");
            }

            HouseholdDataCommandBuilder householdDataCommandBuilder = new HouseholdDataCommandBuilder(selectStatementBuilder.ToString());
            if (parameterAdder == null)
            {
                return householdDataCommandBuilder.Build();
            }

            parameterAdder(householdDataCommandBuilder);
            return householdDataCommandBuilder.Build();
        }

        /// <summary>
        /// Gets the household member identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The household member identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetHouseholdMemberIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the mail address from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The mail address.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetMailAddress(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the membership from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The membership.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Membership GetMembership(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return (Membership) dataReader.GetInt16(columnName);
        }

        /// <summary>
        /// Gets the time for when the membership expires from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The time for when the membership expires.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static DateTime? GetMembershipExpireTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            int columnNo = dataReader.GetOrdinal(columnName);
            return dataReader.IsDBNull(columnNo) == false ? dataReader.GetDateTime(columnNo).ToLocalTime() : (DateTime?) null;
        }

        /// <summary>
        /// Gets the activation code from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The activation code.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetActivationCode(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the time for when the household member has been activated from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The time for when the household member has been activated.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static DateTime? GetActivationTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            int columnNo = dataReader.GetOrdinal(columnName);
            return dataReader.IsDBNull(columnNo) == false ? dataReader.GetDateTime(columnNo).ToLocalTime() : (DateTime?) null;
        }

        /// <summary>
        /// Gets the time for when the privacy policies has been accepted from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The time for when the privacy policies has been accepted.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static DateTime? GetPrivacyPolicyAcceptedTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            int columnNo = dataReader.GetOrdinal(columnName);
            return dataReader.IsDBNull(columnNo) == false ? dataReader.GetDateTime(columnNo).ToLocalTime() : (DateTime?) null;
        }

        /// <summary>
        /// Gets the time for when the household member was created from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The time for when the household member was created.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static DateTime GetCreationTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetDateTime(columnName).ToLocalTime();
        }

        /// <summary>
        /// Handles an affected household.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="affectedHousehold">Implementation of a data proxy to the affected household.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> or <paramref name="affectedHousehold"/> is null.</exception>
        private static void HandleAffectedHousehold(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IHouseholdProxy affectedHousehold)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(affectedHousehold, nameof(affectedHousehold));

            if (affectedHousehold.HouseholdMembers.Any())
            {
                return;
            }

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                subDataProvider.Delete(affectedHousehold);
            }
        }

        #endregion
    }
}
