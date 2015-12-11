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
    /// Hjælper til en liste af grupper til budgetkonti.
    /// </summary>
    public class BudgetkontogruppelisteHelper : DomainObjectListHelper<Budgetkontogruppe, int>, IBudgetkontogruppelisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af grupper til budgetkonti.
        /// </summary>
        /// <param name="budgetkontogrupper">Grupper til budgetkonti.</param>
        public BudgetkontogruppelisteHelper(IEnumerable<Budgetkontogruppe> budgetkontogrupper)
            : base(budgetkontogrupper)
        {
        }

        #endregion

        #region IDomainObjectListHelper<Budgetkontogruppe,int> Members

        /// <summary>
        /// Henter en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="id">Unik identifikation af gruppen til budgetkonti.</param>
        /// <returns>Gruppe til budgetkonti.</returns>
        public override Budgetkontogruppe GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Budgetkontogruppe).Name, id), ex);
            }
        }

        #endregion
    }
}
