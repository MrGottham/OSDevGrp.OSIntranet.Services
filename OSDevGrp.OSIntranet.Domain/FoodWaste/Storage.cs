using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.FoodWaste;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Resources;

namespace OSDevGrp.OSIntranet.Domain.FoodWaste
{
    /// <summary>
    /// Storage.
    /// </summary>
    public class Storage : IdentifiableBase, IStorage
    {
        #region Private variables

        private IHousehold _household;
        private int _sortOrder;
        private IStorageType _storageType;
        private string _description;
        private int _temperature;
        private DateTime _creationTime;
        private readonly IDomainObjectValidations _domainObjectValidations;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a storage.
        /// </summary>
        /// <param name="household">Household where the storage are placed.</param>
        /// <param name="sortOrder">Sort order for the storage.</param>
        /// <param name="storageType">Storage type for the storage.</param>
        /// <param name="temperature">Temperature for the storage.</param>
        /// <param name="creationTime">Creation date and time for when the storage was created.</param>
        /// <param name="description">Description for the storage.</param>
        public Storage(IHousehold household, int sortOrder, IStorageType storageType, int temperature, DateTime creationTime, string description = null)
            : this(household, sortOrder, storageType, description, temperature, creationTime, DomainObjectValidations.Create())
        {
        }

        /// <summary>
        /// Creates a storage.
        /// </summary>
        protected Storage()
        {
            _domainObjectValidations = DomainObjectValidations.Create();
        }

        /// <summary>
        /// Creates a storage
        /// </summary>
        /// <param name="household">Household where the storage are placed.</param>
        /// <param name="sortOrder">Sort order for the storage.</param>
        /// <param name="storageType">Storage type for the storage.</param>
        /// <param name="description">Description for the storage.</param>
        /// <param name="temperature">Temperature for the storage.</param>
        /// <param name="creationTime">Creation date and time for when the storage was created.</param>
        /// <param name="domainObjectValidations">Implementation of the common validations used by domain objects in the food waste domain.</param>
        protected Storage(IHousehold household, int sortOrder, IStorageType storageType, string description, int temperature, DateTime creationTime, IDomainObjectValidations domainObjectValidations)
        {
            _household = household ?? throw new ArgumentNullException(nameof(household));
            _storageType = storageType ?? throw new ArgumentNullException(nameof(storageType));
            _domainObjectValidations = domainObjectValidations ?? throw new ArgumentNullException(nameof(domainObjectValidations));

            _sortOrder = ValidateSortOrder(sortOrder, nameof(sortOrder));
            _description = description;
            _temperature = ValidateTemperatue(temperature, nameof(temperature));
            _creationTime = creationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the household where the storage are placed.
        /// </summary>
        public virtual IHousehold Household
        {
            get => _household;
            protected set => _household = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the sort order for the storage.
        /// </summary>
        public virtual int SortOrder
        {
            get => _sortOrder;
            set => _sortOrder = ValidateSortOrder(value, nameof(value));
        }

        /// <summary>
        /// Gets the storage type for the storage.
        /// </summary>
        public virtual IStorageType StorageType
        {
            get => _storageType;
            protected set => _storageType = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the description for the storage.
        /// </summary>
        public virtual string Description
        {
            get => _description;
            set => _description = value;
        }

        /// <summary>
        /// Gets or sets the temperature for the storage.
        /// </summary>
        public virtual int Temperature
        {
            get => _temperature;
            set => _temperature = ValidateTemperatue(value, nameof(value));
        }

        /// <summary>
        /// Gets the creation date and time for when the storage was created.
        /// </summary>
        public virtual DateTime CreationTime
        {
            get => _creationTime;
            protected set => _creationTime = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the sort order for the storage.
        /// </summary>
        /// <param name="sortOrder">Sort order for the storage.</param>
        /// <param name="propertyName">Name of the property which sets the sort order for the storage.</param>
        /// <returns>Validated sort order for the storage.</returns>
        private int ValidateSortOrder(int sortOrder, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (_domainObjectValidations.InRange(sortOrder, new Range<int>(1, 100)))
            {
                return sortOrder;
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, sortOrder, propertyName));
        }

        /// <summary>
        /// Validates the temperature for the storage.
        /// </summary>
        /// <param name="temperature">Temperature for the storage.</param>
        /// <param name="propertyName">Name of the property which sets the temperature for the storage.</param>
        /// <returns>Validated temperature for the storage.</returns>
        private int ValidateTemperatue(int temperature, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (StorageType == null || _domainObjectValidations.InRange(temperature, StorageType.TemperatureRange))
            {
                return temperature;
            }

            throw new IntranetSystemException(Resource.GetExceptionMessage(ExceptionMessage.IllegalValue, temperature, propertyName));
        }

        #endregion
    }
}
