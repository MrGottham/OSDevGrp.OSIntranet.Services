using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy to a given food group.
    /// </summary>
    public class FoodGroupProxy : FoodGroup, IFoodGroupProxy
    {
        #region Private variables

        private bool _parentHasBeenLoaded;
        private bool _childrenHasBeenLoaded;
        private bool _translationCollectionHasBeenLoaded;
        private bool _foreignKeyCollectionHasBeenLoaded;
        private IFoodWasteDataProvider _dataProvider;

        private static bool _callingFromParentSetter;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a data proxy to a given food group.
        /// </summary>
        public FoodGroupProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a given food group.
        /// </summary>
        /// <param name="children">Foods groups which has this food group as a parent.</param>
        public FoodGroupProxy(IEnumerable<IFoodGroup> children) 
            : base(children)
        {
            _childrenHasBeenLoaded = true;
        }

        /// <summary>
        /// Creates a data proxy to a given food group.
        /// </summary>
        /// <param name="parentFoodGroupIdentifier">The identifier for the parent food group.</param>
        /// <param name="dataProvider">The data provider which the created data proxy should use.</param>
        private FoodGroupProxy(Guid? parentFoodGroupIdentifier, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            if (parentFoodGroupIdentifier.HasValue)
            {
                base.Parent = new FoodGroupProxy
                {
                    Identifier = parentFoodGroupIdentifier
                };
                _parentHasBeenLoaded = false;
            }

            if (dataProvider == null)
            {
                return;
            }

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Food group which has this food group as a child.
        /// </summary>
        public override IFoodGroup Parent
        {
            get
            {
                lock (SyncRoot)
                {
                    if (_callingFromParentSetter)
                    {
                        return base.Parent;
                    }
                }

                if (_parentHasBeenLoaded || _dataProvider == null || base.Parent == null || base.Parent.Identifier.HasValue == false || _childrenHasBeenLoaded)
                {
                    return base.Parent;
                }

                Parent = GetFoodGroup(_dataProvider, base.Parent.Identifier.Value);
                return base.Parent;
            }
            set
            {
                lock (SyncRoot)
                {
                    _callingFromParentSetter = true;
                    try
                    {
                        base.Parent = value;
                        _parentHasBeenLoaded = true;
                    }
                    finally
                    {
                        _callingFromParentSetter = false;
                    }
                }
            }
        }

        /// <summary>
        /// Foods groups which has this food group as a parent.
        /// </summary>
        public override IEnumerable<IFoodGroup> Children
        {
            get
            {
                if (_childrenHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Children;
                }

                Children = new List<IFoodGroup>(GetFoodGroupChildren(_dataProvider, Identifier.Value));
                return base.Children;
            }
            protected set
            {
                base.Children = value;
                _childrenHasBeenLoaded = true;
            }
        }

        ///// <summary>
        ///// Translations for the food group.
        ///// </summary>
        public override IEnumerable<ITranslation> Translations
        {
            get
            {
                if (_translationCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.Translations;
                }

                Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(_dataProvider, Identifier.Value));
                return base.Translations;
            }
            protected set
            {
                base.Translations = value;
                _translationCollectionHasBeenLoaded = true;
            }
        }

        /// <summary>
        /// Foreign keys for the food group.
        /// </summary>
        public override IEnumerable<IForeignKey> ForeignKeys
        {
            get
            {
                if (_foreignKeyCollectionHasBeenLoaded || _dataProvider == null || Identifier.HasValue == false)
                {
                    return base.ForeignKeys;
                }

                ForeignKeys = new List<IForeignKey>(ForeignKeyProxy.GetDomainObjectForeignKeys(_dataProvider, Identifier.Value));
                return base.ForeignKeys;
            }
            protected set
            {
                base.ForeignKeys = value;
                _foreignKeyCollectionHasBeenLoaded = true;
            }
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the food group.
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

            Identifier = GetFoodGroupIdentifier(dataReader, "FoodGroupIdentifier");
            Parent = dataReader.IsDBNull(dataReader.GetOrdinal("ParentIdentifier")) == false ? dataProvider.Create(this, dataReader, "ParentIdentifier", "ParentsParentIdentifier", "ParentIsActive") : null;
            IsActive = GetIsActive(dataReader, "IsActive");

            _childrenHasBeenLoaded = false;
            _translationCollectionHasBeenLoaded = false;
            _foreignKeyCollectionHasBeenLoaded = false;
            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            if (Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, Identifier, "Identifier"));
            }

            Children = new List<IFoodGroup>(GetFoodGroupChildren(dataProvider, Identifier.Value));
            Translations = new List<ITranslation>(TranslationProxy.GetDomainObjectTranslations(dataProvider, Identifier.Value));
            ForeignKeys = new List<IForeignKey>(ForeignKeyProxy.GetDomainObjectForeignKeys(dataProvider, Identifier.Value));

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

            foreach (IFoodGroupProxy foodGroupProxy in GetFoodGroupChildren(dataProvider, Identifier.Value))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(foodGroupProxy);
                }
            }

            FoodItemGroupProxy.DeleteFoodItemGroups(dataProvider, this);
            TranslationProxy.DeleteDomainObjectTranslations(dataProvider, Identifier.Value);
            ForeignKeyProxy.DeleteDomainObjectForeignKeys(dataProvider, Identifier.Value);

            _dataProvider = (IFoodWasteDataProvider) dataProvider;
        }

        /// <summary>
        /// Creates the SQL statement for getting this food group.
        /// </summary>
        /// <returns>SQL statement for getting this food group.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildSystemDataCommandForSelecting("WHERE fg.FoodGroupIdentifier=@foodGroupIdentifier", commandBuilder => commandBuilder.AddFoodGroupIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this food group.
        /// </summary>
        /// <returns>SQL statement for inserting this food group.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new SystemDataCommandBuilder("INSERT INTO FoodGroups (FoodGroupIdentifier,ParentIdentifier,IsActive) VALUES(@foodGroupIdentifier,@parentIdentifier,@isActive)")
                .AddFoodGroupIdentifierParameter(Identifier)
                .AddFoodGroupParentIdentifierParameter(Parent?.Identifier)
                .AddFoodGroupIsActiveParameter(IsActive)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this food group.
        /// </summary>
        /// <returns>SQL statement for updating this food group.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new SystemDataCommandBuilder("UPDATE FoodGroups SET ParentIdentifier=@parentIdentifier,IsActive=@isActive WHERE FoodGroupIdentifier=@foodGroupIdentifier")
                .AddFoodGroupIdentifierParameter(Identifier)
                .AddFoodGroupParentIdentifierParameter(Parent?.Identifier)
                .AddFoodGroupIsActiveParameter(IsActive)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this food group.
        /// </summary>
        /// <returns>SQL statement for deleting this food group.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new SystemDataCommandBuilder("DELETE FROM FoodGroups WHERE FoodGroupIdentifier=@foodGroupIdentifier")
                .AddFoodGroupIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region IMySqlDataProxyCreator<IFoodGroupProxy> Members

        /// <summary>
        /// Creates an instance of the data proxy to a given food group with values from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader from which column values should be read.</param>
        /// <param name="dataProvider">Data provider which supports the data reader.</param>
        /// <param name="columnNameCollection">Collection of column names which should be read from the data reader.</param>
        /// <returns>Instance of the data proxy to a given food group with values from the data reader.</returns>
        public virtual IFoodGroupProxy Create(MySqlDataReader dataReader, IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, params string[] columnNameCollection)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNull(dataProvider, nameof(dataProvider))
                .NotNull(columnNameCollection, nameof(columnNameCollection));

            Guid? parentIdentifier = dataReader.IsDBNull(dataReader.GetOrdinal(columnNameCollection[1])) == false ? GetParentIdentifier(dataReader, columnNameCollection[1]) : (Guid?) null;
            return new FoodGroupProxy(parentIdentifier, dataProvider)
            {
                Identifier = GetFoodGroupIdentifier(dataReader, columnNameCollection[0]),
                IsActive = GetIsActive(dataReader, columnNameCollection[2])
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data proxy for a given food group identifier.
        /// </summary>
        /// <param name="dataProvider">The data provider from which to get the given food group.</param>
        /// <param name="foodGroupIdentifier">The food group identifier on which to get the data proxy.</param>
        /// <returns>The data proxy for the given food group identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> is null.</exception>
        internal static FoodGroupProxy GetFoodGroup(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foodGroupIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                return subDataProvider.Get(new FoodGroupProxy {Identifier = foodGroupIdentifier});
            }
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="FoodGroupProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="FoodGroupProxy"/>.</returns>
        internal static MySqlCommand BuildSystemDataCommandForSelecting(string whereClause = null, Action<SystemDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT fg.FoodGroupIdentifier,fg.ParentIdentifier,fg.IsActive,pfg.ParentIdentifier AS ParentsParentIdentifier,pfg.IsActive AS ParentIsActive FROM FoodGroups AS fg LEFT JOIN FoodGroups AS pfg ON pfg.FoodGroupIdentifier=fg.ParentIdentifier");
            if (string.IsNullOrWhiteSpace(whereClause) == false)
            {
                selectStatementBuilder.Append($" {whereClause}");
            }

            SystemDataCommandBuilder systemDataCommandBuilder = new SystemDataCommandBuilder(selectStatementBuilder.ToString());
            if (parameterAdder == null)
            {
                return systemDataCommandBuilder.Build();
            }

            parameterAdder(systemDataCommandBuilder);
            return systemDataCommandBuilder.Build();
        }

        /// <summary>
        /// Gets the food group identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The food group identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetFoodGroupIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the parent food group identifier from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The parent food group identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static Guid GetParentIdentifier(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return new Guid(dataReader.GetString(columnName));
        }

        /// <summary>
        /// Gets the indication for whether the food group is active from the data reader.
        /// </summary>
        /// <param name="dataReader">The data reader from which to read.</param>
        /// <param name="columnName">The column name for value to read.</param>
        /// <returns>The indication for whether the food group is active.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataReader"/> is null or <paramref name="columnName"/> is null, empty or white space.</exception>
        private static bool GetIsActive(MySqlDataReader dataReader, string columnName)
        {
            ArgumentNullGuard.NotNull(dataReader, nameof(dataReader))
                .NotNullOrWhiteSpace(columnName, nameof(columnName));

            return Convert.ToBoolean(dataReader.GetInt32(columnName));
        }

        /// <summary>
        /// Gets foods groups which has this a given food group as a parent.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="foodGroupIdentifier">Identifier for the food group which is the parent.</param>
        /// <returns>Foods groups which has this a given food group as a parent.</returns>
        private static IEnumerable<FoodGroupProxy> GetFoodGroupChildren(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid foodGroupIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildSystemDataCommandForSelecting("WHERE fg.ParentIdentifier=@parentIdentifier", systemDataCommandBuilder => systemDataCommandBuilder.AddFoodGroupParentIdentifierParameter(foodGroupIdentifier));
                return subDataProvider.GetCollection<FoodGroupProxy>(command);
            }
        }

        #endregion
    }
}
