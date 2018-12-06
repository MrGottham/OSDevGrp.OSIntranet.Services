using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    /// Data proxy to a payment from a stakeholder.
    /// </summary>
    public class PaymentProxy : Payment, IPaymentProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a data proxy to a payment from a stakeholder.
        /// </summary>
        public PaymentProxy()
        {
        }

        /// <summary>
        /// Creates a data proxy to a payment from a stakeholder.
        /// </summary>
        /// <param name="stakeholder">Stakeholder who has made the payment.</param>
        /// <param name="dataProvider">Data provider who has handled the payment.</param>
        /// <param name="paymentTime">Date and time for the payment.</param>
        /// <param name="paymentReference">Payment reference from the data provider who has handled the payment.</param>
        /// <param name="paymentReceipt">Payment receipt from the data provider who has handled the payment.</param>
        /// <param name="creationTime">Creation date and time.</param>
        public PaymentProxy(IStakeholder stakeholder, IDataProvider dataProvider, DateTime paymentTime, string paymentReference, IEnumerable<byte> paymentReceipt, DateTime creationTime)
            : base(stakeholder, dataProvider, paymentTime, paymentReference, paymentReceipt, creationTime)
        {
        }

        #endregion

        #region IMySqlDataProxy Members

        /// <summary>
        /// Gets the unique identification for the payment from a stakeholder.
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

            Identifier = Guid.Parse(dataReader.GetString("PaymentIdentifier"));
            PaymentTime = dataReader.GetDateTime("PaymentTime").ToLocalTime();
            PaymentReference = dataReader.GetString("PaymentReference");
            CreationTime = dataReader.GetDateTime("CreationTime").ToLocalTime();

            int stakeholderTypeAsInt = dataReader.GetInt32("StakeholderType");
            switch ((StakeholderType) stakeholderTypeAsInt)
            {
                case StakeholderType.HouseholdMember:
                    Stakeholder = dataProvider.Create(new HouseholdMemberProxy(), dataReader, "StakeholderIdentifier", "HouseholdMemberMailAddress", "HouseholdMemberMembership", "HouseholdMemberMembershipExpireTime", "HouseholdMemberActivationCode", "HouseholdMemberActivationTime", "HouseholdMemberPrivacyPolicyAcceptedTime", "HouseholdMemberCreationTime");
                    break;

                default:
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, stakeholderTypeAsInt, "StakeholderType", MethodBase.GetCurrentMethod()));
            }

            DataProvider = dataProvider.Create(new DataProviderProxy(), dataReader, "DataProviderIdentifier", "DataProviderName", "DataProviderHandlesPayments", "DataProviderDataSourceStatementIdentifier");

            int paymentReceiptColumnNo = dataReader.GetOrdinal("PaymentReceipt");
            if (dataReader.IsDBNull(paymentReceiptColumnNo))
            {
                PaymentReceipt = null;
                return;
            }

            using (TextReader textReader = dataReader.GetTextReader(paymentReceiptColumnNo))
            {
                PaymentReceipt = Convert.FromBase64String(textReader.ReadToEnd());
                textReader.Close();
            }
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
        /// Creates the SQL statement for getting this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement for getting this payment from a stakeholder.</returns>
        public virtual MySqlCommand CreateGetCommand()
        {
            return BuildHouseholdDataCommandForSelecting("WHERE p.PaymentIdentifier=@paymentIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddPaymentIdentifierParameter(Identifier));
        }

        /// <summary>
        /// Creates the SQL statement for inserting this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement for inserting this payment from a stakeholder.</returns>
        public virtual MySqlCommand CreateInsertCommand()
        {
            return new HouseholdDataCommandBuilder("INSERT INTO Payments (PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime) VALUES(@paymentIdentifier,@stakeholderIdentifier,@stakeholderType,@dataProviderIdentifier,@paymentTime,@paymentReference,@paymentReceipt,@creationTime)")
                .AddPaymentIdentifierParameter(Identifier)
                .AddStakeholderIdentifierParameter(Stakeholder.Identifier)
                .AddStakeholderTypeParameter(Stakeholder.StakeholderType)
                .AddDataProviderIdentifierParameter(DataProvider.Identifier)
                .AddPaymentTimeParameter(PaymentTime)
                .AddPaymentReferenceParameter(PaymentReference)
                .AddPaymentReceiptParameter(PaymentReceipt)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for updating this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement for updating this payment from a stakeholder.</returns>
        public virtual MySqlCommand CreateUpdateCommand()
        {
            return new HouseholdDataCommandBuilder("UPDATE Payments SET StakeholderIdentifier=@stakeholderIdentifier,StakeholderType=@stakeholderType,DataProviderIdentifier=@dataProviderIdentifier,PaymentTime=@paymentTime,PaymentReference=@paymentReference,PaymentReceipt=@paymentReceipt,CreationTime=@creationTime WHERE PaymentIdentifier=@paymentIdentifier")
                .AddPaymentIdentifierParameter(Identifier)
                .AddStakeholderIdentifierParameter(Stakeholder.Identifier)
                .AddStakeholderTypeParameter(Stakeholder.StakeholderType)
                .AddDataProviderIdentifierParameter(DataProvider.Identifier)
                .AddPaymentTimeParameter(PaymentTime)
                .AddPaymentReferenceParameter(PaymentReference)
                .AddPaymentReceiptParameter(PaymentReceipt)
                .AddCreationTimeParameter(CreationTime)
                .Build();
        }

        /// <summary>
        /// Creates the SQL statement for deleting this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement for deleting this payment from a stakeholder.</returns>
        public virtual MySqlCommand CreateDeleteCommand()
        {
            return new HouseholdDataCommandBuilder("DELETE FROM Payments WHERE PaymentIdentifier=@paymentIdentifier")
                .AddPaymentIdentifierParameter(Identifier)
                .Build();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all the payments made by a given stakeholder.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="stakeholderIdentifier">Identifier for the stakeholder on which to get payments.</param>
        /// <returns>All the payments made by the given stakeholder.</returns>
        internal static IEnumerable<PaymentProxy> GetPayments(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid stakeholderIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
            {
                MySqlCommand command = BuildHouseholdDataCommandForSelecting("WHERE p.StakeholderIdentifier=@stakeholderIdentifier", householdDataCommandBuilder => householdDataCommandBuilder.AddStakeholderIdentifierParameter(stakeholderIdentifier));
                return subDataProvider.GetCollection<PaymentProxy>(command);
            }
        }

        /// <summary>
        /// Delete all the payments made by a given stakeholder.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="stakeholderIdentifier">Identifier for the stakeholder on which to delete payments.</param>
        internal static void DeletePayments(IDataProviderBase<MySqlDataReader, MySqlCommand> dataProvider, Guid stakeholderIdentifier)
        {
            ArgumentNullGuard.NotNull(dataProvider, nameof(dataProvider));

            foreach (PaymentProxy paymentProxy in GetPayments(dataProvider, stakeholderIdentifier))
            {
                using (IFoodWasteDataProvider subDataProvider = (IFoodWasteDataProvider) dataProvider.Clone())
                {
                    subDataProvider.Delete(paymentProxy);
                }
            }
        }

        /// <summary>
        /// Creates a MySQL command selecting a collection of <see cref="PaymentProxy"/>.
        /// </summary>
        /// <param name="whereClause">The WHERE clause which the MySQL command should use.</param>
        /// <param name="parameterAdder">The callback to add MySQL parameters to the MySQL command.</param>
        /// <returns>MySQL command selecting a collection of <see cref="PaymentProxy"/>.</returns>
        private static MySqlCommand BuildHouseholdDataCommandForSelecting(string whereClause = null, Action<HouseholdDataCommandBuilder> parameterAdder = null)
        {
            StringBuilder selectStatementBuilder = new StringBuilder("SELECT p.PaymentIdentifier,p.StakeholderIdentifier,p.StakeholderType,p.DataProviderIdentifier,p.PaymentTime,p.PaymentReference,p.PaymentReceipt,p.CreationTime,hm.MailAddress AS HouseholdMemberMailAddress,hm.ActivationCode AS HouseholdMemberActivationCode,hm.ActivationTime AS HouseholdMemberActivationTime,hm.CreationTime AS HouseholdMemberCreationTime,hm.Membership AS HouseholdMemberMembership,hm.MembershipExpireTime AS HouseholdMemberMembershipExpireTime,hm.PrivacyPolicyAcceptedTime AS HouseholdMemberPrivacyPolicyAcceptedTime,dp.Name AS DataProviderName,dp.HandlesPayments AS DataProviderHandlesPayments,dp.DataSourceStatementIdentifier AS DataProviderDataSourceStatementIdentifier FROM Payments AS p LEFT JOIN HouseholdMembers AS hm ON hm.HouseholdMemberIdentifier=p.StakeholderIdentifier INNER JOIN DataProviders AS dp ON dp.DataProviderIdentifier=p.DataProviderIdentifier");
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

        #endregion
    }
}
