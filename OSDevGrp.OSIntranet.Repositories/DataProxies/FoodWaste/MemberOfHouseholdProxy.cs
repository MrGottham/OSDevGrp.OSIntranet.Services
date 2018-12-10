using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProviders;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy which bind a given household member to a given household.
    /// </summary>
    public class MemberOfHouseholdProxy : IdentifiableBase, IMemberOfHouseholdProxy
    {
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
            ArgumentNullGuard.NotNull(householdMember, nameof(householdMember))
                .NotNull(household, nameof(household));

            HouseholdMember = householdMember;
            Household = household;
            CreationTime = creationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Household member which are member of the household.
        /// </summary>
        public virtual IHouseholdMember HouseholdMember { get; private set; }

        /// <summary>
        /// Identifier for the household member which are member of the household.
        /// </summary>
        public virtual Guid? HouseholdMemberIdentifier => HouseholdMember?.Identifier;

        /// <summary>
        /// Household which the household member are member of.
        /// </summary>
        public virtual IHousehold Household { get; private set; }

        /// <summary>
        /// Identifier for the household which the household member are member of.
        /// </summary>
        public virtual Guid? HouseholdIdentifier => Household?.Identifier;

        /// <summary>
        /// Date and time for when the membership to the household was created.
        /// </summary>
        public virtual DateTime CreationTime { get; private set; }

        #endregion

        #region IMySqlDataProxy Members

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

            Identifier = Guid.Parse(dataReader.GetString("MemberOfHouseholdIdentifier"));
            HouseholdMember = dataProvider.Create(new HouseholdMemberProxy(), dataReader, "HouseholdMemberIdentifier", "HouseholdMemberMailAddress", "HouseholdMemberMembership", "HouseholdMemberMembershipExpireTime", "HouseholdMemberActivationCode", "HouseholdMemberActivationTime", "HouseholdMemberPrivacyPolicyAcceptedTime", "HouseholdMemberCreationTime");
            // ReSharper disable StringLiteralTypo
            Household = dataProvider.Create(new HouseholdProxy(), dataReader, "HouseholdIdentifier", "HouseholdName", "HouseholdDescr", "HouseholdCreationTime");
            // ReSharper restore StringLiteralTypo
            CreationTime = dataReader.GetDateTime("CreationTime").ToLocalTime();
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));
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
        }

        /// <summary>
        /// Creates the SQL statement for getting this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for getting this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildHouseholdDataCommandForSelecting("WHERE moh.MemberOfHouseholdIdentifier=@memberOfHouseholdIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddMemberOfHouseholdIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for inserting this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new HouseholdDataCommandBuilder("INSERT INTO MemberOfHouseholds (MemberOfHouseholdIdentifier,HouseholdMemberIdentifier,HouseholdIdentifier,CreationTime) VALUES(@memberOfHouseholdIdentifier,@householdMemberIdentifier,@householdIdentifier,@creationTime)")
                .AddMemberOfHouseholdIdentifierParameter(Identifier)
                .AddHouseholdMemberIdentifierParameter(HouseholdMemberIdentifier)
                .AddHouseholdIdentifierParameter(HouseholdIdentifier)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for updating this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new HouseholdDataCommandBuilder("UPDATE MemberOfHouseholds SET HouseholdMemberIdentifier=@householdMemberIdentifier,HouseholdIdentifier=@householdIdentifier,CreationTime=@creationTime WHERE MemberOfHouseholdIdentifier=@memberOfHouseholdIdentifier")
                .AddMemberOfHouseholdIdentifierParameter(Identifier)
                .AddHouseholdMemberIdentifierParameter(HouseholdMemberIdentifier)
                .AddHouseholdIdentifierParameter(HouseholdIdentifier)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this binding between a given household member and a given household.
        /// </summary>
        /// <returns>SQL statement for deleting this binding between a given household member and a given household.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new HouseholdDataCommandBuilder("DELETE FROM MemberOfHouseholds WHERE MemberOfHouseholdIdentifier=@memberOfHouseholdIdentifier")
                .AddMemberOfHouseholdIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the bindings which bind a given household member to all the households on which there is a membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdMemberProxy">Data proxy for the household member on which to get the bindings.</param>
        /// <returns>Bindings which bind a given household member to all the households on which there is a membership.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> or <paramref name="householdMemberProxy"/> is null.</exception>
        internal static IEnumerable<MemberOfHouseholdProxy> GetMemberOfHouseholds(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IHouseholdMemberProxy householdMemberProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(householdMemberProxy, nameof(householdMemberProxy));

            if (householdMemberProxy.Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdMemberProxy.Identifier, "Identifier"));
            }

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildHouseholdDataCommandForSelecting("WHERE moh.HouseholdMemberIdentifier=@householdMemberIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddHouseholdMemberIdentifierParameter(householdMemberProxy.Identifier.Value));
                return subDataProvider.GetCollection<MemberOfHouseholdProxy>(command);
            }
        }

        /// <summary>
        /// Deletes the bindings which bind a given household member to all the households on which there is a membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdMemberProxy">Data proxy for the household member on which to delete the bindings.</param>
        /// <returns>Affected households.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> or <paramref name="householdMemberProxy"/> is null.</exception>
        internal static IEnumerable<IHouseholdProxy> DeleteMemberOfHouseholds(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IHouseholdMemberProxy householdMemberProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(householdMemberProxy, nameof(householdMemberProxy));


            return DeleteMemberOfHouseholds(dataProvider, () => GetMemberOfHouseholds(dataProvider, householdMemberProxy).ToArray(), memberOfHouseholdProxy => memberOfHouseholdProxy.Household as IHouseholdProxy);
        }

        /// <summary>
        /// Gets the bindings which bind a given household to all the household members who has membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdProxy">Data proxy for the household on which to get the bindings.</param>
        /// <returns>Bindings which bind a given household to all the household members who has membership.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> or <paramref name="householdProxy"/> is null.</exception>
        internal static IEnumerable<MemberOfHouseholdProxy> GetMemberOfHouseholds(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IHouseholdProxy householdProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(householdProxy, nameof(householdProxy));

            if (householdProxy.Identifier.HasValue == false)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, householdProxy.Identifier, "Identifier"));
            }

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildHouseholdDataCommandForSelecting("WHERE moh.HouseholdIdentifier=@householdIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddHouseholdIdentifierParameter(householdProxy.Identifier.Value));
                return subDataProvider.GetCollection<MemberOfHouseholdProxy>(command);
            }
        }

        /// <summary>
        /// Deletes the bindings which bind a given household to all the household members who has membership.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="householdProxy">Data proxy for the household on which to delete the bindings.</param>
        /// <returns>Affected household members.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/> or <paramref name="householdProxy"/> is null.</exception>
        internal static IEnumerable<IHouseholdMemberProxy> DeleteMemberOfHouseholds(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, IHouseholdProxy householdProxy)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(householdProxy, nameof(householdProxy));

            return DeleteMemberOfHouseholds(dataProvider, () => GetMemberOfHouseholds(dataProvider, householdProxy).ToArray(), memberOfHouseholdProxy => memberOfHouseholdProxy.HouseholdMember as IHouseholdMemberProxy);
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="MemberOfHouseholdProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="MemberOfHouseholdProxy"/>.</returns>
        private static MySqlCommand BuildHouseholdDataCommandForSelecting(string whereClause = null, Action<HouseholdDataCommandBuilder> parameterAdder = null)
        {
            // ReSharper disable StringLiteralTypo
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT moh.MemberOfHouseholdIdentifier,moh.HouseholdMemberIdentifier,hm.MailAddress AS HouseholdMemberMailAddress,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,hm.CreationTime AS HouseholdMemberCreationTime,moh.HouseholdIdentifier,h.Name AS HouseholdName,h.Descr AS HouseholdDescr,h.CreationTime AS HouseholdCreationTime,moh.CreationTime FROM MemberOfHouseholds AS moh INNER JOIN HouseholdMembers hm ON hm.HouseholdMemberIdentifier=moh.HouseholdMemberIdentifier INNER JOIN Households h ON h.HouseholdIdentifier=moh.HouseholdIdentifier");
            // ReSharper restore StringLiteralTypo
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
        /// Deletes the bindings which binds household members to households.
        /// </summary>
        /// <typeparam name="T">Type of the result which should be returned for the deleted binding.</typeparam>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="memberOfHouseholdProxyCollectionGetter">The getter which gets the bindings to delete.</param>
        /// <param name="resultGetter">The getter which gets the result for each deleted binding.</param>
        /// <returns>Collection of the results for each deleted binding.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dataProvider"/>, <paramref name="memberOfHouseholdProxyCollectionGetter"/> or <paramref name="resultGetter"/> is null.</exception>
        private static IEnumerable<T> DeleteMemberOfHouseholds<T>(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Func<MemberOfHouseholdProxy[]> memberOfHouseholdProxyCollectionGetter, Func<MemberOfHouseholdProxy, T> resultGetter) where T : IMySqlDataProxy
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider))
                .NotNull(memberOfHouseholdProxyCollectionGetter, nameof(memberOfHouseholdProxyCollectionGetter))
                .NotNull(resultGetter, nameof(resultGetter));

            MemberOfHouseholdProxy[] memberOfHouseholdProxyCollection = memberOfHouseholdProxyCollectionGetter();
            foreach (MemberOfHouseholdProxy memberOfHouseholdProxy in memberOfHouseholdProxyCollection)
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(memberOfHouseholdProxy);
                }
            }

            return memberOfHouseholdProxyCollection
                .Where(memberOfHouseholdProxy => memberOfHouseholdProxy != null && resultGetter(memberOfHouseholdProxy) != null)
                .Select(resultGetter)
                .ToList();
        }

        #endregion
    }
}
