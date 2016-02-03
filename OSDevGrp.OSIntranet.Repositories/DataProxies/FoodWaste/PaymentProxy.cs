using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
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

        #region Properties

        /// <summary>
        /// Gets the type value for the stakeholde.
        /// </summary>
        public virtual int? StakeholderType
        {
            get
            {
                if (Stakeholder == null)
                {
                    return null;
                }
                if (Stakeholder is IHouseholdMember)
                {
                    return 1;
                }
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, Stakeholder.GetType().Name, "Stakeholder", MethodBase.GetCurrentMethod()));
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

            throw new NotImplementedException();
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

            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void MapRelations(IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        /// <param name="isInserting">Indication of whether we are inserting or updating.</param>
        public virtual void SaveRelations(IDataProviderBase dataProvider, bool isInserting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete relations.
        /// </summary>
        /// <param name="dataProvider">Implementation of the data provider used to access data.</param>
        public virtual void DeleteRelations(IDataProviderBase dataProvider)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
