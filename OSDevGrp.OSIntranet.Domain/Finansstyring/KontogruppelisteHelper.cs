using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.Domain.Interfaces.Finansstyring;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Finansstyring
{
    /// <summary>
    /// Hjælper til en liste af kontogrupper.
    /// </summary>
    public class KontogruppelisteHelper : DomainObjectListHelper<Kontogruppe, int>, IKontogruppelisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af kontogrupper.
        /// </summary>
        /// <param name="kontogrupper">Kontogrupper.</param>
        public KontogruppelisteHelper(IEnumerable<Kontogruppe> kontogrupper)
            : base(kontogrupper)
        {
        }

        #endregion

        #region IDomainObjectListHelper<Kontogruppe,int> Members

        /// <summary>
        /// Henter og returnerer en given kontogruppe.
        /// </summary>
        /// <param name="id">Unik identifikation af kontogruppen.</param>
        /// <returns>Kopntogruppe.</returns>
        public override Kontogruppe GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Kontogruppe).Name, id), ex);
            }
        }

        #endregion
    }
}
