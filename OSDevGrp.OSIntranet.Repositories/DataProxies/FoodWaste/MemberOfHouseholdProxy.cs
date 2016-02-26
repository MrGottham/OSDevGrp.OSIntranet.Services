using System;
using OSDevGrp.OSIntranet.Domain.FoodWaste;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Repositories.Interfaces.DataProxies.FoodWaste;

namespace OSDevGrp.OSIntranet.Repositories.DataProxies.FoodWaste
{
    /// <summary>
    /// Data proxy which bind a given household member to a given household.
    /// </summary>
    public class MemberOfHouseholdProxy : IdentifiableBase, IMemberOfHouseholdProxy
    {
        #region Constructors

        /// <summary>
        /// Creates a ata proxy which bind a given household member to a given household.
        /// </summary>
        public MemberOfHouseholdProxy()
        {
        }

        /// <summary>
        /// Creates a ata proxy which bind a given household member to a given household.
        /// </summary>
        /// <param name="householdMember">Household member which are member of the household.</param>
        /// <param name="household">Household which the household member are member of.</param>
        public MemberOfHouseholdProxy(IHouseholdMember householdMember, IHousehold household)
            : this(householdMember, household, DateTime.Now)
        {
        }

        /// <summary>
        /// Creates a ata proxy which bind a given household member to a given household.
        /// </summary>
        /// <param name="householdMember">Household member which are member of the household.</param>
        /// <param name="household">Household which the household member are member of.</param>
        /// <param name="creationTime">Date and time for when the membership to the household was created.</param>
        public MemberOfHouseholdProxy(IHouseholdMember householdMember, IHousehold household, DateTime creationTime)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Household member which are member of the household.
        /// </summary>
        public virtual IHouseholdMember HouseholdMember
        {
            get
            {
                throw new NotImplementedException();
            }
        
        }
        
        /// <summary>
        /// Identifier for the household member which are member of the household.
        /// </summary>
        public virtual Guid? HouseholdMemberIdentifier
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
        /// Household which the household member are member of.
        /// </summary>
        public virtual IHousehold Household
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Identifier for the household which the household member are member of.
        /// </summary>
        public virtual Guid? HouseholdIdentifier
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
        /// Date and time for when the membership to the household was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
