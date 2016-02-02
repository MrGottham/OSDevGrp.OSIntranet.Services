using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste
{
    /// <summary>
    /// Interface for a payment from a stakeholder.
    /// </summary>
    public interface IPayment : IIdentifiable
    {
        /// <summary>
        /// Stakeholder who has made the payment.
        /// </summary>
        IStakeholder Stakeholder { get; }

        /// <summary>
        /// Data provider who has handled the payment.
        /// </summary>
        IDataProvider DataProvider { get; }

        /// <summary>
        /// Date and time for the payment.
        /// </summary>
        DateTime PaymentTime { get; }

        /// <summary>
        /// Payment reference from the data provider who has handled the payment.
        /// </summary>
        string PaymentReference { get; }

        /// <summary>
        /// Payment receipt from the data provider who has handled the payment.
        /// </summary>
        IEnumerable<byte> PaymentReceipt { get; }

        /// <summary>
        /// Creation date and time.
        /// </summary>
        DateTime CreationTime { get; }
    }
}
