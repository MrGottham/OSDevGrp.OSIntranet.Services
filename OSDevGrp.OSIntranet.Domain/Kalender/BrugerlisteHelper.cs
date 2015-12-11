using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Kalender;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Kalender
{
    /// <summary>
    /// Hjælper til en liste af brugere i et givent system.
    /// </summary>
    public class BrugerlisteHelper : DomainObjectListHelper<IBruger, int>, IBrugerlisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af brugere i et givent system.
        /// </summary>
        /// <param name="brugere">Brugere.</param>
        public BrugerlisteHelper(IEnumerable<IBruger> brugere)
            : base(brugere)
        {
        }

        #endregion

        #region IDomainObjectListHelper<IBruger,int> Members

        /// <summary>
        /// Henter en given bruger.
        /// </summary>
        /// <param name="id">Unik identifikation af brugeren.</param>
        /// <returns>Bruger.</returns>
        public override IBruger GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (IBruger).Name, id), ex);
            }
        }

        #endregion
    }
}
