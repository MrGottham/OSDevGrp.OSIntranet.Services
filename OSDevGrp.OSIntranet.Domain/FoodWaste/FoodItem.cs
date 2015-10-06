using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Food item.
    /// </summary>
    public class FoodItem : TranslatableBase, IFoodItem
    {
        #region Private variables

        private IFoodGroup _primaryFoodGroup;
        private IList<IFoodGroup> _foodGroups = new List<IFoodGroup>();
        private IList<IForeignKey> _foreignKeys = new List<IForeignKey>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a food item.
        /// </summary>
        /// <param name="primaryFoodGroup">Primary food group for the food item.</param>
        public FoodItem(IFoodGroup primaryFoodGroup)
        {
            if (primaryFoodGroup == null)
            {
                throw new ArgumentNullException("primaryFoodGroup");
            }
            _primaryFoodGroup = primaryFoodGroup;
            _foodGroups.Add(primaryFoodGroup);
        }

        /// <summary>
        /// Creates a food item.
        /// </summary>
        protected FoodItem()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary food group for the food item.
        /// </summary>
        public virtual IFoodGroup PrimaryFoodGroup
        {
            get
            {
                return _primaryFoodGroup;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                var existingFoodGroup = FoodGroups.SingleOrDefault(foodGroup => foodGroup.Identifier == value.Identifier);
                if (existingFoodGroup != null)
                {
                    _primaryFoodGroup = existingFoodGroup;
                    return;
                }
                _primaryFoodGroup = value;
                _foodGroups.Add(value);
            }
        }

        /// <summary>
        /// Gets or sets whether the food item is active.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets the food groups which this food item belong to.
        /// </summary>
        public virtual IEnumerable<IFoodGroup> FoodGroups
        {
            get
            {
                return _foodGroups;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _foodGroups = value.ToList();
                if (PrimaryFoodGroup == null)
                {
                    PrimaryFoodGroup = _foodGroups.FirstOrDefault();
                    return;
                }
                var primaryFoodGroup = _foodGroups.SingleOrDefault(foodGroup => foodGroup.Identifier == PrimaryFoodGroup.Identifier);
                if (primaryFoodGroup == null)
                {
                    _foodGroups.Add(PrimaryFoodGroup);
                    return;
                }
                PrimaryFoodGroup = primaryFoodGroup;
            }
        }

        /// <summary>
        /// Gets the foreign keys for the food item.
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
        /// Adds a food group which this food item should belong to.
        /// </summary>
        /// <param name="foodGroup">Food group which this food item should belong to.</param>
        public virtual void FoodGroupAdd(IFoodGroup foodGroup)
        {
            if (foodGroup == null)
            {
                throw new ArgumentNullException("foodGroup");
            }
            _foodGroups.Add(foodGroup);
        }

        /// <summary>
        /// Adds a foreign key for the food item.
        /// </summary>
        /// <param name="foreignKey">Foreign key which should be added to the food item.</param>
        public virtual void ForeignKeyAdd(IForeignKey foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException("foreignKey");
            }
            _foreignKeys.Add(foreignKey);
        }

        /// <summary>
        /// Finish up the translation for the food item.
        /// </summary>
        /// <param name="translationCulture">Culture information which are used for translation.</param>
        protected override void OnTranslation(CultureInfo translationCulture)
        {
            foreach (var foodGroup in FoodGroups)
            {
                foodGroup.Translate(translationCulture);
            }
        }

        #endregion
    }
}
