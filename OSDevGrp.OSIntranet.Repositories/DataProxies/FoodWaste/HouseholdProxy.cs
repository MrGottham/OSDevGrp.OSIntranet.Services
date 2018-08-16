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
    /// Data proxy to a household.
    /// </summary>
    public class HouseholdProxy : Household, IHouseholdProxy
    {
        #region Private variables

        private bool _householdMembersIsLoaded;
        private IDataProviderBase<MySqlCommand> _dataProvider;
        private readonly IList<IHouseholdMember> _removedHouseholdMembers = new List<IHouseholdMember>(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        public HouseholdProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        public HouseholdProxy(string name, string description = null)
            : base(name, description)
        {
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        /// <param name="creationTime">Date and time for when the household was created.</param>
        public HouseholdProxy(string name, string description, DateTime creationTime)
            : base(name, description, creationTime)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Household members who is member of this household.
        /// </summary>
        public override IEnumerable<IHouseholdMember> HouseholdMembers
        {
            get
            {
                if (_householdMembersIsLoaded || _dataProvider == null)
                {
                    return base.HouseholdMembers;
                }
                base.HouseholdMembers = MemberOfHouseholdProxy.GetMemberOfHouseholds(_dataProvider, this)
                    .Where(m => m.HouseholdMember != null)
                    .OrderBy(m => m.CreationTime)
                    .Select(m => m.HouseholdMember)
                    .ToList();
                _householdMembersIsLoaded = true;
                return base.HouseholdMembers;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes a household member from this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be removed as a member of this household.</param>
        /// <returns>Household member who has been removed af member of this household.</returns>
        public override IHouseholdMember HouseholdMemberRemove(IHouseholdMember householdMember)
        {
            IHouseholdMember householdMemberToRemove = base.HouseholdMemberRemove(householdMember);
            if (householdMemberToRemove != null)
            {
                _removedHouseholdMembers.Add(householdMemberToRemove);
            }
            return householdMemberToRemove;
        }

        #endregion

        #region IMySqlDataProxy

        /// <summary>
        /// Gets the unique identification for the household.
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
        /// Gets the SQL statement for selecting a household.
        /// </summary>
        /// <param name="household">Household for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a household.</returns>
        public virtual string GetSqlQueryForId(IHousehold household)
        {
            if (household == null)
            {
                throw new ArgumentNullException(nameof(household));
            }
            if (household.Identifier.HasValue)
            {
                return $"SELECT HouseholdIdentifier,Name,Descr,CreationTime FROM Households WHERE HouseholdIdentifier='{household.Identifier.Value.ToString("D").ToUpper()}'";
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, household.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this household.
        /// </summary>
        /// <returns>SQL statement to insert this household.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            string descriptionSqlValue = string.IsNullOrWhiteSpace(Description) ? "NULL" : $"'{Description}'";
            return $"INSERT INTO Households (HouseholdIdentifier,Name,Descr,CreationTime) VALUES('{UniqueId}','{Name}',{descriptionSqlValue},{DataRepositoryHelper.GetSqlValueForDateTime(CreationTime)})";
        }

        /// <summary>
        /// Gets the SQL statement to update this household.
        /// </summary>
        /// <returns>SQL statement to update this household.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            string descriptionSqlValue = string.IsNullOrWhiteSpace(Description) ? "NULL" : $"'{Description}'";
            return $"UPDATE Households SET Name='{Name}',Descr={descriptionSqlValue},CreationTime={DataRepositoryHelper.GetSqlValueForDateTime(CreationTime)} WHERE HouseholdIdentifier='{UniqueId}'";
        }

        /// <summary>
        /// Gets the SQL statement to delete this household.
        /// </summary>
        /// <returns>SQL statement to delete this household.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return $"DELETE FROM Households WHERE HouseholdIdentifier='{UniqueId}'";
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
                throw new ArgumentNullException(nameof(dataReader));
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            MySqlDataReader mySqlDataReader = dataReader as MySqlDataReader;
            if (mySqlDataReader == null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, "dataReader", dataReader.GetType().Name));
            }

            Identifier = Guid.Parse(mySqlDataReader.GetString("HouseholdIdentifier"));
            Name = mySqlDataReader.GetString("Name");
            CreationTime = mySqlDataReader.GetDateTime("CreationTime").ToLocalTime();

            int descriptionColumnNo = mySqlDataReader.GetOrdinal("Descr");
            if (!mySqlDataReader.IsDBNull(descriptionColumnNo))
            {
                Description = mySqlDataReader.GetString(descriptionColumnNo);
            }

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
                throw new ArgumentNullException(nameof(dataProvider));
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
                throw new ArgumentNullException(nameof(dataProvider));
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            IEnumerable<IHouseholdMember> unsavedHouseholdMembers = base.HouseholdMembers.ToArray(); // This will not force the proxy to reload the household members.
            IHouseholdMember unsavedHouseholdMemberWithoutIdentifier = unsavedHouseholdMembers.FirstOrDefault(unsavedHouseholdMember => unsavedHouseholdMember.Identifier.HasValue == false);
            if (unsavedHouseholdMemberWithoutIdentifier != null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, unsavedHouseholdMemberWithoutIdentifier.Identifier, "Identifier"));
            }

            IList<MemberOfHouseholdProxy> existingMemberOfHouseholds = MemberOfHouseholdProxy.GetMemberOfHouseholds(_dataProvider, this).ToList();
            foreach (IHouseholdMember unsavedHouseholdMember in unsavedHouseholdMembers.Where(m => m.Identifier.HasValue))
            {
                IHouseholdMember householdMember = unsavedHouseholdMember;
                if (existingMemberOfHouseholds.Any(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdMemberIdentifier == householdMember.Identifier))
                {
                    continue;
                }
                using (IDataProviderBase<MySqlCommand> subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                {
                    MemberOfHouseholdProxy memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMember, this)
                    {
                        Identifier = Guid.NewGuid()
                    };
                    existingMemberOfHouseholds.Add(subDataProvider.Add(memberOfHouseholdProxy));
                }
            }
            while (_removedHouseholdMembers.Count > 0)
            {
                IHouseholdMember householdMemberToRemove = _removedHouseholdMembers.First();
                if (householdMemberToRemove.Identifier.HasValue == false)
                {
                    _removedHouseholdMembers.Remove(householdMemberToRemove);
                    continue;
                }

                MemberOfHouseholdProxy memberOfHouseholdToRemove = existingMemberOfHouseholds.SingleOrDefault(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdMemberIdentifier == householdMemberToRemove.Identifier);
                if (memberOfHouseholdToRemove == null)
                {
                    _removedHouseholdMembers.Remove(householdMemberToRemove);
                    continue;
                }

                using (IDataProviderBase<MySqlCommand> subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdToRemove);
                }
                IHouseholdMemberProxy householdMemberProxy = memberOfHouseholdToRemove.HouseholdMember as IHouseholdMemberProxy;
                if (householdMemberProxy == null)
                {
                    using (IDataProviderBase<MySqlCommand> subDataProvider = (IDataProviderBase<MySqlCommand>) _dataProvider.Clone())
                    {
                        householdMemberProxy = subDataProvider.Get(new HouseholdMemberProxy {Identifier = memberOfHouseholdToRemove.HouseholdMemberIdentifier});
                    }
                }
                HandleAffectedHouseholdMember(_dataProvider, householdMemberProxy);

                _removedHouseholdMembers.Remove(householdMemberToRemove);
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
                throw new ArgumentNullException(nameof(dataProvider));
            }
            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }

            foreach (IHouseholdMemberProxy affectedHouseholdMember in MemberOfHouseholdProxy.DeleteMemberOfHouseholds(_dataProvider, this))
            {
                HandleAffectedHouseholdMember(_dataProvider, affectedHouseholdMember);
            }
        }

        /// <summary>
        /// Creates the SQL statement for getting this household.
        /// </summary>
        /// <returns>SQL statement for getting this household.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlQueryForId(this)).Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this household.
        /// </summary>
        /// <returns>SQL statement for inserting this household.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForInsert()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this household.
        /// </summary>
        /// <returns>SQL statement for updating this household.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForUpdate()).Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this household.
        /// </summary>
        /// <returns>SQL statement for deleting this household.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new FoodWasteCommandBuilder(GetSqlCommandForDelete()).Build();
        }

        /// <summary>
        /// Handles an affected household member.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="affectedHouseholdMember">Implementation of a data proxy to the affected household member.</param>
        private static void HandleAffectedHouseholdMember(IDataProviderBase<MySqlCommand> dataProvider, IHouseholdMemberProxy affectedHouseholdMember)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }
            if (affectedHouseholdMember == null)
            {
                throw new ArgumentNullException(nameof(affectedHouseholdMember));
            }
            if (affectedHouseholdMember.Membership != Membership.Basic)
            {
                return;
            }
            if (affectedHouseholdMember.Households.Any())
            {
                return;
            }
            using (var subDataProvider = (IDataProviderBase<MySqlCommand>) dataProvider.Clone())
            {
                subDataProvider.Delete(affectedHouseholdMember);
            }
        }

        #endregion
    }
}
