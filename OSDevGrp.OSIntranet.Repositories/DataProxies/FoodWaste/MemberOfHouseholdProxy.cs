using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy which bind a given household member to a given household.
    /// </summary>
    public class MemberOfHouseholdProxy : IdentifiableBase, IMemberOfHouseholdProxy
    {
        #region Private variables

        private IHouseholdMember _householdMember;
        private Guid? _householdMemberIdentifier;
        private IHousehold _household;
        private Guid? _householdIdentifier;
        private DateTime _creationTime;
        private IFoodWasteDataProvider _dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a ata proxy which bind a given household member to a given household.
        /// </summary>
        public MemberOfHouseholdProxy()
        {
        }

        /// <summary>
        /// Creates a ata proxy which bind a given household member to a given household.
        /// </summary>
        /// <param name="householdMember">Household member which are member of the household.</param>
        /// <param name="household">Household which the household member are member of.</param>
        public MemberOfHouseholdProxy(IHouseholdMember householdMember, IHousehold household)
            : this(householdMember, household, DateTime.Now)
        {
        }

        /// <summary>
        /// Creates a ata proxy which bind a given household member to a given household.
        /// </summary>
        /// <param name="householdMember">Household member which are member of the household.</param>
        /// <param name="household">Household which the household member are member of.</param>
        /// <param name="creationTime">Date and time for when the membership to the household was created.</param>
        public MemberOfHouseholdProxy(IHouseholdMember householdMember, IHousehold household, DateTime creationTime)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            if (household == null)
            {
                throw new ArgumentNullException("household");
            }
            _householdMember = householdMember;
            _householdMemberIdentifier = householdMember.Identifier;
            _household = household;
            _householdIdentifier = household.Identifier;
            _creationTime = creationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Household member which are member of the household.
        /// </summary>
        public virtual IHouseholdMember HouseholdMember
        {
            get
            {
                if (_householdMember != null && _householdMember.Identifier.HasValue && _householdMemberIdentifier.HasValue && _householdMember.Identifier.Value == _householdMemberIdentifier.Value)
                {
                    return _householdMember;
                }
                if (_householdMemberIdentifier.HasValue == false || _dataProvider == null)
                {
                    return null;
                }
                using (var subDataProvider = (IFoodWasteDataProvider) _dataProvider.Clone())
                {
                    var householdMemberProxy = new HouseholdMemberProxy
                    {
                        Identifier = _householdMemberIdentifier.Value
                    };
                    _householdMember = subDataProvider.Get(householdMemberProxy);
                }
                return _householdMember;
            }
        }
        
        /// <summary>
        /// Identifier for the household member which are member of the household.
        /// </summary>
        public virtual Guid? HouseholdMemberIdentifier
        {
            get
            {
                return _householdMemberIdentifier;
            }
            set
            {
                _householdMember = null;
                _householdMemberIdentifier = value;
            }
        }

        /// <summary>
        /// Household which the household member are member of.
        /// </summary>
        public virtual IHousehold Household
        {
            get
            {
                if (_household != null && _household.Identifier.HasValue && _householdIdentifier.HasValue && _household.Identifier.Value == _householdIdentifier.Value)
                {
                    return _household;
                }
                if (_householdIdentifier.HasValue == false || _dataProvider == null)
                {
                    return null;
                }
                using (var subDataProvider = (IFoodWasteDataProvider) _dataProvider.Clone())
                {
                    var householdProxy = new HouseholdProxy
                    {
                        Identifier = _householdIdentifier.Value
                    };
                    _household = subDataProvider.Get(householdProxy);
                }
                return _household;
            }
        }

        /// <summary>
        /// Identifier for the household which the household member are member of.
        /// </summary>
        public virtual Guid? HouseholdIdentifier
        {
            get
            {
                return _householdIdentifier;
            }
            set
            {
                _household = null;
                _householdIdentifier = value;
            }
        }

        /// <summary>
        /// Date and time for when the membership to the household was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get
            {
                return _creationTime;
            }
            protected set
            {
                _creationTime = value;
            }
        }

        #endregion

        #region IMySqlDataProxy

        /// <summary>
        /// Gets the unique identification for the binding of a given household member to a given household.
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
        /// Gets the SQL statement for selecting a binding of a given household member to a given household.
        /// </summary>
        /// <param name="memberOfHouseholdProxy">Binding of a given household member to a given household for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a binding of a given household member to a given household.</returns>
        public virtual string GetSqlQueryForId(IMemberOfHouseholdProxy memberOfHouseholdProxy)
        {
            if (memberOfHouseholdProxy == null)
            {
                throw new ArgumentNullException("memberOfHouseholdProxy");
            }
            if (memberOfHouseholdProxy.Identifier.HasValue)
            {
                return string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE MemberOfHouseholdIdentifier='{0}'", memberOfHouseholdProxy.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, memberOfHouseholdProxy.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement for selecting this binding of a given household member to a given household.
        /// </summary>
        /// <returns>SQL statement for selecting this binding of a given household member to a given household.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var householdMemberIdentifierSqlValue = HouseholdMemberIdentifier.HasValue ? HouseholdMemberIdentifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            var householdIdentifierSqlValue = HouseholdIdentifier.HasValue ? HouseholdIdentifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            return string.Format("INSERT INTO MemberOfHouseholds (MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime) VALUES('{0}','{1}','{2}',{3})", UniqueId, householdMemberIdentifierSqlValue, householdIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to update this binding of a given household member to a given household.
        /// </summary>
        /// <returns>SQL statement to update this binding of a given household member to a given household.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var householdMemberIdentifierSqlValue = HouseholdMemberIdentifier.HasValue ? HouseholdMemberIdentifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            var householdIdentifierSqlValue = HouseholdIdentifier.HasValue ? HouseholdIdentifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            return string.Format("UPDATE MemberOfHouseholds SET HouseholdMemberIdentifier='{1}',HouseholdIdentifier='{2}',CreationTime={3} WHERE MemberOfHouseholdIdentifier='{0}'", UniqueId, householdMemberIdentifierSqlValue, householdIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to delete this binding of a given household member to a given household.
        /// </summary>
        /// <returns>SQL statement to delete this binding of a given household member to a given household.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM MemberOfHouseholds WHERE MemberOfHouseholdIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            Identifier = Guid.Parse(dataReader.GetString("MemberOfHouseholdIdentifier"));
            HouseholdMemberIdentifier = Guid.Parse(dataReader.GetString("HouseholdMemberIdentifier"));
            HouseholdIdentifier = Guid.Parse(dataReader.GetString("HouseholdIdentifier"));
            CreationTime = dataReader.GetDateTime("CreationTime").ToLocalTime();

            if (_dataProvider == null)
            {
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
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
        public virtual void SaveRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, bool isInserting)
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
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
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
                _dataProvider = (IFoodWasteDataProvider) dataProvider;
            }
        }

        /// <summary>
        /// Creates the SQL statement for getting this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for getting this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for inserting this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for updating this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for deleting this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        /// <summary>
        /// Gets the bindings which bind a given household member to all the households on which there is a membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdMemberProxy">Data proxy for the household member on which to get the bindings.</param>
        /// <returns>Bindings which bind a given household member to all the households on which there is a membership.</returns>
        internal static IEnumerable<MemberOfHouseholdProxy> GetMemberOfHouseholds(IFoodWasteDataProvider dataProvider, IHouseholdMemberProxy householdMemberProxy)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = new FoodWasteCommandBuilder(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdMemberIdentifier='{0}' ORDER BY CreationTime DESC", householdMemberProxy.UniqueId)).Build();
                return subDataProvider.GetCollection<MemberOfHouseholdProxy>(command);
            }
        }

        /// <summary>
        /// Deletes the bindings which bind a given household member to all the households on which there is a membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdMemberProxy">Data proxy for the household member on which to delete the bindings.</param>
        /// <returns>Affected households.</returns>
        internal static IEnumerable<IHouseholdProxy> DeleteMemberOfHouseholds(IFoodWasteDataProvider dataProvider, IHouseholdMemberProxy householdMemberProxy)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProviderBase");
            }

            var memberOfHouseholdProxyCollection = GetMemberOfHouseholds(dataProvider, householdMemberProxy).ToArray();
            foreach (var memberOfHouseholdProxy in memberOfHouseholdProxyCollection)
            {
                using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdProxy);
                }
            }

            return memberOfHouseholdProxyCollection
                .Where(m => m.Household != null)
                .Select(m =>
                {
                    if (m.Household as IHouseholdProxy != null)
                    {
                        return (IHouseholdProxy) m.Household;
                    }
                    using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                    {
                        return subDataProvider.Get(new HouseholdProxy {Identifier = m.HouseholdIdentifier});
                    }
                })
                .ToList();
        }

        /// <summary>
        /// Gets the bindings which bind a given household to all the household members who has membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdProxy">Data proxy for the household on which to get the bindings.</param>
        /// <returns>Bindings which bind a given household to all the household members who has membership.</returns>
        internal static IEnumerable<MemberOfHouseholdProxy> GetMemberOfHouseholds(IFoodWasteDataProvider dataProvider, IHouseholdProxy householdProxy)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProviderBase");
            }
            using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = new FoodWasteCommandBuilder(string.Format("SELECT MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime FROM MemberOfHouseholds WHERE HouseholdIdentifier='{0}' ORDER BY CreationTime DESC", householdProxy.UniqueId)).Build();
                return subDataProvider.GetCollection<MemberOfHouseholdProxy>(command);
            }
        }

        /// <summary>
        /// Deletes the bindings which bind a given household to all the household members who has membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdProxy">Data proxy for the household on which to delete the bindings.</param>
        /// <returns>Affected household members.</returns>
        internal static IEnumerable<IHouseholdMemberProxy> DeleteMemberOfHouseholds(IFoodWasteDataProvider dataProvider, IHouseholdProxy householdProxy)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProviderBase");
            }

            var memberOfHouseholdProxyCollection = GetMemberOfHouseholds(dataProvider, householdProxy).ToArray();
            foreach (var memberOfHouseholdProxy in memberOfHouseholdProxyCollection)
            {
                using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdProxy);
                }
            }

            return memberOfHouseholdProxyCollection
                .Where(m => m.HouseholdMember != null)
                .Select(m =>
                {
                    if (m.HouseholdMember as IHouseholdMemberProxy != null)
                    {
                        return (IHouseholdMemberProxy) m.HouseholdMember;
                    }
                    using (var subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                    {
                        return subDataProvider.Get(new HouseholdMemberProxy {Identifier = m.HouseholdMemberIdentifier});
                    }
                })
                .ToList();
        }

        #endregion
    }
}
