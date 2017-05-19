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
        /// <param name="temperature">Defualt temperature for the storage type.</param>
        /// <param name="temperatureRange">Temperature range for the storage type.</param>
        /// <param name="creatable">Indicates whether household members can create storages of this type.</param>
        /// <param name="editable">Indicates whether household members can edit storages of this type.</param>
        /// <param name="deletable">Indicates whether household members can delete storages of this type.</param>
        public StorageType(int temperature, IRange<int> temperatureRange, bool creatable, bool editable, bool deletable)
        {
            _temperature = temperature;
            _temperatureRange = temperatureRange ?? throw new ArgumentNullException(nameof(temperatureRange));
            _creatable = creatable;
            _editable = editable;
            _deletable = deletable;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the defualt temperature for the storage type.
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

        #endregion
    }
}
