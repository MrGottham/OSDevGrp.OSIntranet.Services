using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for et brevhoved.
    /// </summary>
    [DataContract(Name = "Brevhoved", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BrevhovedView : BrevhovedreferenceView
    {
        /// <summary>
        /// Brevhovedets 1. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje1
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 2. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje2
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 3. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje3
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 4. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje4
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 5. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje5
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 6. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje6
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 7. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje7
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets CVR-nummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string CvrNr
        {
            get;
            set;
        }
    }
}
