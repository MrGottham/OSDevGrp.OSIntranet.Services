using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en kalenderbruger.
    /// </summary>
    [DataContract(Name = "Kalenderbruger", Namespace = SoapNamespaces.IntranetNamespace)]
    public class KalenderbrugerView : IView
    {
        /// <summary>
        /// System under OSWEBDB, som kalenderbrugeren er tilknyttet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public SystemView System
        {
            get;
            set;
        }

        /// <summary>
        /// Unik identifikation af kalenderbrugeren.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Initialer på kalenderbrugeren.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Initialer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på kalenderbrugeren.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }
    }
}
