using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Payment from a stakeholder.
    /// </summary>
    public class Payment : IdentifiableBase, IPayment
    {
        #region Private variables

        private IStakeholder _stakeholder;
        private IDataProvider _dataProvider;
        private DateTime _paymentTime;
        private string _paymentReference;
        private IEnumerable<byte> _paymentReceipt;
        private DateTime _creationTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a payment from a stakeholder.
        /// </summary>
        /// <param name="stakeholder">Stakeholder who has made the payment.</param>
        /// <param name="dataProvider">Data provider who has handled the payment.</param>
        /// <param name="paymentTime">Date and time for the payment.</param>
        /// <param name="paymentReference">Payment reference from the data provider who has handled the payment.</param>
        /// <param name="paymentReceipt">Payment receipt from the data provider who has handled the payment.</param>
        public Payment(IStakeholder stakeholder, IDataProvider dataProvider, DateTime paymentTime, string paymentReference, IEnumerable<byte> paymentReceipt = null)
            : this(stakeholder, dataProvider, paymentTime, paymentReference, paymentReceipt, DateTime.Now)
        {
        }

        /// <summary>
        /// Creates a payment from a stakeholder.
        /// </summary>
        protected Payment()
        {
        }

        /// <summary>
        /// Creates a payment from a stakeholder.
        /// </summary>
        /// <param name="stakeholder">Stakeholder who has made the payment.</param>
        /// <param name="dataProvider">Data provider who has handled the payment.</param>
        /// <param name="paymentTime">Date and time for the payment.</param>
        /// <param name="paymentReference">Payment reference from the data provider who has handled the payment.</param>
        /// <param name="paymentReceipt">Payment receipt from the data provider who has handled the payment.</param>
        /// <param name="creationTime">Creation date and time.</param>
        protected Payment(IStakeholder stakeholder, IDataProvider dataProvider, DateTime paymentTime, string paymentReference, IEnumerable<byte> paymentReceipt, DateTime creationTime)
        {
            if (stakeholder == null)
            {
                throw new ArgumentNullException("stakeholder");
            }
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (string.IsNullOrWhiteSpace(paymentReference))
            {
                throw new ArgumentNullException("paymentReference");
            }
            _stakeholder = stakeholder;
            _dataProvider = dataProvider;
            _paymentTime = paymentTime;
            _paymentReference = paymentReference;
            if (paymentReceipt != null)
            {
                _paymentReceipt = paymentReceipt.ToArray();
            }
            _creationTime = creationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stakeholder who has made the payment.
        /// </summary>
        public virtual IStakeholder Stakeholder
        {
            get
            {
                return _stakeholder;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _stakeholder = value;
            }
        }

        /// <summary>
        /// Data provider who has handled the payment.
        /// </summary>
        public virtual IDataProvider DataProvider
        {
            get
            {
                return _dataProvider;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _dataProvider = value;
            }
        }

        /// <summary>
        /// Date and time for the payment.
        /// </summary>
        public virtual DateTime PaymentTime
        {
            get { return _paymentTime; }
            protected set { _paymentTime = value; }
        }

        /// <summary>
        /// Payment reference from the data provider who has handled the payment.
        /// </summary>
        public virtual string PaymentReference
        {
            get
            {
                return _paymentReference;
            }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("value");
                }
                _paymentReference = value;
            }
        }

        /// <summary>
        /// Payment receipt from the data provider who has handled the payment.
        /// </summary>
        public virtual IEnumerable<byte> PaymentReceipt
        {
            get
            {
                return _paymentReceipt;
            }
            protected set
            {
                _paymentReceipt = value;
            }
        }

        /// <summary>
        /// Creation date and time.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get { return _creationTime; }
            protected set { _creationTime = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Make translation for the payment made by a stakeholder.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        public virtual void Translate(CultureInfo translationCulture)
        {
            if (translationCulture == null)
            {
                throw new ArgumentNullException("translationCulture");
            }
            DataProvider.Translate(translationCulture);
        }

        #endregion
    }
}
