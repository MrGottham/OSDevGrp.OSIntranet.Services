using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Food group.
    /// </summary>
    public class FoodGroup : TranslatableBase, IFoodGroup
    {
        #region Private variables

        private IList<IForeignKey> _foreignKeys = new List<IForeignKey>(0);
        private IFoodGroup _parent;

        #endregion

        #region Properties

        /// <summary>
        /// Foreign keys for the food group.
        /// </summary>
        public virtual IEnumerable<IForeignKey> ForeignKeys
        {
            get
            {
                return _foreignKeys;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _foreignKeys = value.ToList();
            }
        }

        /// <summary>
        /// Food group which has this food group as a child.
        /// </summary>
        public virtual IFoodGroup Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a foreign key to the food group.
        /// </summary>
        /// <param name="foreignKey">Foreign key which should be added to the food group.</param>
        public virtual void ForeignKeyAdd(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            _foreignKeys.Add(foreignKey);
        }

        #endregion
    }
}
