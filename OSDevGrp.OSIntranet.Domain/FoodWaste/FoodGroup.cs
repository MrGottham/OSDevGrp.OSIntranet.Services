using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Food group.
    /// </summary>
    public class FoodGroup : TranslatableBase, IFoodGroup
    {
        #region Private variables

        private IFoodGroup _parent;
        private IList<IFoodGroup> _children = new List<IFoodGroup>(0); 
        private IList<IForeignKey> _foreignKeys = new List<IForeignKey>(0);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a food group.
        /// </summary>
        public FoodGroup()
        {
        }

        /// <summary>
        /// Creates a food group.
        /// </summary>
        /// <param name="children">Foods groups which has this food group as a parent. </param>
        protected FoodGroup(IEnumerable<IFoodGroup> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException("children");
            }
            _children = children.ToList();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Food group which has this food group as a child.
        /// </summary>
        public virtual IFoodGroup Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (value == null)
                {
                    _parent = null;
                    return;
                }
                if (value.Identifier.HasValue == false)
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.ValueMustBeGivenForProperty, "Identifier"), "value");
                }
                if (value.Identifier.Equals(Identifier) || value.Equals(this))
                {
                    throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "value"), "value");
                }
                var parent = value.Parent;
                while (parent != null)
                {
                    if ((parent.Identifier.HasValue && parent.Identifier.Equals(Identifier)) || parent.Equals(this))
                    {
                        throw new ArgumentException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, value, "value"), "value");
                    }
                    parent = parent.Parent;
                }
                _parent = value;
            }
        }

        /// <summary>
        /// Indicates whether the food group is active.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Foods groups which has this food group as a parent. 
        /// </summary>
        public virtual IEnumerable<IFoodGroup> Children
        {
            get
            {
                return _children;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _children = value.ToList();
            }
        }
        
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
