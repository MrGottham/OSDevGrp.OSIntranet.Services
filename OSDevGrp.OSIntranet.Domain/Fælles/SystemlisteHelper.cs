using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Fælles
{
    /// <summary>
    /// Hjælper til en liste af systemer.
    /// </summary>
    public class SystemlisteHelper : DomainObjectListHelper<ISystem, int>, ISystemlisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af systemer.
        /// </summary>
        /// <param name="systemer">Systemer.</param>
        public SystemlisteHelper(IEnumerable<ISystem> systemer)
            : base(systemer)
        {
        }

        #endregion

        #region IDomainObjectListHelper<ISystem,int> Members

        /// <summary>
        /// Henter et givent system.
        /// </summary>
        /// <param name="id">Unik identifikation af systemet.</param>
        /// <returns>System.</returns>
        public override ISystem GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (ISystem).Name, id), ex);
            }
        }

        #endregion
    }
}
