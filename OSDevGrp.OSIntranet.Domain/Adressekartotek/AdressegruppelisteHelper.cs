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
    /// Hjælper til en liste af adressegrupper.
    /// </summary>
    public class AdressegruppelisteHelper : DomainObjectListHelper<Adressegruppe, int>, IAdressegruppelisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en liste af adressegrupper.
        /// </summary>
        /// <param name="adressegrupper">Adressegrupper.</param>
        public AdressegruppelisteHelper(IEnumerable<Adressegruppe> adressegrupper)
            : base(adressegrupper)
        {
        }

        #endregion

        #region IDomainObjectListHelper<Adressegruppe,int> Members

        /// <summary>
        /// Henter og returnerer en given adressegruppe.
        /// </summary>
        /// <param name="id">Unik identifikation af adressegruppen.</param>
        /// <returns>Adressegruppe.</returns>
        public override Adressegruppe GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (Adressegruppe), id), ex);
            }
        }

        #endregion
    }
}
