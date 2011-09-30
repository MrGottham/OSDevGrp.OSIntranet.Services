using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Hjælper til en liste af aftaler i et givent system.
    /// </summary>
    public class AftalelisteHelper : DomainObjectListHelper<IAftale, int>, IAftalelisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af aftaler i et givent system.
        /// </summary>
        /// <param name="aftaler">Aftaler.</param>
        public AftalelisteHelper(IEnumerable<IAftale> aftaler)
            : base(aftaler)
        {
        }

        #endregion

        #region IDomainObjectListHelper<IAftale,int> Members

        /// <summary>
        /// Henter en given aftale.
        /// </summary>
        /// <param name="id">Unik identifikation af aftalen.</param>
        /// <returns>Aftale.</returns>
        public override IAftale GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (IAftale), id), ex);
            }
        }

        #endregion
    }
}
