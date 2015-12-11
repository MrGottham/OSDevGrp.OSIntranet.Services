using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.Domain.Interfaces.Adressekartotek;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.Adressekartotek
{
    /// <summary>
    /// Hjælper til en liste af betalingsbetingelser.
    /// </summary>
    public class BetalingsbetingelselisteHelper : DomainObjectListHelper<Betalingsbetingelse, int>, IBetalingsbetingelselisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af betalingsbetingelser.
        /// </summary>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        public BetalingsbetingelselisteHelper(IEnumerable<Betalingsbetingelse> betalingsbetingelser)
            : base(betalingsbetingelser)
        {
        }

        #endregion

        #region IDomainObjectListHelper<Betalingsbetingelse-,int> Members

        /// <summary>
        /// Henter og returnerer en given betalingsbetingelse.
        /// </summary>
        /// <param name="id">Unik identifikation af betalingsbetingelsen.</param>
        /// <returns>Betalingsbetingelse.</returns>
        public override Betalingsbetingelse GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Betalingsbetingelse).Name, id), ex);
            }
        }

        #endregion
    }
}
