using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Household.
    /// </summary>
    public class Household : IdentifiableBase, IHousehold
    {
        #region Private variables

        private string _name;
        private string _description;
        private DateTime _creationTime;
        private IList<IHouseholdMember> _householdMembers = new List<IHouseholdMember>(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        public Household(string name, string description = null)
            : this(name, description, DateTime.Now)
        {
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        protected Household()
        {
        }

        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">Name for the household.</param>
        /// <param name="description">Description for the household.</param>
        /// <param name="creationTime">Date and time for when the household was created.</param>
        protected Household(string name, string description, DateTime creationTime)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            _name = name;
            _description = description;
            _creationTime = creationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name for the household.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("value");
                }
                _name = value;
            }
        }

        /// <summary>
        /// Description for the household.
        /// </summary>
        public virtual string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Date and time for when the household was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get
            {
                return _creationTime;
            }
            protected set
            {
                _creationTime = value;
            }
        }

        /// <summary>
        /// Household members who is member of this household.
        /// </summary>
        public virtual IEnumerable<IHouseholdMember> HouseholdMembers
        {
            get
            {
                return _householdMembers;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _householdMembers = value.ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a household member to this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be member on this household.</param>
        public virtual void HouseholdMemberAdd(IHouseholdMember householdMember)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }
            _householdMembers.Add(householdMember);
            if (householdMember.Households.Contains(this))
            {
                return;
            }
            householdMember.HouseholdAdd(this);
        }

        /// <summary>
        /// Removes a household member from this household.
        /// </summary>
        /// <param name="householdMember">Household member which should be removed as a member of this household.</param>
        /// <returns>Household member who has been removed af member of this household.</returns>
        public virtual IHouseholdMember HouseholdMemberRemove(IHouseholdMember householdMember)
        {
            if (householdMember == null)
            {
                throw new ArgumentNullException("householdMember");
            }

            var householdMemberToRemove = HouseholdMembers.SingleOrDefault(householdMember.Equals);
            if (householdMemberToRemove == null)
            {
                return null;
            }

            _householdMembers.Remove(householdMemberToRemove);
            if (householdMemberToRemove.Households.Contains(this))
            {
                householdMemberToRemove.HouseholdRemove(this);
            }
            return householdMemberToRemove;
        }

        /// <summary>
        /// Make translation for the household.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        /// <param name="translateHouseholdMembers">Indicates whether to make translation for all the household members who has a membership on this household.</param>
        public virtual void Translate(CultureInfo translationCulture, bool translateHouseholdMembers)
        {
            if (translationCulture == null)
            {
                throw new ArgumentNullException("translationCulture");
            }
            if (translateHouseholdMembers == false)
            {
                return;
            }
            foreach (var householdMember in HouseholdMembers)
            {
                householdMember.Translate(translationCulture, false);
            }
        }

        #endregion
    }
}
