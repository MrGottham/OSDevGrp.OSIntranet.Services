using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Foreign key for a domain object in the food waste domain.
    /// </summary>
    public class ForeignKey : IdentifiableBase, IForeignKey
    {
        #region Private variables

        private IDataProvider _dataProvider;
        private Guid _foreignKeyForIdentifier;
        private IEnumerable<Type> _foreignKeyForTypes;
        private string _foreignKeyValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a foreign key for a domain object i the food waste domain.
        /// </summary>
        protected ForeignKey()
        {
        }

        /// <summary>
        /// Creates a foreign key for a domain object i the food waste domain.
        /// </summary>
        /// <param name="dataProvider">Data provider who own the foreign key.</param>
        /// <param name="foreignKeyForIdentifier">Identifier for the domain object which has this foreign key.</param>
        /// <param name="foreignKeyForType">Type which has this foreign key.</param>
        /// <param name="foreignKeyValue">Value of the foreign key.</param>
        public ForeignKey(IDataProvider dataProvider, Guid foreignKeyForIdentifier, Type foreignKeyForType, string foreignKeyValue)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (foreignKeyForType == null)
            {
                throw new ArgumentNullException("foreignKeyForType");
            }
            if (string.IsNullOrEmpty(foreignKeyValue))
            {
                throw new ArgumentNullException("foreignKeyValue");
            }
            _dataProvider = dataProvider;
            _foreignKeyForIdentifier = foreignKeyForIdentifier;
            _foreignKeyForTypes = foreignKeyForType.GetInterfaces()
                .Where(m => !m.IsGenericType && m.IsPublic && typeof (IDomainObject).IsAssignableFrom(m))
                .ToArray();
            _foreignKeyValue = foreignKeyValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data provider who own the foreign key.
        /// </summary>
        public virtual IDataProvider DataProvider
        {
            get
            {
                return _dataProvider;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _dataProvider = value;
            }
        }

        /// <summary>
        /// Gets the identifier for the domain object which has this foreign key.
        /// </summary>
        public virtual Guid ForeignKeyForIdentifier
        {
            get { return _foreignKeyForIdentifier; }
            protected set { _foreignKeyForIdentifier = value; }
        }

        /// <summary>
        /// Gets the types which has this foreign key.
        /// </summary>
        public virtual IEnumerable<Type> ForeignKeyForTypes
        {
            get
            {
                return _foreignKeyForTypes;
            }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _foreignKeyForTypes = value;
            }
        }

        /// <summary>
        /// Gets the value of the foreign key.
        /// </summary>
        public virtual string ForeignKeyValue
        {
            get
            {
                return _foreignKeyValue;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                _foreignKeyValue = value;
            }
        }

        #endregion
    }
}
