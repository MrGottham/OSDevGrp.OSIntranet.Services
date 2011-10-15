using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for et system under OSWEBDB.
    /// </summary>
    [DataContract(Name = "System", Namespace = SoapNamespaces.IntranetNamespace)]
    public class SystemView : IView
    {
        /// <summary>
        /// Unik identifikation af systemet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Titel på systemet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Titel
        {
            get;
            set;
        }

        /// <summary>
        /// Angivelse af, om systemet benytter kalenderen under OSWEBDB.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool Kalender
        {
            get;
            set;
        }
    }
}
