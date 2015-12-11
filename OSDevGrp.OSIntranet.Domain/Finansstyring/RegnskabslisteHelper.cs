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
    /// Hjælper til en regnskabsliste.
    /// </summary>
    public class RegnskabslisteHelper : DomainObjectListHelper<Regnskab, int>, IRegnskabslisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en regnskabsliste.
        /// </summary>
        /// <param name="regnskaber">Regnskaber.</param>
        public RegnskabslisteHelper(IEnumerable<Regnskab> regnskaber)
            : base(regnskaber)
        {
        }

        #endregion

        #region IDomainObjectListHelper<Regnskab,int> Members

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="id">Unik identifikation af regnskabet.</param>
        /// <returns>Regnskab.</returns>
        public override Regnskab GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Regnskab).Name, id), ex);
            }
        }

        #endregion
    }
}
