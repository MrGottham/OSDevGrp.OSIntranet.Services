using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Data proxy to a household.
    /// </summary>
    public class HouseholdProxy : Household, IHouseholdProxy
    {
        #region Private variables

        private bool _householdMemberCollectionHasBeenLoaded;
        private bool _storageCollectionHasBeenLoaded;
        private IFoodWasteDataProvider _dataProvider;
        private readonly IList<IHouseholdMember> _removedHouseholdMemberCollection = new List<IHouseholdMember>(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a household.
        /// </summary>
        public HouseholdProxy()
        {
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        /// <param name="creationTime">Date and time for when the household was created.</param>
        /// <param name="dataProvider">The data provider which the created data proxy should use.</param>
        public HouseholdProxy(string name, string description, DateTime creationTime, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider = null)
            : base(name, description, creationTime)
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
        /// Household members who is member of this household.
        /// </summary>
        public override IEnumerable<IHouseholdMember> HouseholdMembers
        {
            get
            {
                if (_householdMemberCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.HouseholdMembers;
                }
                HouseholdMembers = MemberOfHouseholdProxy.GetMemberOfHouseholds(_dataProvider, this)
                    .Where(m => m.HouseholdMember != null)
                    .OrderBy(m => m.CreationTime)
                    .Select(m => m.HouseholdMember)
                    .ToList();
                return base.HouseholdMembers;
            }
            protected set
            {
                base.HouseholdMembers = value;
                _householdMemberCollectionHasBeenLoaded = true;
            }
        }

        /// <summary>
        /// Storages in this household.
        /// </summary>
        public override IEnumerable<IStorage> Storages
        {
            get
            {
                if (_storageCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Storages;
                }
                Storages = StorageProxy.GetStorages(_dataProvider, Identifier.Value)
                    .OrderBy(storage => storage.SortOrder)
                    .ThenByDescending(storage => storage.CreationTime)
                    .ToList();
                return base.Storages;
            }
            protected set
            {
                base.Storages = value;
                _storageCollectionHasBeenLoaded = true;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

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

            Identifier = GetHouseholdIdentifier(dataReader, "HouseholdIdentifier");
            Name = GetName(dataReader, "Name");
            // ReSharper disable StringLiteralTypo
            Description = GetDescription(dataReader, "Descr");
            // ReSharper restore StringLiteralTypo
            CreationTime = GetCreationTime(dataReader, "CreationTime");

            _householdMemberCollectionHasBeenLoaded = false;
            _storageCollectionHasBeenLoaded = false;
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

            IEnumerable<IHouseholdMember> householdMemberCollection = base.HouseholdMembers.ToArray(); // Using base.HouseholdMembers will not force the proxy to reload the household member collection.
            IHouseholdMember householdMemberWithoutIdentifier = householdMemberCollection.FirstOrDefault(householdMember => householdMember.Identifier.HasValue == false);
            if (householdMemberWithoutIdentifier != null)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMemberWithoutIdentifier.Identifier, "Identifier"));
            }

            IList<MemberOfHouseholdProxy> existingMemberOfHouseholdCollection = new List<MemberOfHouseholdProxy>(MemberOfHouseholdProxy.GetMemberOfHouseholds(dataProvider, this));
            foreach (IHouseholdMember householdMember in householdMemberCollection.Where(m => m.Identifier.HasValue))
            {
                if (existingMemberOfHouseholdCollection.Any(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdMemberIdentifier == householdMember.Identifier))
                {
                    continue;
                }

                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    MemberOfHouseholdProxy memberOfHouseholdProxy = new MemberOfHouseholdProxy(householdMember, this)
                    {
                        Identifier = Guid.NewGuid()
                    };
                    existingMemberOfHouseholdCollection.Add(subDataProvider.Add(memberOfHouseholdProxy));
                }
            }

            while (_removedHouseholdMemberCollection.Count > 0)
            {
                IHouseholdMember householdMemberToRemove = _removedHouseholdMemberCollection.First();
                if (householdMemberToRemove.Identifier.HasValue == false)
                {
                    _removedHouseholdMemberCollection.Remove(householdMemberToRemove);
                    continue;
                }

                MemberOfHouseholdProxy memberOfHouseholdProxyToRemove = existingMemberOfHouseholdCollection.SingleOrDefault(existingMemberOfHousehold => existingMemberOfHousehold.HouseholdMemberIdentifier == householdMemberToRemove.Identifier);
                if (memberOfHouseholdProxyToRemove == null)
                {
                    _removedHouseholdMemberCollection.Remove(householdMemberToRemove);
                    continue;
                }

                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdProxyToRemove);
                }
                HandleAffectedHouseholdMember(dataProvider, memberOfHouseholdProxyToRemove.HouseholdMember as IHouseholdMemberProxy);
                _removedHouseholdMemberCollection.Remove(householdMemberToRemove);
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

            foreach (IHouseholdMemberProxy affectedHouseholdMember in MemberOfHouseholdProxy.DeleteMemberOfHouseholds(dataProvider, this))
            {
                HandleAffectedHouseholdMember(dataProvider, affectedHouseholdMember);
            }

            StorageProxy.DeleteStorages(dataProvider, Identifier.Value);

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Creates the SQL statement for getting this household.
        /// </summary>
        /// <returns>SQL statement for getting this household.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            // ReSharper disable StringLiteralTypo
            return new HouseholdDataCommandBuilder("SELECT HouseholdIdentifier,Name,Descr,CreationTime FROM Households WHERE HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddHouseholdIdentifierParameter(Identifier)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for inserting this household.
        /// </summary>
        /// <returns>SQL statement for inserting this household.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            // ReSharper disable StringLiteralTypo
            return new HouseholdDataCommandBuilder("INSERT INTO Households (HouseholdIdentifier,Name,Descr,CreationTime) VALUES(@householdIdentifier,@name,@descr,@creationTime)")
            // ReSharper restore StringLiteralTypo
                .AddHouseholdIdentifierParameter(Identifier)
                .AddHouseholdNameParameter(Name)
                .AddHouseholdDescriptionParameter(Description)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this household.
        /// </summary>
        /// <returns>SQL statement for updating this household.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            // ReSharper disable StringLiteralTypo
            return new HouseholdDataCommandBuilder("UPDATE Households SET Name=@name,Descr=@descr,CreationTime=@creationTime WHERE HouseholdIdentifier=@householdIdentifier")
            // ReSharper restore StringLiteralTypo
                .AddHouseholdIdentifierParameter(Identifier)
                .AddHouseholdNameParameter(Name)
                .AddHouseholdDescriptionParameter(Description)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this household.
        /// </summary>
        /// <returns>SQL statement for deleting this household.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new HouseholdDataCommandBuilder("DELETE FROM Households WHERE HouseholdIdentifier=@householdIdentifier")
                .AddHouseholdIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IHouseholdProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy to a given household with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy to a given household with values from the data reader.</returns>
        public virtual IHouseholdProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            return new HouseholdProxy(GetName(dataReader, columnNameCollection[1]), GetDescription(dataReader, columnNameCollection[2]), GetCreationTime(dataReader, columnNameCollection[3]), dataProvider)
            {
                Identifier = GetHouseholdIdentifier(dataReader, columnNameCollection[0])
            };
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
                _removedHouseholdMemberCollection.Add(householdMemberToRemove);
            }
            return householdMemberToRemove;
        }

        /// <summary>
        /// Gets the household identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The household identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetHouseholdIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the name from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetName(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetString(columnName);
        }

        /// <summary>
        /// Gets the description from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The description.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static string GetDescription(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            int columnNo = dataReader.GetOrdinal(columnName);
            return dataReader.IsDBNull(columnNo) == false ? dataReader.GetString(columnNo) : null;
        }

        /// <summary>
        /// Gets the time for when the household was created from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The time for when the household was created.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static DateTime GetCreationTime(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return dataReader.GetMySqlDateTime(columnName).Value.ToLocalTime();
        }

        /// <summary>
        /// Handles an affected household member.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="affectedHouseholdMember">Implementation of a data proxy to the affected household member.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> or <paramref name="affectedHouseholdMember"/> is null.</exception>
        private static void HandleAffectedHouseholdMember(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IHouseholdMemberProxy affectedHouseholdMember)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(affectedHouseholdMember, nameof(affectedHouseholdMember));

            if (affectedHouseholdMember.Membership != Membership.Basic)
            {
                return;
            }

            if (affectedHouseholdMember.Households.Any())
            {
                return;
            }

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                subDataProvider.Delete(affectedHouseholdMember);
            }
        }

        #endregion
    }
}
