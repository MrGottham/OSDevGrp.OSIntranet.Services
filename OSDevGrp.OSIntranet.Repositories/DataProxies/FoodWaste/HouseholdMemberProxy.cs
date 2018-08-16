using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.DataProviders;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
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

        private bool _householdsIsLoaded;
        private bool _paymentsIsLoaded;
        private IDataProviderBase<MySqlCommand> _dataProvider;
        private readonly IList<IHousehold> _removedHouseholds = new List<IHousehold>(0);

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
        public HouseholdMemberProxy(string mailAddress)
            : base(mailAddress)
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
        public HouseholdMemberProxy(string mailAddress, Membership membership, DateTime? membershipExpireTime, string activationCode, DateTime creationTime)
            : base(mailAddress, membership, membershipExpireTime, activationCode, creationTime)
        {
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
                if (_householdsIsLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Households;
                }
                base.Households = MemberOfHouseholdProxy.GetMemberOfHouseholds(_dataProvider, this)
                    .Where(m => m.Household != null)
                    .OrderByDescending(m => m.CreationTime)
                    .Take(Validator.GetHouseholdLimit(Membership))
                    .Select(m => m.Household)
                    .ToList();
                _householdsIsLoaded = true;
                return base.Households;
            }
        }

        /// <summary>
        /// Payments made by the household member.
        /// </summary>
        public override IEnumerable<IPayment> Payments
        {
            get
            {
                if (_paymentsIsLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Payments;
                }
                base.Payments = PaymentProxy.GetPayments(_dataProvider, Identifier.Value).ToList();
                _paymentsIsLoaded = true;
                return base.Payments;
            }
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
            var householdToRemove = base.HouseholdRemove(household);
            if (householdToRemove != null)
            {
                _removedHouseholds.Add(householdToRemove);
            }
            return householdToRemove;
        }

        #endregion

        #region IMySqlDataProxy

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

        /// <summary>
        /// Gets the SQL statement for selecting a household member.
        /// </summary>
        /// <param name="householdMember">Household member for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a household member.</returns>
        public virtual string GetSqlQueryForId(IHouseholdMember householdMember)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (householdMember.Identifier.HasValue)
            {
                return string.Format("SELECT HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", householdMember.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMember.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this household member.
        /// </summary>
        /// <returns>SQL statement to insert this household member.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            return string.Format("INSERT INTO HouseholdMembers (HouseholdMemberIdentifier,MailAddress,Membership,MembershipExpireTime,ActivationCode,ActivationTime,PrivacyPolicyAcceptedTime,CreationTime) VALUES('{0}','{1}',{2},{3},'{4}',{5},{6},{7})", UniqueId, MailAddress, (int) Membership, DataRepositoryHelper.GetSqlValueForDateTime(MembershipExpireTime), ActivationCode, DataRepositoryHelper.GetSqlValueForDateTime(ActivationTime), DataRepositoryHelper.GetSqlValueForDateTime(PrivacyPolicyAcceptedTime), DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to update this household member.
        /// </summary>
        /// <returns>SQL statement to update this household member.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            return string.Format("UPDATE HouseholdMembers SET MailAddress='{1}',Membership={2},MembershipExpireTime={3},ActivationCode='{4}',ActivationTime={5},PrivacyPolicyAcceptedTime={6},CreationTime={7} WHERE HouseholdMemberIdentifier='{0}'", UniqueId, MailAddress, (int) Membership, DataRepositoryHelper.GetSqlValueForDateTime(MembershipExpireTime), ActivationCode, DataRepositoryHelper.GetSqlValueForDateTime(ActivationTime), DataRepositoryHelper.GetSqlValueForDateTime(PrivacyPolicyAcceptedTime), DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to delete this household member.
        /// </summary>
        /// <returns>SQL statement to delete this household member.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM HouseholdMembers WHERE HouseholdMemberIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(object dataReader, IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            var mySqlDataReader = dataReader as MySqlDataReader;
            if (mySqlDataReader == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name));
            }

            Identifier = Guid.Parse(mySqlDataReader.GetString("HouseholdMemberIdentifier"));
            MailAddress = mySqlDataReader.GetString("MailAddress");
            Membership = (Membership) mySqlDataReader.GetInt16("Membership");
            var membershipExpireTimeColumnNo = mySqlDataReader.GetOrdinal("MembershipExpireTime");
            if (!mySqlDataReader.IsDBNull(membershipExpireTimeColumnNo))
            {
                MembershipExpireTime = mySqlDataReader.GetDateTime(membershipExpireTimeColumnNo).ToLocalTime();
            }
            ActivationCode = mySqlDataReader.GetString("ActivationCode");
            var activationTimeColumnNo = mySqlDataReader.GetOrdinal("ActivationTime");
            if (!mySqlDataReader.IsDBNull(activationTimeColumnNo))
            {
                ActivationTime = mySqlDataReader.GetDateTime(activationTimeColumnNo).ToLocalTime();
            }
            var privacyPolicyAcceptedTimeColumnNo = mySqlDataReader.GetOrdinal("PrivacyPolicyAcceptedTime");
            if (!mySqlDataReader.IsDBNull(privacyPolicyAcceptedTimeColumnNo))
            {
                PrivacyPolicyAcceptedTime = mySqlDataReader.GetDateTime(privacyPolicyAcceptedTimeColumnNo).ToLocalTime();
            }
            CreationTime = mySqlDataReader.GetDateTime("CreationTime").ToLocalTime();

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase<MySqlCommand> dataProvider, bool isInserting)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            var unsavedHouseholds = base.Households.ToArray(); // This will not force the proxy to reload the households.
            var unsavedHouseholdWithoutIdentifier = unsavedHouseholds.FirstOrDefault(unsavedHousehold => unsavedHousehold.Identifier.HasValue == false);
            if (unsavedHouseholdWithoutIdentifier != null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, unsavedHouseholdWithoutIdentifier.Identifier, "Identifier"));
            }

            var existingMemberOfHouseholds = MemberOfHouseholdProxy.GetMemberOfHouseholds(_dataProvider, this).ToList();
            foreach (var unsavedHousehold in unsavedHouseholds.Where(m => m.Identifier.HasValue))
            {
                var household = unsavedHousehold;
                if (existingMemberOfHouseholds.Any(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdIdentifier == household.Identifier))
                {
                    continue;
                }
                using (var subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                {
                    var memberOfHouseholdProxy = new MemberOfHouseholdProxy(this, household)
                    {
                        Identifier = Guid.NewGuid()
                    };
                    existingMemberOfHouseholds.Add(subDataProvider.Add(memberOfHouseholdProxy));
                }
            }
            while (_removedHouseholds.Count > 0)
            {
                var householdToRemove = _removedHouseholds.First();
                if (householdToRemove.Identifier.HasValue == false)
                {
                    _removedHouseholds.Remove(householdToRemove);
                    continue;
                }

                var memberOfHouseholdToRemove = existingMemberOfHouseholds.SingleOrDefault(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdIdentifier == householdToRemove.Identifier);
                if (memberOfHouseholdToRemove == null)
                {
                    _removedHouseholds.Remove(householdToRemove);
                    continue;
                }

                using (var subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdToRemove);
                }
                var householdProxy = memberOfHouseholdToRemove.Household as IHouseholdProxy;
                if (householdProxy == null)
                {
                    using (var subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                    {
                        householdProxy = subDataProvider.Get(new HouseholdProxy {Identifier = memberOfHouseholdToRemove.HouseholdIdentifier});
                    }
                }
                HandleAffectedHousehold(_dataProvider, householdProxy);

                _removedHouseholds.Remove(householdToRemove);
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlCommand> dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            foreach (var affectedHousehold in MemberOfHouseholdProxy.DeleteMemberOfHouseholds(_dataProvider, this))
            {
                HandleAffectedHousehold(_dataProvider, affectedHousehold);
            }

            PaymentProxy.DeletePayments(_dataProvider, Identifier.Value);
        }

        /// <summary>
        /// Creates the SQL statement for getting this household member.
        /// </summary>
        /// <returns>SQL statement for getting this household member.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this household member.
        /// </summary>
        /// <returns>SQL statement for inserting this household member.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this household member.
        /// </summary>
        /// <returns>SQL statement for updating this household member.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this household member.
        /// </summary>
        /// <returns>SQL statement for deleting this household member.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        /// <summary>
        /// Handles an affected household.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="affectedHousehold">Implementation of a data proxy to the affected household.</param>
        private static void HandleAffectedHousehold(IDataProviderBase<MySqlCommand> dataProvider, IHouseholdProxy affectedHousehold)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (affectedHousehold == null)
            {
                throw new ArgumentNullException("affectedHousehold");
            }
            if (affectedHousehold.HouseholdMembers.Any())
            {
                return;
            }
            using (var subDataProvider = (IDataProviderBase<MySqlCommand>) dataProvider.Clone())
            {
                subDataProvider.Delete(affectedHousehold);
            }
        }

        #endregion
    }
}
