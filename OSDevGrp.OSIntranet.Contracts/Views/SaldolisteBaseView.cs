using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Basisviewobjekt for en saldoliste til henholdsvis debitorer og kreditorer.
    /// </summary>
    [DataContract(Name = "SaldolisteBase", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class SaldolisteBaseView : TelefonlisteView
    {
        /// <summary>
        /// Saldo.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal Saldo
        {
            get;
            set;
        }
    }
}
