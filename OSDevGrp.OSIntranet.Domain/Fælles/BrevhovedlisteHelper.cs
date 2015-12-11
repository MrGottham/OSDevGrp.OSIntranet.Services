using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.Domain.Interfaces.Fælles;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Fælles
{
    /// <summary>
    /// Hjælper til en liste af brevhoveder.
    /// </summary>
    public class BrevhovedlisteHelper : DomainObjectListHelper<Brevhoved, int>, IBrevhovedlisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af brevhoveder.
        /// </summary>
        /// <param name="brevhoveder">Brevhoveder.</param>
        public BrevhovedlisteHelper(IEnumerable<Brevhoved> brevhoveder)
            : base(brevhoveder)
        {
        }

        #endregion

        #region IDomainObjectListHelper<Brevhoved,int> Members

        /// <summary>
        /// Henter et givent brevhoved.
        /// </summary>
        /// <param name="id">Unik identifikation af brevhovedet.</param>
        /// <returns>Brevhoved.</returns>
        public override Brevhoved GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Brevhoved).Name, id), ex);
            }
        }

        #endregion
    }
}
