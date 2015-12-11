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
    /// Hjælper til en adresseliste.
    /// </summary>
    public class AdresselisteHelper : DomainObjectListHelper<AdresseBase, int>, IAdresselisteHelper
    {
        #region Constructor

        /// <summary>
        /// Danner hjælper til en adresseliste.
        /// </summary>
        /// <param name="adresser">Adresser.</param>
        public AdresselisteHelper(IEnumerable<AdresseBase> adresser)
            : base(adresser)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Adresser.
        /// </summary>
        public IEnumerable<AdresseBase> Adresser
        {
            get
            {
                return DomainObjetcs;
            }
        }

        #endregion

        #region IDomainObjectListHelper<AdresseBase,int> Members

        /// <summary>
        /// Henter og returnerer en given adresse.
        /// </summary>
        /// <param name="id">Unik identifikation af adressen.</param>
        /// <returns>Adresse.</returns>
        public override AdresseBase GetById(int id)
        {
            try
            {
                return DomainObjetcs.Single(m => m.Nummer == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new IntranetRepositoryException(Resource.GetExceptionMessage(ExceptionMessage.CantFindObjectById, typeof (AdresseBase).Name, id), ex);
            }
        }

        #endregion
    }
}
