using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste.Enums;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Repositories.FoodWaste;
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
        #region Private variables

        private Guid? _stakeholderIdentifier;
        private StakeholderType? _stakeholderType;
        private Guid? _dataProviderIdentifier;
        private IDataProviderBase _dataProvider;

        #endregion

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

        #region Properties

        /// <summary>
        /// Stakeholder who has made the payment.
        /// </summary>
        public override IStakeholder Stakeholder
        {
            get
            {
                if (base.Stakeholder != null || _stakeholderIdentifier.HasValue == false || _stakeholderType.HasValue == false || _dataProvider == null)
                {
                    return base.Stakeholder;
                }
                using (var subDataProvider = (IDataProviderBase) _dataProvider.Clone())
                {
                    switch (_stakeholderType.Value)
                    {
                        case StakeholderType.HouseholdMember:
                            var householdMemberProxy = new HouseholdMemberProxy
                            {
                                Identifier = _stakeholderIdentifier.Value
                            };
                            base.Stakeholder = subDataProvider.Get(householdMemberProxy);
                            break;

                        default:
                            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, _stakeholderType, "_stakeholderType", MethodBase.GetCurrentMethod()));
                    }
                }
                return base.Stakeholder;
            }
        }

        /// <summary>
        /// Data provider who has handled the payment.
        /// </summary>
        public override IDataProvider DataProvider
        {
            get
            {
                if (base.DataProvider != null || _dataProviderIdentifier.HasValue == false || _dataProvider == null)
                {
                    return base.DataProvider;
                }
                using (var subDataProvider = (IDataProviderBase) _dataProvider.Clone())
                {
                    var dataProviderProxy = new DataProviderProxy
                    {
                        Identifier = _dataProviderIdentifier.Value
                    };
                    base.DataProvider = subDataProvider.Get(dataProviderProxy);
                }
                return base.DataProvider;
            }
        }

        #endregion

        #region IMySqlDataProxy<IPayment>

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

        /// <summary>
        /// Gets the SQL statement for selecting a given payment from a stakeholder.
        /// </summary>
        /// <param name="payment">Payment for which to get the SQL statement for selecting.</param>
        /// <returns>SQL statement for selecting a given payment from a stakeholder.</returns>
        public virtual string GetSqlQueryForId(IPayment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException("payment");
            }
            if (payment.Identifier.HasValue)
            {
                return string.Format("SELECT PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime FROM Payments WHERE PaymentIdentifier='{0}'", payment.Identifier.Value.ToString("D").ToUpper());
            }
            throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, payment.Identifier, "Identifier"));
        }

        /// <summary>
        /// Gets the SQL statement to insert this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement to insert this payment from a stakeholder.</returns>
        public virtual string GetSqlCommandForInsert()
        {
            var stakeholderIdentifierSqlValue = Stakeholder.Identifier.HasValue ? Stakeholder.Identifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            var dataProviderIdentifierSqlValue = DataProvider.Identifier.HasValue ? DataProvider.Identifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            var paymentReceiptSqlValue = PaymentReceipt == null ? "NULL" : string.Format("'{0}'", Convert.ToBase64String(PaymentReceipt.ToArray()));

            return string.Format("INSERT INTO Payments (PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime) VALUES('{0}','{1}',{2},'{3}',{4},'{5}',{6},{7})", UniqueId, stakeholderIdentifierSqlValue, (int) Stakeholder.StakeholderType, dataProviderIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(PaymentTime), PaymentReference, paymentReceiptSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to update this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement to update this payment from a stakeholder.</returns>
        public virtual string GetSqlCommandForUpdate()
        {
            var stakeholderIdentifierSqlValue = Stakeholder.Identifier.HasValue ? Stakeholder.Identifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            var dataProviderIdentifierSqlValue = DataProvider.Identifier.HasValue ? DataProvider.Identifier.Value.ToString("D").ToUpper() : default(Guid).ToString("D").ToUpper();
            var paymentReceiptSqlValue = PaymentReceipt == null ? "NULL" : string.Format("'{0}'", Convert.ToBase64String(PaymentReceipt.ToArray()));

            return string.Format("UPDATE Payments SET StakeholderIdentifier='{1}',StakeholderType={2},DataProviderIdentifier='{3}',PaymentTime={4},PaymentReference='{5}',PaymentReceipt={6},CreationTime={7} WHERE PaymentIdentifier='{0}'", UniqueId, stakeholderIdentifierSqlValue, (int) Stakeholder.StakeholderType, dataProviderIdentifierSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(PaymentTime), PaymentReference, paymentReceiptSqlValue, DataRepositoryHelper.GetSqlValueForDateTime(CreationTime));
        }

        /// <summary>
        /// Gets the SQL statement to delete this payment from a stakeholder.
        /// </summary>
        /// <returns>SQL statement to delete this payment from a stakeholder.</returns>
        public virtual string GetSqlCommandForDelete()
        {
            return string.Format("DELETE FROM Payments WHERE PaymentIdentifier='{0}'", UniqueId);
        }

        #endregion

        #region IDataProxyBase Members

        /// <summary>
        /// Maps data from the data reader.
        /// </summary>
        /// <param name="dataReader">Data reader.</param>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapData(object dataReader, IDataProviderBase dataProvider)
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

            Identifier = Guid.Parse(mySqlDataReader.GetString("PaymentIdentifier"));
            PaymentTime = mySqlDataReader.GetDateTime("PaymentTime").ToLocalTime();
            PaymentReference = mySqlDataReader.GetString("PaymentReference");
            CreationTime = mySqlDataReader.GetDateTime("CreationTime").ToLocalTime();

            var paymentReceiptColumnNo = mySqlDataReader.GetOrdinal("PaymentReceipt");
            if (mySqlDataReader.IsDBNull(paymentReceiptColumnNo) == false)
            {
                using (var columnTextReader = mySqlDataReader.GetTextReader(paymentReceiptColumnNo))
                {
                    PaymentReceipt = Convert.FromBase64String(columnTextReader.ReadToEnd());
                    columnTextReader.Close();
                }
            }

            _stakeholderIdentifier = Guid.Parse(mySqlDataReader.GetString("StakeholderIdentifier"));
            switch (mySqlDataReader.GetInt32("StakeholderType"))
            {
                case (int) StakeholderType.HouseholdMember:
                    _stakeholderType = StakeholderType.HouseholdMember;
                    break;

                default:
                    throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, mySqlDataReader.GetInt32("StakeholderType"), "StakeholderType", MethodBase.GetCurrentMethod()));
            }
            _dataProviderIdentifier = Guid.Parse(mySqlDataReader.GetString("DataProviderIdentifier"));

            if (_dataProvider == null)
            {
                _dataProvider = dataProvider;
            }
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
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
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
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
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
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
        }

        /// <summary>
        /// Gets all the payments made by a given stakeholder.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="stakeholderIdentifier">Identifier for the stakeholder on which to get payments.</param>
        /// <returns>All the payments made by the given stakeholder.</returns>
        internal static IEnumerable<PaymentProxy> GetPayments(IDataProviderBase dataProvider, Guid stakeholderIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
            {
                return subDataProvider.GetCollection<PaymentProxy>(string.Format("SELECT PaymentIdentifier,StakeholderIdentifier,StakeholderType,DataProviderIdentifier,PaymentTime,PaymentReference,PaymentReceipt,CreationTime FROM Payments WHERE StakeholderIdentifier='{0}' ORDER BY PaymentTime DESC", stakeholderIdentifier.ToString("D").ToUpper()));
            }
        }

        /// <summary>
        /// Delete all the payments made by a given stakeholder.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="stakeholderIdentifier">Identifier for the stakeholder on which to delete payments.</param>
        internal static void DeletePayments(IDataProviderBase dataProvider, Guid stakeholderIdentifier)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            foreach (var paymentProxy in GetPayments(dataProvider, stakeholderIdentifier))
            {
                using (var subDataProvider = (IDataProviderBase) dataProvider.Clone())
                {
                    subDataProvider.Delete(paymentProxy);
                }
            }
        }

        #endregion
    }
}
