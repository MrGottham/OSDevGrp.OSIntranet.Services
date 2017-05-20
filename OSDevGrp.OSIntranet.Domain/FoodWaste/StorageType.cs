using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Storage type.
    /// </summary>
    public class StorageType : TranslatableBase, IStorageType
    {
        #region Private variables

        private int _sortOrder;
        private int _temperature;
        private IRange<int> _temperatureRange;
        private bool _creatable;
        private bool _editable;
        private bool _deletable;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a storage type.
        /// </summary>
        /// <param name="sortOrder">Order for sortering storage types.</param>
        /// <param name="temperature">Default temperature for the storage type.</param>
        /// <param name="temperatureRange">Temperature range for the storage type.</param>
        /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
        /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
        /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
        public StorageType(int sortOrder, int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
        {
            _sortOrder = sortOrder;
            _temperature = temperature;
            _temperatureRange = temperatureRange ?? throw new ArgumentNullException(nameof(temperatureRange));
            _creatable = creatable;
            _editable = editable;
            _deletable = deletable;
        }

        /// <summary>
        /// Creates a storage type.
        /// </summary>
        protected StorageType()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the order for sortering storage types.
        /// </summary>
        public virtual int SortOrder
        {
            get => _sortOrder;
            protected set => _sortOrder = value;
        }

        /// <summary>
        /// Gets the default temperature for the storage type.
        /// </summary>
        public virtual int Temperature
        {
            get => _temperature;
            protected set => _temperature = value;
        }

        /// <summary>
        /// Gets the temperature range for the storage type.
        /// </summary>
        public virtual IRange<int> TemperatureRange
        {
            get => _temperatureRange;
            protected set => _temperatureRange = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets whether household members can create storages of this type.
        /// </summary>
        public virtual bool Creatable
        {
            get => _creatable;
            protected set => _creatable = value;
        }

        /// <summary>
        /// Gets whether household members can edit storages of this type.
        /// </summary>
        public virtual bool Editable
        {
            get => _editable;
            protected set => _editable = value;
        }

        /// <summary>
        /// Gets whether household members can delete storages of this type.
        /// </summary>
        public virtual bool Deletable
        {
            get => _deletable;
            protected set => _deletable = value;
        }

        /// <summary>
        /// Gets the unique identifier for the refrigerator storage type.
        /// </summary>
        public static Guid IdentifierForRefrigerator => new Guid("3CEA8A7D-01A4-40BF-AB96-F70354015352");

        /// <summary>
        /// Gets the unique identifier for the freezer storage type.
        /// </summary>
        public static Guid IdentifierForFreezer => new Guid("959A0D7D-A034-405C-8F6E-EF49ED5E7553");

        /// <summary>
        /// Gets the unique identifier for the kitchen cabinets storage type.
        /// </summary>
        public static Guid IdentifierForKitchenCabinets => new Guid("0F78276B-87D1-4660-8708-A119C5DAA3A9");

        /// <summary>
        /// Gets the unique identifier for the shopping basket storage type.
        /// </summary>
        public static Guid IdentifierForShoppingBasket => new Guid("B5A0B40D-1709-48D9-83F2-E87D54ED80F5");

        #endregion
    }
}
