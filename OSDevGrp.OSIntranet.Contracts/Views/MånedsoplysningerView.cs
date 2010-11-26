using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Basisviewobject for månedsoplysninger.
    /// </summary>
    [DataContract(Name = "Månedsoplysninger", Namespace = SoapNamespaces.IntranetNamespace)]
    public abstract class MånedsoplysningerView : IView
    {
        /// <summary>
        /// Årstal.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int År
        {
            get;
            set;
        }

        /// <summary>
        /// Måned.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Måned
        {
            get;
            set;
        }
    }
}
