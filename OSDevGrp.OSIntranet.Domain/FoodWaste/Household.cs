using System;
using System.Collections.Generic;
using System.Globalization;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Household.
    /// </summary>
    public class Household : IdentifiableBase, IHousehold
    {
        #region Private variables
        #endregion

        #region Constructors
        #endregion

        #region Properties


        /// <summary>
        /// Name for the household.
        /// </summary>
        public virtual string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Description for the household.
        /// </summary>
        public virtual string Description
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Date and time for when the household was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Household members who is member of this household.
        /// </summary>
        public virtual IEnumerable<IHouseholdMember> HouseholdMembers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        public virtual void Translate(CultureInfo translationCulture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
