using System;
using System.Collections.Generic;
using System.Globalization;

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

        /// <summary>
        /// Make translation for the payment made by a stakeholder.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        void Translate(CultureInfo translationCulture);
    }
}
